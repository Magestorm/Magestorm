using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Helper.Timing;
using SharpDX;
using OrientedBoundingBox = Helper.Math.OrientedBoundingBox;

namespace Helper
{
    public class Grid
    {
        public Shrine BalanceShrine;
        public Shrine ChaosShrine;
        public Shrine OrderShrine;

        public String GameName;
        public String GridFilename;
        public String ObjectsFilename;
        public Int32 GridId;
        public Byte MaxPlayers;
        public String MiscFilename;
        public String Name;  
        public String ShortGameName;
        public Int16 TimeLimit;
        public String TriggerFilename;
        public String WorldFilename;
        public Single ExpBonus;

        public GridBlockCollection GridBlocks;
        public GridObjectCollection GridObjects;
        public GridObjectDefinitionCollection GridObjectDefinitions;
        public ThinCollection Thins;
        public TileCollection Tiles;
        public TriggerCollection Triggers;
        public PoolCollection Pools;


        public Grid()
        {
            GridBlocks = new GridBlockCollection(true);
            GridObjects = new GridObjectCollection();
            GridObjectDefinitions = new GridObjectDefinitionCollection();
            Thins = new ThinCollection();
            Tiles = new TileCollection(true);
            Triggers = new TriggerCollection(true);
            Pools = new PoolCollection();
        }
        public Grid(Grid grid)
        {
            GridBlocks = grid.GridBlocks;
            Thins = grid.Thins;
            Tiles = grid.Tiles;
            Triggers = new TriggerCollection(false);

            foreach (Trigger t in grid.Triggers)
            {
                Trigger nTrigger = new Trigger
                {
                    Duration = null,
                    Cooldown = new Interval(2000, false),
                    Enabled = t.Enabled,
                    EndAngle = t.EndAngle,
                    InitialState = t.InitialState,
                    IsFromValhalla = t.IsFromValhalla,
                    MaxAngleRate = t.MaxAngleRate,
                    MaxRate = t.MaxRate,
                    MoveCeiling = t.MoveCeiling,
                    MoveFloor = t.MoveFloor,
                    MoveRooftop = t.MoveRooftop,
                    NextTrigger = t.NextTrigger,
                    NextTriggerTiming = t.NextTriggerTiming,
                    OffHeight = t.OffHeight,
                    OffSound = t.OffSound,
                    OffText = t.OffText,
                    OnHeight = t.OnHeight,
                    OnSound = t.OnSound,
                    OnText = t.OnText,
                    Position = new Vector3(t.Position.X, t.Position.Y, t.Position.X),
                    Random = t.Random,
                    ResetTimer = t.ResetTimer,
                    SlideAmount = t.SlideAmount,
                    SlideAxis = t.SlideAxis,
                    Speed = t.Speed,
                    StartAngle = t.StartAngle,
                    CurrentState = TriggerState.Inactive,
                    Team = t.Team,
                    TextureOff = t.TextureOff,
                    TextureOn = t.TextureOn,
                    TriggerId = t.TriggerId,
                    TriggerType = t.TriggerType,
                    X0 = t.X0,
                    X1 = t.X1,
                    X2 = t.X2,
                    X3 = t.X3,
                    X4 = t.X4,
                    Y0 = t.Y0,
                    Y1 = t.Y1,
                    Y2 = t.Y2,
                    Y3 = t.Y3,
                    Y4 = t.Y4
                };

                Triggers.Add(nTrigger);
            }

            BalanceShrine = new Shrine(grid.BalanceShrine.Team, grid.BalanceShrine.ShrineId, grid.BalanceShrine.Power, grid.BalanceShrine.CurrentBias);
            ChaosShrine = new Shrine(grid.ChaosShrine.Team, grid.ChaosShrine.ShrineId, grid.ChaosShrine.Power, grid.ChaosShrine.CurrentBias);
            OrderShrine = new Shrine(grid.OrderShrine.Team, grid.OrderShrine.ShrineId, grid.OrderShrine.Power, grid.OrderShrine.CurrentBias);

            GameName = grid.GameName;
            GridFilename = grid.GridFilename;
            GridId = grid.GridId;
            MaxPlayers = grid.MaxPlayers;
            MiscFilename = grid.MiscFilename;
            Name = grid.Name;

            Pools = new PoolCollection();
            foreach (Pool p in grid.Pools)
            {
                Pools.Add(new Pool(p));
            }

            ShortGameName = grid.ShortGameName;
            TimeLimit = grid.TimeLimit;
            TriggerFilename = grid.TriggerFilename;
            WorldFilename = grid.WorldFilename;
            ExpBonus = grid.ExpBonus;
        }

        public Shrine GetShrineById(Byte shrineId)
        {
            if (ChaosShrine.ShrineId == shrineId)
            {
                return ChaosShrine;
            }

            if (BalanceShrine.ShrineId == shrineId)
            {
                return BalanceShrine;
            }

            if (OrderShrine.ShrineId == shrineId)
            {
                return OrderShrine;
            }

            return null;
        }
        public Shrine GetShrineByTeam(Team team)
        {
            switch (team)
            {
                case Team.Chaos:
                {
                    return ChaosShrine;
                }
                case Team.Balance:
                {
                    return BalanceShrine;
                }
                case Team.Order:
                {
                    return OrderShrine;
                }
                default:
                {
                    return null;
                }
            }
        }

