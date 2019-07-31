using System;
using System.Runtime.Serialization;

using Newtonsoft.Json;
using RedditSharp;
using RedditSharp.Things;

using MeepLib.Messages;

namespace MeepReddit.Messages
{
    [DataContract]
    public class ThingMessage : Message, IThingMessage
    {
        [DataMember]
        public Thing Thing { get; set; }

        /// <summary>
        /// Always returns null as an Author cannot be known until the type of Thing is known
        /// </summary>
        public string TargetAuthor
        {
            get
            {
                return null;
            }
        }

        public override string ToString()
        {
            if (Thing is Post)
                return ((Post)Thing).Title;

            return Thing.ToString();
        }
    }
}
