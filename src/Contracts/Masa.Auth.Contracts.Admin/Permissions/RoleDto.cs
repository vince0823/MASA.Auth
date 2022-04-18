﻿namespace Masa.Auth.Contracts.Admin.Permissions;

public class RoleDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public int Limit { get; set; }

    public string Description { get; set; }

    public bool Enabled { get; set; }

    public DateTime CreationTime { get; set; }

    public DateTime? ModificationTime { get; set; }

    public string Creator { get; set; }

    public string Modifier { get; set; }

    public RoleDto()
    {
        Name = "";
        Description = "";
        Creator = "";
        Modifier = "";
    }

    public RoleDto(Guid id, string name, int limit, string description, bool enabled, DateTime creationTime, DateTime? modificationTime, string creator, string modifier)
    {
        Id = id;
        Name = name;
        Limit = limit;
        Description = description;
        Enabled = enabled;
        CreationTime = creationTime;
        ModificationTime = modificationTime;
        Creator = creator;
        Modifier = modifier;
    }
}


