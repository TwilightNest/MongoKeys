using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using MongoKeys.Models;

namespace MongoKeys.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                using (var context = new ApplicationContext())
                {
                    user = context.Users.FirstOrDefault(u => u._Login == model.Login);
                }

                if (user == null)
                {
                    using (var context = new ApplicationContext())
                    {
                        context.Users.Add(new User
                            {_Email = model.Email, _Login = model.Login, _Password = model.Password});
                        context.SaveChanges();
                        user = context.Users.Where(u => u._Login == model.Login && u._Password == model.Password)
                            .FirstOrDefault();
                    }

                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.Login, true);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("UserExists", "Пользователь с таким именем уже существует.");
                }
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                using (var context = new ApplicationContext())
                {
                    user = context.Users.FirstOrDefault(u => u._Login == model.Login && u._Password == model.Password);
                }

                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Login, true);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("UserNotExists", "Такого пользователя не существует.");
            }
            return View(model);
        }
    }
}