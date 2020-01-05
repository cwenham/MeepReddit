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
using RedditSharp;
using RedditSharp.Things;

using MeepLib;
using MeepLib.MeepLang;
using MeepLib.Messages;

using MeepReddit.Messages;

namespace MeepReddit
{
    [MeepNamespace(ARedditModule.PluginNamespace)]
    public class ModQueue : Posts
    {
        protected override IObservable<Message> GetMessagingSource()
        {
            var sub = GetSub(Subreddit);
            var posts = sub.GetModQueue();
            var stream = posts.Stream();

            var source = stream.Select(ConvertToVotable);
            stream.Enumerate(new System.Threading.CancellationToken());

            return source;
        }
    }
}
