using NHibernate.Event;
using NHibernate.Persister.Entity;
using NHibernateSample.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernateSample
{
    public class AuditingEventListener : IPreInsertEventListener, IPreUpdateEventListener
    {
        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            var editedEntity = @event.Entity as IAuditableEntity;
            if (editedEntity == null)
                return false;

            var datAndtime = DateTime.UtcNow;

            PersistAuditInfo(@event.Persister, @event.State, "EditDate", datAndtime);

            editedEntity.EditDate = datAndtime;

            return false;
        }

        public bool OnPreInsert(PreInsertEvent @event)
        {
            var createdEntity = @event.Entity as IAuditableEntity;

            if (createdEntity == null) return false;

            var datAndtime = DateTime.UtcNow;

            PersistAuditInfo(@event.Persister, @event.State, "CreateDate", datAndtime);

            createdEntity.CreateDate = datAndtime;

            return false;
        }

        private void PersistAuditInfo(IEntityPersister persister, object[] state, string propertyName, object value)
        {
            var index = Array.IndexOf(persister.PropertyNames, propertyName);

            if (index == -1) return;

            state[index] = value;
        }
    }
}
