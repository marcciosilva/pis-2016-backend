namespace Emsys.DataAccesLayer.Core
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Model;

    public class EmsysUserManager : UserManager<ApplicationUser>
    {
        public EmsysUserManager() : base(new EmsysUserStore())
        {
        }
    }
}