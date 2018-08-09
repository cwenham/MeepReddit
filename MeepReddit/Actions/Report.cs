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
    public class Report : ARedditModule
    {
        /// <summary>
        /// Fullname of Thing to report, in {Smart.Format}
        /// </summary>
        /// <value></value>
        public string Fullname { get; set; }

        /// <summary>
        /// Reason in {Smart.Format}
        /// </summary>
        /// <value>The reason.</value>
        public string Reason { get; set; }

        public override async Task<Message> HandleMessage(Message msg)
        {
            MessageContext context = new MessageContext(msg, this);
            string fname = Smart.Format(Fullname, context);
            string reason = Smart.Format(Reason, context);

            try
            {
                await ModeratableThing.ReportAsync(Agent, fname, ModeratableThing.ReportType.Other, reason);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{ex.GetType().Name} thrown when removing thing: {ex.Message}");
            }

            return msg;
        }
    }
}
