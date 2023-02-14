namespace Hexalith.Infrastructure.DaprHandlers.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Dapr.Actors;

public static class ActorHelper
{
    public static ActorId ToActorId(this string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        return new ActorId(System.Net.WebUtility.UrlEncode(id));
    }
}
