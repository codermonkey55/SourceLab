using NHibernate;
using NHibernate.Linq;
using QuickSnacks.Data.NHibernate.Database;
using QuickSnacks.Data.NHibernate.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickSnacks.Data.NHibernate
{
    class NHibernateApiSamples
    {
        public void ExploreFetchApi()
        {
            var session = SessionManager.OpenSession(beginTransaction: false);
            var query = session.Query<Menu>();
            query.Fetch(x => x.Brochure)
                 .FetchMany(x => x.MenuItems.Where(mi => mi.CreateDate > DateTime.Now))
                 .Where(menu => menu.Description.Contains("ss"))
                 .ToList();
        }
    }
}
