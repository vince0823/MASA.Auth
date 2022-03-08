﻿namespace Masa.Auth.Service.Domain.SSO.Aggregates;

public class ApiResourceClaim : UserClaim
{
    public int ApiResourceId { get; set; }
    public ApiResource ApiResource { get; set; } = null!;
}

