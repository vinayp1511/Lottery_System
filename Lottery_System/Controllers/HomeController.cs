using Lottery_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lottery_System.Controllers
{
    public class HomeController : Controller
    {
        Lottery_System.API.API _api = new Lottery_System.API.API();
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("SessionTimeout", "Account");
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();

            //if (Request.IsAuthenticated)
            //{
            //    return View();
            //}
            //else
            //{
            //    return RedirectToAction("SessionTimeout", "Account");
            //}
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            if (Request.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("SessionTimeout", "Account");
            }
        }

        public ActionResult Dashboard()
        {
            if (Request.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("SessionTimeout", "Account");
            }
        }

        [HttpPost]
        public JsonResult SaveBet(List<Bet> bets)
        {
            bool IsValid = false;

            if (Request.IsAuthenticated)
            {
                try
                {
                    IsValid = _api.SaveBet(bets);
                }
                catch(Exception ex)
                {
                    IsValid = false;
                }
                return Json(new { IsValid = IsValid},JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { IsValid = IsValid }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetWinner()
        {
            int Number = 0;
            decimal Amount = 0;
            Bet bet = new Bet();

            try
            {
                bet = _api.GetWinner();

                if(_api.UserID == bet.UserID)
                {
                    Number = bet.BetNumber;
                    Amount = bet.TotalBetAmount;
                }
                else
                {
                    Number = 0;
                    Amount = 0;
                }
            }   
            catch(Exception ex)
            {
                Number = 0;
                Amount = 0;
            }

            return Json(new { Number = Number, Amount = Amount }, JsonRequestBehavior.AllowGet);
        }
    }
}