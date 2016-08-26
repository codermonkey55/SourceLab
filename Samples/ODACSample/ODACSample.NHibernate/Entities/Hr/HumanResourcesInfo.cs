using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODAC_Sample_NHibernate.Entities.Hr
{
    public class HumanResourcesInfo
    {
        public virtual int Id { get; set; }
        public virtual string Department_Name { get; set; }
        public virtual string Street_Address { get; set; }
        public virtual string Country_Name { get; set; }
    }
}
