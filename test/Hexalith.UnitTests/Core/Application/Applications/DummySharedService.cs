﻿// <copyright file="DummySharedService.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Applications;

public record DummySharedService(string Id)
{
    public DummySharedService()
    : this("Shared")
    {
    }
}