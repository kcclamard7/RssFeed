using System;
using Xunit;
using RssFeed.Models;
using RssFeed.Contracts;
using System.Collections.Generic;
using NSubstitute;
using FluentAssertions;
using System.Linq;

namespace RssFeed.Test
{
    /// <summary>
    /// class to test our RssFeed Progream
    /// </summary>
    public class Program_UnitTest
    {
        // we use Nsubtitute to simuluate the service 

        ILastPublishedDateServices lastPublishedDateServices;
        IRssCallServices rssCallServices = Substitute.For<IRssCallServices>();

        // we instanciate the service using the lastpublishdate service
        public Program_UnitTest()
        {
            lastPublishedDateServices = new LastPublishedDateServices();
        }

        [Fact]
        public async void ListPublishedDateServiceShouldContainFeedNameAndInactivtyDate()
        {

            //Arrage
            // fake api response xml
            string FakeXmlReponse = "<Channel><pubDate>Tue, 16 Feb 2021 08:05:00 -0000</pubDate></Channel>";

            // dictionnary reponse from the rsscalls service
            var dictionaryResponseApi = new Dictionary<string, string>();
            dictionaryResponseApi.Add("Test Feed", FakeXmlReponse);
            var listOfFakeFeeds = new RssFeeds
            {
                ListOfRssFeeds = new List<RSSFeed>
                 {
                        new RssFeed.Models.RSSFeed
                        {
                            FeedId = 1,
                            FeedName = "Test Feed",
                            FeedUrl = new Uri("https://fakelink"),
                        }
                }
            };

            //Act
            // real call the rsscall service to process the fake xml 
            var apiReponse = rssCallServices.GetFeedXmlContent(listOfFakeFeeds)
                .Returns(dictionaryResponseApi);
            // we get the result as a collection and extract only one
            var result = await lastPublishedDateServices.NumberOfInactivityDays(listOfFakeFeeds, rssCallServices);
            var oneFeedRssResult = result.FirstOrDefault();


            // Assert 
            // we check in the reponse is greater than 1 (there are more than one rssfeed name and inactivty day)
            result.Count.Should().BeGreaterThan(0);
            // we check if the name is the same
            oneFeedRssResult.FeedName.Should().Contain("Test Feed");
            // we check if the inactivity period is more than 1 since the fake date in the xml is 16 it should be greater
            oneFeedRssResult.InactivityPeriod.Should().BeGreaterThan(0);

        }
    }
}
