namespace Emsys.DataAccesLayer.Core
{
    using Microsoft.AspNet.Identity.EntityFramework;

    public class EmsysUserStore : UserStore<IdentityUser>
    {
        public EmsysUserStore() : base(new EmsysContext())
        {
        }
    }
}