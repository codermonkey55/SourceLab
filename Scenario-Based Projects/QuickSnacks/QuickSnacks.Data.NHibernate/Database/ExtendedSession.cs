using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Impl;

namespace QuickSnacks.Data.NHibernate.Database
{
    public class ExtendedSession : IExtendedSession, IDisposable
    {
        protected readonly ISession _internalSession;

        protected readonly ISet<object> _internalItems;

        public ExtendedSession(ISession session)
        {
            _internalSession = session;

            _internalItems = new HashSet<object>();
        }

        public ISet<object> Items
        {
            get { return _internalItems; }
        }

        public ISession Session
        {
            get { return _internalSession; }
        }

        public ITransaction Transaction
        {
            get { return _internalSession.Transaction; }
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_internalSession != null)
                    _internalSession.Dispose();

                if (_internalItems != null) 
                    _internalItems.GetEnumerator().Dispose();
            }
        }
    }
}
