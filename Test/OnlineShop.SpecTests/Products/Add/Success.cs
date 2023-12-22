using System.Security.Claims;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using OnlineShop.Entities.Identities;
using OnlineShop.Entities.Products;
using OnlineShop.Infrastructure.ReadableData;
using OnlineShop.Infrastructure.WritableData;
using OnlineShop.SpecTests.Infrastructures;
using OnlineShop.TestTools.Products;
using OnlineShop.UseCases.Products.Commands.Add.Contracts;
using OnlineShop.UseCases.Products.Commands.Add.Contracts.Events;

namespace OnlineShop.SpecTests.Products.Add;

[Story(
    AsA = "کاربر احراز هویت شده",
    InOrderTo = "محصولات را در بلاگ نمایش دهم",
    IWantTo = "محصول های بلاگ را مدیریت کنم")]
[Scenario("ثبت محصول جدید")]
public class Success : EFDataContextDatabaseFixture
{
    private readonly WritableDb _writableDb;
    private readonly ReadableDb _readableDb;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<UserManager<User>> _mockUserManager;
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private AddProductCommand? _command;
    private User? _registrant;

    public Success(ConfigurationFixture configuration) : base(configuration)
    {
        _writableDb = CreateDataContext();
        _readableDb = CreateReadDataContext();
        _mockMapper = new Mock<IMapper>();
        _mockMediator = new Mock<IMediator>();
        var userStore = new Mock<IUserStore<User>>();
        _mockUserManager = new Mock<UserManager<User>>(
            userStore.Object,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null);
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
    }

    [Given("یک کاربر با یوزر نیم Amir و پسورد Amir007" +
           " در فهرست کاربرا ها از قبل وجود دارد")]
    [And("هیچ محصولی در فهرست محصول ها وجود ندارد")]
    public async Task Given()
    {
        _registrant = new User()
        {
            Id = "d03b9e76-7f83-4594-82bf-1cd838785d15",
            UserName = "Amir",
            PasswordHash = "AQAAAAIAAYagAAAAEJTDX1vl7Ma2XyMQuwBYXiV5VXW+dXOt1G/SS1bfozS2OiO68eLW0xdeegGUv87x7Q==",
            CreationDate = DateTime.Now
        };
        _writableDb.Add(_registrant);
        await _writableDb.SaveChangesAsync();
    }

    [When("یک محصول با شماره ی تولید کننده ی 09389066817 و " +
          "نام محصول تست" +
          "ایمیل تولید کننده ی Amir0007@gmail.com" +
          "و زمان تولید همین ثانیه ثبت میکنم ")]
    public async Task When()
    {
        _command = new AddProductCommand(
            name: "test product",
            manufactureEmail: "Amir0007@gmail.com",
            produceDate: DateTime.Now)
        {
            IsAvailable = true
        };


        var handler = ProductFactory.GenerateAddProductCommandHandler(
            _writableDb, _readableDb, _mockMapper, _mockMediator, _mockUserManager, _mockHttpContextAccessor);
        
        var product = new Product()
        {
            Name = _command.Name,
            IsAvailable = _command.IsAvailable,
            ManufactureEmail = _command.ManufactureEmail,
            ManufacturePhone = _command.ManufacturePhone,
            ProduceDate = _command.ProduceDate
        };
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, _registrant!.Id) };
        _mockHttpContextAccessor.Setup(_ => _.HttpContext!.User.Claims).Returns(claims);
        _mockUserManager.Setup(_ => _.FindByIdAsync(It.Is<string>(_ => _ == _registrant!.Id)))
            .ReturnsAsync(_registrant!);
        _mockMapper.Setup(_ => _.Map<Product>(It.IsAny<AddProductCommand>()))
            .Returns(product);

        await handler.Handle(_command, CancellationToken.None);
    }

    [Then("باید تنها یک محصول با شماره ی تولید کننده ی 09389066817 و " +
          "نام محصول تست" +
          "ایمیل تولید کننده ی Amir0007@gmail.com" +
          "و زمان تولید همین ثانیه در فهرست محصولات وجود داشته باشد")]
    public async Task Then()
    {
        var products = await _writableDb.Set<Product>().ToListAsync();

        products.Should().HaveCount(1);
        products.First().Name.Should().Be(_command!.Name);
        products.First().ManufacturePhone.Should().Be(_command!.ManufacturePhone);
        products.First().ManufactureEmail.Should().Be(_command.ManufactureEmail);
        products.First().ProduceDate.Should().Be(_command.ProduceDate);

        _mockMediator.Verify(_ => _.Publish(
            It.IsAny<UpdateReadableDbAfterAddProductEvent>(),
            It.IsAny<CancellationToken>()));
    }

    [Fact]
    public void Run()
    {
        Runner.RunScenario(
            _ => Given().Wait(),
            _ => When().Wait(),
            _ => Then().Wait());
    }
}