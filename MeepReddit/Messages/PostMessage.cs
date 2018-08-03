using System;
using System.Runtime.Serialization;

using Newtonsoft.Json;
using RedditSharp;
using RedditSharp.Things;

using MeepLib.Messages;

namespace MeepReddit.Messages
{
    [DataContract]
    public class PostMessage : Message
    {
        [DataMember]
        public Post Post { get; set; }

        public override string ToString()
        {
            return Post.Title;
        }
    }
}
