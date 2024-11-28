using TodoList_App.DTOs;
using TodoList_App.Models;

namespace TodoList_App.Interfaces;

public interface IAuthenticationServiceInterface
{
    Task<User> GetUserByEmail(string email);
    Task<bool> DeleteAsync(string bsonString);
    Task<User> CreateAsync(CreateUserDTO dto);
    Task<User> GetByIdAsync(string id);
}
