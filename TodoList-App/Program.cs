using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using TodoList_App.ApiEndpoints;
using TodoList_App.DTOs;
using TodoList_App.Interfaces;
using TodoList_App.Models;
using TodoList_App.Repositories;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        builder.Services.AddScoped<IGenericCrud<Note, CreateNoteDTO>, NotesRepository>();
        builder.Services.AddScoped<IAuthenticationServiceInterface, AuthenticationRepository>();

        builder.Services.AddEndpointsApiExplorer();
        //tokenvalue without "" and <> just bearer + genereted token "Bearer token"
        //Must write bearer and then keyvalue "Bearer keyvalue"
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
            });
            options.OperationFilter<SecurityRequirementsOperationFilter>();
        });


        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "yourIssuer", 
            ValidAudience = "yourAudience", 
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mysecretkeyisnoteasytoguessnonoo")) 
        };
    });


        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        }
        );

        builder.Services.AddAuthorization();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSerilogRequestLogging();

        app.UseHttpsRedirection();

        NotesCRUDEndpoints.MapNoteRoutes(app);
        UsersCRUDEndpoints.MapNoteRoutes(app);

        app.Run();
    }
}