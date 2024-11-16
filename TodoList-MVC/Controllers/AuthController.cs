using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
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

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        if (!ModelState.IsValid)
        {
            return View(loginModel); 
        }

        try
        {
            string token = await _authService.LoginAsync(loginModel);


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
        return View(loginModel);
    }
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("AuthToken");
        return RedirectToAction("Login", "Account");
    }
}
