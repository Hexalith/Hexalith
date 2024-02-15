// <copyright file="PostalAddressViewModel.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.PostalAddresses.Models;

using System.ComponentModel.DataAnnotations;

using Hexalith.Domain.ValueObjets;

/// <summary>
/// Postal address model.
/// </summary>
public class PostalAddressViewModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PostalAddressViewModel"/> class.
    /// </summary>
    public PostalAddressViewModel()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PostalAddressViewModel"/> class.
    /// </summary>
    /// <param name="postalAddress">The postal address.</param>
    public PostalAddressViewModel(PostalAddress postalAddress)
    {
        if (postalAddress is null)
        {
            return;
        }

        City = postalAddress.City;
        CountryId = postalAddress.CountryId;
        CountryIso2 = postalAddress.CountryIso2;
        CountryName = postalAddress.CountryName;
        CountyId = postalAddress.CountyId;
        Description = postalAddress.Description;
        FormattedAddress = postalAddress.FormattedAddress;
        Latitude = postalAddress.Latitude;
        Longitude = postalAddress.Longitude;
        Name = postalAddress.Name;
        PlaceId = postalAddress.PlaceId;
        PostBox = postalAddress.PostBox;
        StateId = postalAddress.StateId;
        StateName = postalAddress.StateName;
        Street = postalAddress.Street;
        StreetNumber = postalAddress.StreetNumber;
        ZipCode = postalAddress.ZipCode;
    }

    /// <summary>
    /// Gets or sets the city.
    /// </summary>
    /// <value>The city.</value>
    [Display(Prompt = "Paris")]
    public string? City { get; set; }

    /// <summary>
    /// Gets or sets the country identifier.
    /// </summary>
    /// <value>The country identifier.</value>
    [StringLength(3, MinimumLength = 3)]
    [Required]
    [Display(Description = "Country ISO 3", Name = "Country ISO 3166-3 code", Prompt = "FRA")]
    public string? CountryId { get; set; }

    /// <summary>
    /// Gets or sets the country identifier.
    /// </summary>
    /// <value>The country identifier.</value>
    [StringLength(3, MinimumLength = 2)]
    [Display(Description = "Country ISO 2", Name = "Country ISO 3166-2 code", Prompt = "FR")]
    public string? CountryIso2 { get; set; }

    /// <summary>
    /// Gets or sets the country identifier.
    /// </summary>
    /// <value>The country identifier.</value>
    [Display(Description = "Country name", Name = "Country name", Prompt = "France")]
    public string? CountryName { get; set; }

    /// <summary>
    /// Gets or sets the county identifier.
    /// </summary>
    /// <value>The county identifier.</value>
    public string? CountyId { get; set; }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>The description.</value>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the formatted address.
    /// </summary>
    /// <value>The formatted address.</value>
    public string? FormattedAddress { get; set; }

    /// <summary>
    /// Gets or sets the latitude.
    /// </summary>
    /// <value>The latitude.</value>
    public double? Latitude { get; set; }

    /// <summary>
    /// Gets or sets the longitude.
    /// </summary>
    /// <value>The longitude.</value>
    public double? Longitude { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>The name.</value>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the place identifier.
    /// </summary>
    /// <value>The place identifier.</value>
    [Display(Name = "Place identifier", Description = "Place unique identifier", Prompt = "identifier")]
    public string? PlaceId { get; set; }

    /// <summary>
    /// Gets or sets the post box.
    /// </summary>
    /// <value>The post box.</value>
    public string? PostBox { get; set; }

    /// <summary>
    /// Gets or sets the state identifier.
    /// </summary>
    /// <value>The state identifier.</value>
    public string? StateId { get; set; }

    /// <summary>
    /// Gets or sets the state identifier.
    /// </summary>
    /// <value>The state identifier.</value>
    public string? StateName { get; set; }

    /// <summary>
    /// Gets or sets the street.
    /// </summary>
    /// <value>The street.</value>
    public string? Street { get; set; }

    /// <summary>
    /// Gets or sets the street number.
    /// </summary>
    /// <value>The street number.</value>
    public string? StreetNumber { get; set; }

    /// <summary>
    /// Gets or sets the zip code.
    /// </summary>
    /// <value>The zip code.</value>
    [Required]
    [DataType(DataType.PostalCode)]
    [Display(Name = "Zip code", Description = "Postal code", Prompt = "75000")]
    public string? ZipCode { get; set; }

    /// <summary>
    /// Converts the PostalAddressModel to a PostalAddress object.
    /// </summary>
    /// <returns>The PostalAddress object.</returns>
    public PostalAddress ToPostalAddress()
    {
        return new PostalAddress
        {
            City = City,
            CountryId = CountryId,
            CountryIso2 = CountryIso2,
            CountryName = CountryName,
            CountyId = CountyId,
            Description = Description,
            FormattedAddress = FormattedAddress,
            Latitude = Latitude,
            Longitude = Longitude,
            Name = Name,
            PlaceId = PlaceId,
            PostBox = PostBox,
            StateId = StateId,
            StateName = StateName,
            Street = Street,
            StreetNumber = StreetNumber,
            ZipCode = ZipCode,
        };
    }
}