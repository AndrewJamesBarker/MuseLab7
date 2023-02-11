using System;
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
using System.Diagnostics;

namespace MuseLab7.Controllers
{
    public class CollabDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // returns collabs in system
        //
        // GET: api/CollabData/listcollabs
        [HttpGet]
        [ResponseType(typeof(CollabDto))]
        public IHttpActionResult ListCollabs()
        {
            List<Collab> Collabs = db.Collabs.ToList();
            List<CollabDto> CollabDtos = new List<CollabDto>();

            Collabs.ForEach(c => CollabDtos.Add(new CollabDto()
            {
                CollabID = c.CollabID,
                CollabTitle = c.CollabTitle,
                CollabDescription = c.CollabDescription,
                IdeaTitle = c.Idea.IdeaTitle,
                CoCreatorName = c.CoCreator.CoCreatorName
            }));
            return Ok(CollabDtos);
        }


        // returns specific collab by id
        //param name="id" collab primary key
        // GET: api/CollabData/findcollab/5
        [ResponseType(typeof(CollabDto))]
        [HttpGet]
        public IHttpActionResult FindCollab(int id)
        {
            Collab Collab = db.Collabs.Find(id);
            CollabDto CollabDto = new CollabDto()
            {
                CollabID = Collab.CollabID,
                CollabTitle = Collab.CollabTitle,
                CollabDescription = Collab.CollabDescription,
                IdeaTitle = Collab.Idea.IdeaTitle,
                CoCreatorName = Collab.CoCreator.CoCreatorName

            };
            if (Collab == null)
            {
                return NotFound();
            }

            return Ok(CollabDto);
        }


        // updates a collab with post input
        //param name="id" represent a collabs primary key
        //param name="collab" json form data
        //Header: 204 (success)
        //Header 404 (not found)
        // Post: api/CollabData/updatecollab/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateCollab(int id, Collab Collab)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Collab.CollabID)
            {
                return BadRequest();
            }

            db.Entry(Collab).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CollabExists(id))
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
        // adds a collab to the system
        //param name="collab">JSON FORM DATA
        // POST: api/CollabData/addcollab
        [ResponseType(typeof(Collab))]
        [HttpPost]
        public IHttpActionResult AddCollab(Collab Collab)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Collabs.Add(Collab);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Collab.CollabID }, Collab);
        }

        // DELETE: api/CollabData/deletecollab/5
        [ResponseType(typeof(Collab))]
        [HttpPost]
        public IHttpActionResult DeleteCollab(int id)
        {
            Collab Collab = db.Collabs.Find(id);
            if (Collab == null)
            {
                return NotFound();
            }

            db.Collabs.Remove(Collab);
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

        private bool CollabExists(int id)
        {
            return db.Collabs.Count(e => e.CollabID == id) > 0;
        }
    }
}