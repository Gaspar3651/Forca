﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Forca.Controllers
{
    public class HomeController : Controller
    {
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

            return View();
        }

        public void img()
        {
            WebImage wbImage = new WebImage("~/Views/Img/Forca-01.png");
            wbImage.Resize(350, 386);
            wbImage.FileName = "img.jpg";
            wbImage.Write();
        }
    }
}