using comebackapi.Models;

namespace comebackapi.Repositories;

public interface ITovarRepository
{
    Task<IEnumerable<Tovar>> GetAllAsync();
    Task<int> AddAsync(Tovar tovar);
    Task<int> UpdateAsync(int id, Tovar tovar);
    Task<int> DeleteAsync(int id);
}