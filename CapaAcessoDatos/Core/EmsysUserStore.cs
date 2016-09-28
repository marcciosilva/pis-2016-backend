namespace Emsys.DataAccesLayer.Core
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using Model;

    public class EmsysUserStore : UserStore<ApplicationUser>
    {
        public EmsysUserStore() : base(new EmsysContext())
        {
        }
    }
}