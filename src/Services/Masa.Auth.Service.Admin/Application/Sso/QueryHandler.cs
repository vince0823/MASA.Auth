﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.Auth.Service.Admin.Application.Sso;

public class QueryHandler
{
    readonly IClientRepository _clientRepository;
    readonly IIdentityResourceRepository _identityResourceRepository;
    readonly IApiResourceRepository _apiResourceRepository;
    readonly IApiScopeRepository _apiScopeRepository;
    readonly IUserClaimRepository _userClaimRepository;
    readonly ICustomLoginRepository _customLoginRepository;
    readonly OidcDbContext _oidcDbContext;
    readonly AuthDbContext _authDbContext;

    public QueryHandler(IClientRepository clientRepository, IIdentityResourceRepository identityResourceRepository, IApiResourceRepository apiResourceRepository, IApiScopeRepository apiScopeRepository, IUserClaimRepository userClaimRepository, ICustomLoginRepository customLoginRepository, OidcDbContext oidcDbContext, AuthDbContext authDbContext)
    {
        _clientRepository = clientRepository;
        _identityResourceRepository = identityResourceRepository;
        _apiResourceRepository = apiResourceRepository;
        _apiScopeRepository = apiScopeRepository;
        _userClaimRepository = userClaimRepository;
        _customLoginRepository = customLoginRepository;
        _oidcDbContext = oidcDbContext;
        _authDbContext = authDbContext;
    }

    [EventHandler]
    public async Task ClientPaginationListAsync(ClientPaginationListQuery clientPaginationListQuery)
    {
        var result = await _clientRepository.GetPaginatedListAsync(clientPaginationListQuery.Page, clientPaginationListQuery.PageSize);
        clientPaginationListQuery.Result = new PaginationDto<ClientDto>(result.Total, result.Result.Adapt<List<ClientDto>>());
    }

    [EventHandler]
    public async Task ClientDetailQueryAsync(ClientDetailQuery clientDetailQuery)
    {
        var client = await _clientRepository.GetDetailAsync(clientDetailQuery.ClientId);
        client.Adapt(clientDetailQuery.Result);
    }

    [EventHandler]
    public void ClientTypeListAsync(ClientTypeListQuery clientTypeListQuery)
    {
        clientTypeListQuery.Result = Enum<ClientTypes>.GetItems()
            .Select(ct => new ClientTypeDetailDto
            {
                ClientType = ct,
                Description = ct switch
                {
                    //todo: i18n
                    ClientTypes.Web => "Server-side applications where authentication and tokens are handled on the server (for example, Go, Java, ASP.Net, Node.js, PHP)",
                    ClientTypes.Native => "Desktop or mobile applications that run natively on a device and redirect users to a non-HTTP callback (for example, iOS, Android, React Native)",
                    ClientTypes.Spa => "Single-page web applications that run in the browser where the client receives tokens (for example, Javascript, Angular, React, Vue)",
                    ClientTypes.Device => "Input-constrained devices such as a smart TV, IoT device, or printer.",
                    ClientTypes.Machine => "Interact with APIs using the scoped OAuth 2.0 access tokens for machine-to-machine authentication.",
                    _ => ""
                },
                Icon = ct switch
                {
                    ClientTypes.Web => "mdi-file-code-outline",
                    ClientTypes.Native => "mdi-cellphone",
                    ClientTypes.Spa => "mdi-laptop",
                    ClientTypes.Device => "mdi-devices",
                    ClientTypes.Machine => "mdi-server",
                    _ => ""
                }
            }).ToList();
    }

    [EventHandler]
    public async Task GetClientSelectAsync(ClientSelectQuery query)
    {
        query.Result = await _oidcDbContext.Set<Client>().Select(client => new ClientSelectDto(client.Id, client.ClientName, client.LogoUri, client.Description, client.ClientId, client.ClientType)).ToListAsync();
    }

    #region IdentityResource

    [EventHandler]
    public async Task GetIdentityResourceAsync(IdentityResourcesQuery query)
    {
        Expression<Func<IdentityResource, bool>> condition = idrs => true;
        if (string.IsNullOrEmpty(query.Search) is false)
            condition = condition.And(idrs => idrs.DisplayName.Contains(query.Search) || idrs.Name.Contains(query.Search));

        var identityResources = await _identityResourceRepository.GetPaginatedListAsync(query.Page, query.PageSize, condition);

        query.Result = new(identityResources.Total, identityResources.Result.Select(idrs =>
            new IdentityResourceDto(idrs.Id, idrs.Name, idrs.DisplayName, idrs.Description, idrs.Enabled, idrs.Required, idrs.Emphasize, idrs.ShowInDiscoveryDocument, idrs.NonEditable)
        ).ToList());
    }

