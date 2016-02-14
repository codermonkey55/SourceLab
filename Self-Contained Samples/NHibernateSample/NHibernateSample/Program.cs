using NHibernateSample.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernateSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var sessionFactory = SessionFactoryBuilder.CreateSessionFactory();

            var session = sessionFactory.OpenSession();

            var lion = session.Get<Lion>(1);

            var gazelle = session.Get<Gazelle>(1);

            var bison = new Bison
            {
                Id = Guid.NewGuid(),
                FurThickness = "Really Thick",
                HasFur = true,
                HasHorns = false,
                HasTeeth = true,
                Name = "Bison",
                Description = "Large grazing animal with lots of fur"
            };

            session.SaveOrUpdate(bison);

            session.Flush();

            System.Console.WriteLine("All Db Transactions Complete!");

            System.Console.ReadLine();
        }
    }
}
