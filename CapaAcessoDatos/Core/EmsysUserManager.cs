namespace Emsys.DataAccesLayer.Core
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class EmsysUserManager : UserManager<IdentityUser>
    {
        public EmsysUserManager() : base(new EmsysUserStore())
        {
        }
    }
}