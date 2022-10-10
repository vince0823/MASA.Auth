﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.Auth.Contracts.Admin.Sso.Validator;

public class AddClientValidator : AbstractValidator<AddClientDto>
{
    public AddClientValidator()
    {
        RuleFor(client => client.ClientId).Required();
        RuleFor(client => client.ClientName).Required().MaxLength(50);
        RuleFor(client => client.Description).MaxLength(100);
        RuleFor(client => client.ClientUri).Url().When(client => !string.IsNullOrEmpty(client.ClientUri));
    }
}
