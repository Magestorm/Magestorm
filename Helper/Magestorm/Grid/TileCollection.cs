using System;
namespace Helper
{
    public class TileCollection : ListCollection<Tile>
    {
        public TileCollection(Boolean isBase)
        {
            if (isBase) Add(new Tile(0));
        }
    }
}