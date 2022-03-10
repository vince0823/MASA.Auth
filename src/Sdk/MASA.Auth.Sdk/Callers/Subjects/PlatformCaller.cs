﻿using Masa.Auth.Sdk.Response;
using Masa.Auth.Sdk.Response.Subjects;

namespace Masa.Auth.Sdk.Callers.Subjects;

internal class PlatformCaller : CallerBase
{
    protected override string BaseAddress { get; set; }

    public override string Name { get; set; }

    internal PlatformCaller(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        Name = nameof(PlatformCaller);
        BaseAddress = Routing.AUTH_SERVICE_BASE_ADRESS;
    }

    public async Task<ApiResultResponse<PaginatedItemResponse<ThirdPartyPlatformItemResponse>>> GetListAsync(int pageIndex = 1, int pageSize = 20, string? search = null)
    {
        var queryArguments = new Dictionary<string, string>()
        {
            { "pageIndex", pageIndex.ToString() },
            { "pageSize", pageSize.ToString() },
            { "search", search ?? "" },
        };

        return await ResultAsync(async () =>
        {
            //var url = QueryHelpers.AddQueryString(Routing.UserList, queryArguments);
            var response = await CallerProvider.GetAsync<PaginatedItemResponse<ThirdPartyPlatformItemResponse>>(Routing.PlatformList, queryArguments);
            return response!;
        });
    }
}
