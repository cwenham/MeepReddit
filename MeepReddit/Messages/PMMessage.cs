using System;
using System.Runtime.Serialization;

using Newtonsoft.Json;
using RedditSharp;
using RedditSharp.Things;

using MeepLib.Messages;

namespace MeepReddit.Messages
{
    [DataContract]
    public class PMMessage : Message, IThingMessage
    {
        [DataMember]
        public PrivateMessage Message { get; set; }

        public Thing Thing
        {
            get
            {
                return Message as Thing;
            }
        }
    }
}
