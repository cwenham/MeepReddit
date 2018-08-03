using System;
using System.Linq;
using System.Xml.Linq;
using System.Threading;

using Xunit;

using MeepLib.Messages;

using MeepReddit;

namespace MeepRedditTests
{
    public class PostsTests
    {
        [Fact]
        public void GetPics()
        {
            XDocument siteConfig = XDocument.Load("/Users/cwenham/MeepSiteConfig.xml");
            var MeepReddit = siteConfig.Root.Elements()
                            .FirstOrDefault(x => x.Attribute("Name")?.Value == "MeepReddit");
            var RedditUser = siteConfig.Root.Elements()
                            .FirstOrDefault(x => x.Attribute("Name")?.Value == "RedditUser");

            Posts posts = new Posts
            {
                Subreddit = "/r/pics",
                ClientID = MeepReddit.Attribute("ClientID").Value,
                ClientSecret = MeepReddit.Attribute("ClientSecret").Value,
                RedirectURI = MeepReddit.Attribute("RedirectURI").Value,
                User = RedditUser.Attribute("User").Value,
                Pass = RedditUser.Attribute("Pass").Value
            };

            var subTask = posts.Client.GetSubredditAsync(posts.Subreddit);
            subTask.Wait();
            var sub = subTask.Result;
            var unmodded = sub.GetUnmoderatedLinks();

            var stream = unmodded.Stream();
            stream.Subscribe(
                    post => Console.WriteLine(post.Title),
                    ex => Console.WriteLine(ex.Message),
                    () => Console.WriteLine("Pipeline completed")
                );
            stream.Enumerate(new CancellationToken());
            Thread.Sleep(300 * 1000);
        }
    }
}
