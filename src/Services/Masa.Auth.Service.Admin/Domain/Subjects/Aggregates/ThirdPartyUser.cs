﻿// Copyright (c) MASA Stack All rights reserved.
// Licensed under the Apache License. See LICENSE.txt in the project root for license information.

namespace Masa.Auth.Service.Admin.Domain.Subjects.Aggregates;

public class ThirdPartyUser : FullAggregateRoot<Guid, Guid>
{
    private User? _user;
    private User? _createUser;
    private User? _modifyUser;
    private IdentityProvider _identityProvider = default!;

    private Guid _thirdPartyIdpId;
    private Guid _userId;
    private string _thridPartyIdentity = "";
    private string _extendedData = "";

    public Guid ThirdPartyIdpId
    {
        get => _thirdPartyIdpId;
        set => _thirdPartyIdpId = ArgumentExceptionExtensions.ThrowIfDefault(value, nameof(ThirdPartyIdpId));
    }

    public Guid UserId
    {
        get => _userId;
        private set => _userId = ArgumentExceptionExtensions.ThrowIfDefault(value, nameof(UserId));
    }

    public bool Enabled { get; private set; }

    public string ThridPartyIdentity
    {
        get => _thridPartyIdentity;
        private set => _thridPartyIdentity = ArgumentExceptionExtensions.ThrowIfNullOrEmpty(value, nameof(ThridPartyIdentity));
    }

    public string ExtendedData
    {
        get => _extendedData;
        private set => _extendedData = ArgumentExceptionExtensions.ThrowIfNullOrEmpty(value, nameof(ExtendedData));
    }

    public User User => _user ?? throw new UserFriendlyException(errorCode: UserFriendlyExceptionCodes.USER_NOT_FOUND);

    public User? CreateUser => _createUser;

    public User? ModifyUser => _modifyUser;

    public IdentityProvider IdentityProvider => _identityProvider;

    public ThirdPartyUser(Guid thirdPartyIdpId, Guid userId, bool enabled, string thridPartyIdentity, string extendedData)
    {
        ThirdPartyIdpId = thirdPartyIdpId;
        UserId = userId;
        Enabled = enabled;
        ThridPartyIdentity = thridPartyIdentity;
        ExtendedData = extendedData;
    }

    public void Update(bool enabled, string thridPartyIdentity, string extendedData)
    {
        Enabled = enabled;
        ThridPartyIdentity = thridPartyIdentity;
        ExtendedData = extendedData;
    }

    public void Update(string thridPartyIdentity, string extendedData)
    {
        ThridPartyIdentity = thridPartyIdentity;
        ExtendedData = extendedData;
    }

    public static implicit operator ThirdPartyUserDetailDto(ThirdPartyUser tpu)
    {
        return new ThirdPartyUserDetailDto(tpu.Id, tpu.Enabled, tpu.IdentityProvider.Adapt<IdentityProviderDetailDto>(), tpu.User, tpu.CreationTime, tpu.ModificationTime, tpu.CreateUser?.Name ?? "", tpu.ModifyUser?.Name ?? "");
    }
}

