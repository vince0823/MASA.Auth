﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.Auth.Contracts.Admin.Infrastructure.Models;

public class OssOptions : ConfigurationApiMasaConfigurationOptions
{
    [JsonIgnore]
    public override string AppId => "public-$Config";

    [JsonIgnore]
    public override string? ObjectName { get; } = "$public.OSS";

    public string AccessId { get; set; } = "";

    public string AccessSecret { get; set; } = "";

    public string Bucket { get; set; } = "";

    public string Endpoint { get; set; } = "";

    public string RoleArn { get; set; } = "";

    public string RoleSessionName { get; set; } = "";

    public string RegionId { get; set; } = "";
}
