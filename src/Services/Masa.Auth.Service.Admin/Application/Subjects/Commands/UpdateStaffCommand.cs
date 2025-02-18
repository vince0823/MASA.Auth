﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.Auth.Service.Admin.Application.Subjects.Commands;

public record UpdateStaffCommand(UpdateStaffDto Staff) : Command
{
    public Staff? Result { get; set; }

    public List<Guid> TeamChangeResult { get; set; } = new List<Guid>();
}