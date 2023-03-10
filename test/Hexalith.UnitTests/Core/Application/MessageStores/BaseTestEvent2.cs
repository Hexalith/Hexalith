// <copyright file="BaseTestEvent2.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.MessageStores;

using System.Runtime.Serialization;

[DataContract]
public class BaseTestEvent2 : BaseTestEvent
{
    public string Value2 { get; set; }
}