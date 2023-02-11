using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MuseLab7.Models
{
    public class Creator
    {
        [Key]
        public int CreatorID { get; set; }

        public string CreatorName { get; set; }

        public string CreatorBio { get; set; }
    }

    public class CreatorDto
    {
        public int CreatorID { get; set; }

        public string CreatorName { get; set; }

        public string CreatorBio { get; set; }
    }
}