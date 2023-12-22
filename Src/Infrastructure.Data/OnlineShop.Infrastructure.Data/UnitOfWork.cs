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
        await _writableDb.SaveChangesAsync();
        await _readableDb.SaveChangesAsync();
    }

    public async Task SaveChangesAsyncForWritableDb()
    {
        await _writableDb.SaveChangesAsync();
    }

    public async Task SaveChangesAsyncForReadableDb()
    {
        await _readableDb.SaveChangesAsync();
    }
}