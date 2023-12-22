using AutoMapper;
using MediatR;
using OnlineShop.Entities.Products;
using OnlineShop.UseCases.Identities.Commands.Register.Contracts.Repositories;
using OnlineShop.UseCases.Infrastructures;

namespace OnlineShop.UseCases.Products.Commands.Add.Contracts.Events;

public class UpdateReadableDbAfterAddProductEventHandler : INotificationHandler<UpdateReadableDbAfterAddProductEvent>
{
    private readonly IQueriesProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateReadableDbAfterAddProductEventHandler(
        IQueriesProductRepository productRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateReadableDbAfterAddProductEvent notification, CancellationToken cancellationToken)
    {
        var product = _mapper.Map<QueriesProduct>(notification);

        await _productRepository.Add(product);
        
        await _unitOfWork.SaveChangesAsyncForReadableDb();
    }
}