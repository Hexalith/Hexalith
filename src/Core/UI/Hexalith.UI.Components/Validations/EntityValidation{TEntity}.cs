// <copyright file="EntityValidation{TEntity}.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UI.Components.Validations;

using System.Globalization;
using System.Text;

using FluentValidation;

using Hexalith.Application.Services;

using Labels = Hexalith.UI.Components.Resources.Validations;

/// <summary>
/// Base class for entity validation.
/// </summary>
/// <typeparam name="TEntity">The type of entity to validate.</typeparam>
public abstract class EntityValidation<TEntity> : AbstractValidator<TEntity>
    where TEntity : IIdDescription
{
    private static readonly CompositeFormat _maxIdLengthFormat = CompositeFormat.Parse(Labels.MaxIdLengthExceeded);
    private static readonly CompositeFormat _maxNameLengthFormat = CompositeFormat.Parse(Labels.MaxNameLengthExceeded);

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityValidation{TEntity}"/> class.
    /// </summary>
    public EntityValidation(int idMaxSize = 32, int descriptionMaxSize = 128)
    {
        if (idMaxSize > 0)
        {
            _ = RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(Labels.IdRequired)
                .MaximumLength(idMaxSize)
                .WithMessage(string.Format(CultureInfo.InvariantCulture, _maxIdLengthFormat, idMaxSize));
        }

        if (descriptionMaxSize > 0)
        {
            _ = RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage(Labels.NameRequired)
                .MaximumLength(512)
                .WithMessage(string.Format(CultureInfo.InvariantCulture, _maxNameLengthFormat, 512));
        }
    }
}