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

        public override string ToString()
        {
            if (Thing is Post)
                return ((Post)Thing).Title;

            return Thing.ToString();
        }
    }
}
