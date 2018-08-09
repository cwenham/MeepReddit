using System;
using System.Runtime.Serialization;

using Newtonsoft.Json;
using RedditSharp;
using RedditSharp.Things;

using MeepLib.Messages;

namespace MeepReddit.Messages
{
    [DataContract]
    public class CommentMessage : Message, IThingMessage
    {
        [DataMember]
        public Comment Comment { get; set; }

        public Thing Thing
        {
            get
            {
                return Comment as Thing;
            }
        }
    }
}
