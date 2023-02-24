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
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = false
            };


            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44350/api/");
        }

        /// <summary>
        /// Grabs the authentication cookie sent to this controller.
        /// For proper WebAPI authentication, you can send a post request with login credentials to the WebAPI and log the access token from the response. 
        /// The controller already knows this token, so we're just passing it up the chain.
        /// 
        /// Here is a descriptive article which walks through the process of setting up authorization/authentication directly.
        /// https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/individual-accounts-in-web-api
        /// </summary>
        private void GetApplicationCookie()
        {
            string token = "";
            //HTTP client is set up to be reused, otherwise it will exhaust server resources.
            //This is a bit dangerous because a previously authenticated cookie could be cached for
            //a follow-up request from someone else. Reset cookies in HTTP client before grabbing a new one.
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            //collect token as it is submitted to the controller
            //use it to pass along to the WebAPI.
            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
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
        [Authorize]
        public ActionResult New()
        {
            GetApplicationCookie();
            return View();

        }


        // POST: CoCreator/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(CoCreator cocreator)
        {
            GetApplicationCookie();
            // create new instance of a cocreator
            string url = "cocreatordata/addcocreator";

            JavaScriptSerializer jss = new JavaScriptSerializer();


            string jsonpayload = jss.Serialize(cocreator);
            Debug.WriteLine("the json payload is: ");
            Debug.WriteLine(jsonpayload);


            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            //client.PostAsync(url, content);

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
        [Authorize]
        public ActionResult Edit(int id)
        {
            GetApplicationCookie();
            string url = "cocreatordata/findcocreator/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CoCreatorDto selectedCoCreator = response.Content.ReadAsAsync<CoCreatorDto>().Result;
            return View(selectedCoCreator);
        }

        // POST: CoCreator/Edit/5
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, CoCreator cocreator)
        {
            GetApplicationCookie();
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
        [Authorize]
        public ActionResult DeleteConfirm(int id)
        {
            GetApplicationCookie();
            string url = "cocreatordata/findCoCreator/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CoCreatorDto selectedCoCreator = response.Content.ReadAsAsync<CoCreatorDto>().Result;
            return View(selectedCoCreator);
        }


        // POST: CoCreator/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();
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
