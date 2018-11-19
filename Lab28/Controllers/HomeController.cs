using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using Newtonsoft.Json.Linq;
using System.IO;
using Lab28.Models;


namespace Lab28.Controllers
{
    public class HomeController : Controller
    {
        public static string deck;
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
        public ActionResult DisplayCards()
        {
            if(deck == null)
            {
                return RedirectToAction("CreateNewDeck");
            }
            
            // should get 5 cards from the api and sends them to the view.
            return View();
        }
        public ActionResult CreateNewDeck()
        {
            HttpWebRequest request = WebRequest.CreateHttp("https://deckofcardsapi.com/api/deck/new/shuffle/?deck_count=1");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader dr = new StreamReader(response.GetResponseStream());
            string data = dr.ReadToEnd();
            dr.Close();
            deck = JObject.Parse(data)["deck_id"].ToString();
            return RedirectToAction("DisplayCards");
        }
    }
}