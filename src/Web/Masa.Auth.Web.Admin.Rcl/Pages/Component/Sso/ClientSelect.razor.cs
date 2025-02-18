﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

using System.Linq.Expressions;

namespace Masa.Auth.Web.Admin.Rcl.Pages.Component.Sso;

public partial class ClientSelect
{
    [Parameter]
    public string Class { get; set; } = "";

    [Parameter]
    public string Style { get; set; } = "";

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public string Value { get; set; } = "";

    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }

    [Parameter]
    public Expression<Func<string>>? ValueExpression { get; set; }

    public ClientSelectDto? Client { get; set; }

    protected List<ClientSelectDto> Clients { get; set; } = new();

    protected ClientService ClientService => AuthCaller.ClientService;

    protected override async Task OnInitializedAsync()
    {
        Clients = await ClientService.GetClientSelectAsync();
        Client = Clients.FirstOrDefault(client => client.ClientId == Value);
    }

    public async Task UpdateValueAsync(string value)
    {
        Client = Clients.FirstOrDefault(client => client.ClientId == value);
        if (ValueChanged.HasDelegate) await ValueChanged.InvokeAsync(value);
        else Value = value;
    }
}

