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
    public class Unmoderated : Posts
    {
        protected override IObservable<Message> GetMessagingSource()
        {
            try
            {
                var sub = GetSub(Subreddit);
                var posts = sub.GetUnmoderatedLinks();
                var stream = posts.Stream();

                var source = stream.Select(ConvertToPost);
                stream.Enumerate(new System.Threading.CancellationToken());

                return source;
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{ex.GetType().Name} thrown when trying to get {Subreddit} unmoderated: {ex.Message}");
                throw;
            }            
        }
    }
}
