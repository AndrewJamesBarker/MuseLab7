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
    public class CollabController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static CollabController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44350/api/");
        }
        // GET: Collab/List
        public ActionResult List()
        {
            //communicate with the collab data api to retrieve a list of collabs
            //curl https://localhost:44350/api/collabdata/listcollabs

            string url = "collabdata/listcollabs";
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            IEnumerable<CollabDto> Collabs = response.Content.ReadAsAsync<IEnumerable<CollabDto>>().Result;
            Debug.WriteLine("Number of Collabs recieved ");
            Debug.WriteLine(Collabs.Count());

            return View(Collabs);
        }

        // GET: Collab/Details/5
        public ActionResult Details(int id)
        {
            //communicate with the collab data api to retrieve one collab
            //curl https://localhost:44350/api/collabdata/findcollab/{id}

  
            string url = "collabdata/findcollab/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DetailsCollab ViewModel = new DetailsCollab();

            CollabDto selectedcollab = response.Content.ReadAsAsync<CollabDto>().Result;
            ViewModel.SelectedCollab = selectedcollab;


            //show associated CoCreators with this Collab


            return View(ViewModel);
        }


        public ActionResult Error()
        {
            return View("Error");
        }

        // GET: Collab/New
        public ActionResult New()
        {

            CreateCollab ViewModel = new CreateCollab();

            string url = "ideadata/listideas/";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<IdeaDto> IdeaOptions = response.Content.ReadAsAsync<IEnumerable<IdeaDto>>().Result;

            ViewModel.IdeaOptions = IdeaOptions;

            // and cocreators

            url = "cocreatordata/listcocreators/";
            response = client.GetAsync(url).Result;
            IEnumerable<CoCreatorDto> CoCreatorOptions = response.Content.ReadAsAsync<IEnumerable<CoCreatorDto>>().Result;

            ViewModel.CoCreatorOptions = CoCreatorOptions;


            return View(ViewModel);
        }

        // POST: Collab/Create
        [HttpPost]
        public ActionResult Create(Collab collab)
        {
            // create new instance of a collab
            string url = "collabdata/addcollab";

            
            string jsonpayload = jss.Serialize(collab);
            Debug.WriteLine("the json payload is: ");
            Debug.WriteLine(jsonpayload);


            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
           

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

            //return RedirectToAction("List");
        }

        // GET: Collab/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateCollab ViewModel = new UpdateCollab();

            //the existing collab info

            string url = "collabdata/findCollab/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CollabDto selectedcollab = response.Content.ReadAsAsync<CollabDto>().Result;
            ViewModel.SelectedCollab = selectedcollab;

            // also like to include all possible ideas to build a collab on

            url = "ideadata/listideas/";
            response = client.GetAsync(url).Result;
            IEnumerable<IdeaDto> IdeaOptions = response.Content.ReadAsAsync<IEnumerable<IdeaDto>>().Result;

            ViewModel.IdeaOptions = IdeaOptions;

            // and cocreators

            url = "cocreatordata/listcocreators/";
            response = client.GetAsync(url).Result;
            IEnumerable<CoCreatorDto> CoCreatorOptions = response.Content.ReadAsAsync<IEnumerable<CoCreatorDto>>().Result;

            ViewModel.CoCreatorOptions = CoCreatorOptions;


            return View(ViewModel);
        }

        // POST: Collab/Update/5
        [HttpPost]
        public ActionResult Update(int id, Collab collab)
        {
            string url = "collabdata/updatecollab/"+id;
            string jsonpayload = jss.Serialize(collab);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: Collab/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "collabdata/findCollab/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CollabDto selectedCollab = response.Content.ReadAsAsync<CollabDto>().Result;
            return View(selectedCollab);
        }

        // POST: Collab/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "collabdata/deletecollab/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if(response.IsSuccessStatusCode)
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
