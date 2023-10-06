using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RescuePetHomework02.Models
{
    public class PostAPetViewModel
    {
        public string PetName { get; set; }
        public string PetStory { get; set; }
        public int Gender { get; set; }
        public int PetType { get; set; }
        public int UserId { get; set; }
        public string PhoneNumber { get; set; }
        public int Breed { get; set; }
        public int Location { get; set; }
        public int Age { get; set; }
        public decimal Weight { get; set; }
        public HttpPostedFileBase PetImage { get; set; }
    }
}