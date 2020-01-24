using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication44.Models;
using ActionResult = System.Web.Mvc.ActionResult;
using HttpGetAttribute = System.Web.Mvc.HttpGetAttribute;
using HttpPostAttribute = System.Web.Mvc.HttpPostAttribute;

namespace WebApplication44.Controllers
{
    public class OffersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private IHttpContextAccessor _httpContextAccessor;

        // GET: Offers
        public ActionResult Index()
        {
            var offers = db.Offers.Include(o => o.Category).Include(o => o.User);
            return View(offers.ToList());
        }

        // GET: Offers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = db.Offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            return View(offer);
        }

        // GET: Offers/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name");
            //ViewBag.UserID = new SelectList(db.ApplicationUsers, "Id", "Email");
            return View();
        }

        // POST: Offers/Create
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public System.Web.Mvc.ActionResult Create([System.Web.Mvc.Bind(Include = "OfferId,UserID,LocationID,CategoryID,Title,Descritpion,Price,Address,Phone,Data")] Offer offer)
        {
            if (ModelState.IsValid)
            {
                offer.UserID = User.Identity.GetUserId();
                db.Offers.Add(offer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", offer.CategoryID);
           // ViewBag.UserID = new SelectList(db.ApplicationUsers, "Id", "Email", offer.UserID);
            return View(offer);
        }

        // GET: Offers/Edit/5
        public System.Web.Mvc.ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = db.Offers.Find(id);
            if(offer.UserID != User.Identity.GetUserId()) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (offer == null)
            {
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", offer.CategoryID);
      //      ViewBag.UserID = new SelectList(db.ApplicationUsers, "Id", "Email", offer.UserID);
            return View(offer);
        }

        // POST: Offers/Edit/5
        // Aby zapewnić ochronę przed atakami polegającymi na przesyłaniu dodatkowych danych, włącz określone właściwości, z którymi chcesz utworzyć powiązania.
        // Aby uzyskać więcej szczegółów, zobacz https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([System.Web.Mvc.Bind(Include = "OfferId,UserID,LocationID,CategoryID,Title,Descritpion,Price,Address,Phone,Data")] Offer offer)
        {
            if (offer.UserID != User.Identity.GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                db.Entry(offer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "Name", offer.CategoryID);
          //  ViewBag.UserID = new SelectList(db.ApplicationUsers, "Id", "Email", offer.UserID);
            return View(offer);
        }

        // GET: Offers/Delete/5
        public ActionResult Delete(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = db.Offers.Find(id);
            if (offer.UserID != User.Identity.GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (offer == null)
            {
                return HttpNotFound();
            }
            return View(offer);
        }

        // POST: Offers/Delete/5
        [HttpPost, System.Web.Mvc.ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
           
            Offer offer = db.Offers.Find(id);
            if (offer.UserID != User.Identity.GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            db.Offers.Remove(offer);
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

        [ResponseCache(Duration = 1200)]
        [HttpGet]
        public ActionResult Rss()
        {
            var feed = new SyndicationFeed("Title", "Description", new Uri(GetBaseUrl()), "RSSUrl", DateTime.Now); //TODO adresy 

            feed.Copyright = new TextSyndicationContent($"{DateTime.Now.Year} Konrad Bałdyga & Jakub Chmielewski");
            var items = new List<SyndicationItem>();
            var postings = db.Offers;
            foreach (var item in postings)
            {
                string BlogURL = $"{GetBaseUrl()}/Offers/Details/{item.OfferId}";
                var title = "Rss feed PROJEKT .NET MVC";
                var description = "Added new offer , Check it out!";
                items.Add(new SyndicationItem(title, description, new Uri(BlogURL), "pb - rss feed", item.Data));
            }

            feed.Items = items;
            var settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                NewLineHandling = NewLineHandling.Entitize,
                NewLineOnAttributes = true,
                Indent = true
            };
            using (var stream = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(stream, settings))
                {
                    var rssFormatter = new Rss20FeedFormatter(feed, false);
                    rssFormatter.WriteTo(xmlWriter);
                    xmlWriter.Flush();
                }
                return File(stream.ToArray(), "application/rss+xml; charset=utf-8");
            }
        }

        public string GetBaseUrl()
        {
            return HttpContext.Request.Url.Authority;
        }
    }
}
