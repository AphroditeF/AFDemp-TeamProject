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
    public class MessagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: All Messages for all users
        public ActionResult AllMessages(string searchString, string SortOrder, string currentFilter, int? page)
        { 
            var Messages = db.Messages.Include(m => m.Receiver).Include(m => m.Sender);

            var messages = SortingFilteringPaging(searchString, SortOrder, currentFilter, page, Messages);

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(messages.ToPagedList(pageNumber, pageSize));

        }

        // GET: All Messages for every user  
        public ActionResult Index(string searchString, string SortOrder, string currentFilter, int? page)
        {
            var userId = User.Identity.GetUserId();
           
            var Messages = db.Messages.Include(m => m.Receiver).Where(r => r.ReceiverID == userId).Union(db.Messages.Include(m => m.Sender).Where(s => s.SenderID == userId));

            var messages=SortingFilteringPaging(searchString, SortOrder, currentFilter, page, Messages);

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(messages.ToPagedList(pageNumber, pageSize));

        }

        // GET: Messages by  Receiver
        public ActionResult IndexReceiver(string searchString, string SortOrder, string currentFilter, int? page)
        {   
            var userId = User.Identity.GetUserId();
           
            var Messages = db.Messages.Include(m => m.Receiver).Where(r=> r.ReceiverID == userId);

            var messages = SortingFilteringPaging(searchString, SortOrder, currentFilter, page, Messages);

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(messages.ToPagedList(pageNumber, pageSize));

           
        }

        // GET: Messages by Sender
        public ActionResult IndexSender(string searchString, string SortOrder, string currentFilter, int? page)
        {
 
            var userId = User.Identity.GetUserId();
            var Messages = db.Messages.Include(m => m.Sender).Where(s=>s.SenderID==userId);

            var messages = SortingFilteringPaging(searchString, SortOrder, currentFilter, page, Messages);

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(messages.ToPagedList(pageNumber, pageSize));

       
        }

        // GET: Messages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            message.IsRead = true;
            db.SaveChanges();
            
            return View(message);
        }

        // GET: Messages/Create
        public ActionResult Create()
        {
            ViewBag.ReceiverID = new SelectList(db.Users, "Id", "UserName");
            ViewBag.SenderID = new SelectList(db.Users, "Id", "UserName");

            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            //user.Roles.Add("User");
            ViewBag.username = user.UserName;
            

            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MessageId,DeletedBySender,DeletedByReceiver,MessageBody,IsRead,Location,ReceiverID,SenderID")] Message message)
        {
            if (ModelState.IsValid)
            {
                message.DateSent = DateTime.Now;

                var userId = User.Identity.GetUserId();
                message.SenderID=userId;
                

                db.Messages.Add(message);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ReceiverID = new SelectList(db.Users, "Id", "UserName", message.ReceiverID);
            ViewBag.SenderID = new SelectList(db.Users, "Id", "UserName", message.SenderID);

            return View(message);
        }

        // GET: Messages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            ViewBag.ReceiverID = new SelectList(db.Users, "Id", "UserName", message.ReceiverID);
            ViewBag.SenderID = new SelectList(db.Users, "Id", "UserName", message.SenderID);
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MessageId,DateSent,DeletedBySender,DeletedByReceiver,MessageBody,IsRead,Location,ReceiverID,SenderID")] Message message)
        {
            if (ModelState.IsValid)
            {
                db.Entry(message).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ReceiverID = new SelectList(db.Users, "Id", "UserName", message.ReceiverID);
            ViewBag.SenderID = new SelectList(db.Users, "Id", "UserName", message.SenderID);
            return View(message);
        }

        // GET: Messages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Message message = db.Messages.Find(id);
            //db.Messages.Remove(message);
            var userId = User.Identity.GetUserId();
            if (userId == message.ReceiverID)
            {
                message.DeletedByReceiver = true;
            }
            else if(userId == message.SenderID)
            {
                message.DeletedBySender = true;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public List<Message> SortingFilteringPaging(string searchString, string SortOrder, string currentFilter, int? page,IQueryable<Message> Messages)
        {

            ViewBag.CurrentSort = SortOrder;
            //Sorting
            ViewBag.DateSent = String.IsNullOrWhiteSpace(SortOrder) ? "DateSent" : "";
            ViewBag.Sender = SortOrder == "Sender" ? "Sender_desc" : "Sender";
            ViewBag.Receiver = SortOrder == "Receiver" ? "Receiver_desc" : "Receiver";
            ViewBag.MessageBody = SortOrder == "MessageBody" ? "MessageBody_desc" : "MessageBody";
            ViewBag.DeletedBySender = SortOrder == "DeletedBySender" ? "DeletedBySender_desc" : "DeletedBySender";
            ViewBag.DeletedByReceiver = SortOrder == "DeletedByReceiver" ? "DeletedByReceiver_desc" : "DeletedByReceiver";
            ViewBag.IsRead = SortOrder == "IsRead" ? "IsRead_desc" : "IsRead";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            ////Show all messages from every user by userId
            //var userId = User.Identity.GetUserId();
            //var messages = db.Messages.Include(m => m.Receiver).Where(r => r.ReceiverID == userId).Union(db.Messages.Include(m => m.Sender).Where(s => s.SenderID == userId));
            
            var messages = Messages;

            //Sorting
            switch (SortOrder)
            {
                case "Sender":
                    messages = messages.OrderBy(m => m.Sender.UserName);
                    break;
                case "Sender_desc":
                    messages = messages.OrderByDescending(m => m.Sender.UserName);
                    break;
                case "Receiver":
                    messages = messages.OrderBy(m => m.Receiver.UserName);
                    break;
                case "Receiver_desc":
                    messages = messages.OrderByDescending(m => m.Receiver.UserName);
                    break;
                case "DataSent":
                    messages = messages.OrderBy(m => m.DateSent);
                    break;
                case "MessageBody":
                    messages = messages.OrderBy(m => m.MessageBody);
                    break;
                case "MessageBody_desc":
                    messages = messages.OrderByDescending(m => m.MessageBody);
                    break;
                case "DeletedBySender":
                    messages = messages.OrderBy(m => m.DeletedBySender);
                    break;
                case "DeletedBySender_desc":
                    messages = messages.OrderByDescending(m => m.DeletedBySender);
                    break;
                case "DeletedByReceiver":
                    messages = messages.OrderBy(m => m.DeletedByReceiver);
                    break;
                case "DeletedByReceiver_desc":
                    messages = messages.OrderByDescending(m => m.DeletedByReceiver);
                    break;
                case "IsRead":
                    messages = messages.OrderBy(m => m.IsRead);
                    break;
                case "IsRead_desc":
                    messages = messages.OrderByDescending(m => m.IsRead);
                    break;
                default:
                    messages = messages.OrderByDescending(m => m.DateSent);
                    break;
            }



            //Search Messages by sender name (Filter)
            if (!string.IsNullOrEmpty(searchString))
            {
                messages = messages.Include(s => s.Sender).Where(u => u.Sender.UserName == searchString);
            }

            return messages.ToList();
            //int pageSize = 5;
            //int pageNumber = (page ?? 1);
            //return View(messages.ToList().ToPagedList(pageNumber, pageSize));

        }

        //public JsonResult AjaxLocation(string location)
        //{
        //    // ........... do whatever you want here. I am simply returning the value.
        //    return Json(location, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public ActionResult SaveJSON(string data)
        //{
        //    return Json(data);
        //}

        //[HttpPost]
        //public ActionResult MyajaxCall(string order)
        //{
        //    return Json("OK");
        //}

        public JsonResult AjaxTest(Message position)
        {
            // do whatever you want here. I am simply returning the value.
            //return Json("You posted: " + position.Location + " items.", JsonRequestBehavior.AllowGet);
            return Json("You posted: " + position.Location + " items.", JsonRequestBehavior.AllowGet);

        }



    }
}
