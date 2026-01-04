using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace WWeb.UI.WebformMVC.Controllers
{
    public class MonitoriaController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}