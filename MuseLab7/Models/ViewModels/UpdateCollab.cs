using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace MuseLab7.Models.ViewModels
{
    public class UpdateCollab
    {

        // this view model is a class which stores info we need to present to /collab/update/{}

        //the existing collab info

        public CollabDto SelectedCollab { get; set; }

        // all possible ideas to build a collab on


        public IEnumerable<IdeaDto> IdeaOptions { get; set; }

        // CoCreator options to choose from when updating this collab 
        public IEnumerable<CoCreatorDto> CoCreatorOptions { get; set; }

    }
}
