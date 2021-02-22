using System;
using System.Collections.Generic;
using System.Text;

namespace RssFeed.Models
{
    internal class RSSFeed
    {
        public int FeedId { get; set; }
        public string FeedName { get; set; }
        public Uri FeedUrl { get; set; }

        public DateTimeOffset? PubDate { get; set; }
    }
}
