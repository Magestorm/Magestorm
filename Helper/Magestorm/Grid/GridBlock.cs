using System;
using System.ComponentModel;
using SharpDX;
using Helper.Math;
using OrientedBoundingBox = Helper.Math.OrientedBoundingBox;

namespace Helper
{
    public enum GridBlockShape
    {
        None,
        CenterPointShort,
        WestShortSlant,
        EastStairway,
        MediumFullArchEastWest,
        SmallWestHalfArch,
        SmallEastHalfArch,
        SmallNorthHalfArch,
        SmallSouthHalfArch,
        CenterPointLong,
        CenterPointMid,
        MediumFullArchNorthSouth,
        Cylinder,
        EastCurvedRamp,
        WestCurvedRamp,
        SouthCurvedRamp,
        NorthCurvedRamp,
        SouthEastCurvedRamp,
        NorthEastCurvedRamp,
        NorthWestCurvedRamp,
        SouthWestCurvedRamp,
        EastAndSouthCurvedRamp,
        EastAndNorthCurvedRamp,
        WestAndNorthCurvedRamp,
        WestAndSouthCurvedRamp,
        LargeWestHalfArch,
        LargeEastHalfArch,
        LargeNorthHalfArch,
        LargeSouthHalfArch,
        LargeWestAndNorthHalfArch,
        LargeWestAndSouthHalfArch,
        LargeEastndSouthHalfArch,
        LargeEastndNorthHalfArch,
        EastLongSlant,
        WestLongSlant,
        SouthLongSlant,
        NorthLongSlant,
        EastLowLongSlant,
        WestLowLongSlant,
        SouthLowLongSlant,
        NorthLowLongSlant,
        EastHalfCutFullArch,
        WestHalfCutFullArch,
        SouthHalfCutFullArch,
        NorthHalfCutFullArch,
        WestFullVerticalHalfArch,
        EastFullVerticalHalfArch,
        NorthFullVerticalHalfArch,
        SouthFullVerticalHalfArch,
        SmallFullArchEastWest,
        SmallFullArchNorthSouth,
    }


    [DefaultPropertyAttribute("BlockId")]
    public class GridBlock
    {
        #region Constants
        private const String LocationCategory = "Location";
        private const String TexturesCategory = "Textures";
        private const String ObjectsCategory = "Objects";
        private const String UnknownCategory = "Unknown";
        #endregion

        #region Fields

        private Int32 _lowTileId;
        private Int32 _lowBoxTopMod;

        private Int32 _blockFlags;
        private Int32 _unknown16;

        private Int32 _lowTopTextureId;
        private Int32 _lowSidesTextureId;
        private Int32 _midTopTextureId;
        private Int32 _midSidesTextureId;
        private Int32 _highTextureId;
        private Int32 _ceilingTextureId;

        private GridBlockShape _lowTopShape;
        private GridBlockShape _midBottomShape;

        private Int32 _lowBoxTopZ;
        private Int32 _midBoxBottomZ;
        private Int32 _modBoxTopZ;
        private Int32 _highBoxBottomZ;

        public OrientedBoundingBox ContainerBox;
        public OrientedBoundingBox LowBox;
        public OrientedBoundingBox MidBox;
        public OrientedBoundingBox HighBox;
        public Tile LowBoxTile;

        private Int32 _unknown0;
        private Int32 _unknown17;
        private Int32 _unknown18;  
        #endregion  

        #region Properties
        [ReadOnly(true), CategoryAttribute(LocationCategory)]
        public Int32 BlockId { get; private set; }

        [ReadOnly(true), CategoryAttribute(LocationCategory)]
        public Int32 X { get; private set; }

        [ReadOnly(true), CategoryAttribute(LocationCategory)]
        public Int32 Y { get; private set; }

        [CategoryAttribute(LocationCategory)]
        public Int32 LowBoxTopZ
        {
            get { return _lowBoxTopZ; }
            set
            {
                _lowBoxTopZ = value;

                LowBox = new OrientedBoundingBox(new Vector3(X, Y, -512), new Vector3(64, 64, 512 + _lowBoxTopZ), 0.0f);
            }
        }

