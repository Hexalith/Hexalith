// <copyright file="DummyBaseNotification.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Notifications;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Application.Metadatas;
using Hexalith.Application.Notifications;
using Hexalith.Extensions.Helpers;

[DataContract]
public abstract class DummyBaseNotification : BaseNotification
{
    [Obsolete("For serialization only", true)]
    protected DummyBaseNotification()
        : base("1233", "Customer", "Cust353", "Test", "Dummy message", NotificationSeverity.Information, "Dummy technical description") => BaseValue = string.Empty;

    [JsonConstructor]
    protected DummyBaseNotification(string baseValue)
        : base("1233", "Customer", "Cust353", "Test", "Dummy message", NotificationSeverity.Information, "Dummy technical description") => BaseValue = baseValue;

    public string BaseValue { get; }

    public Metadata CreateMetadata()
    {
        return new Metadata(
                UniqueIdHelper.GenerateUniqueStringId(),
                this,
                DateTimeOffset.UtcNow,
                new ContextMetadata(
                    UniqueIdHelper.GenerateUniqueStringId(),
                    "Test user",
                    DateTimeOffset.UtcNow.AddMinutes(-1),
                    1,
                    "Test session"),
                ["TestScope"]);
    }

    protected override string DefaultAggregateName() => "Test";
}