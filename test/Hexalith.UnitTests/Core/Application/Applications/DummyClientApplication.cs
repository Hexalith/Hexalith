﻿// <copyright file="DummyClientApplication.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Applications;

using System;
using System.Collections.Generic;

using Hexalith.Application.Modules.Applications;

internal class DummyWebAppApplication : HexalithWebAppApplication
{
    public override Type SharedUIElementsApplicationType => typeof(DummySharedApplication);

    public override IEnumerable<Type> WebAppModules => [typeof(DummyClientModule)];
}