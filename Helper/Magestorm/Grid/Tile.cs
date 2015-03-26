using System;

namespace Helper
{
    public class Tile
    {
        public Int32 TileId;
        public ListCollection<TileBlock> TileBlocks;

        public Tile(Int32 tileId)
        {
            TileId = tileId;
            TileBlocks = new ListCollection<TileBlock>();
        }
    }
}
