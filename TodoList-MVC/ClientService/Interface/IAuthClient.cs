using TodoList_MVC.Models;

namespace TodoList_MVC.ClientService.Interface;

public interface IAuthClient
{
    Task<string> LoginAsync(LoginModel loginModel);
    Task<CreateUserDTO> CreateUser(CreateUserDTO createUserModel);
}
