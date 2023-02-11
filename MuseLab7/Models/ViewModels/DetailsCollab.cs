using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace MuseLab7.Models.ViewModels
{
    public class DetailsCollab
    {
        public CollabDto SelectedCollab { get; set; }
        public IEnumerable<CoCreatorDto> CoCreators { get; set; }
        public IEnumerable<IdeaDto> Ideas { get; set; }

    }
}
