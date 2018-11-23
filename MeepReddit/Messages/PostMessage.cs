using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Newtonsoft.Json;
using RedditSharp;
using RedditSharp.Things;

using MeepLib;
using MeepLib.Messages;
using System.Collections.Generic;

namespace MeepReddit.Messages
{
    [DataContract, Table("Posts")]
    public class PostMessage : Message, IThingMessage, ITokenisable
    {
        [DataMember, NotMapped]
        public Post Post { get; set; }

        public override string ToString()
        {
            return Post.Title;
        }

        [NotMapped, XmlIgnore, JsonIgnore]
        public Thing Thing
        {
            get
            {
                return Post as Thing;
            }
        }

        [MaxLength(20), Key]
        public string FullName
        {
            get
            {
                return Post.FullName;
            }
        }

        [MaxLength(20), Index(IsUnique = false)]
        public string AuthorName
        {
            get
            {
                return Post.AuthorName;
            }
        }

        [MaxLength(20), Index(IsUnique = false)]
        public string Subreddit
        {
            get
            {
                return Post.SubredditName;
            }
        }

        public string Title
        {
            get
            {
                return Post.Title;
            }
        }

        public string SelfText
        {
            get
            {
                return Post.SelfText;
            }
        }

        [Index(IsUnique = false)]
        public DateTime ThingCreated
        {
            get
            {
                return Post.CreatedUTC;
            }
        }

        [Index(IsUnique = false)]
        public Uri Url
        {
            get
            {
                return Post.Url;
            }
        }

        [Index(IsUnique = false)]
        public Uri Permalink
        {
            get
            {
                return Post.Permalink;
            }
        }

        [Index(IsUnique = false)]
        public string Flair
        {
            get 
            {
                return Post.LinkFlairText;
            }
        }

        [Index(IsUnique = false)]
        public string FlairClass
        {
            get
            {
                return Post.LinkFlairCssClass;
            }
        }

        [Index(IsUnique = false)]
        public string AuthorFlair
        {
            get
            {
                return Post.AuthorFlairText;
            }
        }

        [Index(IsUnique = false)]
        public string AuthorFlairClass
        {
            get
            {
                return Post.AuthorFlairCssClass;
            }
        }

        [NotMapped, XmlIgnore, JsonIgnore]
        public IEnumerable<string> Tokens 
        {
            get 
            {
                if (_tokens is null)
                    _tokens = (from t in Post.Title.Split(' ').Union(Post.SelfText.Split(' '))
                               where !String.IsNullOrWhiteSpace(t)
                               select t).ToArray();

                return _tokens;
            }
        }
        private string[] _tokens;
    }
}
