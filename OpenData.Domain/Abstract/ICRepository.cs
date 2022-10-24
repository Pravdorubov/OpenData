using System.Linq;
using OpenData.Domain.Entities;

namespace OpenData.Domain.Abstract
{
    public interface ICRepository
    {
        IQueryable<cat_Category> Category { get; }
        void SaveCategory(cat_Category Category);
        cat_Category DeleteCategory(int ID);
    }
}
