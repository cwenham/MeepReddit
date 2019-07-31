using System;

using RedditSharp.Things;

namespace MeepReddit.Messages
{
    public interface IThingMessage
    {
        Thing Thing { get; }

        /// <summary>
        /// The author of a post, comment, or the target of a mod action
        /// </summary>
        string TargetAuthor { get; }
    }
}
