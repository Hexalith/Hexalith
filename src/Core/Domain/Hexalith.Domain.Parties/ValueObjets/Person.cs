// ***********************************************************************
// Assembly         : Hexalith.Domain.Parties
// Author           : Jérôme Piquot
// Created          : 09-12-2023
//
// Last Modified By : Jérôme Piquot
// Last Modified On : 12-04-2023
// ***********************************************************************
// <copyright file="Person.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Hexalith.Domain.ValueObjets;

using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

using Hexalith.Extensions;

/// <summary>
/// Class Person.
/// </summary>
[DataContract]
[Serializable]
public class Person
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Person" /> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="firstName">The first name.</param>
    /// <param name="lastName">The last name.</param>
    /// <param name="title">The title.</param>
    /// <param name="birthDate">The birth date.</param>
    /// <param name="gender">The gender.</param>
    [JsonConstructor]
    public Person(
        string? name,
        string? firstName,
        string? lastName,
        string? title,
        DateTimeOffset? birthDate,
        Gender? gender)
    {
        Name = name;
        FirstName = firstName;
        LastName = lastName;
        Title = title;
        BirthDate = birthDate;
        Gender = gender;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Person"/> class.
    /// </summary>
    /// <param name="person">The person.</param>
    public Person(Person person)
        : this(
            (person ?? throw new ArgumentNullException(nameof(person))).Name,
            person.FirstName,
            person.LastName,
            person.Title,
            person.BirthDate,
            person.Gender)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Person" /> class.
    /// </summary>
    [Obsolete(DefaultLabels.ForSerializationOnly, true)]
    public Person()
    {
    }

    /// <summary>
    /// Gets or sets the city.
    /// </summary>
    /// <value>The city.</value>
    [DataMember(Order = 5)]
    [JsonPropertyOrder(5)]
    public DateTimeOffset? BirthDate { get; set; }

    /// <summary>
    /// Gets or sets the city.
    /// </summary>
    /// <value>The city.</value>
    [DataMember(Order = 2)]
    [JsonPropertyOrder(2)]
    public string? FirstName { get; set; }

    /// <summary>
    /// Gets or sets the city.
    /// </summary>
    /// <value>The city.</value>
    [DataMember(Order = 6)]
    [JsonPropertyOrder(6)]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Gender? Gender { get; set; }

    /// <summary>
    /// Gets or sets the city.
    /// </summary>
    /// <value>The city.</value>
    [DataMember(Order = 3)]
    [JsonPropertyOrder(3)]
    public string? LastName { get; set; }

    /// <summary>
    /// Gets or sets the city.
    /// </summary>
    /// <value>The city.</value>
    [DataMember(Order = 1)]
    [JsonPropertyOrder(1)]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>The title.</value>
    [DataMember(Order = 4)]
    [JsonPropertyOrder(4)]
    public string? Title { get; set; }

    /// <summary>
    /// Ares the same.
    /// </summary>
    /// <param name="a">a.</param>
    /// <param name="b">The b.</param>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
    public static bool AreSame(Person? a, Person? b)
    {
        return a is null
            ? b is null
            : a == b ||
                (a.Gender == b?.Gender &&
                a.FirstName == b?.FirstName &&
                a.LastName == b?.LastName &&
                a.Name == b?.Name &&
                a.Title == b?.Title &&
                a.BirthDate == b?.BirthDate);
    }
}