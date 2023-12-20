using MediatR;
using OnlineShop.Entities.Identities;
using OnlineShop.UseCases.Identities.Commands.Add.Contracts.Repositories;

namespace OnlineShop.UseCases.Identities.Commands.Add.Contracts.Events;

public class UpdateReadableDbAfterRegisterUserEventNotificationHandler : INotificationHandler<UpdateReadableDbAfterRegisterUserEvent>
{
    private readonly IUserQueriesRepository _queriesRepository;

    public UpdateReadableDbAfterRegisterUserEventNotificationHandler(IUserQueriesRepository queriesRepository)
    {
        _queriesRepository = queriesRepository;
    }

    public async Task Handle(
        UpdateReadableDbAfterRegisterUserEvent notification,
        CancellationToken cancellationToken)
    {
        var user = new QueriesUser()
        {
            Id = notification.UserId,
            UserName = notification.UserName,
            CreationDate = notification.CreationDate
        };

        await _queriesRepository.AddAsync(user);
    }
}