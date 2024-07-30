// <copyright file="DummySharedApplication.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.UnitTests.Core.Application.Applications;

using System;
using System.Collections.Generic;

using Hexalith.Application.Modules.Applications;

public class DummySharedApplication : HexalithSharedApplication
{
    public override string HomePath => "dummy";

    public override string Id => "dummy1";

    public override string LoginPath => "dummy/login";

    public override string LogoutPath => "dummy/logout";

    public override string Name => "Dummy";

    public override string Version => "?.?.?";

    public override IEnumerable<Type> SharedModules => [typeof(DummySharedModule)];
}