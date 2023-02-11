using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace MuseLab7.Models.ViewModels
{
    public class DetailsIdea
    {
        public IdeaDto SelectedIdea { get; set; }
        public IEnumerable<CreatorDto> CoCreators { get; set; }

    }
}
