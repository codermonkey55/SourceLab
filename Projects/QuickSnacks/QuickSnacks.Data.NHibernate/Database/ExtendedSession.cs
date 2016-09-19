using System;
using System.Collections.Generic;
using NHibernate;

namespace QuickSnacks.Data.NHibernate.Database
{
    public class ExtendedSession : IExtendedSession, IDisposable
    {
        protected readonly ISession InternalSession;

        protected readonly ISet<object> InternalItems;

        public ExtendedSession(ISession session)
        {
            InternalSession = session;

            InternalItems = new HashSet<object>();
        }

        public ISet<object> Items
        {
            get { return InternalItems; }
        }

        public ISession Session
        {
            get { return InternalSession; }
        }

        public ITransaction Transaction
        {
            get { return InternalSession.Transaction; }
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
                if (InternalSession != null)
                    InternalSession.Dispose();

                if (InternalItems != null)
                    InternalItems.GetEnumerator().Dispose();
            }
        }
    }
}
