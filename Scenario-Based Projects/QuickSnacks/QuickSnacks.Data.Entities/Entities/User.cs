using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickSnacks.Data.Entities
{
    public class User : Entity<Guid>
    {
        public User()
        {
            Tokens = new HashSet<Token>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName {

            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }

            protected set
            {
                if (value == null) return;

                var fullNameComponents = value.Split(' ');

                if (fullNameComponents == null || fullNameComponents.Length < 2) return;

                this.FirstName = fullNameComponents[0];

                this.LastName = fullNameComponents[1];
            }
        }

        public Token ActiveToken { get; set; }

        public virtual ICollection<Token> Tokens { get; protected set; }

        public void AddToken(Token token)
        {
            this.Tokens.Add(token);
        }
    }
}
