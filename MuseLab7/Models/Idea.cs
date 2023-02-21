using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MuseLab7.Models
{
    public class Idea
    {
        [Key]
        public int IdeaID { get; set; }

        public string IdeaTitle { get; set; }

        public string IdeaDescription { get; set; }

        // an idea belongs to a creator
        [ForeignKey("Creator")]
        public int CreatorID { get; set; }
        public virtual Creator Creator { get; set; }
    }
    public class IdeaDto
    {
        public int IdeaID { get; set; }

        public string IdeaTitle { get; set; }

        public string IdeaDescription { get; set; }

        // an idea belongs to a creator
        public int CreatorID { get; set; }

        public string CreatorName { get; set; }

    }
}