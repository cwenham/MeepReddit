using System;
using System.Collections.Generic;

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
                    Agents.TryGetValue(AppKey, out Agent);

                    if (Agent == null)
                    {
                        MessageContext context = new MessageContext(null, this);
                        string user = Smart.Format(User.Replace("#Login", Login), context);
                        string pass = Smart.Format(Pass.Replace("#Login", Login), context);
                        string clientID = Smart.Format(ClientID.Replace("#AppKey", AppKey), context);
                        string clientSecret = Smart.Format(ClientSecret.Replace("#AppKey", AppKey), context);
                        string redirectURI = Smart.Format(RedirectURI.Replace("#AppKey", AppKey), context);

                        Agent = new BotWebAgent(user, pass, clientID, clientSecret, redirectURI);
                        Agents.Add(AppKey, Agent);
                    }

                    _client = new Reddit(Agent);
                }

                return _client;
            }
        }
        private Reddit _client;

        protected BotWebAgent Agent;

        /// <summary>
        /// Static collection of agents
        /// </summary>
        /// <remarks>We need to maintain the same instance for each login so it
        /// can enforce API polling limits and avoid having reddit shut us off.</remarks>
        protected static Dictionary<string, BotWebAgent> Agents = new Dictionary<string, BotWebAgent>();

        /// <summary>
        /// Name of AppKey config element
        /// </summary>
        /// <value>The AppKey config name.</value>
        public string AppKey { get; set; } = "MeepReddit";

        /// <summary>
        /// Name of Login config element with username and password
        /// </summary>
        /// <value>The login config name.</value>
        public string Login { get; set; } = "RedditUser";

        // For the following, we're going to replace the value of #Login and
        // #AppKey with the above at runtime. This is to allow a mix of
        // configuration styles, from outright setting the user/pass in place
        // to using the safer method of AppKey and Login elements in a separate
        // file XIncluded into the main.

        /// <summary>
        /// Explicitly set the username, in {Smart.Format}
        /// </summary>
        /// <value>The user.</value>
        /// <remarks>Usually left unchanged if you're using the Login
        /// attribute to name a &lt;Login&gt; element.</remarks>
        public string User { get; set; } = "{cfg.#Login.User}";

        /// <summary>
        /// Explicitly set the password, in {Smart.Format}
        /// </summary>
        /// <value>The password.</value>
        /// <remarks>Usually left unchanged if you're using the Login attribute.</remarks>
        public string Pass { get; set; } = "{cfg.#Login.Pass}";

        /// <summary>
        /// Client ID
        /// </summary>
        /// <value>The reddit client identifier.</value>
        /// <remarks>Usually left unchanged if you're using the AppKey attribute
        /// to name a &lt;AppKey&gt; element.</remarks>
        public string ClientID { get; set; } = "{cfg.#AppKey.ClientID}";

        /// <summary>
        /// Client secret
        /// </summary>
        /// <value>The reddit client secret.</value>
        /// <remarks>Usually left unchanged if you're using the AppKey attribute.</remarks>
        public string ClientSecret { get; set; } = "{cfg.#AppKey.ClientSecret}";

        /// <summary>
        /// Gets or sets the redirect URI.
        /// </summary>
        /// <value>The redirect URI.</value>
        /// <remarks>Usually left unchanged if you're using the AppKey attribute.</remarks>
        public string RedirectURI { get; set; } = "{cfg.#AppKey.RedirectURI}";

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

        protected Subreddit GetSub(string subName)
        {
            var subTask = Client.GetSubredditAsync(subName);
            subTask.Wait();
            return subTask.Result;
        }

        protected PostMessage ConvertToPost(Thing thing)
        {
            return new PostMessage
            {
                Post = thing as Post,
                Name = this.Name
            };
        }

        protected VotableMessage ConvertToVotable(Thing thing)
        {
            return new VotableMessage
            {
                Votable = thing as VotableThing,
                Name = this.Name
            };
        }

        protected CommentMessage ConvertToComment(Thing thing)
        {
            return new CommentMessage
            {
                Comment = thing as Comment,
                Name = this.Name
            };
        }

        protected PMMessage ConvertToPM(Thing thing)
        {
            return new PMMessage
            {
                Message = thing as PrivateMessage,
                Name = this.Name
            };
        }

        protected ModActionMessage ConvertToModAction(Thing thing)
        {
            return new ModActionMessage
            {
                Action = thing as ModAction,
                Name = this.Name
            };
        }
    }
}
