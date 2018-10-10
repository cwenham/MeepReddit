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
    public class Flair : AMessageModule
    {
        /// <summary>
        /// Flair text, in {Smart.Format}
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        /// <summary>
        /// Flair CSS class, in {Smart.Format}
        /// </summary>
        /// <value>The class.</value>
        public string Class { get; set; }

        public override async Task<Message> HandleMessage(Message msg)
        {
            PostMessage postMsg = msg as PostMessage;
            if (msg == null)
                return msg;               

            try
            {
                MessageContext context = new MessageContext(msg, this);
                string sfText = Smart.Format(Text ?? "", context);
                string sfClass = Smart.Format(Class ?? "", context);

                await postMsg.Post.SetFlairAsync(sfText, sfClass);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{ex.GetType().Name} thrown when setting post flair: {ex.Message}");
            }

            return msg;
        }
    }
}
