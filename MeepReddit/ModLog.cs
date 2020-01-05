using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web;
using System.Reactive.Linq;

using SmartFormat;
using NLog;
using RedditSharp;
using RedditSharp.Things;

using MeepLib;
using MeepLib.MeepLang;
using MeepLib.Messages;

using MeepReddit.Messages;

namespace MeepReddit
{
    [MeepNamespace(ARedditModule.PluginNamespace)]
    public class ModLog : Posts
    {
        protected override IObservable<Message> GetMessagingSource()
        {
            var sub = GetSub(Subreddit);
            var posts = sub.GetModerationLog();
            var stream = posts.Stream();

            var source = stream.Select(ConvertToModAction);
            stream.Enumerate(new System.Threading.CancellationToken());

            return source;
        }
    }
}
