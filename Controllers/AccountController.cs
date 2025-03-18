using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service;

namespace Controllers{
    public class AccountController:Controller{
        public ApiService apiService;
        public AccountController(ApiService apiService){
            this.apiService=apiService;
        }
        [HttpGet]
        public IActionResult Login(){
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Login login){
            Console.WriteLine($"userName: {login.userName}, userPassword: {login.userPassword}");
            if(!ModelState.IsValid){
                return View(login);
            }
            var token=await apiService.LoginAsync(login);
            if(token!=null){
                var claims=new List<Claim>{
                    new(ClaimTypes.Name,login.userName)
                };
                var identity=new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(identity));
                HttpContext.Session.SetString("Token",token);
                return RedirectToAction("Index","Article");
            }
            ModelState.AddModelError("","Invalid username or password");
            return View(login);
        }
        public async Task<IActionResult> Logout(){
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}