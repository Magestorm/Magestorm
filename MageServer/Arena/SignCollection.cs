using System;
using System.Linq;
using Helper;

namespace MageServer
{
    public class SignCollection : ListCollection<Sign>
    {
        public Sign FindById(Int16 objectId)
        {
            return this.FirstOrDefault(s => objectId == s.ObjectId);
        }
    }
}
