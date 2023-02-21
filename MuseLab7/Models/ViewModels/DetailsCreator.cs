using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace MuseLab7.Models.ViewModels
{
    public class DetailsCreator
    {
        public CreatorDto SelectedCreator { get; set; }
        public IEnumerable<IdeaDto> RelatedIdeas { get; set; }


    }
}