    [EventHandler]
    public async Task GetIdentityResourceDetailAsync(IdentityResourceDetailQuery query)
    {
        var idrs = await _identityResourceRepository.GetDetailAsync(query.IdentityResourceId);
        if (idrs is null) throw new UserFriendlyException("This identityResource data does not exist");

        query.Result = new(idrs.Id, idrs.Name, idrs.DisplayName, idrs.Description, idrs.Enabled, idrs.Required, idrs.Emphasize, idrs.ShowInDiscoveryDocument, idrs.NonEditable, idrs.UserClaims.Select(u => u.UserClaimId).ToList(), idrs.Properties.ToDictionary(p => p.Key, p => p.Value));
    }

    [EventHandler]
    public async Task GetIdentityResourceSelectAsync(IdentityResourceSelectQuery query)
    {
        var idrs = await _oidcDbContext.Set<IdentityResource>()
                                .Select(idrs => new IdentityResourceSelectDto(idrs.Id, idrs.Name, idrs.DisplayName, idrs.Description))
                                .ToListAsync();

        query.Result = idrs;
    }

    #endregion

    #region ApiResource

    [EventHandler]
    public async Task GetApiResourceAsync(ApiResourcesQuery query)
    {
        Expression<Func<ApiResource, bool>> condition = apiResource => true;
        if (string.IsNullOrEmpty(query.Search) is false)
            condition = condition.And(apiResource => apiResource.DisplayName.Contains(query.Search) || apiResource.Name.Contains(query.Search));

        var apiResources = await _apiResourceRepository.GetPaginatedListAsync(query.Page, query.PageSize, condition);

        query.Result = new(apiResources.Total, apiResources.Result.Select(apiResource => new ApiResourceDto()
        {
            Id = apiResource.Id,
            Enabled = apiResource.Enabled,
            Name = apiResource.Name,
            Description = apiResource.Description,
            AllowedAccessTokenSigningAlgorithms = apiResource.AllowedAccessTokenSigningAlgorithms,
            ShowInDiscoveryDocument = apiResource.ShowInDiscoveryDocument,
            LastAccessed = apiResource.LastAccessed,
            NonEditable = apiResource.NonEditable
        }).ToList());
    }

    [EventHandler]
    public async Task GetApiResourceDetailAsync(ApiResourceDetailQuery query)
    {
        var apiResource = await _apiResourceRepository.GetDetailAsync(query.ApiResourceId);
        if (apiResource is null) throw new UserFriendlyException("This apiResource data does not exist");

        query.Result = new ApiResourceDetailDto()
        {
            Id = apiResource.Id,
            Enabled = apiResource.Enabled,
            Name = apiResource.Name,
            Description = apiResource.Description,
            AllowedAccessTokenSigningAlgorithms = apiResource.AllowedAccessTokenSigningAlgorithms,
            ShowInDiscoveryDocument = apiResource.ShowInDiscoveryDocument,
            LastAccessed = apiResource.LastAccessed,
            NonEditable = apiResource.NonEditable,
            ApiScopes = apiResource.ApiScopes.Select(a => a.ApiScopeId).ToList(),
            UserClaims = apiResource.UserClaims.Select(u => u.UserClaimId).ToList(),
            Properties = apiResource.Properties.ToDictionary(p => p.Key, p => p.Value),
            Secrets = apiResource.Secrets.Select(s => s.Value).ToList()
        };
    }

    [EventHandler]
    public async Task GetApiResourceSelectAsync(ApiResourceSelectQuery query)
    {
        var apiResourceSelect = await _oidcDbContext.Set<ApiResource>()
                                .Where(apiResource => apiResource.Enabled == true)
                                .OrderByDescending(apiResource => apiResource.ModificationTime)
                                .ThenByDescending(apiResource => apiResource.CreationTime)
                                .Select(apiResource => new ApiResourceSelectDto(apiResource.Id, apiResource.Name, apiResource.DisplayName, apiResource.Description))
                                .ToListAsync();

        query.Result = apiResourceSelect;
    }

    #endregion

    #region ApiScope

    [EventHandler]
    public async Task GetApiScopeAsync(ApiScopesQuery query)
    {
        Expression<Func<ApiScope, bool>> condition = apiScopes => true;
        if (string.IsNullOrEmpty(query.Search) is false)
            condition = condition.And(apiScope => apiScope.DisplayName.Contains(query.Search) || apiScope.Name.Contains(query.Search));

        var apiScopes = await _apiScopeRepository.GetPaginatedListAsync(query.Page, query.PageSize, condition);

        query.Result = new(apiScopes.Total, apiScopes.Result.Select(apiScope => new ApiScopeDto()
        {
            Id = apiScope.Id,
            Name = apiScope.Name,
            Enabled = apiScope.Enabled,
            DisplayName = apiScope.DisplayName,
            Description = apiScope.Description,
            Required = apiScope.Required,
            Emphasize = apiScope.Emphasize,
            ShowInDiscoveryDocument = apiScope.ShowInDiscoveryDocument
        }).ToList());
    }

