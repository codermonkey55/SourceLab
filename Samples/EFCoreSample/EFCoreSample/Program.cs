using System;
using System.Linq;
using EFCoreSample.Database;
using EFCoreSample.Database.Extensions;
using EFCoreSample.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFCoreSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var ctx = new QuickSnacksDbContext();

            var brochureCollection = ctx.Brochures.ToArray();

            var brochureObject = ctx.Set<Brochure>().FirstOrDefault();

            ctx.Entry(brochureObject).State = EntityState.Detached;

            brochureObject.Description = "Includes sweet, savory and wholesame tarty treats.";

            //ctx.Add(brochureObject);
            //ctx.Update(brochureObject);
            //ctx.Remove(brochureObject);

            //--> SaveOrUpdate self-implemented via extendion method...
            ctx.SaveOrUpdate(brochureObject);

            ctx.SaveChanges();

            Console.WriteLine("All Db Transactions completed!");

            Console.ReadLine();
        }
    }
}
