using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace QuickSnacks.Data.NHibernate.Database
{
    public interface IExtendedSession
    {
        ISet<object> Items { get; }

        ISession Session { get; }

        ITransaction Transaction { get; }
    }
}
