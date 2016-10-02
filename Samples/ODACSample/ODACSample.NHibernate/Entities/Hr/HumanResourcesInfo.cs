namespace ODAC_Sample_NHibernate.Entities.Hr
{
    public class HumanResourcesInfo
    {
        public virtual int Id { get; set; }
        public virtual string Department { get; set; }
        public virtual string Address { get; set; }
        public virtual string Country { get; set; }
    }
}