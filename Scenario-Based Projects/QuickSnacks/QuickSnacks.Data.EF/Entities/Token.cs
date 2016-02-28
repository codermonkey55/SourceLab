using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickSnacks.Data.EF.Entities
{
    public class Token : Entity<int>
    {
        public Token()
        {
            Users = new HashSet<User>();
        }

        public virtual Guid AuthKey { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public void AddUser(User user)
        {
            this.Users.Add(user);
        }
    }
}
