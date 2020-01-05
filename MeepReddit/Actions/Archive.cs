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
    public class Archive : ARedditModule
    {
        private static string ArchiveURL = "/api/mod/conversations/{0}/archive";
        /// <summary>
        /// Conversation ID to archive, in {Smart.Format}
        /// </summary>
        public string ConversationID { get; set; }

        public async override Task<Message> HandleMessage(Message msg)
        {
            PMMessage pm = msg as PMMessage;
            if (pm is null)
                return null;

            MessageContext context = new MessageContext(msg, this);
            string sfConversationID = Smart.Format(ConversationID, context);
            string sfArchiveURL = Smart.Format(ArchiveURL, sfConversationID);

            try
            {
                var thing = await Client.GetThingByFullnameAsync(sfConversationID);
                await Agent.Post(sfArchiveURL, null);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"{ex.GetType().Name} thrown when archiving conversation: {ex.Message}");
            }

            return msg;
        }
    }
}
