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

            DetailsIdea ViewModel = new DetailsIdea();

            string url = "ideadata/findidea/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

       

            IdeaDto SelectedIdea = response.Content.ReadAsAsync<IdeaDto>().Result;

            ViewModel.SelectedIdea = SelectedIdea;

            //showcase information about collabs related to this idea
            //send a request to gather information about collabs related to a particular idea ID
            url = "collabdata/listcollabsforidea/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<CollabDto> RelatedCollabs = response.Content.ReadAsAsync<IEnumerable<CollabDto>>().Result;

            ViewModel.RelatedCollabs = RelatedCollabs;


            return View(ViewModel);
        }

            // Error page 
            public ActionResult Error()
        {
            return View("Error");
        }

        // GET: Idea/New
        [Authorize]
        public ActionResult New()
        {
            GetApplicationCookie();
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
        [Authorize]
        public ActionResult Create(Idea idea)
        {
            GetApplicationCookie();
            // create new instance of a idea
            string url = "ideadata/addidea";


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
        // GET: Idea/Edit/5
        [Authorize]
        public ActionResult Edit(int id)

        {
            GetApplicationCookie();
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
        [Authorize]
        public ActionResult Update(int id, Idea idea)
        {
            GetApplicationCookie();
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
        [Authorize]
        public ActionResult DeleteConfirm(int id)
        {
            GetApplicationCookie();
            string url = "ideadata/findidea/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            IdeaDto selectedidea = response.Content.ReadAsAsync<IdeaDto>().Result;
            return View(selectedidea);
        }


        // POST: Idea/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();
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