		public static void LoadAllGrids(LogBox logBox)
		{
			String fName = String.Format("{0}\\Arenas.dat", Directory.GetCurrentDirectory());
			Int32 aCount = NativeMethods.GetPrivateProfileInt32("arenadefs", "numarenas", fName);

			logBox.WriteMessage(String.Format("Loading {0} Arenas...", aCount), System.Drawing.Color.Blue);

			Int16 i;
			for (i = 0; i < aCount; i++)
			{
				Grid grid = new Grid();
				if (!grid.Load(i))
				{
					logBox.WriteMessage(String.Format("Error loading Grid #{0}", i), System.Drawing.Color.Red);
					continue;
				}

				GridManager.Grids.Add(grid);
				logBox.WriteMessage(String.Format("Loaded Grid: {0} ({1})", grid.GameName, grid.Name), System.Drawing.Color.Green);

				Application.DoEvents();
			}

			logBox.WriteMessage(String.Format("{0} out of {1} Arenas loaded.", i, aCount), System.Drawing.Color.Blue);
		}

        public Boolean Load(Int32 gridId)
        {
            try
            {
                String arenaDatFilename = String.Format("{0}\\Arenas.dat", Directory.GetCurrentDirectory());
                const String gridDatLocation = "{0}\\Grids\\Grid{1:00}\\{2}";

                String keyName = String.Format("arena{0:00}", gridId);

                WorldFilename = String.Format(gridDatLocation, Directory.GetCurrentDirectory(), gridId, "World.dat");
                GridFilename = String.Format(gridDatLocation, Directory.GetCurrentDirectory(), gridId, "Grid.dat");
                ObjectsFilename = String.Format(gridDatLocation, Directory.GetCurrentDirectory(), gridId, "Objects.dat");
                MiscFilename = String.Format(gridDatLocation, Directory.GetCurrentDirectory(), gridId, "Misc.dat");
                TriggerFilename = String.Format(gridDatLocation, Directory.GetCurrentDirectory(), gridId, "Trigger.dat");
                GridId = gridId;
                Name = NativeMethods.GetPrivateProfileString(keyName, "grid", arenaDatFilename);
                GameName = NativeMethods.GetPrivateProfileString(keyName, "name", arenaDatFilename);
                ShortGameName = NativeMethods.GetPrivateProfileString(keyName, "short_name", arenaDatFilename);
                MaxPlayers = NativeMethods.GetPrivateProfileByte(keyName, "maxplayers", arenaDatFilename);
                TimeLimit = NativeMethods.GetPrivateProfileInt16(keyName, "timelimit", arenaDatFilename);
                ExpBonus = NativeMethods.GetPrivateProfileSingle(keyName, "expbonus", arenaDatFilename);

                Pools = new PoolCollection();

                Int32 poolCount = NativeMethods.GetPrivateProfileInt32("earthblooddefs", "numearthblood", WorldFilename);
                for (Int32 x = 0; x < poolCount; x++)
                {
                    Pools.Add(new Pool(Convert.ToByte(x), NativeMethods.GetPrivateProfileInt16(String.Format("earthblood{0:00}", x), "power", WorldFilename), 100));
                }

                Int32 shrineCount = NativeMethods.GetPrivateProfileInt32("shrinedefs", "numshrines", WorldFilename);
                for (Int32 x = 0; x < shrineCount; x++)
                {
                    String shrineString = String.Format("shrine{0:00}", x);

                    Int16 power = NativeMethods.GetPrivateProfileInt16(shrineString, "power", WorldFilename);
                    Int16 bias = NativeMethods.GetPrivateProfileInt16(shrineString, "bias", WorldFilename);

                    switch (NativeMethods.GetPrivateProfileString(shrineString, "alignment", WorldFilename))
                    {
                        case "chaos":
                            {
                                ChaosShrine = new Shrine(Team.Chaos, (Byte)x, power, bias);
                                break;
                            }

                        case "balance":
                            {
                                BalanceShrine = new Shrine(Team.Balance, (Byte)x, power, bias);
                                break;
                            }

                        case "order":
                            {
                                OrderShrine = new Shrine(Team.Order, (Byte)x, power, bias);
                                break;
                            }
                    }
                }

                LoadTriggers();
                LoadThins();
                LoadTiles();
                LoadObjectDefinitions();
                LoadGrid(true);

            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        public String Load(String folderName)
        {
            try
            {
                String arenaDatFilename = String.Format("{0}\\Arenas.dat", Directory.GetCurrentDirectory());

                const String gridDatLocation = "{0}\\{1}";

                WorldFilename = String.Format(gridDatLocation, folderName, "World.dat");
                GridFilename = String.Format(gridDatLocation, folderName, "Grid.dat");
                ObjectsFilename = String.Format(gridDatLocation, folderName, "Objects.dat");
                MiscFilename = String.Format(gridDatLocation, folderName, "Misc.dat");
                TriggerFilename = String.Format(gridDatLocation, folderName, "Trigger.dat");

                if (!File.Exists(WorldFilename))
                {
                    return "File not found: World.dat";
                }

                if (!File.Exists(GridFilename))
                {
                    return "File not found: Grid.dat";
                }

                if (!File.Exists(ObjectsFilename))
                {
                    return "File not found: Objects.dat";
                }

                if (!File.Exists(MiscFilename))
                {
                    return "File not found: Misc.dat";
                }

                if (!File.Exists(TriggerFilename))
                {
                    return "File not found: Trigger.dat";
                }

                GridId = NativeMethods.GetPrivateProfileInt32("map", "grid_id", WorldFilename);

                if (GridId == -1)
                {
                    return "grid_id is not defined in World.dat.";
                }

                String keyName = String.Format("arena{0:00}", GridId);

                Name = NativeMethods.GetPrivateProfileString(keyName, "grid", arenaDatFilename);
                GameName = NativeMethods.GetPrivateProfileString(keyName, "name", arenaDatFilename);
                ShortGameName = NativeMethods.GetPrivateProfileString(keyName, "short_name", arenaDatFilename);
                MaxPlayers = NativeMethods.GetPrivateProfileByte(keyName, "maxplayers", arenaDatFilename);
                TimeLimit = NativeMethods.GetPrivateProfileInt16(keyName, "timelimit", arenaDatFilename);
                ExpBonus = NativeMethods.GetPrivateProfileSingle(keyName, "expbonus", arenaDatFilename);

                Pools = new PoolCollection();

                Int32 poolCount = NativeMethods.GetPrivateProfileInt32("earthblooddefs", "numearthblood", WorldFilename);
                for (Int32 x = 0; x < poolCount; x++)
                {
                    Pools.Add(new Pool(Convert.ToByte(x), NativeMethods.GetPrivateProfileInt16(String.Format("earthblood{0:00}", x), "power", WorldFilename), 100));
                }

                Int32 shrineCount = NativeMethods.GetPrivateProfileInt32("shrinedefs", "numshrines", WorldFilename);
                for (Int32 x = 0; x < shrineCount; x++)
                {
                    String shrineString = String.Format("shrine{0:00}", x);

                    Int16 power = NativeMethods.GetPrivateProfileInt16(shrineString, "power", WorldFilename);
                    Int16 bias = NativeMethods.GetPrivateProfileInt16(shrineString, "bias", WorldFilename);

                    switch (NativeMethods.GetPrivateProfileString(shrineString, "alignment", WorldFilename))
                    {
                        case "chaos":
                        {
                            ChaosShrine = new Shrine(Team.Chaos, (Byte)x, power, bias);
                            break;
                        }

                        case "balance":
                        {
                            BalanceShrine = new Shrine(Team.Balance, (Byte)x, power, bias);
                            break;
                        }

                        case "order":
                        {
                            OrderShrine = new Shrine(Team.Order, (Byte)x, power, bias);
                            break;
                        }
                    }
                }

                LoadTriggers();
                LoadThins();
                LoadTiles();
                LoadObjectDefinitions();
                LoadGrid(false);

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "";
        }
        public Boolean Save()
        {
            using (FileStream gridStream = new FileStream(GridFilename, FileMode.Create, FileAccess.ReadWrite))
            {
                for (Int32 i = 1; i <= 16384; i++)
                {
                    Byte[] gridBytes = BitConverter.GetBytes(GridBlocks[i].Unknown0);
                    gridStream.Write(gridBytes, 0, 2);
                    gridBytes = BitConverter.GetBytes(GridBlocks[i].LowBoxTopMod);
                    gridStream.Write(gridBytes, 0, 2);
                    gridBytes = BitConverter.GetBytes(GridBlocks[i].LowSidesTextureId);
                    gridStream.Write(gridBytes, 0, 2);
                    gridBytes = BitConverter.GetBytes(GridBlocks[i].LowTopTextureId);
                    gridStream.Write(gridBytes, 0, 2);
                    gridBytes = BitConverter.GetBytes(GridBlocks[i].HighTextureId);
                    gridStream.Write(gridBytes, 0, 2);
                    gridBytes = BitConverter.GetBytes(GridBlocks[i].LowBoxTopZ);
                    gridStream.Write(gridBytes, 0, 2);
                    gridBytes = BitConverter.GetBytes(GridBlocks[i].MidBoxBottomZ);
                    gridStream.Write(gridBytes, 0, 2);
                    gridBytes = BitConverter.GetBytes(GridBlocks[i].MidBoxTopZ);
                    gridStream.Write(gridBytes, 0, 2);
                    gridBytes = BitConverter.GetBytes(GridBlocks[i].LowTileId);
                    gridStream.Write(gridBytes, 0, 2);
                    gridBytes = BitConverter.GetBytes(GridBlocks[i].HighBoxBottomZ);
                    gridStream.Write(gridBytes, 0, 2);
                    gridBytes = BitConverter.GetBytes(GridBlocks[i].BlockFlags);
                    gridStream.Write(gridBytes, 0, 2);
                    gridBytes = BitConverter.GetBytes((Int16)GridBlocks[i].LowTopShape);
                    gridStream.Write(gridBytes, 0, 2);
                    gridBytes = BitConverter.GetBytes((Int16)GridBlocks[i].MidBottomShape);
                    gridStream.Write(gridBytes, 0, 2);
                    gridBytes = BitConverter.GetBytes(GridBlocks[i].MidSidesTextureId);
                    gridStream.Write(gridBytes, 0, 2);
                    gridBytes = BitConverter.GetBytes(GridBlocks[i].MidTopTextureId);
                    gridStream.Write(gridBytes, 0, 2);
                    gridBytes = BitConverter.GetBytes(GridBlocks[i].CeilingTextureId);
                    gridStream.Write(gridBytes, 0, 2);
                    gridBytes = BitConverter.GetBytes(GridBlocks[i].Unknown16);
                    gridStream.Write(gridBytes, 0, 2);
                    gridBytes = BitConverter.GetBytes(GridBlocks[i].Unknown17);
                    gridStream.Write(gridBytes, 0, 2);
                    gridBytes = BitConverter.GetBytes(GridBlocks[i].Unknown18);
                    gridStream.Write(gridBytes, 0, 2);
                }

                gridStream.Write(new byte[4], 0, 4);

                for (Int32 i = 1; i <= 997; i++)
                {
                    Byte[] gridBytes = BitConverter.GetBytes(GridObjects[i].ObjectId);
                    gridStream.Write(gridBytes, 0, 4);
                    gridBytes = BitConverter.GetBytes(GridObjects[i].X);
                    gridStream.Write(gridBytes, 0, 4);
                    gridBytes = BitConverter.GetBytes(GridObjects[i].Y);
                    gridStream.Write(gridBytes, 0, 4);
                    gridBytes = BitConverter.GetBytes(GridObjects[i].Z);
                    gridStream.Write(gridBytes, 0, 4);
                }
            }

            using (FileStream objectStream = new FileStream(ObjectsFilename, FileMode.Create, FileAccess.ReadWrite))
            {
                for (Int32 i = 1; i <= 139; i++)
                {
                    Byte[] objectBytes = BitConverter.GetBytes(GridObjectDefinitions[i].DefinitionId);
                    objectStream.Write(objectBytes, 0, 4);
                    objectBytes = BitConverter.GetBytes(GridObjectDefinitions[i].ImageId);
                    objectStream.Write(objectBytes, 0, 4);
                    objectBytes = BitConverter.GetBytes(GridObjectDefinitions[i].Unk3);
                    objectStream.Write(objectBytes, 0, 4);
                    objectBytes = BitConverter.GetBytes(GridObjectDefinitions[i].Unk4);
                    objectStream.Write(objectBytes, 0, 4);
                    objectBytes = BitConverter.GetBytes(GridObjectDefinitions[i].Unk5);
                    objectStream.Write(objectBytes, 0, 4);
                    objectBytes = BitConverter.GetBytes(GridObjectDefinitions[i].Unk6);
                    objectStream.Write(objectBytes, 0, 4);
                    objectBytes = BitConverter.GetBytes(GridObjectDefinitions[i].Unk7);
                    objectStream.Write(objectBytes, 0, 4);
                    objectBytes = BitConverter.GetBytes(GridObjectDefinitions[i].Unk8);
                    objectStream.Write(objectBytes, 0, 4);
                    objectBytes = BitConverter.GetBytes(GridObjectDefinitions[i].Unk9);
                    objectStream.Write(objectBytes, 0, 4);
                    objectBytes = BitConverter.GetBytes(GridObjectDefinitions[i].Unk10);
                    objectStream.Write(objectBytes, 0, 4);
                    objectBytes = BitConverter.GetBytes(GridObjectDefinitions[i].Unk11);
                    objectStream.Write(objectBytes, 0, 4);
                    objectBytes = BitConverter.GetBytes(GridObjectDefinitions[i].Unused1);
                    objectStream.Write(objectBytes, 0, 4);
                    objectBytes = BitConverter.GetBytes(GridObjectDefinitions[i].Unk12);
                    objectStream.Write(objectBytes, 0, 4);
                    objectBytes = BitConverter.GetBytes(GridObjectDefinitions[i].Unk13);
                    objectStream.Write(objectBytes, 0, 4);
                    objectBytes = BitConverter.GetBytes(GridObjectDefinitions[i].Unk14);
                    objectStream.Write(objectBytes, 0, 4);
                    objectBytes = BitConverter.GetBytes(GridObjectDefinitions[i].Unk15);
                    objectStream.Write(objectBytes, 0, 4);
                    objectBytes = BitConverter.GetBytes(GridObjectDefinitions[i].Unk16);
                    objectStream.Write(objectBytes, 0, 4);
                    objectBytes = BitConverter.GetBytes(GridObjectDefinitions[i].Unk17);
                    objectStream.Write(objectBytes, 0, 4);
                    objectBytes = BitConverter.GetBytes(GridObjectDefinitions[i].Unk18);
                    objectStream.Write(objectBytes, 0, 4);
                    objectBytes = BitConverter.GetBytes(GridObjectDefinitions[i].Unused2);
                    objectStream.Write(objectBytes, 0, 4);
                    objectBytes = BitConverter.GetBytes(GridObjectDefinitions[i].Unused3);
                    objectStream.Write(objectBytes, 0, 4);
                    objectBytes = BitConverter.GetBytes(GridObjectDefinitions[i].Unk19);
                    objectStream.Write(objectBytes, 0, 4);
                    objectBytes = BitConverter.GetBytes(GridObjectDefinitions[i].Unk20);
                    objectStream.Write(objectBytes, 0, 4);
                    objectBytes = BitConverter.GetBytes(GridObjectDefinitions[i].Unk21);
                    objectStream.Write(objectBytes, 0, 4);
                    objectStream.Write(Encoding.UTF8.GetBytes(GridObjectDefinitions[i].Identifier), 0, 20);
                }
            }

            return true;
        }

        private void LoadTriggers()
        {
            Int32 numtriggers = NativeMethods.GetPrivateProfileInt32("triggerdefs", "numtriggers", TriggerFilename);

            for (Int16 i = 1; i <= numtriggers; i++)
            {
                String keyName = String.Format("trigger{0:00}", i);
                String stype = NativeMethods.GetPrivateProfileString(keyName, "type", TriggerFilename);

                Trigger trigger = new Trigger
                {
                    TriggerId = i,
                    TextureOff = NativeMethods.GetPrivateProfileInt32(keyName, "texture_off", TriggerFilename),
                    TextureOn = NativeMethods.GetPrivateProfileInt32(keyName, "texture_on", TriggerFilename),
                    ResetTimer = NativeMethods.GetPrivateProfileInt32(keyName, "reset_timer", TriggerFilename),
                    Enabled = NativeMethods.GetPrivateProfileBoolean(keyName, "enabled", TriggerFilename),
                    InitialState = (TriggerState)NativeMethods.GetPrivateProfileInt32(keyName, "initial_state", TriggerFilename),
                    NextTrigger = NativeMethods.GetPrivateProfileInt32(keyName, "next_trigger", TriggerFilename),
                    OnSound = NativeMethods.GetPrivateProfileInt32(keyName, "on_sound", TriggerFilename),
                    OffSound = NativeMethods.GetPrivateProfileInt32(keyName, "off_sound", TriggerFilename)
                };

                switch (stype)
                {
                    case "door":
                        {
                            trigger.TriggerType = TriggerType.Door;
                            trigger.SlideAxis = NativeMethods.GetPrivateProfileInt32(keyName, "slide_axis", TriggerFilename);
                            trigger.SlideAmount = NativeMethods.GetPrivateProfileInt32(keyName, "slide_amount", TriggerFilename);
                            trigger.MaxRate = NativeMethods.GetPrivateProfileInt32(keyName, "max_rate", TriggerFilename);
                            trigger.StartAngle = NativeMethods.GetPrivateProfileInt32(keyName, "start_angle", TriggerFilename);
                            trigger.EndAngle = NativeMethods.GetPrivateProfileInt32(keyName, "end_angle", TriggerFilename);
                            break;
                        }

                    case "elevator":
                        {
                            trigger.TriggerType = TriggerType.Elevator;
                            trigger.X1 = NativeMethods.GetPrivateProfileInt32(keyName, "x1", TriggerFilename);
                            trigger.Y1 = NativeMethods.GetPrivateProfileInt32(keyName, "y1", TriggerFilename);
                            trigger.X2 = NativeMethods.GetPrivateProfileInt32(keyName, "x2", TriggerFilename);
                            trigger.Y2 = NativeMethods.GetPrivateProfileInt32(keyName, "y2", TriggerFilename);
                            trigger.OffHeight = NativeMethods.GetPrivateProfileInt32(keyName, "off_height", TriggerFilename);
                            trigger.OnHeight = NativeMethods.GetPrivateProfileInt32(keyName, "on_height", TriggerFilename);
                            trigger.Speed = NativeMethods.GetPrivateProfileInt32(keyName, "speed", TriggerFilename);
                            trigger.MoveCeiling = NativeMethods.GetPrivateProfileInt32(keyName, "move_ceiling", TriggerFilename);
                            trigger.MoveRooftop = NativeMethods.GetPrivateProfileInt32(keyName, "move_rooftop", TriggerFilename);
                            trigger.MoveFloor = NativeMethods.GetPrivateProfileInt32(keyName, "move_floor", TriggerFilename);
                            break;
                        }
                    case "teleport":
                        {
                            trigger.TriggerType = TriggerType.Teleport;
                            trigger.Random = NativeMethods.GetPrivateProfileInt32(keyName, "random", TriggerFilename);
                            trigger.Team = NativeMethods.GetPrivateProfileInt32(keyName, "team", TriggerFilename);
                            trigger.X0 = NativeMethods.GetPrivateProfileInt32(keyName, "x0", TriggerFilename);
                            trigger.Y0 = NativeMethods.GetPrivateProfileInt32(keyName, "y0", TriggerFilename);
                            trigger.X1 = NativeMethods.GetPrivateProfileInt32(keyName, "x1", TriggerFilename);
                            trigger.Y1 = NativeMethods.GetPrivateProfileInt32(keyName, "y1", TriggerFilename);
                            trigger.X2 = NativeMethods.GetPrivateProfileInt32(keyName, "x2", TriggerFilename);
                            trigger.Y2 = NativeMethods.GetPrivateProfileInt32(keyName, "y2", TriggerFilename);
                            trigger.X3 = NativeMethods.GetPrivateProfileInt32(keyName, "x3", TriggerFilename);
                            trigger.Y3 = NativeMethods.GetPrivateProfileInt32(keyName, "y3", TriggerFilename);
                            trigger.X4 = NativeMethods.GetPrivateProfileInt32(keyName, "x4", TriggerFilename);
                            trigger.Y4 = NativeMethods.GetPrivateProfileInt32(keyName, "y4", TriggerFilename);
                            trigger.IsFromValhalla = NativeMethods.GetPrivateProfileBoolean(keyName, "valhalla", TriggerFilename);
                            break;
                        }
                    case "null":
                        {
                            trigger.TriggerType = TriggerType.Lever;
                            trigger.OnText = NativeMethods.GetPrivateProfileString(keyName, "on_text", TriggerFilename);
                            trigger.OffText = NativeMethods.GetPrivateProfileString(keyName, "off_text", TriggerFilename);

                            break;
                        }
                }

                trigger.CurrentState = trigger.InitialState;
                trigger.Position = new Vector3(trigger.X1, trigger.Y1, trigger.OffHeight);
                trigger.Duration = null;

                Triggers.Add(trigger);
            }
        }
        private void LoadThins()
        {
            FileStream thinBuffer = File.OpenRead(MiscFilename);

            for (Int32 i = 1; i <= 250; i++)
            {
                Int32 pos = 0;

                Byte[] thinBytes = new Byte[92];
                thinBuffer.Read(thinBytes, 0, 92);

                Thin thin = new Thin
                                {
                                    ThinId = i,
                                    Unknown0 = BitConverter.ToInt32(thinBytes, 0),
                                    Unknown1 = BitConverter.ToInt32(thinBytes, pos += 4),
                                    Unknown2 = BitConverter.ToInt32(thinBytes, pos += 4),
                                    Unknown3 = BitConverter.ToInt32(thinBytes, pos += 4),
                                    Unknown4 = BitConverter.ToInt32(thinBytes, pos += 4),
                                    X1 = BitConverter.ToInt32(thinBytes, pos += 4),
                                    Y1 = BitConverter.ToInt32(thinBytes, pos += 4),
                                    X2 = BitConverter.ToInt32(thinBytes, pos += 4),
                                    Y2 = BitConverter.ToInt32(thinBytes, pos += 4),
                                    TextureId = BitConverter.ToInt32(thinBytes, pos += 4),
                                    Unknown10 = BitConverter.ToInt32(thinBytes, pos += 4),
                                    Tall = BitConverter.ToInt32(thinBytes, pos += 4),
                                    Unknown12 = BitConverter.ToInt32(thinBytes, pos += 4),
                                    Unknown13 = BitConverter.ToInt32(thinBytes, pos += 4),
                                    Unknown14 = BitConverter.ToInt32(thinBytes, pos += 4),
                                    Unknown15 = BitConverter.ToInt32(thinBytes, pos += 4),
                                    TriggerId = BitConverter.ToInt32(thinBytes, pos += 4),
                                    Unknown17 = BitConverter.ToInt32(thinBytes, pos += 4),
                                    Unknown18 = BitConverter.ToInt32(thinBytes, pos += 4),
                                    Z = BitConverter.ToInt32(thinBytes, pos += 4),
                                    Unknown20 = BitConverter.ToInt32(thinBytes, pos += 4),
                                    BlockPlayers = BitConverter.ToInt32(thinBytes, pos += 4) > 0,
                                    BlockProjectiles = BitConverter.ToInt32(thinBytes, pos) > 0
                                };

                thin.BoundingBox = new OrientedBoundingBox(new Vector3(thin.X1, thin.Y1, thin.Z), new Vector3(thin.X2, thin.Y2, thin.Z), new Vector3(0, 0, thin.Tall));

                Thins.Add(thin);
            }

            thinBuffer.Close();
        }
        private void LoadTiles()
        {
            FileStream tileBuffer = File.OpenRead(MiscFilename);
            tileBuffer.Seek(0x0BF68, SeekOrigin.Begin);

            for (Int32 i = 1; i <= 100; i++)
            {
                Byte[] tileBytes = new Byte[256];
                tileBuffer.Read(tileBytes, 0, 256);

                Tile tile = new Tile(i);

                Int32 pos = 0;

                for (Int32 j = 0; j < 64; j++)
                {
                    tile.TileBlocks.Add(new TileBlock(BitConverter.ToInt16(tileBytes, pos+2), BitConverter.ToInt16(tileBytes, pos), j));
                    pos = pos + 4;
                }

                Tiles.Add(tile);
            }

            tileBuffer.Close();
        }
        private void LoadGrid(Boolean isServer)
        {
            using (FileStream gridBuffer = new FileStream(GridFilename, FileMode.Open, FileAccess.Read))
            {
                for (Int32 i = 1; i <= 16384; i++)
                {
                    Int32 pos = 0;

					Int32 blockX = (Int32)System.Math.Floor((i - 1f) % 128f) * 64;
					Int32 blockY = (Int32)System.Math.Floor((i - 1f) / 128f) * 64;

                    Byte[] gridBytes = new Byte[38];
                    gridBuffer.Read(gridBytes, 0, 38);

                    GridBlock block = new GridBlock(i, blockX, blockY)
                                          {
                                              Unknown0 = BitConverter.ToInt16(gridBytes, 0),
                                              LowBoxTopMod = BitConverter.ToInt16(gridBytes, pos += 2),
                                              LowSidesTextureId = BitConverter.ToInt16(gridBytes, pos += 2),
                                              LowTopTextureId = BitConverter.ToInt16(gridBytes, pos += 2),
                                              HighTextureId = BitConverter.ToInt16(gridBytes, pos += 2),
                                              LowBoxTopZ = BitConverter.ToInt16(gridBytes, pos += 2),
                                              MidBoxBottomZ = BitConverter.ToInt16(gridBytes, pos += 2),
                                              MidBoxTopZ = BitConverter.ToInt16(gridBytes, pos += 2),
                                              LowTileId = BitConverter.ToInt16(gridBytes, pos += 2),
                                              HighBoxBottomZ = BitConverter.ToInt16(gridBytes, pos += 2),
                                              BlockFlags = BitConverter.ToInt16(gridBytes, pos += 2),
                                              LowTopShape = (GridBlockShape) BitConverter.ToInt16(gridBytes, pos += 2),
                                              MidBottomShape = (GridBlockShape) BitConverter.ToInt16(gridBytes, pos += 2),
                                              MidSidesTextureId = BitConverter.ToInt16(gridBytes, pos += 2),
                                              MidTopTextureId = BitConverter.ToInt16(gridBytes, pos += 2),
                                              CeilingTextureId = BitConverter.ToInt16(gridBytes, pos += 2),
                                              Unknown16 = BitConverter.ToInt16(gridBytes, pos += 2),
                                              Unknown17 = BitConverter.ToInt16(gridBytes, pos += 2),
                                              Unknown18 = BitConverter.ToInt16(gridBytes, pos)
                                          };

                    if (isServer)
                    {
                        if (block.LowBoxTopMod != 0)
                        {
                            GridBlocks[i - 1].LowBoxTopZ -= block.LowBoxTopMod;
                        }
                    }

					Int32 locX1 = (Int32)System.Math.Floor((i - 1f) % 128f) * 64;
					Int32 locY1 = (Int32)System.Math.Floor((i - 1f) / 128f) * 64;

                    block.ContainerBox = new OrientedBoundingBox(new Vector3(locX1, locY1, -1024), new Vector3(64, 64, 2048), 0.0f);
                    block.LowBox = new OrientedBoundingBox(new Vector3(locX1, locY1, -1024), new Vector3(64, 64, 1024 + block.LowBoxTopZ), 0.0f);
                    block.MidBox = new OrientedBoundingBox(new Vector3(locX1, locY1, block.MidBoxBottomZ), new Vector3(64, 64, block.MidBoxTopZ - block.MidBoxBottomZ), 0.0f);
                    
                    if (block.HasSkybox)
                    {
                        block.HighBox = new OrientedBoundingBox(new Vector3(locX1, locY1, block.HighBoxBottomZ+17), new Vector3(64, 64, block.MidBoxTopZ - block.HighBoxBottomZ), 0.0f);
                    }
                    else
                    {
                        block.HighBox = new OrientedBoundingBox(new Vector3(locX1, locY1, block.HighBoxBottomZ+17), new Vector3(64, 64, block.MidBoxTopZ), 0.0f);
                    }

                    if (block.LowTileId > 0)
                    {
                        block.LowBoxTile = new Tile(block.LowTileId);

                        foreach (TileBlock tileBlock in Tiles[block.LowTileId].TileBlocks)
                        {
                            block.LowBoxTile.TileBlocks.Add(new TileBlock(tileBlock.TopHeight, tileBlock.BottomHeight, tileBlock.Index));
                        }

                        for (Int32 y = 0; y < 8; y++)
                        {
                            for (Int32 x = 0; x < 8; x++)
                            {
                                TileBlock tileBlock = block.LowBoxTile.TileBlocks[(y*8)+x];

                                Single x1 = 0f, y1 = 0f, z1 = 0f;

                                if (tileBlock.BottomHeight > 0 || tileBlock.TopHeight > 0)
                                {
                                    x1 = block.LowBox.Location.X + (8 * x);
                                    y1 = block.LowBox.Location.Y + (8 * y);
                                    z1 = block.LowBoxTopZ;
                                }

                                if (tileBlock.TopHeight > 0)
                                {
                                    tileBlock.TopBoundingBox = new OrientedBoundingBox(new Vector3(x1, y1, z1 - tileBlock.TopHeight + 128), new Vector3(8f, 8f, tileBlock.TopHeight), 0);
                                }

                                if (tileBlock.BottomHeight > 0)
                                {
                                    tileBlock.BottomBoundingBox = new OrientedBoundingBox(new Vector3(x1, y1, z1), new Vector3(8f, 8f, tileBlock.BottomHeight), 0);
                                }
                            }
                        }

                    }

                    GridBlocks.Add(block);
                }

                gridBuffer.Seek(4, SeekOrigin.Current);

                for (Int32 i = 1; i <= 997; i++)
                {
                    Int32 pos = 0;

                    Byte[] gridBytes = new Byte[16];
                    gridBuffer.Read(gridBytes, 0, 16);

                    GridObject obj = new GridObject
                                         {
                                             ObjectId = BitConverter.ToInt32(gridBytes, 0),
                                             X = BitConverter.ToInt32(gridBytes, pos += 4),
                                             Y = BitConverter.ToInt32(gridBytes, pos += 4),
                                             Z = BitConverter.ToInt32(gridBytes, pos + 4)
                                         };

                    GridObjects.Add(obj);
                }
            }
        }

        private void LoadObjectDefinitions()
        {
            using (FileStream objectBuffer = new FileStream(ObjectsFilename, FileMode.Open, FileAccess.Read))
            {
                for (Int32 i = 1; i <= 139; i++)
                {
                    Int32 pos = 0;

                    Byte[] objectBytes = new Byte[116];
                    objectBuffer.Read(objectBytes, 0, 116);

                    GridObjectDefinition obj = new GridObjectDefinition
                    {
                        DefinitionId = BitConverter.ToInt32(objectBytes, 0),
                        ImageId = BitConverter.ToInt32(objectBytes, pos += 4),
                        Unk3 = BitConverter.ToInt32(objectBytes, pos += 4),
                        Unk4 = BitConverter.ToInt32(objectBytes, pos += 4),
                        Unk5 = BitConverter.ToInt32(objectBytes, pos += 4),
                        Unk6 = BitConverter.ToInt32(objectBytes, pos += 4),
                        Unk7 = BitConverter.ToInt32(objectBytes, pos += 4),
                        Unk8 = BitConverter.ToInt32(objectBytes, pos += 4),
                        Unk9 = BitConverter.ToInt32(objectBytes, pos += 4),
                        Unk10 = BitConverter.ToInt32(objectBytes, pos += 4),
                        Unk11 = BitConverter.ToInt32(objectBytes, pos += 4),
                        Unused1 = BitConverter.ToInt32(objectBytes, pos += 4),
                        Unk12 = BitConverter.ToInt32(objectBytes, pos += 4),
                        Unk13 = BitConverter.ToInt32(objectBytes, pos += 4),
                        Unk14 = BitConverter.ToInt32(objectBytes, pos += 4),
                        Unk15 = BitConverter.ToInt32(objectBytes, pos += 4),
                        Unk16 = BitConverter.ToInt32(objectBytes, pos += 4),
                        Unk17 = BitConverter.ToInt32(objectBytes, pos += 4),
                        Unk18 = BitConverter.ToInt32(objectBytes, pos += 4),
                        Unused2 = BitConverter.ToInt32(objectBytes, pos += 4),
                        Unused3 = BitConverter.ToInt32(objectBytes, pos += 4),
                        Unk19 = BitConverter.ToInt32(objectBytes, pos += 4),
                        Unk20 = BitConverter.ToInt32(objectBytes, pos += 4),
                        Unk21 = BitConverter.ToInt32(objectBytes, pos += 4),
                    };

                    Array.Copy(objectBytes, pos + 4, obj.Identifier, 0, 20);

                    GridObjectDefinitions.Add(obj);
                }
            }
        } 

        public Boolean Collides(OrientedBoundingBox box)
        {
            try
            {
                GridBlockCollection gridBlockCollection = GridBlocks.GetBlocksNearBoundingBox(box);
                if (gridBlockCollection == null) return true;

                if (gridBlockCollection.Any(gridBlock => box.Collides(gridBlock.LowBox) || box.Collides(gridBlock.MidBox) || (box.Collides(gridBlock.HighBox) && !gridBlock.HasSkybox)))
                {
                    return true;
                }

                if (gridBlockCollection.Where(gridBlock => gridBlock.LowBoxTile != null).Any(gridBlock => gridBlock.LowBoxTile.TileBlocks.Where(tileBlock => tileBlock.BottomBoundingBox != null).Any(tileBlock => box.Collides(tileBlock.BottomBoundingBox))))
                {
                    return true;
                }

				if (Triggers.Where(t => t.TriggerType == TriggerType.Elevator).Where(t => (Int32)System.Math.Floor(box.Origin.X / 64) == t.X1 && (Int32)System.Math.Floor(box.Origin.Y / 64) == t.Y1).Any(t => box.Origin.Z > t.OffHeight && box.Origin.Z < t.Position.Z))
                {
                    return true;
                }

            }
            catch (Exception)
            {
                return true;
            }

            return false;
        }

        public Boolean TileCollides(OrientedBoundingBox box)
        {
            GridBlockCollection gridBlockCollection = GridBlocks.GetBlocksNearBoundingBox(box);

            if (gridBlockCollection.Count == 0) return false;

            foreach (GridBlock gridBlock in gridBlockCollection)
            {
                if (gridBlock != null && gridBlock.LowBoxTile != null)
                {
                    foreach (TileBlock tileBlock in gridBlock.LowBoxTile.TileBlocks)
                    {
                        if (tileBlock.TopBoundingBox != null)
                        {
                            if (box.Collides(tileBlock.TopBoundingBox)) return true;
                        }

                        if (tileBlock.BottomBoundingBox != null)
                        {
                            if (box.Collides(tileBlock.BottomBoundingBox)) return true;
                        }
                    }
                }
            }

            return false;
        }


        public Boolean LineToBoxIsBlocked(Vector3 startPoint, OrientedBoundingBox targetBox)
        {
            try
            {
                if (targetBox.Corners.Any(t => GridBlocks.GetBlocksInLine(startPoint, t).Count > 0))
                {
                    if (GridBlocks.GetBlocksInLine(startPoint, targetBox.Origin).Count > 0)
                    {
                        return true;
                    }
                }

                GridBlockCollection gridBlockCollection = GridBlocks.GetBlocksAroundLine(startPoint, targetBox.Origin);

                foreach (GridBlock gridBlock in gridBlockCollection)
                {
                    if (gridBlock.LowBoxTile != null)
                    {
                        foreach (TileBlock tileBlock in gridBlock.LowBoxTile.TileBlocks)
                        {
                            if (tileBlock.TopBoundingBox != null)
                            {
                                if (tileBlock.TopBoundingBox.LineInBox(startPoint, targetBox.Origin))
                                {
                                    return true;
                                }
                            }

                            if (tileBlock.BottomBoundingBox != null)
                            {
                                if (tileBlock.BottomBoundingBox.LineInBox(startPoint, targetBox.Origin))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return true;
            }

            return false;
        }
    }
}
