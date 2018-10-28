using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [MaxLength(20), Key]
        public string FullName
        {
            get
            {
                return Message.FullName;
            }
        }

        public Thing Thing
        {
            get
            {
                return Message as Thing;
            }
        }
    }
}
