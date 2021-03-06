﻿using Enterprise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Dropbox.Api;
using Dropbox.Api.Files;

namespace Enterprise.Controllers
{
    [Authorize(Roles = "RegisteredUser")]

    public class ItemTypeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ItemType
        public ActionResult Index()
        {
            var itemTypes = db.ItemTypes.Include(p => p.Category);
            return View(itemTypes.ToList());
        }

        //GET: Properties/Create
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemType itemType = db.ItemTypes.Find(id);
            if(itemType == null)
            {
                return HttpNotFound();
            }
            return View(itemType);
        }

        public ActionResult Create() {
            ViewBag.CategoryId = new SelectList(db.Categories,"Id","Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CategoryId,Name")] HttpPostedFileBase Image , ItemType itemType)
        {

            if (ModelState.IsValid)
            {
                if (Image == null)
                {
                    ModelState.AddModelError("Image", "Image cannot be empty");
                    ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", itemType.CategoryId);
                    return View(itemType);
                }
                else
                



                    if (!db.ItemTypes.Any(i => i.Name == itemType.Name))
                    {
                        string accessToken = "rLO2hkO01AAAAAAAAABGip4dgqIUMoDGUcO0qvfukgANmcBQTL7_B2LvRb0hRf2";
                        using (DropboxClient client =
                              new DropboxClient(accessToken, new DropboxClientConfig(ApplicationName)))
                        {
                            string[] spitInputFileName = Image.FileName.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                            string fileNameAndExtension = spitInputFileName[spitInputFileName.Length - 1];

                            string[] fileNameAndExtensionSplit = fileNameAndExtension.Split('.');
                            string originalFileName = fileNameAndExtensionSplit[0];
                            string originalExtension = fileNameAndExtensionSplit[1];


                            String fileName = "@/Images/" + originalFileName + Guid.NewGuid().ToString().Replace("-", "") + "." + originalExtension;
                            var updated = client.Files.UploadAsync(
                                  fileName,
                                  mode: WriteMode.Overwrite.Overwrite.Instance,
                                 body: Image.InputStream).Result;

                            var result = client.Sharing.CreateSharedLinkWithSettingsAsync(fileName).Result;
                            itemType.ImageUrl = result.Name;
                            itemType.Image = result.Url.Replace("?dl=0", "?raw=1");


                        }
                        db.ItemTypes.Add(itemType);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                
            }

            

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", itemType.CategoryId);
            return View(itemType);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemType itemType = db.ItemTypes.Find(id);
            if (itemType == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", itemType.CategoryId);
            return View(itemType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CategoryId,Name")] ItemType itemType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(itemType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", itemType.CategoryId);
            return View(itemType);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemType itemType = db.ItemTypes.Find(id);
            if (itemType == null)
            {
                return HttpNotFound();
            }
            return View(itemType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ItemType itemType = db.ItemTypes.Find(id);
            db.ItemTypes.Remove(itemType);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult UploadImage()
        {
            return View();
        }


        static string ApplicationName = "Enterprise";

        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase file )
        {
            ItemType itemType = new ItemType(); ;
            string accessToken = "****";
            using (DropboxClient client =
                 new DropboxClient(accessToken, new DropboxClientConfig(ApplicationName)))
            {
                string[] spitInputFileName = file.FileName.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                string fileNameAndExtension = spitInputFileName[spitInputFileName.Length - 1];

                string[] fileNameAndExtensionSplit = fileNameAndExtension.Split('.');
                string originalFileName = fileNameAndExtensionSplit[0];
                string originalExtension = fileNameAndExtensionSplit[1];


                String fileName = "@/Images/"+originalFileName+Guid.NewGuid().ToString().Replace("-","") +"."+ originalExtension;
                var updated = client.Files.UploadAsync(
                      fileName,
                      mode: WriteMode.Overwrite.Overwrite.Instance,
                      body: file.InputStream).Result;

                var result = client.Sharing.CreateSharedLinkWithSettingsAsync(fileName).Result;
                itemType.Image = result.Url.Replace("?dl=0","?raw=1");


            }
            db.ItemTypes.Add(itemType);
            db.SaveChanges();
            return RedirectToAction("Index" );

        }




        public ActionResult ViewImage(String imageUrl)
        {
            throw new NotImplementedException();
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



    }
}