using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RescuePetHomework02.Models
{
    public class PetViewModel
    {
        public int PetID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string StatusName { get; set; }
        public string TypeName { get; set; }
        public string BreedName { get; set; }
        public string LocationName { get; set; }
        public string UserName { get; set; }
        public string GenderName { get; set; }
        public int Age { get; set; }
        public double Weight { get; set; }
        public string Story { get; set; }
    }

}