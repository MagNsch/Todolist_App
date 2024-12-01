using FluentAssertions;
using TodoList_App.DTOs;
using TodoList_App.Interfaces;
using TodoList_App.Models;
using TodoList_App.Repositories;

namespace Todo_Test_App.Users;

public class CreateUser
{
    [Fact]
    public async Task CreateUserShouldNotBeNUll()
    {
        IAuthenticationServiceInterface repo = new AuthenticationRepository();

        CreateUserDTO userDTO = new("123han@mail.com", "1234567Hansen!2");

        User user = await repo.CreateAsync(userDTO);

        user.Should().NotBeNull();
        user.Email.Should().Be(userDTO.Email);
        user.PasswordHash.Should().NotBe(userDTO.PasswordHash);
        user.PasswordHash.Should().StartWith("$2");

    }

    

}
