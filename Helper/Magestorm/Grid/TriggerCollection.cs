using System;
using System.Linq;

namespace Helper
{
    public class TriggerCollection : ListCollection<Trigger>
    {
        public TriggerCollection(Boolean isBase)
        {
            if (isBase) Add(new Trigger());
        }

        public Trigger FindById(Int32 triggerId)
        {
            return this.FirstOrDefault(t => triggerId == t.TriggerId);
        }
    }
}
