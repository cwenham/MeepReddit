using System;
using System.Runtime.Serialization;

using Newtonsoft.Json;
using RedditSharp;
using RedditSharp.Things;

using MeepLib.Messages;

namespace MeepReddit.Messages
{
    [DataContract]
    public class ModActionMessage : Message, IThingMessage
    {
        [DataMember]
        public ModAction Action { get; set; }

        public Thing Thing
        {
            get
            {
                return Action as Thing;
            }
        }
    }
}
