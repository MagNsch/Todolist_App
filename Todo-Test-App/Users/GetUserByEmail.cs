using FluentAssertions;

using TodoList_App.Interfaces;
using TodoList_App.Repositories;
using TodoList_App.Models;

namespace Todo_Test_App.Users;

public class GetUserByEmail
{
    [Fact]
    public async Task GetUserShouldFindById()
    {
        //Arrange
        IAuthenticationServiceInterface repo = new AuthenticationRepository();
        string userEmail = "brian@mail.com";
        //Act
        User user = await repo.GetUserByEmail(userEmail);
        //Assert
        user.Email.Should().Be(userEmail);
        user.Should().NotBeNull();
        user.UserId.Should().NotBeNullOrEmpty();
        user.PasswordHash.Should().NotBeEmpty();
        user.PasswordHash.Should().StartWith("$2");

    }
}
