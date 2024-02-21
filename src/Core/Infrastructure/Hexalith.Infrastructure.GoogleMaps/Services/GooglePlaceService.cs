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
using GoogleApi.Entities.Maps.Geocoding.Common;
using GoogleApi.Entities.Maps.Geocoding.Place.Request;
using GoogleApi.Entities.Places.AutoComplete.Request;
using GoogleApi.Entities.Places.AutoComplete.Response;

using Hexalith.Application.Geolocations.Services;
using Hexalith.Domain.ValueObjets;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.GoogleMaps.Abstractions.Configurations;

using Microsoft.Extensions.Caching.Memory;
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
public class GooglePlaceService : IPlaceService
{
    private readonly string _apiKey;
    private readonly GooglePlaces.AutoCompleteApi _autoCompleteService;
    private readonly IMemoryCache _cache;
    private readonly GoogleMaps.Geocode.PlaceGeocodeApi _geocodeService;

    /// <summary>
    /// Initializes a new instance of the <see cref="GooglePlaceService"/> class.
    /// </summary>
    /// <param name="cache">The cache.</param>
    /// <param name="autoCompleteService">The automatic complete service.</param>
    /// <param name="geocodeService">The geocode service.</param>
    /// <param name="settings">The settings.</param>
    /// <exception cref="System.ArgumentNullException">null.</exception>
    public GooglePlaceService(
        IMemoryCache cache,
        GooglePlaces.AutoCompleteApi autoCompleteService,
        GoogleMaps.Geocode.PlaceGeocodeApi geocodeService,
        IOptions<GoogleSettings> settings)
    {
        ArgumentNullException.ThrowIfNull(cache);
        ArgumentNullException.ThrowIfNull(autoCompleteService);
        ArgumentNullException.ThrowIfNull(geocodeService);
        ArgumentNullException.ThrowIfNull(settings);
        SettingsException<GoogleSettings>.ThrowIfNullOrEmpty(settings.Value.ApiKey);
        _apiKey = settings.Value.ApiKey;
        _cache = cache;
        _autoCompleteService = autoCompleteService;
        _geocodeService = geocodeService;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<PlaceDescription>> GetAutocompleteOptionsAsync(
        string search,
        string cultureCode,
        int maxResultCount,
        double? latitude,
        double? longitude,
        CancellationToken cancellationToken)
        => await GetPlacesAutocompleteAsync(
                search,
                cultureCode ?? throw new ArgumentNullException(nameof(cultureCode)),
                latitude,
                longitude,
                cancellationToken)
            .ConfigureAwait(false);

    /// <inheritdoc/>
    public async Task<string> GetAutocompleteTextAsync(
        string search,
        string cultureCode,
        int maxResultCount,
        double? latitude,
        double? longitude,
        CancellationToken cancellationToken)
        => (await GetPlacesAutocompleteAsync(
                search,
                cultureCode ?? throw new ArgumentNullException(nameof(cultureCode)),
                latitude,
                longitude,
                cancellationToken)
        .ConfigureAwait(false))
        .FirstOrDefault()?.Description ?? "No results";

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
        ArgumentException.ThrowIfNullOrWhiteSpace(placeId);
        if (_cache.TryGetValue(placeId, out Domain.ValueObjets.PostalAddress? postalAddress))
        {
            if (postalAddress != null)
            {
                return postalAddress;
            }
        }

        GeocodeResponse result = await _geocodeService
            .QueryAsync(new PlaceGeocodeRequest { PlaceId = placeId, Key = _apiKey }, cancellationToken)
            .ConfigureAwait(false);

        if (result.Status != Status.Ok)
        {
            throw new InvalidOperationException("The postal address cannot be retrieved from Google services : " + result.ErrorMessage);
        }

        Result? address = result.Results.FirstOrDefault()
            ?? throw new InvalidOperationException("The postal address was not found by Google services.");

        string? iso2 = address.AddressComponents.FirstOrDefault(p => p.Types.Contains(AddressComponentType.Country))?.ShortName;
        postalAddress = new Domain.ValueObjets.PostalAddress(
            null,
            null,
            address.AddressComponents.FirstOrDefault(p => p.Types.Contains(AddressComponentType.Street_Number))?.LongName,
            address.AddressComponents.FirstOrDefault(p => p.Types.Contains(AddressComponentType.Route))?.LongName,
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
        return _cache.Set(placeId, postalAddress, TimeSpan.FromDays(1));
    }

    private static string? ConvertToIso3(string? countryIso2)
    {
        return ISO3166.Country.List
            .FirstOrDefault(p => string.Equals(p.TwoLetterCode, countryIso2, StringComparison.OrdinalIgnoreCase))
            ?.ThreeLetterCode;
    }

    private static string GetSearchCacheId(string search, string cultureCode, double? latitude, double? longitude)
    {
        string id = $"{search}/{cultureCode}";
        if (latitude != null && latitude.Value != 0 && longitude != null && longitude.Value != 0)
        {
            id += $"/{latitude}-{longitude}";
        }

        return id;
    }

    private async Task<IEnumerable<PlaceDescription>> GetPlacesAutocompleteAsync(
                string search,
                string cultureCode,
                double? latitude,
                double? longitude,
                CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(search))
        {
            return [];
        }

        string cacheId = GetSearchCacheId(search, cultureCode, latitude, longitude);
        if (_cache.TryGetValue(cacheId, out IEnumerable<PlaceDescription>? places))
        {
            if (places != null)
            {
                return places;
            }
        }

        Language language = cultureCode.Equals("fr", StringComparison.OrdinalIgnoreCase)
            ? Language.French
            : Language.English;
        PlacesAutoCompleteRequest placesRequest = new()
        {
            Input = search,
            Language = language,
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
        places = result.Predictions.Select(p => new PlaceDescription
        {
            Id = p.PlaceId,
            Description = p.Description,
        });
        return _cache.Set(cacheId, places, TimeSpan.FromDays(1));
    }
}