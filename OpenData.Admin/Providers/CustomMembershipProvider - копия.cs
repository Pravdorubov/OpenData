using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Helpers;
using System.Security.Cryptography;
using System.Web.WebPages;
using System.Text;
using Microsoft.Internal.Web.Utils;
using OpenData.Domain.Entities;
using OpenData.Domain.Concrete;
using OpenData.Domain.Abstract;
using OpenData.Admin.Models;
using OpenData.Admin.Infrastructure.Abstract;
using Ninject;


namespace OpenData.Admin.Providers
{
    public class CustomMembershipProvider : MembershipProvider
    {
        [Inject]
        public IURepository repository { get; set; }

        public int GetId(string username)
        {
            int iid = repository.Users.FirstOrDefault(u => u.Login == username).ID;
            return repository.Users.FirstOrDefault(u => u.Login == username).ID;
        }
        
            
      
     private string GetHashString(string s)  
        {  
          //переводим строку в байт-массим  
          byte[] bytes = Encoding.Unicode.GetBytes(s);  
      
          //создаем объект для получения средст шифрования  
          MD5CryptoServiceProvider CSP =  
              new MD5CryptoServiceProvider();  
              
          //вычисляем хеш-представление в байтах  
          byte[] byteHash = CSP.ComputeHash(bytes);  
      
          string hash = string.Empty;  
      
          //формируем одну цельную строку из массива  
          foreach (byte b in byteHash)  
              hash += string.Format("{0:x2}", b);  
      
          return hash;  
        }  

        public override bool ValidateUser(string username, string password)
        {
            bool isValid = false;
            
                try
                {
                    User user = repository.Users.Where(u => u.Login == username).FirstOrDefault();
                    if (user != null && user.UserProfile.Password==GetHashString(password))
                    {
                        isValid = true;
                    }
                }
                catch
                {
                    isValid = false;
                }
            
            return isValid;
        }

        public MembershipUser CreateUser(string username, string password, int roleId, CreateUserModel model)
        {
            MembershipUser membershipUser = GetUser(username, false);
            
            if (membershipUser == null)
            {
                try
                {
                    User user = new User();
                    user.Login = username;
                    user.UserProfile = new UserProfile();
                    user.UserProfile.Password = GetHashString(password);
                    user.UserProfile.CreateDate = DateTime.Now;
                    user.UserProfile.FNS = model.FNS;
                    user.UserProfile.Email = model.Email;
                    user.UserProfile.Duty = model.Duty;
                    user.UserProfile.Phone = model.Phone;
                    user.UserProfile.AuthorityINN = model.AuthorityINN;
                    user.IsActive = true;
                    user.RoleID= roleId;
                    //repository.s
                    repository.SaveUser(user);
                          
                    membershipUser = GetUser(user.Login, false);
                    return membershipUser;
                    //}
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        public override MembershipUser GetUser(string login, bool userIsOnline)
        {
            try
            {
                var users = repository.Users.Where(u => u.Login == login);
                    if (users.Count() > 0)
                    {
                        User user = users.First();
                        
                        UserProfile userProfile = repository.GetProfile(user.ID);
                        MembershipUser memberUser = new MembershipUser("MyMembershipProvider", user.Login, null, null, null, null,
                            false, false, userProfile.CreateDate, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);
                        return memberUser;
                    }
                //}
            }
            catch
            {
                return null;
            }
            return null;
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }
        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }
        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }
        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }
        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }
        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }
        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }
        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }
        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }
        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }
        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }
        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }
        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }
        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }
        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }
        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }
    }
}