using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickSnacks.Data.NHibernate.Entities
{
    public class User : Base.Entity<Guid>
    {
        public User()
        {
            Tokens = new HashSet<Token>();
        }

        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string FullName { get; set; }

        //public virtual string FullName
        //{
        //    get
        //    {
        //        return this.FirstName + " " + this.LastName;
        //    }

        //    protected set
        //    {
        //        if (value == null) return;

        //        var fullNameComponents = value.Split(' ');

        //        if (fullNameComponents == null || fullNameComponents.Length < 2) return;

        //        this.FirstName = fullNameComponents[0];
        //        this.LastName = fullNameComponents[1];
        //    }
        //}

        public virtual Token ActiveToken { get; set; }

        public virtual ISet<Token> Tokens { get; protected set; }

        public virtual void AddToken(Token token)
        {
            token.User = this;
            this.Tokens.Add(token);
        }

        public virtual void AssignNewGuidId()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
