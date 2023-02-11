using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MuseLab7.Models
{
    public class CoCreator
    {
        [Key]
        public int CoCreatorID { get; set; }

        public string CoCreatorName { get; set; }

        public string CoCreatorBio { get; set; }
    }

    public class CoCreatorDto
    {
        public int CoCreatorID { get; set; }

        public string CoCreatorName { get; set; }

        public string CoCreatorBio { get; set; }
    }
}