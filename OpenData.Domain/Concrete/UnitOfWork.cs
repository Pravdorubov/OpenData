using System;
using OpenData.Domain.Entities;

namespace OpenData.Domain.Concrete
{
    public class UnitOfWork : IDisposable
    {
        private EFDbContext context = new EFDbContext();
        private GenericRepository<cat_Authority> a_Repository;
        private GenericRepository<cat_Category> c_Repository;
        private GenericRepository<User> u_Repository;
        private GenericRepository<UserProfile> up_Repository;
        private GenericRepository<Role> r_Repository;

        public GenericRepository<cat_Authority> AuthorityRepository
        {
            get
            {

                if (this.a_Repository == null)
                {
                    this.a_Repository = new GenericRepository<cat_Authority>(context);
                }
                return a_Repository;
            }
        }

        public GenericRepository<cat_Category> CategoryRepository
        {
            get
            {

                if (this.c_Repository == null)
                {
                    this.c_Repository = new GenericRepository<cat_Category>(context);
                }
                return c_Repository;
            }
        }

        public GenericRepository<User> UserRepository
        {
            get
            {

                if (this.u_Repository == null)
                {
                    this.u_Repository = new GenericRepository<User>(context);
                }
                return u_Repository;
            }
        }

        public GenericRepository<UserProfile> UserProfileRepository
        {
            get
            {

                if (this.up_Repository == null)
                {
                    this.up_Repository = new GenericRepository<UserProfile>(context);
                }
                return up_Repository;
            }
        }

        public GenericRepository<Role> RoleRepository
        {
            get
            {

                if (this.r_Repository == null)
                {
                    this.r_Repository = new GenericRepository<Role>(context);
                }
                return r_Repository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
