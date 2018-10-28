using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Newtonsoft.Json;
using RedditSharp;
using RedditSharp.Things;

using MeepLib;
using MeepLib.Messages;

namespace MeepReddit.Messages
{
    [DataContract, Table("ModActions")]
    public class ModActionMessage : Message, IThingMessage
    {
        [DataMember, NotMapped]
        public ModAction Action { get; set; }

        [NotMapped]
        public Thing Thing
        {
            get
            {
                return Action as Thing;
            }
        }

        [MaxLength(46), Key]
        public string ActionID
        {
            get
            {
                return Action.Id;
            }
        }

        [Index(IsUnique = false), MaxLength(20)]
        public string Subreddit
        {
            get
            {
                return Action.SubredditName;
            }
        }

        [Index(IsUnique = false)]
        public DateTime ThingCreated
        {
            get
            {
                return Action.TimeStamp.Value;
            }
        }

        [Index(IsUnique = false), MaxLength(20)]
        public string Moderator
        {
            get
            {
                return Action.ModeratorName;
            }
        }

        [Index(IsUnique = false)]
        public string Kind
        {
            get
            {
                return Action.Kind;
            }
        }

        public string Description
        {
            get
            {
                return Action.Description;
            }
        }

        public string Details
        {
            get
            {
                return Action.Details;
            }
        }

        [Index(IsUnique = false), MaxLength(20)]
        public string Target
        {
            get
            {
                return Action.TargetThingFullname;
            }
        }

        [Index(IsUnique = false), MaxLength(20)]
        public string TargetAuthor
        {
            get
            {
                return Action.TargetAuthorName;
            }
        }

        public string TargetTitle
        {
            get
            {
                return Action.TargetTitle;
            }
        }

        public string TargetBody
        {
            get
            {
                return Action.TargetBody;
            }
        }
    }
}
