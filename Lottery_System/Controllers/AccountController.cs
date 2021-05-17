using Lottery_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Lottery_System.Controllers
{
    public class AccountController : Controller
    {
        Lottery_System.API.API _api = new Lottery_System.API.API();
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


        public ActionResult SessionTimeout()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login login)
        {
            bool IsValid = false;

            IsValid = _api.Login(login);

            if (IsValid)
            {
                FormsAuthentication.SetAuthCookie(login.Email, true);
                return Json(new { IsValid = IsValid, redirectToUrl = Url.Action("Dashboard", "Home") }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { IsValid = IsValid }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Register(Users user)
        {
            bool IsValid = false;

            IsValid = _api.RegisterUser(user);

            return Json(new { IsValid = IsValid }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult RegistrerUser()
        {
            List<Country> lstCountry = new List<Country>();

            lstCountry = _api.GetCountryList();

            ViewBag.Country = lstCountry;
            return View();
        }

        [HttpGet]
        public JsonResult GetState(long Countryid)
        {
            List<State> lstState = new List<State>();

            lstState = _api.GetStateList(Countryid);
            return Json(new { lstState = lstState }, JsonRequestBehavior.AllowGet);
        }
    }
}