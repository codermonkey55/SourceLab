using System.Linq;
using NHibernate.Linq;
using QuickSnacks.Data.NHibernate.Database;
using QuickSnacks.Data.NHibernate.Entities;

namespace QuickSnacks.Console.NHibernate
{
    class Program
    {
        static void Main(string[] args)
        {
            var session = SessionManager.OpenSession(true);

            var newUser = new User
            {
                FirstName = "Keira",
                LastName = "Leigh"
            };

            newUser.AssignNewGuidId();

            //session.SaveOrUpdate(newUser);
            //session.Transaction.Commit();

            var brochure = session.Get<Brochure>(1);
            var fingerItem = session.Query<FingerItem>().FirstOrDefault();
            var menu = session.Query<Menu>().FirstOrDefault();
            var menuItem = session.Query<MenuItem>().FirstOrDefault();
            var inActive_token = session.Get<Token>(2);
            var active_token = session.Get<Token>(3);
            var user = session.Query<User>().FirstOrDefault();

            System.Console.WriteLine("All Db Transactions Complete!");

            System.Console.ReadKey();
        }
    }
}
