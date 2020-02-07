using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System.Web.Mvc;
using Rockola.ServiceReference1;
using System.Net.Http;
using Newtonsoft.Json;

namespace Rockola.Controllers
{
    public class HomeController : Controller
    {
        List<string> PlayList = new List<string>();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult SearchVideo(string Keyword)
        {
            /*Consuming a WCF Service.

             * List<Videos> vidios = new List<Videos>();
            ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
            var v = client.GetListYoutube(Keyword);
            foreach (var item in v)
            {
                vidios.Add(new Videos { ID=item.ID, tittle = item.tittle });
            }
            return PartialView("Search", vidios);*/


            //Consumiendo API
            List<Videos> lisvideos = new List<Videos>();
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:57657/");
            var response =  client.GetAsync("api/MTT/GetVideo?keyword="+Keyword);
            response.Wait();
            var result = response.Result;
            var readresult = result.Content.ReadAsStringAsync().Result;
            var resultadoFinal = JsonConvert.DeserializeObject<List<Videos>>(readresult);
            //foreach (var item in resultadoFinal)
            //{
            //    lisvideos.Add(new Videos { ID = item.ID, tittle = item.tittle });
            //}
            return PartialView("Search", resultadoFinal);
        }
        [HttpGet]
        public ActionResult AddPlayList(string idVideo)
        {
            //var arreglo = idVideo.Split('_');
            //var id = arreglo[0].Trim(' ');
            //var titulo = arreglo[1];
            Declare();
            List<string> auxList = (List<string>)Session["Playlist"];
            auxList.Add(idVideo);
            Session["Playlist"] = auxList;
            return PartialView("AddPlay", auxList);
        }
        [HttpGet]
        public ActionResult reproduceVideo(string idVideo)
        {
            return PartialView("reproduceVideo", idVideo);
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
        public void Declare()
        {
            List<string> PLAYLISTVIDEOS = new List<string>();
            if (Session["Playlist"] == null)
            {
                Session["Playlist"] = PLAYLISTVIDEOS;

            }
        }
    }
}