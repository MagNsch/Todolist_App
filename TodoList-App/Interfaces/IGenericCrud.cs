using TodoList_App.Models;

namespace TodoList_App.Interfaces;

public interface IGenericCrud<TEntity, TDto>
{
    Task<IEnumerable<TEntity>> GetAllAsync(string userId);
    Task<TEntity> CreateAsync(TDto dto);
    Task<TEntity> GetByIdAsync(string id);
    Task<bool> DeleteAsync(string bsonString);
}
