using FluentAssertions;
using TodoList_App.Interfaces;
using TodoList_App.Models;
using TodoList_App.Repositories;

namespace Todo_Test_App.Users;

public class GetUserById
{
    [Fact]
    public async Task GetUserByIdShouldFindUser()
    {
        IAuthenticationServiceInterface repo = new AuthenticationRepository();

        string userId = "674ca6a7d43a26d521921078";


        User user = await repo.GetByIdAsync(userId);

        user.Should().NotBeNull();
        user.UserId.Should().Be(userId);
        user.UserId.Should().NotBeNullOrEmpty();
        user.PasswordHash.Should().NotBeEmpty();
        user.PasswordHash.Should().StartWith("$2a");
    }
}
