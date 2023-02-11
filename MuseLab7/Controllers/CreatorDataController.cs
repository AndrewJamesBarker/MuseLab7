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

namespace MuseLab7.Controllers
{
    public class CreatorDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/CreatorData/ListCreators
        [HttpGet]
        public IEnumerable<CreatorDto> ListCreators()
        {
            List<Creator> Creators = db.Creators.ToList();
            List<CreatorDto> CreatorDtos = new List<CreatorDto>();

            Creators.ForEach(c => CreatorDtos.Add(new CreatorDto()
            {
                CreatorID = c.CreatorID,
                CreatorName = c.CreatorName,
                CreatorBio = c.CreatorBio
            }));
            return CreatorDtos;
        }

        // GET: api/CreatorData/FindCreator/5
        [ResponseType(typeof(Creator))]
        [HttpGet]
        public IHttpActionResult FindCreator(int id)
        {
            Creator Creator = db.Creators.Find(id);
            CreatorDto CreatorDto = new CreatorDto()
            {
                CreatorID = Creator.CreatorID,
                CreatorName = Creator.CreatorName,
                CreatorBio = Creator.CreatorBio
            };
            if (Creator == null)
            {
                return NotFound();
            }

            return Ok(CreatorDto);
        }

        // Post: api/CreatorData/UpdateCreator/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateCreator(int id, Creator Creator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Creator.CreatorID)
            {
                return BadRequest();
            }

            db.Entry(Creator).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CreatorExists(id))
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

        // POST: api/CreatorData/AddCreator
        [ResponseType(typeof(Creator))]
        [HttpPost]
        public IHttpActionResult AddCreator(Creator Creator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Creators.Add(Creator);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Creator.CreatorID }, Creator);
        }

        // Post: api/CreatorData/DeleteCreator/5
        [ResponseType(typeof(Creator))]
        [HttpPost]
        public IHttpActionResult DeleteCreator(int id)
        {
            Creator Creator = db.Creators.Find(id);
            if (Creator == null)
            {
                return NotFound();
            }

            db.Creators.Remove(Creator);
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

        private bool CreatorExists(int id)
        {
            return db.Creators.Count(e => e.CreatorID == id) > 0;
        }
    }
}