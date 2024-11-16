// <copyright file="DummyServerApplication.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Applications;

using System;
using System.Collections.Generic;

using Hexalith.Application.Modules.Applications;

internal class DummyServerApplication : HexalithWebServerApplication
{
    /// <inheritdoc/>
    public override string Id => $"Dummy.{ApplicationType}";

    /// <inheritdoc/>
    public override string Name => $"Dummy {ApplicationType}";

    /// <inheritdoc/>
    public override string ShortName => "Dummy";

    public override Type WebAppApplicationType => typeof(DummyWebAppApplication);

    public override IEnumerable<Type> WebServerModules => [typeof(DummyServerModule)];
}