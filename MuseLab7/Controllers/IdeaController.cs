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
    public class IdeaController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static IdeaController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44350/api/");
        }
        // GET: Idea/List
        public ActionResult List()
        {

            string url = "ideadata/listideas";
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            IEnumerable<IdeaDto> Ideas = response.Content.ReadAsAsync<IEnumerable<IdeaDto>>().Result;
         

            return View(Ideas);
        }

        // GET: Idea/Details/5
        public ActionResult Details(int id)
        {

            string url = "ideadata/findidea/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DetailsIdea ViewModel = new DetailsIdea();

            IdeaDto selectedidea = response.Content.ReadAsAsync<IdeaDto>().Result;
            ViewModel.SelectedIdea = selectedidea;

            return View(ViewModel);
        }

            // Error page 
            public ActionResult Error()
        {
            return View("Error");
        }

        // GET: Idea/New
        public ActionResult New()
        {

            CreateIdea ViewModel = new CreateIdea();

        
            // and creators

            string url = "creatordata/listcreators/";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<CreatorDto> CreatorOptions = response.Content.ReadAsAsync<IEnumerable<CreatorDto>>().Result;

            ViewModel.CreatorOptions = CreatorOptions;

            return View(ViewModel);
        }

        // POST: Idea/Create
        [HttpPost]
        public ActionResult Create(Idea idea)
        {
            // create new instance of a idea
            string url = "ideadata/addidea";


            string jsonpayload = jss.Serialize(idea);
          

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            client.PostAsync(url, content);

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
        // GET: Idea/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateIdea ViewModel = new UpdateIdea();

            //the existing collab info

            string url = "ideadata/findIdea/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            IdeaDto selectedidea = response.Content.ReadAsAsync<IdeaDto>().Result;
            ViewModel.SelectedIdea = selectedidea;

            // and creators

            url = "creatordata/listcreators/";
            response = client.GetAsync(url).Result;
            IEnumerable<CreatorDto> CreatorOptions = response.Content.ReadAsAsync<IEnumerable<CreatorDto>>().Result;

            ViewModel.CreatorOptions = CreatorOptions;


            return View(ViewModel);
        }

        // POST: Idea/Edit/5
           
        [HttpPost]
        public ActionResult Update(int id, Idea idea)
        {
            string url = "ideadata/updateidea/" + id;
            string jsonpayload = jss.Serialize(idea);
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

        // GET: Idea/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "ideadata/findidea/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            IdeaDto selectedidea = response.Content.ReadAsAsync<IdeaDto>().Result;
            return View(selectedidea);
        }


        // POST: Idea/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "ideadata/deleteidea/" + id;
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
