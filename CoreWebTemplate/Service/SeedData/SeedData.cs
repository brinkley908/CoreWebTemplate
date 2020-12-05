using CoreWebTemplate.Models.AspNetUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebTemplate.Service.SeedData
{
    public class SeedData : ISeedData
    {

        private readonly List<UserEntity> _users;

        private readonly MemoryIdentities _context;

        private readonly UserManager<UserEntity> _userManager;

        private readonly RoleManager<UserRoleEntity> _userRoleManager;

        private readonly SignInManager<UserEntity> _signInManager;

        private readonly IOptions<IdentityOptions> _identityOptions;



        public List<UserEntity> Users { get { return _users; } }

        public UserManager<UserEntity> UserManager { get { return _userManager; } }

        public RoleManager<UserRoleEntity> UserRoleManager { get { return _userRoleManager; } }

        public SignInManager<UserEntity> SignInManager { get { return _signInManager; } }

        public IOptions<IdentityOptions> IdentityOptions { get { return _identityOptions; } }

        //IDateLogicService dateLogicService,
        public SeedData
        (
            IOptions<List<UserEntity>> users,
            MemoryIdentities context,
            UserManager<UserEntity> userManager,
            RoleManager<UserRoleEntity> roleManager,
            SignInManager<UserEntity> signInManager,
            IOptions<IdentityOptions> identityOptions
        )
        {
            _context = context;
            _users = users.Value;
            _userManager = userManager;
            _userRoleManager = roleManager;
            _signInManager = signInManager;
            _identityOptions = identityOptions;
        }


        public async Task AddTestUsers()
        {
            if ( _userRoleManager.Roles.Any() || _userManager.Users.Any() )
                return;


            await _userRoleManager.CreateAsync( new UserRoleEntity( "Admin" ) { Id = Guid.NewGuid().ToString() } );


            foreach ( var user in _users )
            {
                var newUser = new UserEntity
                {

                    Id = Guid.NewGuid().ToString(),
                    Email = user.Email,
                    UserName = user.UserName
                };

                await _userManager.CreateAsync( newUser, "LetMeIn2020!!" );

                await _userManager.AddToRoleAsync( newUser, "Admin" );
                await _userManager.UpdateAsync( newUser );
            }



        }


    }


}
