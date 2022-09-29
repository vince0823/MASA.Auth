﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

global using BlazorComponent;
global using BlazorComponent.I18n;
global using FluentValidation;
global using Mapster;
global using Masa.Auth.ApiGateways.Caller;
global using Masa.Auth.ApiGateways.Caller.Services.Logs;
global using Masa.Auth.ApiGateways.Caller.Services.Organizations;
global using Masa.Auth.ApiGateways.Caller.Services.Permissions;
global using Masa.Auth.ApiGateways.Caller.Services.Projects;
global using Masa.Auth.ApiGateways.Caller.Services.Sso;
global using Masa.Auth.ApiGateways.Caller.Services.Subjects;
global using Masa.Auth.Contracts.Admin.Infrastructure.Constants;
global using Masa.Auth.Contracts.Admin.Infrastructure.Dtos;
global using Masa.Auth.Contracts.Admin.Infrastructure.Enums;
global using Masa.Auth.Contracts.Admin.Infrastructure.Utils;
global using Masa.Auth.Contracts.Admin.Logs;
global using Masa.Auth.Contracts.Admin.Organizations;
global using Masa.Auth.Contracts.Admin.Oss;
global using Masa.Auth.Contracts.Admin.Permissions;
global using Masa.Auth.Contracts.Admin.Projects;
global using Masa.Auth.Contracts.Admin.Sso;
global using Masa.Auth.Contracts.Admin.Subjects;
global using Masa.Auth.Web.Admin.Rcl.Global.Config;
global using Masa.Auth.Web.Admin.Rcl.Pages.Component.Permissions;
global using Masa.Auth.Web.Admin.Rcl.Pages.Component.Permissions.Model;
global using Masa.Auth.Web.Admin.Rcl.Pages.Component.Sso;
global using Masa.Auth.Web.Admin.Rcl.Pages.Component.Subjects;
global using Masa.Auth.Web.Admin.Rcl.Pages.RolePermissions.Permissions.ViewModels;
global using Masa.Auth.Web.Admin.Rcl.Pages.Sso.Client.Section;
global using Masa.Auth.Web.Admin.Rcl.Pages.Sso.CustomLoginRegister.Model;
global using Masa.Auth.Web.Admin.Rcl.Pages.Subjects.Users;
global using Masa.Auth.Web.Admin.Rcl.Pages.Subjects.Users.Model;
global using Masa.Blazor;
global using Masa.BuildingBlocks.Authentication.Identity;
global using Masa.BuildingBlocks.Authentication.OpenIdConnect.Domain.Enums;
global using Masa.BuildingBlocks.StackSdks.Auth.Contracts.Enum;
global using Masa.BuildingBlocks.StackSdks.Auth.Contracts.Model;
global using Masa.BuildingBlocks.StackSdks.Pm.Enum;
global using Masa.Stack.Components;
global using Masa.Stack.Components.Models;
global using Microsoft.AspNetCore.Components;
global using Microsoft.AspNetCore.Components.Forms;
global using Microsoft.AspNetCore.Components.Rendering;
global using Microsoft.AspNetCore.Components.Web;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.JSInterop;
global using System.Security.Claims;
global using System.Text;
