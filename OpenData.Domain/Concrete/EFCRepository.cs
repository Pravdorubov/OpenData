using OpenData.Domain.Abstract;
using OpenData.Domain.Entities;
using System.Linq;

namespace OpenData.Domain.Concrete
{
    public class EFCRepository : ICRepository
    {
        private EFDbContext context;

        public EFCRepository(EFDbContext context)
        {
            this.context = context;
        }
        
        public IQueryable<cat_Category> Category
        {
            get { return context.Categories; }
        }

        public void SaveCategory(cat_Category Category)
        {
            if (Category.ID == null)
            {
                context.Categories.Add(Category);
            }
            else
            {
                cat_Category dbEntry = context.Categories.Find(Category.ID);
                if (dbEntry != null)
                {
                    dbEntry.ID = Category.ID;
                    dbEntry.Name = Category.Name;
                    dbEntry.Description = Category.Description;
                    dbEntry.ImageData = Category.ImageData;
                    dbEntry.ImageMimeType = Category.ImageMimeType;
                }
            }
            context.SaveChanges();
        }

        public cat_Category DeleteCategory(int INN)
        {
            return new cat_Category();
        }
    }
}
