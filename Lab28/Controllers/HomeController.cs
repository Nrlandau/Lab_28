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
            HttpWebRequest request = WebRequest.CreateHttp($"https://deckofcardsapi.com/api/deck/{deck}/draw/?count=5");
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:64.0) Gecko/20100101 Firefox/64.0";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader dr = new StreamReader(response.GetResponseStream());
            string data = dr.ReadToEnd();
            dr.Close();
            JObject cardsData = JObject.Parse(data);
            List<JToken> cards = cardsData["cards"].ToList();
            List<Card> hand = new List<Card>();
            foreach(JToken c in cards)
            {
                Card temp = new Card();
                temp.Suit = c["suit"].ToString();
                temp.Value = c["value"].ToString();
                temp.ImageURL = c["image"].ToString();
                hand.Add(temp);
            }
            ViewBag.CardsLeft = cardsData["remaining"].ToString();
            return View(hand);
        }
        public ActionResult CreateNewDeck()
        {
            HttpWebRequest request = WebRequest.CreateHttp("https://deckofcardsapi.com/api/deck/new/shuffle/?deck_count=1");
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:64.0) Gecko/20100101 Firefox/64.0";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader dr = new StreamReader(response.GetResponseStream());
            string data = dr.ReadToEnd();
            dr.Close();
            deck = JObject.Parse(data)["deck_id"].ToString();
            return RedirectToAction("DisplayCards");
        }
    }
}