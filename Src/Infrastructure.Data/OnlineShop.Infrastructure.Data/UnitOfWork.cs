
using OnlineShop.Infrastructure.ReadableData;
using OnlineShop.Infrastructure.WritableData;
using OnlineShop.UseCases.Infrastructures;

namespace OnlineShop.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly WritableDb _writableDb;
    private readonly ReadableDb _readableDb;

    public UnitOfWork(WritableDb db, ReadableDb readableDb)
    {
        _writableDb = db;
        _readableDb = readableDb;
    }

    public async Task SaveChangesAsync()
    {
        try
        {
            await _writableDb.SaveChangesAsync();
            await _readableDb.SaveChangesAsync();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}