        [CategoryAttribute(LocationCategory)]
        public Int32 MidBoxBottomZ
        {
            get { return _midBoxBottomZ; }
            set
            {
                _midBoxBottomZ = value;

                MidBox = new OrientedBoundingBox(new Vector3(X, Y, _midBoxBottomZ), new Vector3(64, 64, _modBoxTopZ - _midBoxBottomZ), 0.0f);
            }
        }

        [CategoryAttribute(LocationCategory)]
        public Int32 MidBoxTopZ
        {
            get { return _modBoxTopZ; }
            set
            {
                _modBoxTopZ = value;

                MidBox = new OrientedBoundingBox(new Vector3(X, Y, _midBoxBottomZ), new Vector3(64, 64, _modBoxTopZ - _midBoxBottomZ), 0.0f);
            }
        }

        [CategoryAttribute(LocationCategory)]
        public Int32 HighBoxBottomZ
        {
            get { return _highBoxBottomZ; }
            set
            {
                _highBoxBottomZ = value;

                HighBox = new OrientedBoundingBox(new Vector3(X, Y, _highBoxBottomZ), new Vector3(64, 64, 64), 0.0f);
            }
        }

        [CategoryAttribute(LocationCategory)]
        public Int32 LowBoxTopMod
        {
            get { return _lowBoxTopMod; }
            set { _lowBoxTopMod = value; }
        }

        [CategoryAttribute(ObjectsCategory)]
        public Int32 LowTileId
        {
            get { return _lowTileId; }
            set { _lowTileId = value; }
        }

        [CategoryAttribute(ObjectsCategory)]
        public GridBlockShape LowTopShape
        {
            get { return _lowTopShape; }
            set { _lowTopShape = value; }
        }

        [CategoryAttribute(ObjectsCategory)]
        public GridBlockShape MidBottomShape
        {
            get { return _midBottomShape; }
            set { _midBottomShape = value; }
        }

        [CategoryAttribute(UnknownCategory)]
        public Int32 BlockFlags
        {
            get { return _blockFlags; }
            set { _blockFlags = value; }
        }

        [CategoryAttribute(UnknownCategory)]
        public Int32 Unknown16
        {
            get { return _unknown16; }
            set { _unknown16 = value; }
        }

        [CategoryAttribute(TexturesCategory)]
        public Int32 LowTopTextureId
        {
            get { return _lowTopTextureId; }
            set
            {
                _lowTopTextureId = value;
            }
        }

        [CategoryAttribute(TexturesCategory)]
        public Int32 LowSidesTextureId
        {
            get { return _lowSidesTextureId; }
            set
            {
                _lowSidesTextureId = value;
            }
        }

        [CategoryAttribute(TexturesCategory)]
        public Int32 MidTopTextureId
        {
            get { return _midTopTextureId; }
            set
            {
                _midTopTextureId = value;
            }
        }

        [CategoryAttribute(TexturesCategory)]
        public Int32 MidSidesTextureId
        {
            get { return _midSidesTextureId; }
            set
            {
                _midSidesTextureId = value;
            }
        }

        [CategoryAttribute(TexturesCategory)]
        public Int32 HighTextureId
        {
            get { return _highTextureId; }
            set
            {
                _highTextureId = value;
            }
        }

        [CategoryAttribute(TexturesCategory)]
        public Int32 CeilingTextureId
        {
            get { return _ceilingTextureId; }
            set { _ceilingTextureId = value; }
        }

        [CategoryAttribute(UnknownCategory)]
        public Int32 Unknown17
        {
            get { return _unknown17; }
            set { _unknown17 = value; }
        }

        [CategoryAttribute(UnknownCategory)]
        public Int32 Unknown18
        {
            get { return _unknown18; }
            set { _unknown18 = value; }
        }

        [CategoryAttribute(UnknownCategory)]
        public Int32 Unknown0
        {
            get { return _unknown0; }
            set { _unknown0 = value; }
        }

        #endregion

        public Boolean HasSkybox
        {
            get
            {
                return MidBoxTopZ == 32767;
            }
        }

        public GridBlock(Int32 blockId, Int32 x, Int32 y)
        {
            BlockId = blockId;
            X = x;
            Y = y;
        }
    }
}
