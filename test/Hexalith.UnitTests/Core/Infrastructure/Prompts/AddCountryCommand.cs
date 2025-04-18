// <copyright file="AddCountryCommand.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Infrastructure.Prompts;

using System.ComponentModel.DataAnnotations;

using Hexalith.Extensions.Common;
using Hexalith.PolymorphicSerializations;

[PolymorphicSerialization]
[Display(Name = "Add a country", Description = "Add a new country")]
public partial record AddCountryCommand(
    [property: Display(Name = "ISO2 Code", Description = "ISO 3166-1 alpha-2 code")]
    [property: ExampleValue("FR")]
    [property: StringLength(2)]
    string Iso2,
    [property: Required]
    [property : Display(Name = "ISO3 Code", Description = "ISO 3166-1 alpha-3 code")]
    [property : ExampleValue("FRA")]
    [property : StringLength(3)]
    string Iso3,
    [property : Display(Name = "ISO Number", Description = "ISO 3166-1 numeric code")]
    [property : ExampleValue(250)]
    int IsoNumber,
    [property : Required]
    [property : ExampleValue("France")]
    string Name,
    [property: Display(Name = "Currency name", Description = "Currency name")]
    [property: ExampleValue("Euro")]
    string CurrencyName,
    [property : Display(Name = "Currency symbol", Description = "Currency symbol")]
    [property : ExampleValue("€")]
    [property: Required]
    string CurrencySymbol)
{
    public AddCountryCommand()
        : this(string.Empty, string.Empty, 0, string.Empty, string.Empty, string.Empty)
    {
    }

    public string AggregateId => Iso3;

    public string AggregateName => "Country";
}