using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickSnacks.Data.EF.Database;
using QuickSnacks.Data.EF.Entities;

namespace QuickSnacks.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var ctx = new QuickSnacksContext();

            var brochure = ctx.Brochures.FirstOrDefault();
            var menu = ctx.Menus.FirstOrDefault();
            var menuItem = ctx.MenuItems.FirstOrDefault();
            var fingterItem = ctx.FingerItems.FirstOrDefault();
            var inActive_token = ctx.Tokens.Find(2);
            var active_token = ctx.Tokens.Find(3);
            var user = ctx.Users.FirstOrDefault();

            System.Console.WriteLine("All Db Transactions Complete!");
            System.Console.ReadKey();
        }
    }
}
