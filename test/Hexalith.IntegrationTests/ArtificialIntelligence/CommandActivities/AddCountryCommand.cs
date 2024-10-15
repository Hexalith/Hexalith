﻿// <copyright file="AddCountryCommand.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.IntegrationTests.ArtificialIntelligence.CommandActivities;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using Hexalith.Extensions.Common;
using Hexalith.PolymorphicSerialization;

[Display(Name = "Add a country", Description = "Add a new country")]
[ExampleName("France")]
[PolymorphicSerialization]
public record AddCountryCommand
{
    [JsonConstructor]
    public AddCountryCommand(string iso2, string iso3, int isoNumber, string name, string currencyName, string currencySymbol, string currencyCode)
    {
        Iso2 = iso2;
        Iso3 = iso3;
        IsoNumber = isoNumber;
        Name = name;
        CurrencyName = currencyName;
        CurrencySymbol = currencySymbol;
        CurrencyCode = currencyCode;
    }

    [Obsolete("For serialization only", error: true)]
    public AddCountryCommand() => Iso2 = Iso3 = Name = CurrencyName = CurrencySymbol = string.Empty;

    [Display(Name = "Currency code", Description = "ISO 4217 currency code")]
    [ExampleValue("EUR")]
    [Required]
    public string CurrencyCode { get; set; }

    [Display(Name = "Currency name", Description = "Currency name")]
    [ExampleValue("Euro")]
    public string CurrencyName { get; set; }

    [Display(Name = "Currency symbol", Description = "Currency symbol")]
    [ExampleValue("€")]
    [Required]
    public string CurrencySymbol { get; set; }

    [Display(Name = "ISO2 Code", Description = "ISO 3166-1 alpha-2 code")]
    [ExampleValue("FR")]
    [StringLength(2)]
    public string Iso2 { get; set; }

    [Required]
    [Display(Name = "ISO3 Code", Description = "ISO 3166-1 alpha-3 code")]
    [ExampleValue("FRA")]
    [StringLength(3)]
    public string Iso3 { get; set; }

    [Display(Name = "ISO Number", Description = "ISO 3166-1 numeric code")]
    [ExampleValue(250)]
    public int IsoNumber { get; set; }

    [Required]
    [ExampleValue("France")]
    public string Name { get; set; }

    public string AggregateId => Iso3;

    public string AggregateName => "Country";
}