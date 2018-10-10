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
    /// <summary>
    /// User history
    /// </summary>
    [MeepNamespace(ARedditModule.PluginNamespace)]
    public class History : ARedditModule
    {
        /// <summary>
        /// Username in {Smart.Format}
        /// </summary>
        /// <value></value>
        public string Username { get; set; }

        /// <summary>
        /// Max number of results to return per API request
        /// </summary>
        /// <value>The limit.</value>
        /// <remarks>Only change this for performance tuning, otherwise use
        /// <see cref="Max"/> to control how much history is fetched.</remarks>
        public int Limit { get; set; } = 25;

        /// <summary>
        /// Max number of results to return
        /// </summary>
        /// <value>The max.</value>
        /// <remarks><see cref="Limit"/> refers to how many results we request
        /// each time we go to the reddit API, and is like a page size or
        /// the size of an individual sip. Max refers to the highest number of
        /// results we wait to collect in Limit-size sips.
        /// 
        /// <remarks>Should be a multiple of Limit.</remarks></remarks>
        public int Max { get; set; } = 25;

        public override async Task<Message> HandleMessage(Message msg)
        {
            MessageContext context = new MessageContext(msg, this);
            string uname = Smart.Format(Username, context);

            var user = await Client.GetUserAsync(uname);
            List<VotableThing> things = new List<VotableThing>();
            await user.GetOverview(Sort.New, Limit).Take(Max).ForEachAsync(h => things.Add(h));
            return new OverviewMessage
            {
                DerivedFrom = msg,
                User = user,
                Messages = from vt in things
                           select new VotableMessage
                           {
                               DerivedFrom = msg,
                               Votable = vt
                           }
            };
        }
    }
}
