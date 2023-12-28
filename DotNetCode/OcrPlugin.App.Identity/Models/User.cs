using Microsoft.AspNetCore.Identity;
using System;

namespace OcrPlugin.App.Identity.Models
{
    public class User : IdentityUser<Guid>
    {
        public byte[] Salt { get; set; }
        public Role RoleId { get; set; }
    }
}