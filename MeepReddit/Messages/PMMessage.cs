using System;
using System.Runtime.Serialization;

using Newtonsoft.Json;
using RedditSharp;
using RedditSharp.Things;

using MeepLib.Messages;

namespace MeepReddit.Messages
{
    [DataContract]
    public class PMMessage : Message
    {
        [DataMember]
        public PrivateMessage Message { get; set; }
    }
}
