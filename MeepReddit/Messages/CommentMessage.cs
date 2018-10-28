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
    [DataContract, Table("Comments")]
    public class CommentMessage : Message, IThingMessage
    {
        [DataMember, NotMapped]
        public Comment Comment { get; set; }

        [NotMapped, XmlIgnore, JsonIgnore]
        public Thing Thing
        {
            get
            {
                return Comment as Thing;
            }
        }

        [MaxLength(20), Key]
        public string FullName 
        {
            get 
            {
                return Comment.FullName;
            }
        }

        [MaxLength(20), Index(IsUnique = false)]
        public string AuthorName
        {
            get 
            {
                return Comment.AuthorName;
            }
        }

        [MaxLength(20), Index(IsUnique = false)]
        public string Subreddit
        {
            get
            {
                return Comment.Subreddit;
            }
        }

        [MaxLength(20), Index(IsUnique = false)]
        public string ParentID
        {
            get
            {
                return Comment.ParentId;
            }
        }

        [Index(IsUnique = false)]
        public DateTime ThingCreated
        {
            get
            {
                return Comment.CreatedUTC;
            }
        }

        public string Body
        {
            get
            {
                return Comment.Body;
            }
        }

        [Index(IsUnique = false)]
        public string AuthorFlair
        {
            get
            {
                return Comment.AuthorFlairText;
            }
        }

        [Index(IsUnique = false)]
        public string AuthorFlairClass
        {
            get
            {
                return Comment.AuthorFlairCssClass;
            }
        }
    }
}
