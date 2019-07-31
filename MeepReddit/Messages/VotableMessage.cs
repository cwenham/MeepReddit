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
    public class VotableMessage : Message, IThingMessage
    {
        [DataMember, NotMapped]
        public VotableThing Votable { get; set; }

        [NotMapped]
        public Thing Thing
        {
            get
            {
                return Votable as Thing;
            }
        }

        [NotMapped]
        public string TargetAuthor
        {
            get
            {
                return Votable.AuthorName;
            }
        }
    }
}
