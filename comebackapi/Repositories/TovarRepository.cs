using comebackapi.Infrastructure;
using comebackapi.Models;
using Microsoft.EntityFrameworkCore;

namespace comebackapi.Repositories;

public class TovarRepository : ITovarRepository
{
    private readonly AppDbContext _context;

    public TovarRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Tovar>> GetAllAsync()
    {
        return await _context.Tovars.AsNoTracking().ToListAsync();
    }

    public async Task<int> AddAsync(Tovar tovar)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var newtovar = new Tovar{ Name = tovar.Name, Price = tovar.Price };
            await _context.Tovars.AddAsync(newtovar);
            await _context.SaveChangesAsync();
            var audit = new TovarAudit()
            {
                Description = $"Tovar with id: {newtovar.Id} was created",
                UpdatedOn = DateTime.Now.ToString(),
                TovarId = newtovar.Id
            };
            await _context.TovarAudits.AddAsync(audit);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }

        return tovar.Id;
    }

    public async Task<int> UpdateAsync(int id, Tovar tovar)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var tovar1 = await _context.Tovars.FindAsync(id);
            var audit = new TovarAudit()
            {
                Description = $"Tovar with id: {id} was updated : {tovar1.Name} => {tovar.Name}; {tovar1.Price} => {tovar.Price}",
                UpdatedOn = DateTime.Now.ToString(),
                TovarId = tovar1.Id,
            };
            await _context.Tovars.Where(p => p.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(s => s.Name, tovar.Name)
                    .SetProperty(s => s.Price, tovar.Price));
            await _context.TovarAudits.AddAsync(audit);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
        return tovar.Id;
    }

    public async Task<int> DeleteAsync(int id)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await _context.TovarAudits
                .Where(a => a.TovarId == id)
                .ExecuteUpdateAsync(setters => setters.SetProperty(a => a.TovarId, (int?)null));
            var audit = new TovarAudit()
            {
                Description = $"Tovar with id: {id} was deleted",
                UpdatedOn = DateTime.Now.ToString(),
                TovarId = null
            };
            await _context.TovarAudits.AddAsync(audit);
            await _context.SaveChangesAsync();
            await _context.Tovars.Where(p => p.Id == id)
                .ExecuteDeleteAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
        return id;
    }
}