using System;

namespace Slottet.Domain.Entities;

public class Role
{
    public int Id { get; set;}
    public string RoleName {get; set;} = string.Empty;

    private Role() { }

    public static Role Create(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            throw new ArgumentException("Rolle navn er påkrævet");

        return new Role
        {
            RoleName = roleName
        };
    }

    public void UpdateRoleName(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            throw new ArgumentException("Rolle navn er påkrævet");

        RoleName = roleName;
    }
}
