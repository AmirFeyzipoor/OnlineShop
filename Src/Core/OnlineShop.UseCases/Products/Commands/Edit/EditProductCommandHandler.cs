using System.Security.Claims;
using System.Text.RegularExpressions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OnlineShop.Entities.Identities;
using OnlineShop.Entities.Products;
using OnlineShop.UseCases.Infrastructures;
using OnlineShop.UseCases.Products.Commands.Add.Contracts.Exceptions;
using OnlineShop.UseCases.Products.Commands.Add.Contracts.Repositories;
using OnlineShop.UseCases.Products.Commands.Delete.Contracts.Exceptions;
using OnlineShop.UseCases.Products.Commands.Edit.Contracts;
using OnlineShop.UseCases.Products.Commands.Edit.Contracts.Events;
using OnlineShop.UseCases.Products.Commands.Edit.Contracts.Exceptions;

namespace OnlineShop.UseCases.Products.Commands.Edit;

public class EditProductCommandHandler : IRequestHandler<EditProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _accessor;

    public EditProductCommandHandler(
        IProductRepository productRepository,
        IMediator mediator,
        IUnitOfWork unitOfWork,
        IHttpContextAccessor accessor)
    {
        _productRepository = productRepository;
        _mediator = mediator;
        _unitOfWork = unitOfWork;
        _accessor = accessor;
    }

    public async Task Handle(EditProductCommand request, CancellationToken cancellationToken)
    {
        StopIfWrongPhoneNumberFormat(request.ManufacturePhone);

        var productId = request.GetProductId();
        var product = await _productRepository.Find(productId);

        StopIfProductNotFound(product);

        await StopIfProductAlreadyExist(request.ManufactureEmail, product!.ProduceDate);
        
        var userId = _accessor.HttpContext!.User.Claims
            .FirstOrDefault(_ => _.Type == ClaimTypes.NameIdentifier)?.Value;
        StopIfUnauthorizedEditProductAccess(product!.RegistrantId, userId);

        EditProduct(request, product);

        _productRepository.Update(product);
        
        await _unitOfWork.SaveChangesAsyncForWritableDb();
        
        await PublishEventForUpdateReadableDb(product);
    }

    private static void EditProduct(EditProductCommand request, Product product)
    {
        product.Name = request.Name;
        product.ManufactureEmail = request.ManufactureEmail;
        product.ManufacturePhone = request.ManufacturePhone;
        product.IsAvailable = request.IsAvailable;
    }

    private static void StopIfProductNotFound(Product? product)
    {
        if (product == null)
            throw new ProductNotFoundException();
    }

    private async Task PublishEventForUpdateReadableDb(Product product)
    {
        var updateReadableDbEvent = new UpdateReadableDbAfterEditProductEvent()
        {
            ProductId = product.Id,
            Name = product.Name,
            ManufactureEmail = product.ManufactureEmail,
            ManufacturePhone = product.ManufacturePhone,
            IsAvailable = product.IsAvailable
        };
        await _mediator.Publish(updateReadableDbEvent, CancellationToken.None);
    }
    
    private static void StopIfUnauthorizedEditProductAccess(string registrantId, string? userId)
    {
        if (registrantId != userId)
            throw new UnauthorizedEditProductAccessException();
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

    private async Task StopIfProductAlreadyExist(string manufactureEmail, DateTime produceDate)
    {
        var isExistProduct = await _productRepository.IsExist(manufactureEmail, produceDate);

        if (isExistProduct)
        {
            throw new ProductAlreadyExistException();
        }
    }
}