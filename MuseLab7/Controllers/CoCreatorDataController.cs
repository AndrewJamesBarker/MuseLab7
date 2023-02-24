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
    public class CoCreatorDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        /// <summary>
        /// returns cocreators in system
        /// </summary>
        /// <returns>
        /// all cocreators in the system
        /// </returns>
    
        // GET: api/CoCreatorData/ListCoCreators
        [HttpGet]
        public IEnumerable<CoCreatorDto> ListCoCreators()
        {
            List<CoCreator> CoCreators = db.CoCreators.ToList();
            List<CoCreatorDto> CoCreatorDtos = new List<CoCreatorDto>();

            CoCreators.ForEach(c => CoCreatorDtos.Add(new CoCreatorDto()
            {
                CoCreatorID = c.CoCreatorID,
                CoCreatorName = c.CoCreatorName,
                CoCreatorBio = c.CoCreatorBio
            }));
            return CoCreatorDtos;
        }

        /// <summary>
        /// returns specific cocreator
        /// </summary>
        /// <param name="id"></param>
        /// 
        /// <returns>
        /// returns specific cocreator by id
        /// </returns>
        // GET: api/CoCreatorData/findCoCreator/5
        [ResponseType(typeof(CoCreatorDto))]
        [HttpGet]
        public IHttpActionResult FindCoCreator(int id)
        {
            CoCreator CoCreator = db.CoCreators.Find(id);
            CoCreatorDto CoCreatorDto = new CoCreatorDto()
            {
                CoCreatorID = CoCreator.CoCreatorID,
                CoCreatorName = CoCreator.CoCreatorName,
                CoCreatorBio = CoCreator.CoCreatorBio
            };
            if (CoCreator == null)
            {
                return NotFound();
            }

            return Ok(CoCreatorDto);
        }

        /// <summary>
        /// updates a cocreator
        /// </summary>
        /// <param name="id"></param>
        /// <param name="CoCreator"></param>
        /// <returns>updated cocreator by id</returns>
        // Post: api/CoCreatorData/updateCoCreator/5
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult UpdateCoCreator(int id, CoCreator CoCreator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != CoCreator.CoCreatorID)
            {
                return BadRequest();
            }

            db.Entry(CoCreator).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CoCreatorExists(id))
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


        /// <summary>
        /// adds a cocreator to the system
        /// </summary>
        /// <param name="CoCreator">jason data of cocreator</param>
        /// <returns>cocreator id and data</returns>
        // POST: api/CoCreatorData/addCoCreator
        [ResponseType(typeof(CoCreator))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult AddCoCreator(CoCreator CoCreator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CoCreators.Add(CoCreator);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = CoCreator.CoCreatorID }, CoCreator);
        }
        /// <summary>
        /// deletes a cocreator
        /// </summary>
        /// <param name="id"></param>
        /// <returns>to list adfter deleting slected cocreator by id</returns>
        // DELETE: api/CoCreatorData/DeleteCoCreator/5
        [ResponseType(typeof(CoCreator))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult DeleteCoCreator(int id)
        {
            CoCreator CoCreator = db.CoCreators.Find(id);
            if (CoCreator == null)
            {
                return NotFound();
            }

            db.CoCreators.Remove(CoCreator);
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

        private bool CoCreatorExists(int id)
        {
            return db.CoCreators.Count(e => e.CoCreatorID == id) > 0;
        }
    }
}