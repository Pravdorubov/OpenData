using System.Collections.Generic;
using OpenData.Domain.Entities;

namespace OpenData.WebUI.Models
{
    public class OpenDataSetsListViewModel
    {
        public IEnumerable<DataSetListView> OpenDataSets { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public List<int> CurrentCategories { get; set; }
        public List<int> CurrentAuthorities { get; set; }
    }
}