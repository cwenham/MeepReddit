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

namespace MeepReddit.Actions
{
    [MeepNamespace(ARedditModule.PluginNamespace)]
    public class Remove : ARedditModule
    {
        /// <summary>
        /// Fullname of Thing to remove, in {Smart.Format}
        /// </summary>
        /// <value></value>
        public string FullName { get; set; }

        /// <summary>
        /// Remove as spam
        /// </summary>
        /// <value><c>true</c> if spam; otherwise, <c>false</c>.</value>
        public bool Spam { get; set; }

        public override async Task<Message> HandleMessage(Message msg)
        {
            MessageContext context = new MessageContext(msg, this);
            string fname = Smart.Format(FullName, context);

            try
            {
                if (Spam)
                    await ModeratableThing.SpamAsync(Agent, fname);
                else
                    await ModeratableThing.RemoveAsync(Agent, fname);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{ex.GetType().Name} thrown when removing thing: {ex.Message}");
            }

            return msg;
        }
    }
}
