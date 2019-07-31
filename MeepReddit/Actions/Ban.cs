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
    /// <summary>
    /// Ban users from a subreddit
    /// </summary>
    public class Ban : ARedditModule
    {
        /// <summary>
        /// Ban note, seen by the user
        /// </summary>
        public DataSelector Note { get; set; }

        /// <summary>
        /// Ban reason, seen only by other moderators
        /// </summary>
        public DataSelector Reason { get; set; }

        /// <summary>
        /// Message sent to the user
        /// </summary>
        public DataSelector Message { get; set; }

        /// <summary>
        /// Duration of ban in days
        /// </summary>
        public DataSelector Duration { get; set; }

        public override async Task<Message> HandleMessage(Message msg)
        {
            MessageContext context = new MessageContext(msg, this);

            string dsNote = await Note.SelectStringAsync(context);
            string dsReason = await Reason.SelectStringAsync(context);
            string dsMessage = null;
            if (Message != null)
                dsMessage = await Message.SelectStringAsync(context);

            int iDuration = 0;
            if (Duration != null)
            {
                var dsDuration = await Duration.TrySelectLongAsync(context);
                if (dsDuration.Parsed)
                    iDuration = (int)dsDuration.Value;
            }

            string sfSub = Smart.Format(Subreddit, context);
            var sub = GetSub(sfSub);

            IThingMessage thingMsg = msg as IThingMessage;
            if (thingMsg != null && thingMsg.TargetAuthor != null)
            try
            {
                await sub.BanUserAsync(thingMsg.TargetAuthor, dsReason, dsNote, iDuration, dsMessage);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "{0} thrown when banning user {1} from {2}: {3}", ex.GetType().Name, thingMsg.TargetAuthor, sfSub, ex.Message);
            }

            return msg;
        }
    }
}
