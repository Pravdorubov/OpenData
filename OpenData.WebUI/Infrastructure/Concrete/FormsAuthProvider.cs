using System.Web.Security;
using OpenData.WebUI.Infrastructure.Abstract;

namespace OpenData.WebUI.Infrastructure.Concrete
{
    public class FormsAuthProvider : IAuthProvider
    {
        public bool Authenticate(string username, string password)
        {
            bool result = FormsAuthentication.Authenticate(username, password);  
            //bool result = Membership.ValidateUser (username, password); 
            if (result)
            {
                FormsAuthentication.SetAuthCookie(username, false);
            }
            return result;
        }

    }
}