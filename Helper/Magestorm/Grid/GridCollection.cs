using System;
using System.Linq;

namespace Helper
{
    public class GridCollection : ListCollection<Grid>
    {
        public Grid FindById(UInt32 gridId)
        {
            return this.FirstOrDefault(g => gridId == g.GridId);
        }
    }
}
