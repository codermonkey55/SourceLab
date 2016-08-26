using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickSnacks.Data.NHibernate.Entities;

namespace QuickSnacks.Console
{
    public class SeedData
    {
        public void Populate()
        {
            var brochure = new Brochure
            {
                Title = "QuickSnacks DownTown",
                Description = "Serving all local city folks with quick scrumscious snacks."
            };

            var menu = new Menu
            {
                Brochure = brochure,
                Name = "Today's special Menu",
                Description = "All your favorite delights"
            };

            var fingerItem = new FingerItem
            {
                Name = "Hamlets",
                Description = "another Pig in a blanket"
            };

            var menuItem = new MenuItem
            {
                Menu = menu,
                FingerItem = fingerItem,
                Name = "Sausage Rolls",
                Description = "a.k.a Pig in a blanket"
            };

            var user = new User
            {
                FirstName = "John",
                LastName = "Doe",
                ActiveToken = null
            };

            var token = new Token
            {
                AuthKey = Guid.NewGuid(),
                User = user
            };
        }
    }
}
