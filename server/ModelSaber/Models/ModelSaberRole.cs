using System;

namespace ModelSaber.Models
{
    [Flags]
    public enum ModelSaberRole
    {
        None = 0,
        Banned = 1,
        Trusted = 2,
        Approver = 4,
        Admin = 8
    }
}