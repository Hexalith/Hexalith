// <copyright file="BaseTestEvent2.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Tests.Core.Infrastructure.DaprEventStore;

using System.Runtime.Serialization;

[DataContract]
public class BaseTestEvent2 : BaseTestEvent
{
    public BaseTestEvent2(string id, string value2)
        : base(id)
    {
    }
}
