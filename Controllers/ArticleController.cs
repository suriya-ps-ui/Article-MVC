using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using Service;
namespace Controllers{
    [Authorize]
    public class ArticleController:Controller{
        public ApiService apiService;
        public ArticleController(ApiService apiService){
            this.apiService=apiService;
        }
        public async Task<IActionResult> Index(){
            var token=HttpContext.Session.GetString("Token");
            var articles=await apiService.GetArticlesAsync(token);
            return View(articles);
        }
        [HttpGet]
        public IActionResult Create(){
            return View("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Create(Article article){
            if (!ModelState.IsValid){
                return View(article);
            }
            var token=HttpContext.Session.GetString("Token");
            try{await apiService.CreateUpdateArticlesAsync(token,article);}
            catch(Exception){return View();}
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id){
            var token=HttpContext.Session.GetString("Token");
            await apiService.DeleteArticlesAsync(token,id);
            return RedirectToAction("Index");
        }
    }
}