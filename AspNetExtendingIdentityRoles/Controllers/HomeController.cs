using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AspNetExtendingIdentityRoles.Models;
using Microsoft.AspNet.Identity;
using PagedList;

namespace AspNetExtendingIdentityRoles.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
       
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            //Show all messages from every user by userId
            var userId = User.Identity.GetUserId();
            var messages = db.Messages.Include(m => m.Receiver).Where(r => r.ReceiverID == userId).Union(db.Messages.Include(m => m.Sender).Where(s => s.SenderID == userId));
            return View(messages.ToList());
        }

        public ActionResult Intro()
        {
            return View();
        }

    }
}