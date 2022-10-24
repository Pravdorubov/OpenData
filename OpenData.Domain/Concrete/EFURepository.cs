using OpenData.Domain.Abstract;
using OpenData.Domain.Entities;
using System.Linq;

namespace OpenData.Domain.Concrete
{
    public class EFURepository : IURepository
    {
        private EFDbContext context;

        public EFURepository(EFDbContext context)
        {
            this.context = context;
        }
    
        public IQueryable<User> Users
        {
            get { return context.Users.Include("UserProfile"); }
        }

        public UserProfile GetProfile(int userId)
        {
            return context.UserProfiles.Where(up => up.ID == userId).FirstOrDefault();
        }

        public Role GetRole(int roleId)
        {
            return context.Roles.Where(r => r.ID == roleId).FirstOrDefault();
        }
        
        public IQueryable<UserProfile> UserProfiles
        {
            get { return context.UserProfiles; }
        }

        public void SaveUser(User user)
        {
            if (user.ID == 0)
            {
                context.Users.Add(user);
            }
            else
            {
                User dbEntry = context.Users.Find(user.ID);
                if (dbEntry != null)
                {
                    dbEntry.Login = user.Login;
                    dbEntry.RoleID = user.RoleID;
                    dbEntry.Role = user.Role;
                   
                }
            }
            context.SaveChanges();
        }

        public void SaveProfile(UserProfile userProfile)
        {
            if (userProfile.ID == 0)
            {
                context.UserProfiles.Add(userProfile);
            }
            else
            {
                UserProfile dbEntry = context.UserProfiles.Find(userProfile.ID);
                if (dbEntry != null)
                {
                    dbEntry.FNS = userProfile.FNS;
                    dbEntry.Duty = userProfile.Duty;
                    dbEntry.Email = userProfile.Email;
                    dbEntry.Phone = userProfile.Phone;

                }
            }
            context.SaveChanges();
        }

        public void SaveRole(Role role)
        {
            if (role.ID == 0)
            {
                context.Roles.Add(role);
            }
            else
            {
                Role dbEntry = context.Roles.Find(role.ID);
                if (dbEntry != null)
                {
                    dbEntry.Name = role.Name;
                    dbEntry.Description = role.Description;
                }
            }
            context.SaveChanges();
        }

    //public OpenDataSet DeleteOpenDataSet(string ODID)
    //{
    //    OpenDataSet dbEntry = context.OpenDataSets.Find(ODID);
    //    if (dbEntry != null)
    //    {
    //        context.OpenDataSets.Remove(dbEntry);
    //        context.SaveChanges();
    //    }
    //    return dbEntry;
    //}
    }
}
