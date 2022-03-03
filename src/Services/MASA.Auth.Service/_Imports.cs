﻿global using FluentValidation;
global using FluentValidation.AspNetCore;
global using MASA.Auth.Service.Domain.Organization.Aggregates;
global using MASA.Auth.Service.Domain.Organization.Repositories;
global using MASA.Auth.Service.Domain.Organizations.Aggregates;
global using MASA.Auth.Service.Domain.Permissions.Aggregates;
global using MASA.Auth.Service.Domain.SSO.Aggregates.Abstract;
global using MASA.Auth.Service.Domain.Subjects.Aggregates;
global using MASA.Auth.Service.Enums;
global using MASA.Auth.Service.Infrastructure;
global using MASA.Auth.Service.Infrastructure.Middleware;
global using MASA.BuildingBlocks.Data.UoW;
global using MASA.BuildingBlocks.DDD.Domain.Entities;
global using MASA.BuildingBlocks.DDD.Domain.Entities.Auditing;
global using MASA.BuildingBlocks.DDD.Domain.Repositories;
global using MASA.BuildingBlocks.Dispatcher.Events;
global using MASA.Contrib.Data.UoW.EF;
global using MASA.Contrib.DDD.Domain;
global using MASA.Contrib.DDD.Domain.Repository.EF;
global using MASA.Contrib.Dispatcher.Events;
global using MASA.Contrib.Dispatcher.IntegrationEvents.Dapr;
global using MASA.Contrib.Dispatcher.IntegrationEvents.EventLogs.EF;
global using MASA.Contrib.Service.MinimalAPIs;
global using MASA.Utils.Data.EntityFrameworkCore;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.OpenApi.Models;
global using System.Reflection;
