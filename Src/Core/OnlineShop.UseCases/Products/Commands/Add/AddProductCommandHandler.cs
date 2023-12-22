using System.Security.Claims;
using System.Text.RegularExpressions;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OnlineShop.Entities.Identities;
using OnlineShop.Entities.Products;
using OnlineShop.UseCases.Identities.Commands.Login.Contracts.Exceptions;
using OnlineShop.UseCases.Infrastructures;
using OnlineShop.UseCases.Products.Commands.Add.Contracts;
using OnlineShop.UseCases.Products.Commands.Add.Contracts.Events;
using OnlineShop.UseCases.Products.Commands.Add.Contracts.Exceptions;
using OnlineShop.UseCases.Products.Commands.Add.Contracts.Repositories;

namespace OnlineShop.UseCases.Products.Commands.Add;

public class AddProductCommandHandler : IRequestHandler<AddProductCommand, int>
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;
    private readonly IHttpContextAccessor _accessor;

    public AddProductCommandHandler(
        IMapper mapper,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        IMediator mediator,
        UserManager<User> userManager,
        IHttpContextAccessor accessor)
    {
        _mapper = mapper;
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _userManager = userManager;
        _accessor = accessor;
    }

    public async Task<int> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        StopIfWrongPhoneNumberFormat(request.ManufacturePhone);
        
        await StopIfProductAlreadyExist(request.ManufactureEmail, request.ProduceDate);

        var registrantId = _accessor.HttpContext!.User.Claims
            .FirstOrDefault(_ => _.Type == ClaimTypes.NameIdentifier)?.Value;
        var user = await _userManager.FindByIdAsync(registrantId!);
    
        StopIfUserNotFound(user);

        var product = _mapper.Map<Product>(request);
        product.RegistrantId = registrantId!;

        await _productRepository.Add(product);

        await _unitOfWork.SaveChangesAsyncForWritableDb();

        await PublishEventForUpdateReadableDb(product, user!.UserName!);

        return product.Id;
    }

    private static void StopIfWrongPhoneNumberFormat(string? phoneNumber)
    {
        if (phoneNumber == null) return;
        var mobileReg = @"^(0|0098|\+98)9(0[1-5]|[1 3]\d|2[0-2]|98)\d{7}$";
        var reg = new Regex(mobileReg);
        var isCorrectPhoneNumberFormat = reg.IsMatch(phoneNumber);
        if (!isCorrectPhoneNumberFormat)
            throw new WrongPhoneNumberFormatException();
    }

    private static void StopIfUserNotFound(User? user)
    {
        if (user == null)
            throw new UserNotFoundException();
    }

    private async Task PublishEventForUpdateReadableDb(Product product, string userName)
    {
        var updateReadableDbEvent = new UpdateReadableDbAfterAddProductEvent()
        {
            Id = product.Id,
            Name = product.Name,
            ProduceDate = product.ProduceDate,
            ManufactureEmail = product.ManufactureEmail,
            ManufacturePhone = product.ManufacturePhone,
            IsAvailable = product.IsAvailable,
            RegistrantId = product.RegistrantId,
            RegistrantUserName = userName
        };
        await _mediator.Publish(updateReadableDbEvent, CancellationToken.None);
    }

    private async Task StopIfProductAlreadyExist(string manufactureEmail, DateTime produceDate)
    {
        var isExistProduct = await _productRepository.IsExist(manufactureEmail, produceDate);

        if (isExistProduct)
        {
            throw new ProductAlreadyExistException();
        }
    }
}