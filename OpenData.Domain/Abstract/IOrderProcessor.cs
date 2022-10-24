using OpenData.Domain.Entities;

namespace OpenData.Domain.Abstract
{
    public interface IOrderProcessor
    {
        void ProcessOrder(ShippingDetails shippingDetails,string OperatorEmail);
    }
}
