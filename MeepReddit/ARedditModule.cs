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

        /// <summary>
        /// First 32 bits of Reddit Message ID GUIDs
        /// </summary>
        /// <remarks>All Messages that trace back to a Reddit Thing have IDs based on this prefix and
        /// the Reddit ThingID.
        ///
        /// <para>Derives from ASCII values; 77 = M, 80 = P, 82 = R, 84 = T</para></remarks>
        public const Int32 GUIDPrefix = 77808284;

        /// <summary>
        /// Lowercase Base36 alphabet
        /// </summary>
        /// <remarks>Should be safe to assume it's always returned in lowercase by their API, so we can save the cost
        /// of casting it.</remarks>
        public const string Alphabet_base36_lowercase = "0123456789abcdefghijklmnopqrstuvwxyz";

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

        /// <summary>
        /// Encode the ThingID as a GUID with a globally static prefix
        /// </summary>
        /// <param name="thingID"></param>
        /// <returns></returns>
        /// <remarks>A Thing is the same Thing no matter who fetched it or what account they were logged in with,
        /// so by using guaranteed identical GUIDs as the Message.ID we can adopt a simple "ignore dupe" policy when
        /// merging databases or streams, and that means we can increase API throughput by having many clients reading
        /// from the API simultaneously without having to worry about resolving overlap.
        ///
        /// <para>The first 32 bits will be the fixed GUIDPrefix, next 16 bits are zero, next 16 bits are the
        /// thing type, and the last 64 bits is the ThingID converted to a ulong.</para>
        ///
        /// <para>E.G.: "t3_c2a2to" converts to 04a3429c-0000-0003-0000-00002b7a1ecc</para></remarks>
        public Guid ThingIDToGuid(string thingID)
        {
            var thingSpan = thingID.AsSpan();

            int ixSeparator = thingSpan.IndexOf('_');
            ReadOnlySpan<char> type = thingSpan.Slice(0, ixSeparator);
            ReadOnlySpan<char> val = thingSpan.Slice(ixSeparator + 1);

            short thingType = (short)(type[1] - 48); // Only 6 kinds of type so far, so we only have to convert 1 char
            var lngVal = BaseNToLong(val, Alphabet_base36_lowercase);
            if (BitConverter.IsLittleEndian)
                lngVal = System.Net.IPAddress.HostToNetworkOrder(lngVal);
            byte[] byteVal = BitConverter.GetBytes(lngVal);

            return new Guid(GUIDPrefix, 0, thingType, byteVal);
        }

        public static long BaseNToLong(ReadOnlySpan<char> value, string alphabet)
        {
            long result = 0;

            int pos = 0;
            for (int i = value.Length - 1; i >= 0; i--)
            {
                int idx = alphabet.IndexOf(value[i]);
                if (idx >= 0)
                    result += idx * (long)Math.Pow(alphabet.Length, pos);
                else
                    throw new InvalidOperationException(String.Format("Invalid character {0}", value[pos]));
                pos++;
            }

            return result;
        }

        protected PostMessage ConvertToPost(Thing thing)
        {
            return new PostMessage
            {
                ID = ThingIDToGuid(thing.Id),
                Post = thing as Post,
                Name = this.Name
            };
        }

        protected VotableMessage ConvertToVotable(Thing thing)
        {
            return new VotableMessage
            {
                ID = ThingIDToGuid(thing.Id),
                Votable = thing as VotableThing,
                Name = this.Name
            };
        }

        protected CommentMessage ConvertToComment(Thing thing)
        {
            return new CommentMessage
            {
                ID = ThingIDToGuid(thing.Id),
                Comment = thing as Comment,
                Name = this.Name
            };
        }

        protected PMMessage ConvertToPM(Thing thing)
        {
            return new PMMessage
            {
                ID = ThingIDToGuid(thing.Id),
                Message = thing as PrivateMessage,
                Name = this.Name
            };
        }

        protected ModActionMessage ConvertToModAction(Thing thing)
        {
            return new ModActionMessage
            {
                ID = ThingIDToGuid(thing.Id),
                Action = thing as ModAction,
                Name = this.Name
            };
        }
    }
}
