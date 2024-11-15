// <copyright file="DummySharedApplication.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Applications;

using System;
using System.Collections.Generic;

using Hexalith.Application.Modules.Applications;

public class DummySharedApplication : HexalithSharedUIElementsApplication
{
    public override string HomePath => "dummy";

    public override string Id => "dummy1";

    public override string LoginPath => "dummy/login";

    public override string LogoutPath => "dummy/logout";

    public override string Name => "Dummy";

    public override IEnumerable<Type> SharedUIElementsModules => [typeof(DummySharedModule)];

    public override string Version => "?.?.?";
}