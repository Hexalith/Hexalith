// <copyright file="DummyClientApplication.cs" company="Jérôme Piquot">
//     Copyright (c) Jérôme Piquot. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Applications;

using System;
using System.Collections.Generic;

using Hexalith.Application.Modules.Applications;

internal class DummyClientApplication : HexalithClientApplication
{
    public override IEnumerable<Type> ClientModules => [typeof(DummyClientModule)];

    public override Type SharedApplicationType => typeof(DummySharedApplication);
}