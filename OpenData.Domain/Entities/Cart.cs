using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Domain.Entities
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();
        public void AddItem(OpenDataSet OpenDataSet, int quantity) 
        {
            CartLine line = lineCollection.Where(p => p.OpenDataSet.ODID == OpenDataSet.ODID).FirstOrDefault();
            if (line == null) 
            {
                lineCollection.Add(new CartLine { OpenDataSet = OpenDataSet,Quantity = quantity });
            } 
            else 
            {
                line.Quantity += quantity;
            }
        }
        
        public void RemoveLine(OpenDataSet OpenDataSet)
        {
            lineCollection.RemoveAll(l => l.OpenDataSet.ODID == OpenDataSet.ODID);
        }
        
        public decimal ComputeTotalValue()
        {
            return 0;
                //lineCollection.Sum(e => e.OpenDataSet.Price * e.Quantity);
        }
        
        public void Clear()
        {
            lineCollection.Clear();
        }
        
        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }
    }
    
    public class CartLine
    {
        public OpenDataSet OpenDataSet { get; set; }
        public int Quantity { get; set; }
    }
}