    [EventHandler]
    public async Task GetApiScopeDetailAsync(ApiScopeDetailQuery query)
    {
        var apiScope = await _apiScopeRepository.GetDetailAsync(query.ApiScopeId);
        if (apiScope is null) throw new UserFriendlyException("This apiScope data does not exist");

        query.Result = new ApiScopeDetailDto()
        {
            Id = apiScope.Id,
            Name = apiScope.Name,
            DisplayName = apiScope.DisplayName,
            Description = apiScope.Description,
            Required = apiScope.Required,
            Emphasize = apiScope.Emphasize,
            Enabled = apiScope.Enabled,
            ShowInDiscoveryDocument = apiScope.ShowInDiscoveryDocument,
            UserClaims = apiScope.UserClaims.Select(u => u.UserClaimId).ToList(),
            Properties = apiScope.Properties.ToDictionary(p => p.Key, p => p.Value)
        };
    }

    [EventHandler]
    public async Task GetApiScopeSelectAsync(ApiScopeSelectQuery query)
    {
        var apiScopeSelect = await _oidcDbContext.Set<ApiScope>()
                                .Where(apiScope => apiScope.Enabled == true)
                                .OrderByDescending(apiScope => apiScope.ModificationTime)
                                .ThenByDescending(apiScope => apiScope.CreationTime)
                                .Select(apiScope => new ApiScopeSelectDto(apiScope.Id, apiScope.Name, apiScope.DisplayName, apiScope.Description))
                                .ToListAsync();

        query.Result = apiScopeSelect;
    }

    #endregion

    #region UserClaim

    [EventHandler]
    public async Task GetUserClaimAsync(UserClaimsQuery query)
    {
        Expression<Func<UserClaim, bool>> condition = userClaim => true;
        if (string.IsNullOrEmpty(query.Search) is false)
            condition = condition.And(userClaim => userClaim.Name.Contains(query.Search));

        var userClaims = await _userClaimRepository.GetPaginatedListAsync(query.Page, query.PageSize, condition);

        query.Result = new(userClaims.Total, userClaims.Result.Select(userClaim => new UserClaimDto()
        {
            Id = userClaim.Id,
            Name = userClaim.Name,
            Description = userClaim.Description,
            UserClaimType = StandardUserClaims.Claims.ContainsKey(userClaim.Name) ? UserClaimType.Standard : UserClaimType.Customize
        }).ToList());
    }

    [EventHandler]
    public async Task GetUserClaimDetailAsync(UserClaimDetailQuery query)
    {
        var userClaim = await _userClaimRepository.GetDetailAsync(query.UserClaimId);
        if (userClaim is null) throw new UserFriendlyException("This userClaim data does not exist");

        query.Result = new UserClaimDetailDto()
        {
            Id = userClaim.Id,
            Name = userClaim.Name,
            Description = userClaim.Description,
            UserClaimType = StandardUserClaims.Claims.ContainsKey(userClaim.Name) ? UserClaimType.Standard : UserClaimType.Customize
        };
    }

    [EventHandler]
    public async Task GetUserClaimSelectAsync(UserClaimSelectQuery query)
    {
        var userClaimSelect = await _oidcDbContext.Set<UserClaim>()
                                .OrderByDescending(userClaim => userClaim.ModificationTime)
                                .ThenByDescending(userClaim => userClaim.CreationTime)
                                .Select(userClaim => new UserClaimSelectDto(userClaim.Id, userClaim.Name, userClaim.Description))
                                .ToListAsync();
        userClaimSelect.ForEach(userClaim =>
        {
            if (StandardUserClaims.Claims.ContainsKey(userClaim.Name)) userClaim.UserClaimType = UserClaimType.Standard;
            else userClaim.UserClaimType = UserClaimType.Customize;
        });
        query.Result = userClaimSelect;
    }

    #endregion

    #region CustomLogin

    [EventHandler]
    public async Task GetCustomLoginAsync(CustomLoginsQuery query)
    {
        Expression<Func<CustomLogin, bool>> condition = customLogin => true;
        if (string.IsNullOrEmpty(query.Search) is false)
            condition = condition.And(customLogin => customLogin.Name.Contains(query.Search));

        var customLoginQuery = _authDbContext.Set<CustomLogin>().Where(condition);
        var total = await customLoginQuery.LongCountAsync();
        var customLogins = await customLoginQuery.Include(customLogin => customLogin.Client)
                                    .Include(customLogin => customLogin.CreateUser)
                                    .Include(customLogin => customLogin.ModifyUser)
                                   .OrderByDescending(s => s.ModificationTime)
                                   .ThenByDescending(s => s.CreationTime)
                                   .Skip((query.Page - 1) * query.PageSize)
                                   .Take(query.PageSize)
                                   .ToListAsync();

        query.Result = new(total, customLogins.Select(customLogin => (CustomLoginDto)customLogin).ToList());
    }

    [EventHandler]
    public async Task GetCustomLoginDetailAsync(CustomLoginDetailQuery query)
    {
        var customLogin = await _customLoginRepository.GetDetailAsync(query.CustomLoginId);
        if (customLogin is null) throw new UserFriendlyException("This customLogin data does not exist");

        query.Result = customLogin;
    }

    #endregion
}
