using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.Configuration;
using OpenData.Admin.Models;
using OpenData.Domain.Abstract;
using OpenData.Domain.Concrete;
using OpenData.Domain.Entities;
using System.Data;

namespace OpenData.Admin.Providers
{
    public class CustomProfileProvider : ProfileProvider
    {

        private IURepository repository;
        
        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            // коллекция, которая возвращает значения свойств профиля
            SettingsPropertyValueCollection result = new SettingsPropertyValueCollection();

            if (collection == null || collection.Count < 1 || context == null)
            {
                return result;
            }
            // получаем из контекста имя пользователя - логин в системе
            string username = (string)context["UserName"];
            if (String.IsNullOrEmpty(username))
                return result;
           
            // получаем id пользователя из таблицы Users по логину
            int userId = repository.Users.Where(u => u.Login.Equals(username)).FirstOrDefault().ID;
            // по этому id извлекаем профиль из таблицы профилей
            UserProfile profile = repository.GetProfile(userId);

            if (profile != null)
            {
                foreach (SettingsProperty prop in collection)
                {
                    SettingsPropertyValue svp = new SettingsPropertyValue(prop);
                    svp.PropertyValue = profile.GetType().GetProperty(prop.Name).GetValue(profile, null);
                    result.Add(svp);
                }
            }
            else
            {
                foreach (SettingsProperty prop in collection)
                {
                    SettingsPropertyValue svp = new SettingsPropertyValue(prop);
                    svp.PropertyValue = null;
                    result.Add(svp);
                }
            }
            return result;
        }

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            // получаем логин пользователя
            string username = (string)context["UserName"];

            if (username == null || username.Length < 1 || collection.Count < 1)
                return;

            // получаем id пользователя из таблицы Users по логину
            int userId = repository.Users.Where(u => u.Login.Equals(username)).FirstOrDefault().ID;
            // по этому id извлекаем профиль из таблицы профилей
            UserProfile profile = repository.GetProfile(userId);

            // если такой профиль уже есть изменяем его
            if (profile != null)
            {
                foreach (SettingsPropertyValue val in collection)
                {
                    profile.GetType().GetProperty(val.Property.Name).SetValue(profile, val.PropertyValue);
                }
                //profile.LastUpdateDate = DateTime.Now;
                //db.Entry(profile).State = EntityState.Modified;
            }
            else
            {
                // если нет, то создаем новый профиль и добавляем его
                profile = new UserProfile();
                foreach (SettingsPropertyValue val in collection)
                {
                    profile.GetType().GetProperty(val.Property.Name).SetValue(profile, val.PropertyValue);
                }
                //profile.LastUpdateDate = DateTime.Now;
                profile.ID = userId;
                repository.SaveProfile(profile);
            }
            repository.SaveProfile(profile);
        }

        public override int DeleteInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            throw new NotImplementedException();
        }

        public override int DeleteProfiles(string[] usernames)
        {
            throw new NotImplementedException();
        }

        public override int DeleteProfiles(ProfileInfoCollection profiles)
        {
            throw new NotImplementedException();
        }

        public override ProfileInfoCollection FindInactiveProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override ProfileInfoCollection FindProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override ProfileInfoCollection GetAllInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override ProfileInfoCollection GetAllProfiles(ProfileAuthenticationOption authenticationOption, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            throw new NotImplementedException();
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
    }
}