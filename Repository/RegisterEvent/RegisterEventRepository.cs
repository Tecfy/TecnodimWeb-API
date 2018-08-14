﻿using DataEF.DataAccess;
using System;
using System.Web.Configuration;

namespace Repository
{
    public class RegisterEventRepository
    {
        #region .: Methods :.

        public void SaveRegisterEvent(Guid userId, Guid identifier, string type, string source, string text)
        {
            if (WebConfigurationManager.AppSettings["Repository.SaveRegisterEvent"].ToString() == "true")
            {
                using (var context = new DBContext())
                {
                    RegisterEvents registerEvent = new RegisterEvents() { UserId = userId, Identifier = identifier, Type = type, Source = source, Text = text };
                    context.RegisterEvents.Add(registerEvent);
                    context.SaveChanges();
                }
            }
        }

        #endregion
    }
}
