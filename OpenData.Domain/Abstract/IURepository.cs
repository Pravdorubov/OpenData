using System.Linq;
using OpenData.Domain.Entities;

namespace OpenData.Domain.Abstract
{
    public interface IURepository
    {
        IQueryable<User> Users { get; }
        UserProfile GetProfile(int userId);
        Role GetRole(int roleId);
        IQueryable<UserProfile> UserProfiles { get; }
        void SaveUser(User User);
        void SaveProfile(UserProfile UserProfile);
        void SaveRole(Role Role);
        
    }
}
