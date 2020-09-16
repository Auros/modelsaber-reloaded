using System;

namespace ModelSaber.Common
{
    [Flags]
    public enum Role
    {
        None = 0,
        Uploader = 1,
        Verified = 2,
        Trusted = 4,
        Manager = 8,
        Admin = 16
    }
}