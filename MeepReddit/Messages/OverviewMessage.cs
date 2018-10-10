using System;
using System.Linq;
using System.Runtime.Serialization;

using RedditSharp;
using RedditSharp.Things;

using MeepLib.Messages;
using System.Collections.Generic;

namespace MeepReddit.Messages
{
    /// <summary>
    /// User overview
    /// </summary>
    /// <remarks>Inherits from Batch so the post history can be broken out and 
    /// processed downstream.</remarks>
    [DataContract]
    public class OverviewMessage : Batch, IThingMessage, ITokenisable
    {
        [DataMember]
        public RedditUser User { get; set; }

        [DataMember]
        public Thing Thing
        {
            get
            {
                return User as Thing;
            }
        }

        /// <summary>
        /// Tokenise to the names of subreddits the user has history in
        /// </summary>
        /// <value>The tokens.</value>
        /// <remarks>If training a classifier on the content of the messages
        /// rather than where they were posted, you'd need to unbatch them first.</remarks>
        public IEnumerable<string> Tokens 
        {
            get {
                return (from m in Messages
                        let vm = m as VotableMessage
                        where vm != null
                        let p = vm.Thing as Post
                        let c = vm.Thing as Comment
                        let subreddit = p?.SubredditName ?? c?.Subreddit
                        where subreddit != null
                        select subreddit).ToArray();
            }
        }
    }
}
