namespace OnlineShop.UseCases.Infrastructures;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
    Task SaveChangesAsyncForWritableDb();
    Task SaveChangesAsyncForReadableDb();
}