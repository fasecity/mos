using System;

namespace Mosan
{
    public class Announcement
    {
        public int Id { get; set; }
        public string DateMade { get; set; }
        public string Priority { get; set; }
        public string Description { get; set; }

        public Announcement()
        {

        }
        public Announcement(int id,string datee, string proirity,string des)
        {
            //if (id <= 0) ---for testing
            //{
            //    throw new Exception("less than 0");
            //}

            //if (this.DateMade == null)
            //{
            //    throw new Exception("date cant be null");
            //}

            if (String.IsNullOrWhiteSpace(proirity))
            {
                throw new Exception("priority cant be null or white space");
            }

            if (String.IsNullOrWhiteSpace(des))
            {
                throw new Exception("des cant be null or white space ");
            }

            this.Id = id;
            this.DateMade = datee;
            this.Priority = proirity.Trim().ToLower();
            this.Description = des.Trim().ToLower();

        }


        public override string ToString()
        {
            var s = "Id:" + this.Id.ToString() + ", Date: " + this.DateMade 
                + ", Priority: "+ this.Priority + ", Descrip: " + this.Description;
            return s;
        }
    }
}
