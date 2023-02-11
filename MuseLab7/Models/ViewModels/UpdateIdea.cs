using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace MuseLab7.Models.ViewModels
{
    public class UpdateIdea
    {

        //the existing idea info

        public IdeaDto SelectedIdea { get; set; }

        // Creator options to choose from when updating this idea
        public IEnumerable<CreatorDto> CreatorOptions { get; set; }

    }
}
