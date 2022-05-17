﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.Auth.Service.Admin.Domain.Sso.Aggregates;

public class PersistedGrant : FullAuditAggregateRoot<int, Guid>
{
    public string Key { get; private set; } = null!;

    public string Type { get; private set; } = string.Empty;

    public string SubjectId { get; private set; } = string.Empty;

    public string SessionId { get; private set; } = string.Empty;

    public string ClientId { get; private set; } = string.Empty;

    public string Description { get; private set; } = string.Empty;

    public DateTime? Expiration { get; private set; }

    public DateTime? ConsumedTime { get; private set; }

    public string Data { get; private set; } = string.Empty;
}
