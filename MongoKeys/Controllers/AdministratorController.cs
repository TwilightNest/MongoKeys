using System;
using System.IO;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using MongoKeys.Models;

namespace MongoKeys.Controllers
{
    public class AdministratorController : Controller
    {
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            using (var context = new ApplicationContext())
            {
                var currentUser = context.Users.FirstOrDefault(u => u._Login == User.Identity.Name);
                if (!currentUser._IsAdmin) return RedirectToAction("NeedAdministratorRights", "Administrator");
            }

            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddUser()
        {
            using (var context = new ApplicationContext())
            {
                var currentUser = context.Users.FirstOrDefault(u => u._Login == User.Identity.Name);
                if (!currentUser._IsAdmin) return RedirectToAction("NeedAdministratorRights", "Administrator");
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddUser(RegisterModel model)
        {
            using (var context = new ApplicationContext())
            {
                var currentUser = context.Users.FirstOrDefault(u => u._Login == User.Identity.Name);
                if (!currentUser._IsAdmin) return RedirectToAction("NeedAdministratorRights", "Administrator");
                if (context.Users.FirstOrDefault(u => u._Login == model.Login) == null)
                {
                    var user = new User
                    {
                        _Login = model.Login,
                        _Email = model.Email,
                        _Password = model.Password,
                        _IsAdmin = model.IsAdmin
                    };
                    context.Users.Add(user);
                    context.SaveChanges();
                    var id = user._Id;
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
                    ModelState.AddModelError("UserExists", "Пользователь с таким именем уже существует.");
                    return View(model);
                }
            }

            return RedirectToAction("ChangeOrDeleteUser", "Administrator");
        }

        [HttpGet]
        [Authorize]
        public ActionResult ChangeOrDeleteUser()
        {
            using (var context = new ApplicationContext())
            {
                var currentUser = context.Users.FirstOrDefault(u => u._Login == User.Identity.Name);
                if (!currentUser._IsAdmin) return RedirectToAction("NeedAdministratorRights", "Administrator");
            }

            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult ChangeUser(int id)
        {
            using (var context = new ApplicationContext())
            {
                var currentUser = context.Users.FirstOrDefault(u => u._Login == User.Identity.Name);
                if (!currentUser._IsAdmin) return RedirectToAction("NeedAdministratorRights", "Administrator");
            }

            ViewBag._Id = id;
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult ChangeUser(RegisterModel model)
        {
            using (var context = new ApplicationContext())
            {
                ViewBag._Id = model.Id;
                var currentUser = context.Users.FirstOrDefault(u => u._Login == User.Identity.Name);
                if (!currentUser._IsAdmin) return RedirectToAction("NeedAdministratorRights", "Administrator");
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
                    user._IsAdmin = model.IsAdmin;
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
                    user._IsAdmin = model.IsAdmin;
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

            return RedirectToAction("ChangeOrDeleteUser", "Administrator");
        }

        [HttpGet]
        [Authorize]
        public ActionResult DeleteUser(int id)
        {
            using (var context = new ApplicationContext())
            {
                var currentUser = context.Users.FirstOrDefault(u => u._Login == User.Identity.Name);
                if (!currentUser._IsAdmin) return RedirectToAction("NeedAdministratorRights", "Administrator");
            }

            ViewBag._Id = id;
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteUser(LoginModel model)
        {
            using (var context = new ApplicationContext())
            {
                var currentUser = context.Users.FirstOrDefault(u => u._Login == User.Identity.Name);
                if (!currentUser._IsAdmin) return RedirectToAction("NeedAdministratorRights", "Administrator");
                var user = context.Users.FirstOrDefault(u => u._Login == model.Login);
                context.Users.Remove(user);
                context.SaveChanges();
            }

            return RedirectToAction("ChangeOrDeleteUser", "Administrator");
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddGame()
        {
            using (var context = new ApplicationContext())
            {
                var currentUser = context.Users.FirstOrDefault(u => u._Login == User.Identity.Name);
                if (!currentUser._IsAdmin) return RedirectToAction("NeedAdministratorRights", "Administrator");
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddGame(GameModel model)
        {
            using (var context = new ApplicationContext())
            {
                var currentUser = context.Users.FirstOrDefault(u => u._Login == User.Identity.Name);
                if (!currentUser._IsAdmin) return RedirectToAction("NeedAdministratorRights", "Administrator");
                if (context.Games.FirstOrDefault(g => g._Name == model.Name) == null)
                {
                    var game = new Game
                    {
                        _Name = model.Name,
                        _SteamId = model.SteamId,
                        _Review = model.Review,
                        _ShortDescription = model.ShortDescription,
                        _FullDescription = model.FullDescription,
                        _Price = model.Price,
                        _DateOfRelease = model.DateOfRelease,
                        _Sale = model.Sale,
                        _Genre = model.Genre
                    };
                    context.Games.Add(game);
                    context.SaveChanges();
                    if (model.ImagePath == null)
                    {
                        var img = new WebImage("~/Content/Images/Default.png");
                        img.Save("~/Content/Images/" + game._Id, "PNG");
                    }
                    else
                    {
                        var byteArray = Convert.FromBase64CharArray(model.ImagePath.ToCharArray(), 22,
                            model.ImagePath.Length - 22);
                        using (var ms = new MemoryStream(byteArray))
                        {
                            var img = new WebImage(byteArray);
                            img.Resize(765, 500, false);
                            img.Save("~/Content/Images/" + game._Id, "PNG");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("GameExists", "Игра с таким названием уже существует.");
                    return View(model);
                }
            }

            return RedirectToAction("ChangeOrDeleteGame", "Administrator");
        }

        [HttpGet]
        [Authorize]
        public ActionResult ChangeOrDeleteGame()
        {
            using (var context = new ApplicationContext())
            {
                var currentUser = context.Users.FirstOrDefault(u => u._Login == User.Identity.Name);
                if (!currentUser._IsAdmin) return RedirectToAction("NeedAdministratorRights", "Administrator");
            }

            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult ChangeGame(int id)
        {
            using (var context = new ApplicationContext())
            {
                var currentUser = context.Users.FirstOrDefault(u => u._Login == User.Identity.Name);
                if (!currentUser._IsAdmin) return RedirectToAction("NeedAdministratorRights", "Administrator");
            }

            ViewBag._Id = id;
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult ChangeGame(GameModel model)
        {
            using (var context = new ApplicationContext())
            {
                ViewBag._Id = model.Id;
                var currentUser = context.Users.FirstOrDefault(u => u._Login == User.Identity.Name);
                if (!currentUser._IsAdmin) return RedirectToAction("NeedAdministratorRights", "Administrator");
                var game = context.Games.FirstOrDefault(g => g._Id == model.Id);
                if (context.Games.FirstOrDefault(g => g._Name == model.Name) != null)
                {
                    if (context.Games.FirstOrDefault(g => g._Name == model.Name)._Id != model.Id)
                    {
                        ModelState.AddModelError("UserExists", "Пользователь с таким именем уже существует.");
                        return View(model);
                    }

                    game._Name = model.Name;
                    game._SteamId = model.SteamId;
                    game._Review = model.Review;
                    game._ShortDescription = model.ShortDescription;
                    game._FullDescription = model.FullDescription;
                    game._Price = model.Price;
                    game._DateOfRelease = model.DateOfRelease;
                    game._Sale = model.Sale;
                    game._Genre = model.Genre;
                    context.SaveChanges();
                    if (model.ImagePath == null)
                    {
                        var img = new WebImage("~/Content/Images/Default.png");
                        img.Save("~/Content/Images/" + game._Id, "PNG");
                    }
                    else
                    {
                        var byteArray = Convert.FromBase64CharArray(model.ImagePath.ToCharArray(), 22,
                            model.ImagePath.Length - 22);
                        using (var ms = new MemoryStream(byteArray))
                        {
                            var img = new WebImage(byteArray);
                            img.Resize(765, 500, false);
                            img.Save("~/Content/Images/" + game._Id, "PNG");
                        }
                    }
                }
                else
                {
                    game._Name = model.Name;
                    game._SteamId = model.SteamId;
                    game._Review = model.Review;
                    game._ShortDescription = model.ShortDescription;
                    game._FullDescription = model.FullDescription;
                    game._Price = model.Price;
                    game._DateOfRelease = model.DateOfRelease;
                    game._Sale = model.Sale;
                    game._Genre = model.Genre;
                    context.SaveChanges();
                    if (model.ImagePath == null)
                    {
                        var img = new WebImage("~/Content/Images/Default.png");
                        img.Save("~/Content/Images/" + game._Id, "PNG");
                    }
                    else
                    {
                        var byteArray = Convert.FromBase64CharArray(model.ImagePath.ToCharArray(), 22,
                            model.ImagePath.Length - 22);
                        using (var ms = new MemoryStream(byteArray))
                        {
                            var img = new WebImage(byteArray);
                            img.Resize(765, 500, false);
                            img.Save("~/Content/Images/" + game._Id, "PNG");
                        }
                    }
                }
            }

            return RedirectToAction("ChangeOrDeleteGame", "Administrator");
        }

        [HttpGet]
        [Authorize]
        public ActionResult DeleteGame(int id)
        {
            using (var context = new ApplicationContext())
            {
                var currentUser = context.Users.FirstOrDefault(u => u._Login == User.Identity.Name);
                if (!currentUser._IsAdmin) return RedirectToAction("NeedAdministratorRights", "Administrator");
            }

            ViewBag._Id = id;
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteGame(GameModel model)
        {
            using (var context = new ApplicationContext())
            {
                var currentUser = context.Users.FirstOrDefault(u => u._Login == User.Identity.Name);
                if (!currentUser._IsAdmin) return RedirectToAction("NeedAdministratorRights", "Administrator");
                var game = context.Games.FirstOrDefault(п => п._Name == model.Name);
                context.Games.Remove(game);
                context.SaveChanges();
            }

            return RedirectToAction("ChangeOrDeleteGame", "Administrator");
        }

        [HttpGet]
        [Authorize]
        public ActionResult NeedAdministratorRights()
        {
            return View();
        }
    }
}