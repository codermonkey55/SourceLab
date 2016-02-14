using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickSnacks.Data.Entities
{
    public class Token : Entity<int>
    {
        public Token()
        {
            Users = new HashSet<User>();
        }

        public Guid AuthKey { get; set; }

        public User User { get; set; }

        public virtual ICollection<User> Users { get; protected set; }

        public void AddUser(User user)
        {
            this.Users.Add(user);
        }
    }
}
