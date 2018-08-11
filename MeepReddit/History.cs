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
    public class History : ARedditModule
    {
        /// <summary>
        /// Username in {Smart.Format}
        /// </summary>
        /// <value></value>
        public string Username { get; set; }

        /// <summary>
        /// Max number of results to return
        /// </summary>
        /// <value>The limit.</value>
        public int Limit { get; set; } = 25;

        public override async Task<Message> HandleMessage(Message msg)
        {
            MessageContext context = new MessageContext(msg, this);
            string uname = Smart.Format(Username, context);

            var user = await Client.GetUserAsync(uname);
            return new Batch
            {
                // ToDo: We could convert Batch to use IAsyncEnumerable
                Messages = from vt in user.GetOverview(Sort.New, Limit).ToEnumerable()
                           select new VotableMessage
                           {
                               DerivedFrom = msg,
                               Votable = vt
                           }
            };
        }
    }
}
