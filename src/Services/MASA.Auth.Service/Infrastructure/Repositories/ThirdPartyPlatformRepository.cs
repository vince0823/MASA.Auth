﻿namespace Masa.Auth.Service.Infrastructure.Repositories;

public class ThirdPartyPlatformRepository : Repository<AuthDbContext, ThirdPartyPlatform>, IThirdPartyPlatformRepository
{
    public ThirdPartyPlatformRepository(AuthDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
    {
    }
}
