using CarBoyWebservice.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarBoyWebservice.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Tracker(string hash)
        {
            hash = Base64UrlEncoder.Decode(hash);

            string[] parts = ClassCollection.Method.Decrypt(hash).Split('|');

            var db = new DataAccessDataContext();

            long geoTrackID = long.Parse(parts[0]);

            long serviceOrderID = long.Parse(parts[1]);

            var serviceOrder = db.ServiceOrderTbls.Single(c => c.ID == serviceOrderID);

            if (serviceOrder.state != (byte)ClassCollection.ServiceOrderState.MoveToCustomer 
                
                && serviceOrder.state != (byte)ClassCollection.ServiceOrderState.Proccessing

                )
            {
                return HttpNotFound();
            }

            var geoTrack = db.UserGeoTrackTbls.Single(c => c.ID == geoTrackID);


            var image = string.IsNullOrEmpty(geoTrack.UserTbl.image) ? "" : geoTrack.UserTbl.image;

            if (image != "")
            {
                image = ConfigurationManager.AppSettings["url"] + image;

            }
            return View(
                new TrackerModel()
                {
                    id = geoTrackID,
                    fullName = geoTrack.UserTbl.name + " " + geoTrack.UserTbl.family,
                    tel = geoTrack.UserTbl.mobile,
                    sp = serviceOrder.PackageTbl.name,
                    image = image,
                    hash = Base64UrlEncoder.Encode(hash),
                    srclat = geoTrack.latitude.ToString(),
                    srclng = geoTrack.longitude.ToString(),
                    deslat = serviceOrder.latitude.ToString(),
                    deslng = serviceOrder.longitude.ToString()
                } as object);
        }


        public JsonResult Geo(string hash)
        {
            hash = Base64UrlEncoder.Decode(hash);

            string[] parts = ClassCollection.Method.Decrypt(hash).Split('|');

            var db = new DataAccessDataContext();

            long geoTrackID = long.Parse(parts[0]);

            long serviceOrderID = long.Parse(parts[1]);

            var serviceOrder = db.ServiceOrderTbls.Single(c => c.ID == serviceOrderID);


            if (serviceOrder.state != (byte)ClassCollection.ServiceOrderState.MoveToCustomer

                && serviceOrder.state != (byte)ClassCollection.ServiceOrderState.Proccessing

                )
            {
                return Json("");
            }

            var geoTrack = db.UserGeoTrackTbls.Single(c => c.ID == geoTrackID);

            return Json(new { lat = geoTrack.latitude, lng = geoTrack.longitude });
        }

    }
}