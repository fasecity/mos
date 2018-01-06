using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosan;

namespace MosqueService.Controllers
{
    [Produces("application/json")]
    [Route("api/Holla")]
    public class HollaController : Controller
    {
        private readonly ApiContext ctx = ApiContext.db;

        [HttpGet]
        public IActionResult Get()
        {           
            return Ok();
        }

       // [HttpPost("api/Holla/addmsg")]
        public IActionResult Post([FromBody] Announcement announcement)
        {
            //date
            var ddate = DateTime.Now;
            var datesmooth = ddate.ToShortDateString();
            announcement.DateMade = datesmooth;

            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            //-----------make Model---------//
            var x = new Announcement();
            x.DateMade = datesmooth;
            x.Description = announcement.Description;
            x.Priority = announcement.Priority;
           
            //--save object            
            ctx.Add(x);
            ctx.SaveChanges();

            return Ok();
        }
    }
}