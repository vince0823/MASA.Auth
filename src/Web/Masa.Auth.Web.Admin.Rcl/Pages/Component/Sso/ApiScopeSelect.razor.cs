﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.Auth.Web.Admin.Rcl.Pages.Component.Sso;

public partial class ApiScopeSelect
{
    [Parameter]
    public string Class { get; set; } = "";

    [Parameter]
    public string Style { get; set; } = "";

    [Parameter]
    public int Chunk { get; set; } = 5;

    [Parameter]
    public List<Guid> Value { get; set; } = new();

    [Parameter]
    public EventCallback<List<Guid>> ValueChanged { get; set; }

    IEnumerable<ApiScopeSelectDto[]> ApiScopeChunks { get; set; } = new List<ApiScopeSelectDto[]>();

    ApiScopeService ApiScopeService => AuthCaller.ApiScopeService;

    protected override async Task OnInitializedAsync()
    {
        var apiScopes = await ApiScopeService.GetSelectAsync();
        ApiScopeChunks = apiScopes.Chunk(Chunk);
    }

    void OnValueChanged(bool value, Guid id)
    {
        if (value) Value.Add(id);
        else Value.Remove(id);
    }
}

