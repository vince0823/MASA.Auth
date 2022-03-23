﻿namespace Masa.Auth.Contracts.Admin.Permissions;

public class GetRolesDto : Pagination
{
    public string Search { get; set; }

    public bool Enabled { get; set; }

    public GetRolesDto(int page, int pageSize, string search, bool enabled)
    {
        Page = page;
        PageSize = pageSize;
        Search = search;
        Enabled = enabled;
    }
}

