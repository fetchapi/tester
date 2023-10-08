using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Session;
using TrainingProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using TrainingProject.ViewModels;

namespace TrainingProject.Controllers
{
    public class LoginController : Controller
    {
        private GasStationContext _context;
        public LoginController(GasStationContext context)
        {
            _context = context;
        }
        [HttpGet]
        public  IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public  IActionResult Index(User user)
        {
            var account = _context.User.Where(p => p.Email == user.Email && p.Password == user.Password && p.UserType == "A0001").FirstOrDefault();

            if (account != null && ModelState.IsValid)
            {
                HttpContext.Session.SetString("UserId", account.UserId.ToString());
                HttpContext.Session.SetString("Email", account.Email.ToString());
                return RedirectToAction("Index", "GasStationList");
            }
            else 
            {
                ModelState.AddModelError("", "ログインに失敗しました。電子メールまたはパスワードを確認してください。  ");
                return View();
            }
        } 
    }
}
    