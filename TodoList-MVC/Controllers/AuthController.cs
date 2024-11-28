using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TodoList_MVC.ClientService.Interface;
using TodoList_MVC.Models;

namespace TodoList_MVC.Controllers;

public class AuthController : Controller
{

    private readonly IAuthClient _authService;

    public AuthController(IAuthClient authClient)
    {
        _authService = authClient;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpGet]
    public IActionResult CreateUser()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateUser(CreateUserDTO userModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(userModel);
            }

            var user = await _authService.CreateUser(userModel);

            if (user != null)
            {
                TempData["SuccessMessage"] = "User registered successfully!";
                return RedirectToAction("Login", "Auth");  
            }

            TempData["ErrorMessage"] = "User creation failed.";
            return View(userModel);
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "An error occurred while creating the user.";
            return View(userModel);  
        }
    }



    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("AuthToken");
        return RedirectToAction("Login", "Auth");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            string token = await _authService.LoginAsync(model);


            if (!string.IsNullOrEmpty(token))
            {
                HttpContext.Session.SetString("AuthToken", token);

                return RedirectToAction("Index", "Notes");
            }
        }
        catch (Exception ex)
        {

            ModelState.AddModelError("", "Login dit not succed: " + ex.Message);
        }
        return View(model);
    }
   
}
