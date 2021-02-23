using System;
using System.Collections.Generic;
using System.Text;

namespace RssFeed.Models
{
    public class RssFeedWithInactivityPeriod
    {
        public string FeedName { get; set; }
        public int InactivityPeriod { get; set; }
    }
}
