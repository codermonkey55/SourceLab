using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace QuickSnacks.Data.NHibernate.Database
{
    public abstract class SessionManager
    {
        private static ISessionFactory _sessionFactory;

        private static Dictionary<string, object> _keyedSessions;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                    InitializeInternalSessionFactory();

                return _sessionFactory;
            }
        }


        static SessionManager()
        {
            _keyedSessions = new Dictionary<string, object>();
        }

        public SessionManager(ISessionFactory sessionFactory)
        {

        }


        private static void InitializeInternalSessionFactory()
        {
            _sessionFactory = SessionFactoryBuilder.CreateSessionFactory();
        }


        public static ISession OpenSession(bool beginTransaction)
        {
            ISession session = SessionFactory.OpenSession();

            if (beginTransaction) session.BeginTransaction();

            return session;
        }

        public static ISession OpenSession(bool beginTransaction, IDbConnection dbconnection)
        {
            ISession session = SessionFactory.OpenSession(dbconnection);

            if (beginTransaction) session.BeginTransaction();

            return session;
        }


        public static IExtendedSession GetDecoratedSession(bool beginTransaction)
        {
            IExtendedSession extSession = new ExtendedSession(SessionFactory.OpenSession());

            if (beginTransaction) extSession.Session.BeginTransaction();

            return extSession;
        }

        public static IExtendedSession GetDecoratedSession(bool beginTransaction, IDbConnection dbconnection)
        {
            IExtendedSession extSession = new ExtendedSession(SessionFactory.OpenSession(dbconnection));

            if(beginTransaction) extSession.Session.BeginTransaction();

            return extSession;
        }


        public static string BindSession(object session)
        {
            string sessionKey = GenerateSessionKey();

            _keyedSessions.Add(sessionKey, session);

            return sessionKey;
        }

        public static TSession GetBoundedSession<TSession>(string sessionKey) where TSession : class
        {
            TSession session = null;

            if(_keyedSessions.ContainsKey(sessionKey))
            {
                session = _keyedSessions[sessionKey] as TSession;
            }

            return session;
        }


        private static string GenerateSessionKey()
        {
            Guid guid = Guid.NewGuid();

            string guidString = Convert.ToBase64String(guid.ToByteArray());

            guidString = guidString.Replace("=", "").Replace("+", "");

            return guidString;
        }
    }
}
