using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MuseLab7.Models.ViewModels
{
    public class DetailsCoCreator
    {
        public CoCreatorDto SelectedCoCreator { get; set; }
        public IEnumerable<CollabDto> RelatedCollabs { get; set; }

    }
}
