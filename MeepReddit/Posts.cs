using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web;
using System.Reactive.Linq;

using SmartFormat;
using NLog;
using Newtonsoft.Json;
using RedditSharp.Things;

using MeepLib;
using MeepLib.MeepLang;
using MeepLib.Messages;

using MeepReddit.Messages;

namespace MeepReddit
{
    [MeepNamespace(ARedditModule.PluginNamespace)]
    public class Posts : AListingModule
    {
        public string Sort { get; set; }

        protected Subreddit.Sort SortType
        {
            get
            {
                object type;
                if (!String.IsNullOrWhiteSpace(Sort))
                    if (Enum.TryParse(typeof(Subreddit.Sort), Sort, out type))
                        return (Subreddit.Sort)type;

                return RedditSharp.Things.Subreddit.Sort.New;
            }
        }

        protected override IObservable<Message> GetMessagingSource()
        {
            var sub = GetSub(Subreddit);
            var posts = sub.GetPosts(SortType);
            var stream = posts.Stream();

            var source = stream.Select(ConvertToPost);

            stream.Enumerate(new System.Threading.CancellationToken());

            return source;
        }
    }
}
