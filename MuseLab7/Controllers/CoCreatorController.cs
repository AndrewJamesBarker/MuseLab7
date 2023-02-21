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
    public class CoCreatorController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static CoCreatorController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44350/api/");
        }


        // GET: CoCreator/List
        public ActionResult List()
        {
            //communicate with the collab data api to retrieve a list of collabs
            //curl https://localhost:44350/api/cocreatordata/listcocreators

            string url = "cocreatordata/listcocreators";
            HttpResponseMessage response = client.GetAsync(url).Result;

            
            Debug.WriteLine(response.StatusCode);

            IEnumerable<CoCreatorDto> CoCreators = response.Content.ReadAsAsync<IEnumerable<CoCreatorDto>>().Result;
          

            return View(CoCreators);
        }

        // GET: CoCreator/Details/5
        public ActionResult Details(int id)
        {
            DetailsCoCreator ViewModel = new DetailsCoCreator();

            string url = "cocreatordata/findcocreator/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            CoCreatorDto SelectedCoCreator = response.Content.ReadAsAsync<CoCreatorDto>().Result;

            ViewModel.SelectedCoCreator = SelectedCoCreator;

            //showcase information about ideas related to this creator
            //send a request to gather information about ideas related to a particular creator ID
            url = "collabdata/listcollabsforcocreator/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<CollabDto> RelatedCollabs = response.Content.ReadAsAsync<IEnumerable<CollabDto>>().Result;

            ViewModel.RelatedCollabs = RelatedCollabs;


            return View(ViewModel);
        }


        // Error response
        public ActionResult Error()
        {
            return View("Error");
        }

        //GET: CoCreator/New
        public ActionResult New()
        {
            return View();

        }


        // POST: CoCreator/Create
        [HttpPost]
        public ActionResult Create(CoCreator cocreator)
        {

            // create new instance of a cocreator
            string url = "cocreatordata/addcocreator";

            JavaScriptSerializer jss = new JavaScriptSerializer();


            string jsonpayload = jss.Serialize(cocreator);
            Debug.WriteLine("the json payload is: ");
            Debug.WriteLine(jsonpayload);


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

        // GET: CoCreator/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "cocreatordata/findcocreator/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CoCreatorDto selectedCoCreator = response.Content.ReadAsAsync<CoCreatorDto>().Result;
            return View(selectedCoCreator);
        }

        // POST: CoCreator/Edit/5
        [HttpPost]
        public ActionResult Update(int id, CoCreator cocreator)
        {

            string url = "cocreatordata/updatecocreator/" + id;
            string jsonpayload = jss.Serialize(cocreator);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
           
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                Debug.WriteLine("You made it this far... ");
                return RedirectToAction("Error");
            }
        }
        // GET: CoCreator/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "cocreatordata/findCoCreator/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CoCreatorDto selectedCoCreator = response.Content.ReadAsAsync<CoCreatorDto>().Result;
            return View(selectedCoCreator);
        }


        // POST: CoCreator/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "cocreatordata/deletecocreator/" + id;
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
