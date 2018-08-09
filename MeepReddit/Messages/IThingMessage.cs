using System;

using RedditSharp.Things;

namespace MeepReddit.Messages
{
    public interface IThingMessage
    {
        Thing Thing { get; }
    }
}
