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
        [Authorize]
        public ActionResult New()
        {
            GetApplicationCookie();
            return View();

        }


        // POST: Creator/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Creator creator)
        {
            GetApplicationCookie();
            // create new instance of a creator
            string url = "creatordata/addcreator";


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

            //return RedirectToAction("List");
        }
        // GET: Creator/Edit/5
        [Authorize]
        public ActionResult Edit(int id)

        {
            GetApplicationCookie();
            string url = "creatordata/findcreator/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CreatorDto selectedCreator = response.Content.ReadAsAsync<CreatorDto>().Result;
            return View(selectedCreator);
        }

        // POST: Creator/Edit/5
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, Creator creator)
        {
            GetApplicationCookie();
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
        [Authorize]
        public ActionResult DeleteConfirm(int id)
        {

            GetApplicationCookie();
            string url = "creatordata/findCreator/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CreatorDto selectedCreator = response.Content.ReadAsAsync<CreatorDto>().Result;
            return View(selectedCreator);
        }


        // POST: Creator/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();
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
