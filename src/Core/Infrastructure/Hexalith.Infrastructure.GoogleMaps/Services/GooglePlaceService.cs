// <copyright file="GooglePlaceService.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.GoogleMaps.Services;

using System.Threading.Tasks;

using GoogleApi;
using GoogleApi.Entities.Common;
using GoogleApi.Entities.Common.Enums;
using GoogleApi.Entities.Maps.Geocoding;
using GoogleApi.Entities.Maps.Geocoding.Place.Request;
using GoogleApi.Entities.Places.AutoComplete.Request;
using GoogleApi.Entities.Places.AutoComplete.Response;

using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.GoogleMaps.Abstractions.Configurations;
using Hexalith.Infrastructure.GoogleMaps.Models;

using Microsoft.Extensions.Options;

/// <summary>
/// Class GooglePlaceService.
/// Implements the <see cref="FluentUILocation.IGooglePlaceService" />.
/// </summary>
/// <seealso cref="FluentUILocation.IGooglePlaceService" />
/// <remarks>
/// Initializes a new instance of the <see cref="GooglePlaceService" /> class.
/// </remarks>
/// <param name="jsRuntime">The js runtime.</param>
public class GooglePlaceService : IGooglePlaceService
{
    private readonly string _apiKey;
    private readonly GooglePlaces.AutoCompleteApi _autoCompleteService;
    private readonly GoogleMaps.Geocode.PlaceGeocodeApi _geocodeService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GooglePlaceService"/> class.
    /// </summary>
    /// <param name="addressValidationService">The address validation service.</param>
    /// <param name="autoCompleteService">The automatic complete service.</param>
    /// <param name="queryAutoCompleteService">The query automatic complete service.</param>
    /// <param name="geocodeService">The geocode service.</param>
    /// <param name="settings">The settings.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public GooglePlaceService(
        GooglePlaces.AutoCompleteApi autoCompleteService,
        GoogleMaps.Geocode.PlaceGeocodeApi geocodeService,
        IOptions<GoogleSettings> settings)
    {
        ArgumentNullException.ThrowIfNull(autoCompleteService);
        ArgumentNullException.ThrowIfNull(geocodeService);
        ArgumentNullException.ThrowIfNull(settings);
        SettingsException<GoogleSettings>.ThrowIfNullOrEmpty(settings.Value.ApiKey);
        _apiKey = settings.Value.ApiKey;
        _autoCompleteService = autoCompleteService;
        _geocodeService = geocodeService;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<GooglePlace>> GetAutocompleteOptionsAsync(
        string search,
        string cultureCode,
        int maxResultCount,
        double? latitude,
        double? longitude,
        CancellationToken cancellationToken)
        => await GetPlacesAutocompleteAsync(search, cultureCode, longitude, latitude, cancellationToken).ConfigureAwait(false);

    /// <inheritdoc/>
    public async Task<string> GetAutocompleteTextAsync(
        string search,
        string cultureCode,
        int maxResultCount,
        double? latitude,
        double? longitude,
        CancellationToken cancellationToken)
        => (await GetPlacesAutocompleteAsync(search, cultureCode, longitude, latitude, cancellationToken).ConfigureAwait(false)).FirstOrDefault()?.Description ?? "No results";

    /// <summary>
    /// Get postal address as an asynchronous operation.
    /// </summary>
    /// <param name="placeId">The place identifier.</param>
    /// <param name="cancellationToken">The cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <returns>A Task&lt;PostalAddress&gt; representing the asynchronous operation.</returns>
    /// <exception cref="System.InvalidOperationException">The postal address cannot be retrieved from Google services : " + result.ErrorMessage.</exception>
    /// <exception cref="System.InvalidOperationException">The postal address was not found by Google services.</exception>
    public async Task<Domain.ValueObjets.PostalAddress> GetPostalAddressAsync(string placeId, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(placeId);
        GeocodeResponse result = await _geocodeService
            .QueryAsync(new PlaceGeocodeRequest { PlaceId = placeId, Key = _apiKey }, cancellationToken)
            .ConfigureAwait(false);

        if (result.Status != Status.Ok)
        {
            throw new InvalidOperationException("The postal address cannot be retrieved from Google services : " + result.ErrorMessage);
        }

        GoogleApi.Entities.Maps.Geocoding.Common.Result? address = result.Results.FirstOrDefault();
        if (address is null)
        {
            throw new InvalidOperationException("The postal address was not found by Google services.");
        }

        string? iso2 = address.AddressComponents.FirstOrDefault(p => p.Types.Contains(AddressComponentType.Country))?.ShortName;
        return new Domain.ValueObjets.PostalAddress(
            null,
            null,
            address.AddressComponents.FirstOrDefault(p => p.Types.Contains(AddressComponentType.Street_Number))?.LongName,
            address.AddressComponents.FirstOrDefault(p => p.Types.Contains(AddressComponentType.Street_Address))?.LongName,
            address.AddressComponents.FirstOrDefault(p => p.Types.Contains(AddressComponentType.Post_Box))?.LongName,
            address.AddressComponents.FirstOrDefault(p => p.Types.Contains(AddressComponentType.Postal_Code))?.LongName,
            address.AddressComponents.FirstOrDefault(p => p.Types.Contains(AddressComponentType.Locality))?.LongName,
            address.AddressComponents.FirstOrDefault(p => p.Types.Contains(AddressComponentType.Administrative_Area_Level_2))?.LongName,
            address.AddressComponents.FirstOrDefault(p => p.Types.Contains(AddressComponentType.Administrative_Area_Level_1))?.ShortName,
            address.AddressComponents.FirstOrDefault(p => p.Types.Contains(AddressComponentType.Administrative_Area_Level_1))?.LongName,
            ConvertToIso3(iso2),
            address.AddressComponents.FirstOrDefault(p => p.Types.Contains(AddressComponentType.Country))?.LongName,
            iso2,
            address.Geometry.Location.Latitude,
            address.Geometry.Location.Longitude,
            address.PlaceId,
            address.FormattedAddress);
    }

    private static string? ConvertToIso3(string? countryIso2)
    {
        return ISO3166.Country.List
            .FirstOrDefault(p => string.Equals(p.TwoLetterCode, countryIso2, StringComparison.OrdinalIgnoreCase))
            ?.ThreeLetterCode;
    }

    private async Task<IEnumerable<GooglePlace>> GetPlacesAutocompleteAsync(
            string search,
            string cultureCode,
            double? longitude,
            double? latitude,
            CancellationToken cancellationToken)
    {
        PlacesAutoCompleteRequest placesRequest = new()
        {
            Input = search,
            Language = Enum.Parse<Language>(cultureCode, true),
            Key = _apiKey,
        };
        if (longitude != null && latitude != null)
        {
            placesRequest.Location = new Coordinate(latitude.Value, longitude.Value);
        }

        PlacesAutoCompleteResponse result = await _autoCompleteService
            .QueryAsync(placesRequest, cancellationToken)
            .ConfigureAwait(false);

        // Initialize the Google Places client
        return result.Predictions.Select(p => new GooglePlace
        {
            Id = p.PlaceId,
            Description = p.Description,
            Address = p.StructuredFormatting.MainText,
        });
    }
}