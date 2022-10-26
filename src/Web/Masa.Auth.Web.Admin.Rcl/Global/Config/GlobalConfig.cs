﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

using Microsoft.AspNetCore.Http;

namespace Masa.Auth.Web.Admin.Rcl.Global.Config
{
    public class GlobalConfig
    {
        #region Field

        private bool _Loading;
        private CookieStorage _cookieStorage;

        #endregion

        #region Property

        public static string ElementPermissionsCookieKey { get; set; } = "element_permissions";

        public bool Loading
        {
            get => _Loading;
            set
            {
                if (_Loading != value)
                {
                    _Loading = value;
                    OnLoadingChanged?.Invoke(_Loading);
                }
            }
        }

        #endregion


        #region event

        public delegate void GlobalConfigChanged();
        public delegate void LoadingChanged(bool Loading);

        public event LoadingChanged? OnLoadingChanged;

        #endregion

        public GlobalConfig(CookieStorage cookieStorage, IHttpContextAccessor httpContextAccessor)
        {
            _cookieStorage = cookieStorage;
        }
    }
}