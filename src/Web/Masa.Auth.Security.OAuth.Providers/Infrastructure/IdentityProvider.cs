﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.Auth.Security.OAuth.Providers;

public static class IdentityProvider
{
    readonly static List<IIdentityBuilder> identityBuilders;

    static IdentityProvider()
    {
        var builderTypes = Assembly.GetExecutingAssembly()
                             .GetTypes()
                             .Where(type => type.IsInterface is false && type.IsAssignableTo(typeof(IIdentityBuilder)));

        identityBuilders = builderTypes.Select(type => (IIdentityBuilder)type.Assembly.CreateInstance(type.FullName!)!)
                                       .ToList();
    }

    public static Identity GetIdentity(string scheme, ClaimsPrincipal principal)
    {
        var builder = identityBuilders.First(builder => builder.Scheme == scheme);
        return builder.BuildIdentity(principal);
    }
}
