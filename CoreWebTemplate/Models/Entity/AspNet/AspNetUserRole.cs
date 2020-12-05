using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebTemplate.Models.AspNetUsers
{
    public class AspNetUserRole: IdentityUserRole<string>
    {
       // [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors" )]
        public AspNetUserRole()
        {
           // this.AspNetUsers = new HashSet<AspNetUser>();
            
        }

        //[Key]
       // public string UserId { get; set; }
        //public string RoleId { get; set; }

        //[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly" )]
        //public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
    }
}
