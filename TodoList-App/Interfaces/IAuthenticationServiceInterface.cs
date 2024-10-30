using TodoList_App.Models;

namespace TodoList_App.Interfaces
{
    public interface IAuthenticationServiceInterface
    {
        Task<User> GetUserByEmail(string email);
    }
}
