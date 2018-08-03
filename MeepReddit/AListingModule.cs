using System;

namespace MeepReddit
{
    public class AListingModule : ARedditModule
    {
        public int Limit { get; set; } = 25;

        public string Show { get; set; } = "all";
    }
}
