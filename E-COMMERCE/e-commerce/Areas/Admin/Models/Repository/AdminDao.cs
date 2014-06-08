using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using e_commerce.Models;

namespace e_commerce.Areas.Admin.Models.Repository
{
    public class AdminDao
    {
        private vsasliteEntities context = new vsasliteEntities();

        public cadusu getAdminByIdAndPassword(string user, string password) {

            return context.cadusu.FirstOrDefault(model => model.loguser.Equals(user) && model.senuser.Equals(password));

        }
    }
}