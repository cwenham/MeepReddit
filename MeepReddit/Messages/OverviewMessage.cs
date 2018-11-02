using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using RedditSharp;
using RedditSharp.Things;

using MeepLib;
using MeepLib.Messages;

using System.Collections.Generic;

namespace MeepReddit.Messages
{
    /// <summary>
    /// User overview
    /// </summary>
    /// <remarks>Inherits from Batch so the post history can be broken out and 
    /// processed downstream.</remarks>
    [DataContract, Table("Users")]
    public class OverviewMessage : Batch, IThingMessage, ITokenisable
    {
        [DataMember, NotMapped]
        public RedditUser User { get; set; }

        [DataMember, NotMapped]
        public Thing Thing
        {
            get
            {
                return User as Thing;
            }
        }

        [MaxLength(20), Key]
        public string FullName
        {
            get
            {
                return User.FullName;
            }
        }

        [Index(IsUnique = false)]
        public DateTime ThingCreated
        {
            get
            {
                return User.CreatedUTC;
            }
        }

        [Index(IsUnique = false)]
        public bool Gold
        {
            get
            {
                return User.HasGold;
            }
        }

        [Index(IsUnique = false)]
        public int LinkKarma
        {
            get
            {
                return User.LinkKarma;
            }
        }

        [Index(IsUnique = false)]
        public int CommentKarma
        {
            get
            {
                return User.CommentKarma;
            }
        }

        [Index(IsUnique = false)]
        public bool Verified
        {
            get
            {
                return User.IsVerified;
            }
        }

        /// <summary>
        /// Tokenise to the names of subreddits the user has history in
        /// </summary>
        /// <value>The tokens.</value>
        /// <remarks>If training a classifier on the content of the messages
        /// rather than where they were posted, you'd need to unbatch them first.</remarks>
        [NotMapped]
        public IEnumerable<string> Tokens
        {
            get
            {
                if (_tokens == null)
                    _tokens = (from m in Messages
                               let p = m as PostMessage
                               let c = m as CommentMessage
                               let subreddit = p?.Subreddit ?? c?.Subreddit
                               where subreddit != null
                               select subreddit).ToArray();

                return _tokens;
            }
        }
        private string[] _tokens;
    }
}
