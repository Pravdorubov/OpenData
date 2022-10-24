using System.Linq;
using OpenData.Domain.Entities;

namespace OpenData.Domain.Abstract
{
    public interface IODRepository
    {
        IQueryable<OpenDataSet> OpenData { get; }
        void SaveOpenDataSet(OpenDataSet opendataset);
        OpenDataSet DeleteOpenDataSet(string ODID);
        IQueryable<Structure> Structures { get; }
        int AddStructure(Structure NewStructure, int CurStructureId);
        IQueryable<Version> Versions { get; }
        void AddVersion(Version NewVersion, long CurVersionId);
        //IQueryable<Owner> Owners { get; }
        //void SaveOwner(Owner owner);
        
    }

}
