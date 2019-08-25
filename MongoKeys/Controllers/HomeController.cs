using System;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using MongoKeys.Models;

namespace MongoKeys.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Latest()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Pre()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Sales()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Try()
        {
            using (var context = new ApplicationContext())
            {
                ViewBag.User = context.Users.FirstOrDefault(u => u._Login == User.Identity.Name);
            }

            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Giveaway()
        {
            return View();
        }

        [HttpGet]
        public ActionResult BuyGame(int id)
        {
            ViewBag._Id = id;
            using (var context = new ApplicationContext())
            {
                ViewBag.User = context.Users.FirstOrDefault(u => u._Login == User.Identity.Name);
            }

            return View();
        }

        [HttpPost]
        public ActionResult BuyGame(BuyModel model)
        {
            Game game = null;
            using (var context = new ApplicationContext())
            {
                var user = context.Users.FirstOrDefault(u => u._Login == model.UserName);
                game = context.Games.FirstOrDefault(g => g._Id == model.GameId);
                if (!user._Games.Contains(game)) user._Games.Add(game);
                if (!game._Users.Contains(user)) game._Users.Add(user);
                context.SaveChanges();
            }

            var message = new MailMessage();
            message.To.Add(model.Email);
            message.Subject = "MongoKeys.com";
            var key = GenerateKey();
            message.Body = "Благодарим за приобретение игры " + game._Name + " " + model.UserName + ", вот ваш ключ " +
                           key + ".";
            var smtp = new SmtpClient();
            try
            {
                smtp.Send(message);
            }
            catch (Exception)
            {
                return RedirectToAction("ThankForPurchase", "Home");
            }

            return RedirectToAction("ThankForPurchase", "Home");
        }

        [HttpPost]
        public ActionResult RandomGame(BuyModel model)
        {
            Game game = null;
            using (var context = new ApplicationContext())
            {
                var user = context.Users.FirstOrDefault(u => u._Login == model.UserName);
                var rnd = new Random();
                do
                {
                    var id = rnd.Next(1, context.Games.Count());
                    game = context.Games.FirstOrDefault(g => g._Id == id);
                } while (game == null);

                if (!user._Games.Contains(game))
                {
                    user._Games.Add(game);
                    context.SaveChanges();
                }
            }

            var message = new MailMessage();
            message.To.Add(model.Email);
            message.Subject = "MongoKeys.com";
            var key = GenerateKey();
            message.Body = "Спасибо за покупку " + model.UserName + ", вы выйграли " + game._Name + ", вот ваш ключ " +
                           key + ".";
            var smtp = new SmtpClient();
            try
            {
                smtp.Send(message);
            }
            catch (Exception)
            {
                return RedirectToAction("ThankForPurchase", "Home");
            }

            return RedirectToAction("ThankForPurchase", "Home");
        }

        [HttpGet]
        public ActionResult TakeGame(int id)
        {
            using (var context = new ApplicationContext())
            {
                var currentUser = context.Users.FirstOrDefault(u => u._Login == User.Identity.Name);
                var game = context.Games.FirstOrDefault(g => g._Id == id);
                if (currentUser._Games.Contains(game)) return RedirectToAction("GameExists", "Home");

                currentUser._Games.Add(game);
                context.SaveChanges();
                return RedirectToAction("Index", "User", new {id = currentUser._Id});
            }
        }

        [HttpGet]
        public ActionResult GameExists()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ThankForPurchase()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ViewGame(int id)
        {
            ViewBag._Id = id;
            return View();
        }

        public string GenerateKey()
        {
            var key_chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var key = "";
            var rnd = new Random();
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++) key += key_chars[rnd.Next(0, key_chars.Length)];
                key += "-";
            }

            for (var j = 0; j < 4; j++) key += key_chars[rnd.Next(0, key_chars.Length)];

            return key;
        }
    }
}