using OpenData.Domain.Abstract;
using OpenData.Domain.Entities;
using System.Linq;

namespace OpenData.Domain.Concrete
{
  public class EFODRepository : IODRepository
  {
    private EFDbContext context;

    public EFODRepository(EFDbContext context)
    {
        this.context = context;
    }
    
    public IQueryable<OpenDataSet> OpenData 
    {
        get { return context.OpenDataSets.Include("Authority").Include("Category").Include("Users").Include("Versions"); }
    }

    public void SaveOpenDataSet(OpenDataSet opendataset)
    {
        if (opendataset.ODID == null)
        {
            opendataset.ODID = opendataset.AuthorityINN + "-" + opendataset.Name;
            context.OpenDataSets.Add(opendataset);
        }
        else
        {
            OpenDataSet dbEntry = context.OpenDataSets.Find(opendataset.ODID);
            if (dbEntry != null)
            {
                dbEntry.ODID = opendataset.AuthorityINN + "-" + opendataset.Name;
                dbEntry.Name = opendataset.Name;
                dbEntry.Description = opendataset.Description;
                dbEntry.FullDescription = opendataset.FullDescription;
                //dbEntry.AuthorityINN = opendataset.AuthorityINN;
                //dbEntry.CategorySt = opendataset.CategorySt;
                //dbEntry.ImageData = opendataset.ImageData;
                //dbEntry.ImageMimeType = opendataset.ImageMimeType;
                dbEntry.CategoryID = opendataset.CategoryID;
            }
        }
        context.SaveChanges();
        
    }

    public OpenDataSet DeleteOpenDataSet(string ODID)
    {
        OpenDataSet dbEntry = context.OpenDataSets.Find(ODID);
        if (dbEntry != null)
        {
            context.OpenDataSets.Remove(dbEntry);
            context.SaveChanges();
        }
        return dbEntry;
    }

    public IQueryable<Structure> Structures
    {
        get { return context.Structures; }
    }

    public int AddStructure(Structure NewStructure, int CurStructureId = 0)
    {
        if (CurStructureId != 0)
        {
            Structure dbEntry = context.Structures.Find(CurStructureId);
            dbEntry.IsCurrent = false;
            context.SaveChanges();
        }
        context.Structures.Add(NewStructure);
        context.SaveChanges();
        return NewStructure.ID;
    }

    public IQueryable<Version> Versions
    {
        get { return context.Versions; }
    }

    public void AddVersion(Version NewVersion, long CurVersionId=0)
    {
        if (CurVersionId != 0)
        {
            Version dbEntry = context.Versions.Find(CurVersionId);
            dbEntry.IsCurrent = false;
            context.SaveChanges();
        }
        context.Versions.Add(NewVersion);
        context.SaveChanges();
    }

    //public IQueryable<Owner> Owners
    //{
    //    get { return context.Owners; }
    //}

    //public void SaveOwner(Owner owner)
    //{
    //    if (owner.ID == null)
    //    {
    //        context.Owners.Add(owner);
    //    }
    //    else
    //    {
    //        Owner dbEntry = context.Owners.Find(owner.ID);
    //        if (dbEntry != null)
    //        {
    //            dbEntry.ID = owner.ID;
    //            dbEntry.SNP = owner.SNP;
    //            dbEntry.Phone = owner.Phone;
    //            dbEntry.Email = owner.Email;
    //        }
    //    }
    //    context.SaveChanges();

    //}


  }
}
