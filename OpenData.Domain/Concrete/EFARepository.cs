using OpenData.Domain.Abstract;
using OpenData.Domain.Entities;
using System.Linq;

namespace OpenData.Domain.Concrete
{
    public class EFARepository : IARepository
    {
        private EFDbContext context;

        public EFARepository(EFDbContext context)
        {
            this.context = context;
        }

        
        public IQueryable<Authority> Authorities
        {
            get { return context.Authorities; }
        }

        public void SaveAuthority(Authority Authority)
        { 
            
        }

        public Authority DeleteAuthority(int INN)
        {
            return new Authority();
        }
    }
}
