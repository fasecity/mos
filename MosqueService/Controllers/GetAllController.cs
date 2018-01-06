using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mosan;

namespace MosqueService.Controllers
{
    [Produces("application/json")]
    [Route("api/GetAll")]
    public class GetAllController : Controller
    {
        private readonly ApiContext ctx = ApiContext.db;

        //public IActionResult Index()
        //{
        //    return View();
        //}

        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            var x = ctx.MosqueAnncounments.ToList();
            return Ok(x.ToList());
        }
    }
}