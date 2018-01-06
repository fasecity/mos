using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mosan;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MosqueService
{
    public class ApiContext:DbContext
    {
        ///////--------------------------------------db static ----------------------------------------
        public static DbContextOptions<ApiContext> options = new DbContextOptions<ApiContext>();
        public static ApiContext db = new ApiContext(Options);
        public static DbContextOptions<ApiContext> Options { get => options; set => options = value; }
        ////// ------------------------------------------------------------------------------------------

        //ctor
        public ApiContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Announcement> MosqueAnncounments { get; set; }

        //overides
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            //set src sqlite local--works All remember check  appsettings json file
             optionsBuilder.UseSqlite("Data Source=MosqueDb.db");

        
        }

    }
}
