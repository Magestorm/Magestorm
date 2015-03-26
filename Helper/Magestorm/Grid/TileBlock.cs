using System;
using Helper.Math;

namespace Helper
{
    public class TileBlock
    {
        public const Int32 Size = 12;

        public readonly Int16 TopHeight;
        public readonly Int16 BottomHeight;
        public readonly Int32 Index;
        public OrientedBoundingBox TopBoundingBox;
        public OrientedBoundingBox BottomBoundingBox;

        public TileBlock(Int16 topHeight, Int16 bottomHeight, Int32 index)
        {
            TopHeight = topHeight;
            BottomHeight = bottomHeight;
            Index = index;
        }

        public TileBlock(Int16 topHeight, Int16 bottomHeight, Int32 index, OrientedBoundingBox topBoundingBox, OrientedBoundingBox bottomBoundingBox)
        {
            TopHeight = topHeight;
            BottomHeight = bottomHeight;
            Index = index;
            TopBoundingBox = topBoundingBox;
            BottomBoundingBox = bottomBoundingBox;
        }
    }
}
