﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.Auth.Web.Admin.Rcl.Pages.RolePermissions.Permissions;

public partial class Index
{
    string _tab = "", _search = "";
    bool _showMenuInfo, _showApiInfo;
    List<AppPermissionsViewModel> _menuPermissions = new(), _apiPermissions = new();
    List<Guid> _menuPermissionActive = new List<Guid>(), _apiPermissionActive = new List<Guid>()
        , _menuOpenNode = new List<Guid>(), _apiOpenNode = new List<Guid>();
    string _curProjectId = "";
    MenuPermissionDetailDto _menuPermissionDetailDto = new();
    ApiPermissionDetailDto _apiPermissionDetailDto = new();
    List<ProjectDto> _projectItems = new();
    List<AppDto> _curAppItems = new();
    List<SelectItemDto<Guid>> _childApiItems = new();
    MForm _formMenu = default!, _formApi = default!;
    AddMenuPermission _addMenuPermission = null!;
    AddApiPermission _addApiPermission = null!;

    PermissionService PermissionService => AuthCaller.PermissionService;

    ProjectService ProjectService => AuthCaller.ProjectService;
    List<SelectItemDto<PermissionTypes>> _permissionTypes = new();
    string _showUrlPrefix = "";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _permissionTypes = await PermissionService.GetTypesAsync();
            //_tab = "Menu Permission";
            _tab = "0";
            _projectItems = await ProjectService.GetListAsync();
            if (!_projectItems.Any())
            {
                return;
            }
            await SelectProjectItem(_projectItems.First());
            StateHasChanged();
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    private List<AppPermissionsViewModel> MenuTreeParent(MenuPermissionDetailDto menuPermissionDetailDto)
    {
        var menus = _menuPermissions.Where(m => m.AppId == menuPermissionDetailDto.AppId)
            .Select(m => (AppPermissionsViewModel)m.Clone()).ToList();
        menus.ForEach(item =>
        {
            item.Id = Guid.Empty;
        });
        RemoveAll(menus, x => x.Id == menuPermissionDetailDto.Id);
        if (menuPermissionDetailDto.Type == PermissionTypes.Menu)
        {
            RemoveChildElementAll(menus);
            RemoveAll(menus, p => p.Type != null && p.Type != PermissionTypes.Menu);
        }
        return menus;

        void RemoveAll(List<AppPermissionsViewModel> menus, Predicate<AppPermissionsViewModel> match)
        {
            menus.RemoveAll(match);
            foreach (var menu in menus)
            {
                RemoveAll(menu.Children, match);
            }
        }

        void RemoveChildElementAll(List<AppPermissionsViewModel> menus)
        {
            foreach (var menu in menus.ToArray())
            {
                if (menu.Children?.Any() == true)
                {
                    if (menu.Children.Any(x => x.Type == PermissionTypes.Element))
                    {
                        RemoveAll(menus, x => x.Id == menu.Id);
                    }
                    else
                    {
                        RemoveChildElementAll(menu.Children);
                    }
                }
            }
        }
    }

    private async Task SelectProjectItem(ProjectDto project)
    {
        _curAppItems = project.Apps;
        _curProjectId = project.Identity;
        _childApiItems = await PermissionService.GetApiPermissionSelectAsync(_curProjectId);
        await InitAppPermissions();
    }

