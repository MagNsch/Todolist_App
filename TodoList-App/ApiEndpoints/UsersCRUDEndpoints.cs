using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoList_App.DTOs;
using TodoList_App.HelperMethods;
using TodoList_App.Interfaces;
using TodoList_App.Models;

namespace TodoList_App.ApiEndpoints;

public static class UsersCRUDEndpoints
{
    public static void MapNoteRoutes(this IEndpointRouteBuilder app)
    {
        app.MapGet("getuserbyid/{id}", [Authorize] async ([FromServices] IAuthenticationServiceInterface userRepo, [FromServices] ILogger<Program> logger, [FromRoute] string id) =>
        {
            User user = await userRepo.GetByIdAsync(id);

            if (string.IsNullOrEmpty(user.UserId))
            {
                logger.LogWarning("UserId was not found or is empty");
                return Results.BadRequest("Id is null");
            }
            if (user == null)
            {
                logger.LogWarning("Note with id {Id} was not found.", id);
                return Results.NotFound("Note was not found");
            }

            logger.LogInformation("Successfully retrieved note with id {Id}.", id);
            return Results.Ok(user);
        }).RequireAuthorization();


        app.MapPost("auth/create", async ([FromServices] IAuthenticationServiceInterface userRepo, [FromServices] ILogger<Program> logger, [FromBody] CreateUserDTO userDTO) =>
        {
            if (userDTO == null)
            {
                logger.LogWarning("Note is null");
                return Results.NotFound("Note data is missing");
            }

            var existingUser = await userRepo.GetUserByEmail(userDTO.Email);
            if (existingUser != null)
            {
                logger.LogWarning("User with this email already exists: {Email}", userDTO.Email);
                return Results.BadRequest("User with this email already exists.");
            }

            try
            {
                var newUser = await userRepo.CreateAsync(userDTO);
                logger.LogInformation("Successfully created a new note");
                return Results.Created($"/users/{newUser.UserId}", newUser);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while creating user.");
                return Results.Problem("An error occurred while creating the user.");
            }
        });

        app.MapPost("/auth/login", async ([FromServices] IAuthenticationServiceInterface auth, [FromServices] ILogger<Program> logger, [FromBody] LoginModel loginModel) =>
        {
            var user = await auth.GetUserByEmail(loginModel.Email);

            if (user == null || !BCryptHashing.VerifyPassword(loginModel.Password, user.PasswordHash))
            {
                return Results.Unauthorized();
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserId),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mysecretkeyisnoteasytoguessnonoo"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "yourIssuer",
                audience: "yourAudience",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds);

            return Results.Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        });
    }
}
