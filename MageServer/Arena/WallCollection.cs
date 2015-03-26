using System;
using System.Linq;
using Helper;

namespace MageServer
{
    public class WallCollection : ListCollection<Wall>
    {
        public Wall FindById(Int16 objectId)
        {
            return this.FirstOrDefault(w => objectId - w.ObjectId < 4 && objectId - w.ObjectId >= 0);
        }
    }
}
