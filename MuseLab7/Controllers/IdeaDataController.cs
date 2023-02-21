using System;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MuseLab7.Models;

namespace MuseLab7.Controllers
{
    public class IdeaDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/IdeaData/listideas
        [HttpGet]
        [ResponseType(typeof(IdeaDto))]
        public IHttpActionResult ListIdeas()
        {
            List<Idea> Ideas = db.Ideas.ToList();
            List<IdeaDto> IdeaDtos = new List<IdeaDto>();

            Ideas.ForEach(i => IdeaDtos.Add(new IdeaDto()
            {
                IdeaID = i.IdeaID,
                IdeaTitle = i.IdeaTitle,
                IdeaDescription = i.IdeaDescription,
                CreatorID = i.Creator.CreatorID,
                CreatorName = i.Creator.CreatorName
            }));
            return Ok(IdeaDtos);
        }

        /// <summary>
        /// finds an idea based on id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>returns a specific idea by id</returns>
        // GET: api/IdeaData/findidea/5
        [ResponseType(typeof(Idea))]
        [HttpGet]
        public IHttpActionResult FindIdea(int id)
        {
            Idea Idea = db.Ideas.Find(id);
            IdeaDto IdeaDto = new IdeaDto()
            {
                IdeaID = Idea.IdeaID,
                IdeaTitle = Idea.IdeaTitle,
                IdeaDescription = Idea.IdeaDescription,
                CreatorName = Idea.Creator.CreatorName
            };

            if (Idea == null)
            {
                return NotFound();
            }

            return Ok(IdeaDto);
        }

        /// <summary>
        /// gathers info on all ideas related to a specific creator id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// all ideas in the database by their associated creators matched by id
        /// </returns>
        // GET: api/IdeaData/findidea/5

        [HttpGet]
        [ResponseType(typeof(IdeaDto))]
        public IHttpActionResult ListIdeasForCreator(int id)
        {
            List<Idea> Ideas = db.Ideas.Where(i=>i.CreatorID ==id).ToList();
            List<IdeaDto> IdeaDtos = new List<IdeaDto>();

            Ideas.ForEach(i => IdeaDtos.Add(new IdeaDto()
            {
                IdeaID = i.IdeaID,
                IdeaTitle = i.IdeaTitle,
                IdeaDescription = i.IdeaDescription,
                CreatorID = i.Creator.CreatorID,
                CreatorName = i.Creator.CreatorName
            }));

            return Ok(IdeaDtos);
        }

        // PUT: api/IdeaData/updateidea/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateIdea(int id, Idea Idea)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Idea.IdeaID)
            {
                return BadRequest();
            }

            db.Entry(Idea).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IdeaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/IdeaData/addidea
        [ResponseType(typeof(Idea))]
        [HttpPost]
        public IHttpActionResult AddIdea(Idea Idea)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Ideas.Add(Idea);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Idea.IdeaID }, Idea);
        }

        // post: api/IdeaData/deleteidea/5
        [ResponseType(typeof(Idea))]
        [HttpPost]
        public IHttpActionResult DeleteIdea(int id)
        {
            Idea Idea = db.Ideas.Find(id);
            if (Idea == null)
            {
                return NotFound();
            }

            db.Ideas.Remove(Idea);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool IdeaExists(int id)
        {
            return db.Ideas.Count(e => e.IdeaID == id) > 0;
        }
    }
}