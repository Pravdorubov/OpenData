using System.Linq;
using OpenData.Domain.Entities;

namespace OpenData.Domain.Abstract
{
    public interface IARepository
    {
        IQueryable<Authority> Authorities { get; }
        void SaveAuthority(Authority Authority);
        Authority DeleteAuthority(int INN);
        
    }
}
