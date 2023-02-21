using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using MuseLab7.Models;
using MuseLab7.Models.ViewModels;
using System.Web.Script.Serialization;

namespace MuseLab7.Controllers
{
    public class CreatorController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static CreatorController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44350/api/");
        }


        // GET: Creator/List

        public ActionResult List()
        {
            //communicate with the collab data api to retrieve a list of collabs
            //curl https://localhost:44350/api/cocreatordata/listcocreators

            string url = "creatordata/listcreators";
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            IEnumerable<CreatorDto> Creators = response.Content.ReadAsAsync<IEnumerable<CreatorDto>>().Result;


            return View(Creators);
        }

        


        // GET: Creator/Details/5
        public ActionResult Details(int id)
        {
            
            DetailsCreator ViewModel = new DetailsCreator();

            string url = "creatordata/findcreator/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            CreatorDto SelectedCreator = response.Content.ReadAsAsync<CreatorDto>().Result;
            
            ViewModel.SelectedCreator = SelectedCreator;

            //showcase information about ideas related to this creator
            //send a request to gather information about ideas related to a particular creator ID
            url = "ideadata/listideasforcreator/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<IdeaDto> RelatedIdeas = response.Content.ReadAsAsync<IEnumerable<IdeaDto>>().Result;

            ViewModel.RelatedIdeas = RelatedIdeas;


            return View(ViewModel);
        }


        // Error response
        public ActionResult Error()
        {
            return View("Error");
        }

        //GET: Creator/New
        public ActionResult New()
        {
            return View();

        }


        // POST: Creator/Create
        [HttpPost]
        public ActionResult Create(Creator creator)
        {

            // create new instance of a creator
            string url = "creatordata/addcreator";

            JavaScriptSerializer jss = new JavaScriptSerializer();


            string jsonpayload = jss.Serialize(creator);
            Debug.WriteLine("the json payload is: ");
            Debug.WriteLine(jsonpayload);


            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
          

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        // GET: Creator/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "creatordata/findcreator/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CreatorDto selectedCreator = response.Content.ReadAsAsync<CreatorDto>().Result;
            return View(selectedCreator);
        }

        // POST: Creator/Edit/5
        [HttpPost]
        public ActionResult Update(int id, Creator creator)
        {
            string url = "creatordata/updatecreator/" + id;
            string jsonpayload = jss.Serialize(creator);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: Creator/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "creatordata/findCreator/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CreatorDto selectedCreator = response.Content.ReadAsAsync<CreatorDto>().Result;
            return View(selectedCreator);
        }


        // POST: Creator/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "creatordata/deletecreator/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
