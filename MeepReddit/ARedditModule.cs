using System;

using SmartFormat;
using RedditSharp;
using RedditSharp.Things;

using MeepLib;

using MeepReddit.Messages;

namespace MeepReddit
{
    public abstract class ARedditModule : AMessageModule
    {
        public const string PluginNamespace = "http://meep.example.com/MeepReddit/V1";

        public Reddit Client
        {
            get
            {
                if (_client == null)
                {
                    MessageContext context = new MessageContext(null, this);
                    string user = Smart.Format(User, context);
                    string pass = Smart.Format(Pass, context);
                    string clientID = Smart.Format(ClientID, context);
                    string clientSecret = Smart.Format(ClientSecret, context);
                    string redirectURI = Smart.Format(RedirectURI, context);

                    _agent = new BotWebAgent(user, pass, clientID, clientSecret, redirectURI);
                    _client = new Reddit(_agent);
                }

                return _client;
            }
        }
        private BotWebAgent _agent;
        private Reddit _client;

        public string User { get; set; } = "{cfg.RedditUser.User}";

        public string Pass { get; set; } = "{cfg.RedditUser.Pass}";

        /// <summary>
        /// Client ID
        /// </summary>
        /// <value>The reddit client identifier.</value>
        public string ClientID { get; set; } = "{cfg.MeepReddit.ClientID}";

        /// <summary>
        /// Client secret
        /// </summary>
        /// <value>The reddit client secret.</value>
        /// <remarks>Don't set this directly, use a {Smart.Formatted} reference
        /// to a configuration key.</remarks>
        public string ClientSecret { get; set; } = "{cfg.MeepReddit.ClientSecret}";

        public string RedirectURI { get; set; } = "{cfg.MeepReddit.RedirectURI}";

        /// <summary>
        /// Reddit's main URL
        /// </summary>
        /// <value>The base URL.</value>
        /// <remarks>Will probably never change unless you're connecting to a
        /// beta endpoint or a site that exposes the same API.</remarks>
        public string BaseURL { get; set; } = "https://www.reddit.com";

        /// <summary>
        /// Subreddit to read posts from
        /// </summary>
        /// <value>The subreddit.</value>
        public string Subreddit { get; set; } = "/r/all";

        /// <summary>
        /// Unique name with developer's username
        /// </summary>
        /// <value>The user agent.</value>
        /// <remarks>Developer's username is required by reddit so they can
        /// contact the creator in case of problems.</remarks>
        public string UserAgent { get; set; } = "MeepReddit v0.1 (by /u/cwenham)";

        protected PostMessage ConvertToPost(Thing thing)
        {
            return new PostMessage
            {
                Post = thing as Post
            };
        }

        protected VotableMessage ConvertToVotable(Thing thing)
        {
            return new VotableMessage
            {
                Votable = thing as VotableThing
            };
        }

        protected CommentMessage ConvertToComment(Thing thing)
        {
            return new CommentMessage
            {
                Comment = thing as Comment
            };
        }

        protected PMMessage ConvertToPM(Thing thing)
        {
            return new PMMessage
            {
                Message = thing as PrivateMessage
            };
        }

        protected ModActionMessage ConvertToModAction(Thing thing)
        {
            return new ModActionMessage
            {
                Action = thing as ModAction
            };
        }
    }
}