    private async Task InitAppPermissions()
    {
        _menuPermissions = _curAppItems.Where(a => a.Type == AppTypes.UI).Select(a => new AppPermissionsViewModel
        {
            IsPermission = false,
            AppId = a.Identity,
            AppTag = a.Tag,
            Id = Guid.NewGuid(),
            AppUrl = a.Url,
            Name = a.Name
        }).ToList();
        _menuOpenNode = _menuPermissions.Select(m => m.Id).ToList();
        if (!_menuPermissionActive.Any())
        {
            _menuPermissionActive = _menuOpenNode.Take(1).ToList();
        }

        _apiPermissions = _curAppItems.Where(a => a.Type == AppTypes.Service).Select(a => new AppPermissionsViewModel
        {
            IsPermission = false,
            AppId = a.Identity,
            AppTag = a.Tag,
            Id = Guid.NewGuid(),
            AppUrl = a.Url,
            Name = a.Name
        }).ToList();
        _apiOpenNode = _apiPermissions.Select(m => m.Id).ToList();
        if (!_apiPermissionActive.Any())
        {
            _apiPermissionActive = _apiOpenNode.Take(1).ToList();
        }

        var applicationPermissions = await PermissionService.GetApplicationPermissionsAsync(_curProjectId);

        var config = new TypeAdapterConfig();
        config.NewConfig<AppPermissionDto, AppPermissionsViewModel>().Map(dest => dest.Id, src => src.PermissonId)
            .Map(dest => dest.Name, src => src.PermissionName)
            .Map(dest => dest.IsPermission, src => true)
            .Map(dest => dest.AppUrl, src => MapContext.Current == null ? "" : MapContext.Current.Parameters["appUrl"]);

        _menuPermissions.ForEach(mp =>
        {
            var permissions = applicationPermissions.Where(p => (p.Type == PermissionTypes.Menu || p.Type == PermissionTypes.Element) && p.AppId == mp.AppId);
            mp.Children.AddRange(permissions
                .BuildAdapter(config)
                .AddParameters("appUrl", mp.AppUrl)
                .AdaptToType<List<AppPermissionsViewModel>>());
        });

        _apiPermissions.ForEach(mp =>
        {
            var permissions = applicationPermissions.Where(p => p.Type == PermissionTypes.Api && p.AppId == mp.AppId);
            mp.Children.AddRange(permissions
                .BuildAdapter(config)
                .AddParameters("appUrl", mp.AppUrl)
                .AdaptToType<List<AppPermissionsViewModel>>());
        });
    }

    private async Task ActiveMenuPermission(List<AppPermissionsViewModel> activeItems)
    {
        if (activeItems.Any())
        {
            _showMenuInfo = true;
            var curItem = activeItems.First();
            if (curItem.IsPermission)
            {
                _menuPermissionDetailDto = await PermissionService.GetMenuPermissionDetailAsync(curItem.Id);
            }
            else
            {
                _menuPermissionDetailDto = new();
            }
            _showUrlPrefix = curItem.AppUrl;
        }
        else
        {
            _showMenuInfo = false;
        }
    }

    private async Task ActiveApiPermission(List<AppPermissionsViewModel> activeItems)
    {
        if (activeItems.Any())
        {
            var curItem = activeItems.First();
            _showApiInfo = true;
            if (curItem.IsPermission)
            {
                _apiPermissionDetailDto = await PermissionService.GetApiPermissionDetailAsync(curItem.Id);
            }
            else
            {
                _apiPermissionDetailDto = new();
            }
            _showUrlPrefix = curItem.AppUrl;
        }
        else
        {
            _showApiInfo = false;
        }
    }

    private async Task AddMenuPermissionAsync(MenuPermissionDetailDto dto)
    {
        if (string.IsNullOrWhiteSpace(_curProjectId))
        {
            OpenErrorMessage(T("Project identifier is empty"));
            return;
        }
        dto.SystemId = _curProjectId;
        await PermissionService.UpsertMenuPermissionAsync(dto);
        if (!dto.IsUpdate)
        {
            await InitAppPermissions();
        }
        OpenSuccessMessage(T("Add menu permission data success"));
    }

    private async Task AddApiPermissionAsync(ApiPermissionDetailDto dto)
    {
        if (string.IsNullOrWhiteSpace(_curProjectId))
        {
            OpenErrorMessage(T("Project identifier is empty"));
            return;
        }
        dto.SystemId = _curProjectId;
        await PermissionService.UpsertApiPermissionAsync(dto);
        if (!dto.IsUpdate)
        {
            await InitAppPermissions();
        }
        OpenSuccessMessage(T("Add api permission data success"));
    }

    private async Task UpdateMenuPermissionAsync()
    {
        if (_formMenu.Validate())
        {
            _menuPermissionDetailDto.SystemId = _curProjectId;
            await PermissionService.UpsertMenuPermissionAsync(_menuPermissionDetailDto);
            OpenSuccessMessage(T("Edit menu permission data success"));
            await InitAppPermissions();
        }
    }

    private async Task UpdateApiPermissionAsync()
    {
        if (_formApi.Validate())
        {
            _apiPermissionDetailDto.SystemId = _curProjectId;
            await PermissionService.UpsertApiPermissionAsync(_apiPermissionDetailDto);
            OpenSuccessMessage(T("Edit api permission data success"));
            await InitAppPermissions();
        }
    }

    private async Task DeletePermissionAsync(PermissionDetailDto permission)
    {
        var isConfirmed = await OpenConfirmDialog(T("Delete Permission"), T("Are you sure to delete permission {0}", DT(permission.Name)));
        if (isConfirmed)
        {
            await PermissionService.RemoveAsync(permission.Id);
            await InitAppPermissions();
        }
    }
}
