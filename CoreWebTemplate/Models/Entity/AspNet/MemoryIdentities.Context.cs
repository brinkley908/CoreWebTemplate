using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace CoreWebTemplate.Models.AspNetUsers
{
    public class MemoryIdentities : IdentityDbContext<UserEntity, UserRoleEntity, string>
    {
        public MemoryIdentities( DbContextOptions options )
            : base( options ) { }

    }
}
