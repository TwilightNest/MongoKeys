using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using MongoKeys.Models;

namespace MongoKeys.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        [Authorize]
        public ActionResult Index(int id)
        {
            ViewBag._Id = id;
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Settings()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Settings(RegisterModel model)
        {
            using (var context = new ApplicationContext())
            {
                ViewBag._Id = model.Id;
                var user = context.Users.FirstOrDefault(u => u._Id == model.Id);
                if (context.Users.FirstOrDefault(u => u._Login == model.Login) != null)
                {
                    if (context.Users.FirstOrDefault(u => u._Login == model.Login)._Id != model.Id)
                    {
                        ModelState.AddModelError("UserExists", "Пользователь с таким именем уже существует.");
                        return View(model);
                    }

                    user._Login = model.Login;
                    user._Email = model.Email;
                    user._Password = model.Password;
                    context.SaveChanges();
                    FormsAuthentication.SignOut();
                    FormsAuthentication.SetAuthCookie(model.Login, true);
                    if (model.ImagePath == null)
                    {
                        var img = new WebImage("~/Content/Avatars/Default.png");
                        img.Save("~/Content/Avatars/" + user._Id, "PNG");
                    }
                    else
                    {
                        var byteArray = Convert.FromBase64CharArray(model.ImagePath.ToCharArray(), 22,
                            model.ImagePath.Length - 22);
                        using (var ms = new MemoryStream(byteArray))
                        {
                            var img = new WebImage(byteArray);
                            img.Resize(765, 500, false);
                            img.Save("~/Content/Avatars/" + user._Id, "PNG");
                        }
                    }
                }
                else
                {
                    user._Login = model.Login;
                    user._Email = model.Email;
                    user._Password = model.Password;
                    context.SaveChanges();
                    FormsAuthentication.SignOut();
                    FormsAuthentication.SetAuthCookie(model.Login, true);
                    if (model.ImagePath == null)
                    {
                        var img = new WebImage("~/Content/Avatars/Default.png");
                        img.Save("~/Content/Avatars/" + user._Id, "PNG");
                    }
                    else
                    {
                        var byteArray = Convert.FromBase64CharArray(model.ImagePath.ToCharArray(), 22,
                            model.ImagePath.Length - 22);
                        using (var ms = new MemoryStream(byteArray))
                        {
                            var img = new WebImage(byteArray);
                            img.Resize(765, 500, false);
                            img.Save("~/Content/Avatars/" + user._Id, "PNG");
                        }
                    }
                }
            }

            return RedirectToAction("Index", "User", new {id = model.Id});
        }

        [HttpGet]
        [Authorize]
        public ActionResult PlayGame(int steamId)
        {
            using (var context = new ApplicationContext())
            {
                var user = context.Users.FirstOrDefault(u => u._Login == User.Identity.Name);
                Process.Start("steam://rungameid/" + steamId);
                return RedirectToAction("Index", "User", new {id = user._Id});
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return Redirect("~/Home/Index");
        }
    }
}