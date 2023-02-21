using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MuseLab7.Models.ViewModels
{
    public class CreateCollab
    {

        // all possible ideas to build a collab on

        public IEnumerable<IdeaDto> IdeaOptions { get; set; }

        // CoCreator options to choose from when updating this collab 
        public IEnumerable<CoCreatorDto> CoCreatorOptions { get; set; }
    }
}