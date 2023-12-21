using MediatR;

namespace OnlineShop.UseCases.Identities.Commands.Register.Contracts.Events;

public class UpdateReadableDbAfterRegisterUserEvent : INotification
{
    public UpdateReadableDbAfterRegisterUserEvent(
        string userId,
        string userName,
        DateTime creationDate)
    {
        UserId = userId;
        UserName = userName;
        CreationDate = creationDate;
    }

    public string UserId { get; set; }  
    public string UserName { get; set; }
    public DateTime CreationDate { get; set; }
}