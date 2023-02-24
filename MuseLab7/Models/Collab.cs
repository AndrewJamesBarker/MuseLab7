using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MuseLab7.Models
{
    public class Collab
    {
        [Key]
        public int CollabID { get; set; }

        public string CollabTitle { get; set; }

        public string CollabDescription { get; set; }

        [ForeignKey("Idea")]
        public int IdeaID { get; set; }
        public virtual Idea Idea { get; set; }

        [ForeignKey("CoCreator")]
        public int CoCreatorID { get; set; }
        public virtual CoCreator CoCreator { get; set; }
    }

    public class CollabDto
    {
        public int CollabID { get; set; }

        public string CollabTitle { get; set; }

        public string CollabDescription { get; set; }
 
        public int IdeaID { get; set; }
        public string IdeaTitle { get; set; }

        public int CoCreatorID { get; set; }

        public string CoCreatorName { get; set; }
        
    }
}