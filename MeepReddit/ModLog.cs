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
    public class ModLog : Posts
    {
        public override IObservable<Message> Pipeline
        {
            get
            {
                if (_pipeline == null)
                {
                    var sub = GetSub(Subreddit);
                    var posts = sub.GetModerationLog();
                    var stream = posts.Stream();

                    _pipeline = stream.Select(ConvertToModAction);
                    stream.Enumerate(new System.Threading.CancellationToken());
                }

                return _pipeline;
            }
            protected set
            {
                _pipeline = value;
            }
        }
    }
}
