using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Threading;
using Helper;
using Helper.Math;
using Helper.Network;
using MageServer.Sound;
using SharpDX;
using Color = System.Drawing.Color;
using OrientedBoundingBox = Helper.Math.OrientedBoundingBox;

namespace MageServer
{

    namespace GamePacket
    {
        namespace Incoming
        {
            public static class Arena
            {
                public static void Jump(MageServer.Player player, MemoryStream inStream)
                {
                    if (player.ActiveArena == null || player.ActiveArenaPlayer == null || !player.IsAdmin) return;

                    Byte[] tBuffer = new Byte[2];
                    inStream.Seek(2, SeekOrigin.Begin);
                    inStream.Read(tBuffer, 0, 2);

                    Int16 targetId = NetHelper.FlipBytes(BitConverter.ToInt16(tBuffer, 0));

                    Network.Send(player, Outgoing.Arena.PlayerJump(player.ActiveArenaPlayer, targetId));
                }
                public static void God(MageServer.Player player, MemoryStream inStream)
                {
                    if (player.ActiveArena == null || player.ActiveArenaPlayer == null || !player.IsAdmin) return;

                    inStream.Seek(3, SeekOrigin.Begin);

                    Boolean godStatus = Convert.ToBoolean(inStream.ReadByte());

                    if (godStatus)
                    {
                        player.ActiveArenaPlayer.SpecialFlags |= ArenaPlayer.SpecialFlag.God;
                    }
                    else
                    {
                        player.ActiveArenaPlayer.SpecialFlags &= ~ArenaPlayer.SpecialFlag.God;
                    }

                    Network.Send(player, Outgoing.Arena.PlayerGod(player.ActiveArenaPlayer, godStatus));
                }
                public static void Yank(MageServer.Player player, MemoryStream inStream)
                {
                    if (player.ActiveArena == null || player.ActiveArenaPlayer == null || !player.IsAdmin) return;

                    inStream.Seek(3, SeekOrigin.Begin);
                    Byte targetId = (Byte)inStream.ReadByte();

                    ArenaPlayer targetArenaPlayer = player.ActiveArena.ArenaPlayers.FindById(targetId);

                    if (targetArenaPlayer != null)
                    {
                        player.ActiveArena.PlayerYank(player, targetArenaPlayer, player.ActiveArenaPlayer.ArenaPlayerId, player.ActiveArenaPlayer.Location);
                    }
                }
                public static void PlayerMoveState(MageServer.Player player, MemoryStream inStream)
                {
                    if (player.ActiveArena == null || player.ActiveArenaPlayer == null || player.Flags.HasFlag(PlayerFlag.Hidden)) return;

                    Byte[] tBuffer = new Byte[2];
                    inStream.Seek(2, SeekOrigin.Begin);

                    ArenaPlayer.StatusFlag statusFlags = ((ArenaPlayer.StatusFlag)inStream.ReadByte()) & ~ArenaPlayer.StatusFlag.Hurt;

                    if (player.ActiveArenaPlayer.StatusFlags.HasFlag(ArenaPlayer.StatusFlag.Hurt))
                    {
                        statusFlags |= ArenaPlayer.StatusFlag.Hurt;
                    }

                    Byte mSpeed = (Byte) inStream.ReadByte();

                    inStream.Read(tBuffer, 0, 2);
                    Int16 zPos = (Int16) (NetHelper.FlipBytes(BitConverter.ToInt16(tBuffer, 0)) & 0xFFF);
                    zPos = zPos > 0x7FF ? (Int16) (-(zPos & 0x7FF)) : zPos;

                    inStream.Read(tBuffer, 0, 2);
                    Int16 xPos = NetHelper.FlipBytes(BitConverter.ToInt16(tBuffer, 0));

                    inStream.Read(tBuffer, 0, 2);
                    Int16 yPos = NetHelper.FlipBytes(BitConverter.ToInt16(tBuffer, 0));

                    inStream.Read(tBuffer, 0, 2);
                    Single direction = MathHelper.DirectionToRadians(NetHelper.FlipBytes(BitConverter.ToInt16(tBuffer, 0)));

                    Vector3 location = new Vector3(xPos, yPos, zPos) - ArenaPlayer.PlayerOrigin;

                    Byte[] relayBuffer = new Byte[12];
                    inStream.Seek(2, SeekOrigin.Begin);
                    inStream.Read(relayBuffer, 0, 12);
                    relayBuffer[0] = (Byte)player.ActiveArenaPlayer.StatusFlags;

                    player.ActiveArena.PlayerMove(player.ActiveArenaPlayer, statusFlags, mSpeed, location, direction);

                    Network.SendToArena(player.ActiveArenaPlayer, Outgoing.Arena.PlayerMoveState(player.ActiveArenaPlayer, relayBuffer), false);
                }
                
                public static void PlayerMoveStateShort(MageServer.Player player, MemoryStream inStream)
                {
                    if (player.ActiveArena == null || player.ActiveArenaPlayer == null || player.Flags.HasFlag(PlayerFlag.Hidden)) return;

                    Byte[] tBuffer = new Byte[2];
                    inStream.Seek(2, SeekOrigin.Begin);

                    player.ActiveArenaPlayer.Direction = MathHelper.DirectionToRadians(NetHelper.FlipBytes(BitConverter.ToInt16(tBuffer, 0)));

                    Byte[] relayBuffer = new Byte[8];
                    inStream.Seek(2, SeekOrigin.Begin);
                    inStream.Read(relayBuffer, 0, 8);

                    Network.SendToArena(player.ActiveArenaPlayer, Outgoing.Arena.PlayerMoveStateShort(player.ActiveArenaPlayer, relayBuffer), false);
                }   
                public static void TappedAtShrine(MageServer.Player player)
                {
                    if (player.ActiveArena == null || player.ActiveArenaPlayer == null) return;

                    player.ActiveArena.TappedAtShrine(player.ActiveArenaPlayer);   
                }      
                public static void CalledGhost(MageServer.Player player, MemoryStream inStream)
                {
                    if (player.ActiveArena == null || player.ActiveArenaPlayer == null) return;

                    inStream.Seek(5, SeekOrigin.Begin);
                    Byte targetId = (Byte)inStream.ReadByte();

                    ArenaPlayer targetArenaPlayer = player.ActiveArena.ArenaPlayers.FindById(targetId);

                    if (targetArenaPlayer != null)
                    {
                        player.ActiveArena.CalledGhost(player.ActiveArenaPlayer, targetArenaPlayer); 
                    }
                }
                public static void BiasedPool(MageServer.Player player, MemoryStream inStream)
                {
                    if (player.ActiveArena == null || player.ActiveArenaPlayer == null) return;

                    inStream.Seek(2, SeekOrigin.Begin);

                    Byte poolTeam = (Byte)inStream.ReadByte();

                    player.ActiveArena.BiasedPool(player.ActiveArenaPlayer, poolTeam);
                }
                public static void BiasedShrine(MageServer.Player player, MemoryStream inStream)
                {
                    if (player.ActiveArena == null || player.ActiveArenaPlayer == null) return;

                    inStream.Seek(2, SeekOrigin.Begin);

                    Byte shrineTeam = (Byte)inStream.ReadByte();

                    player.ActiveArena.BiasedShrine(player.ActiveArenaPlayer, shrineTeam);
                }
                public static void CastEffect(MageServer.Player player, MemoryStream inStream)
                {
                    if (player.ActiveArena == null || player.ActiveArenaPlayer == null) return;

                    Byte[] tBuffer = new Byte[2];
                    inStream.Seek(2, SeekOrigin.Begin);

                    inStream.Read(tBuffer, 0, 2);
                    Int16 spellId = NetHelper.FlipBytes(BitConverter.ToInt16(tBuffer, 0));

                    Spell spell = SpellManager.Spells[spellId];
                    if (spell == null) return;

                    if (player.ActiveArena.CastEffect(player.ActiveArenaPlayer, spell))
                    {
                        Network.SendToArena(player.ActiveArenaPlayer, Outgoing.Arena.CastEffect(player.ActiveArenaPlayer, spellId), false);
                    }
                }
                public static void CastTargeted(MageServer.Player player, MemoryStream inStream)
                {
                    if (player.ActiveArena == null || player.ActiveArenaPlayer == null) return;

                    Byte[] tBuffer = new Byte[2];
                    inStream.Seek(2, SeekOrigin.Begin);

                    inStream.Read(tBuffer, 0, 2);
                    Int16 spellId = NetHelper.FlipBytes(BitConverter.ToInt16(tBuffer, 0));

                    inStream.Seek(5, SeekOrigin.Current);
                    Byte targetId = (Byte)inStream.ReadByte();

                    inStream.Seek(8, SeekOrigin.Current);
                    Boolean isResisted = Convert.ToBoolean((Byte)inStream.ReadByte());

                    Byte[] relayBuffer = new Byte[28];
                    inStream.Seek(2, SeekOrigin.Begin);
                    inStream.Read(relayBuffer, 0, 28);

                    Spell spell = SpellManager.Spells[spellId];
                    if (spell == null) return;

                    ArenaPlayer targetArenaPlayer = player.ActiveArena.ArenaPlayers.FindById(targetId);
                    if (targetArenaPlayer == null) return;

                    if (isResisted)
                    {
                        relayBuffer[5] = 0;

                        targetArenaPlayer.IsInCombat = true;
                        player.ActiveArenaPlayer.IsInCombat = true;

                        Network.Send(targetArenaPlayer.WorldPlayer, Outgoing.Arena.CastTargeted(player.ActiveArenaPlayer, relayBuffer));
                    }
                    else
                    {
                        if (player.ActiveArena.CastTargeted(player.ActiveArenaPlayer, targetArenaPlayer, spell))
                        {
                            Network.SendToArena(player.ActiveArenaPlayer, Outgoing.Arena.CastTargeted(player.ActiveArenaPlayer, relayBuffer), false);
                        }
                    }
                }
                public static void CastSign(MageServer.Player player, MemoryStream inStream)
                {
                    if (player.ActiveArena == null || player.ActiveArenaPlayer == null) return;

                    Byte[] tBuffer = new Byte[2];
                    inStream.Seek(2, SeekOrigin.Begin);

                    inStream.Read(tBuffer, 0, 2);
                    Int16 spellId = NetHelper.FlipBytes(BitConverter.ToInt16(tBuffer, 0));

                    inStream.Read(tBuffer, 0, 2);
                    Int16 objectId = NetHelper.FlipBytes(BitConverter.ToInt16(tBuffer, 0));

                    inStream.Read(tBuffer, 0, 2);
                    Int16 xPos = NetHelper.FlipBytes(BitConverter.ToInt16(tBuffer, 0));

                    inStream.Read(tBuffer, 0, 2);
                    Int16 yPos = NetHelper.FlipBytes(BitConverter.ToInt16(tBuffer, 0));

                    inStream.Read(tBuffer, 0, 2);
                    Int16 zPos = NetHelper.FlipBytes(BitConverter.ToInt16(tBuffer, 0));
                    zPos = zPos > 0x7FF ? (Int16) (-((zPos & 0x7FF) ^ 0x7FF)) : zPos;

                    inStream.Read(tBuffer, 0, 2);
                    Single fDirection = MathHelper.DirectionToRadians(NetHelper.FlipBytes(BitConverter.ToInt16(tBuffer, 0)));

                    Byte[] relayBuffer = new Byte[20];
                    inStream.Seek(2, SeekOrigin.Begin);
                    inStream.Read(relayBuffer, 0, 20);

                    Spell spell = SpellManager.Spells[spellId];
                    if (spell == null) return;

                    Sign sign = new Sign(objectId, player.ActiveArenaPlayer, spell, new Vector3(xPos, yPos, zPos), fDirection, relayBuffer);

                    if (player.ActiveArena.CastSign(player.ActiveArenaPlayer, sign))
                    {
                        Network.SendToArena(player.ActiveArenaPlayer, Outgoing.Arena.CastSign(player.ActiveArenaPlayer, relayBuffer), false);
                    }
                }
                public static void CastBolt(MageServer.Player player, MemoryStream inStream)
                {
                    if (player.ActiveArena == null || player.ActiveArenaPlayer == null) return;

                    Byte[] tBuffer = new Byte[2];
                    inStream.Seek(2, SeekOrigin.Begin);

                    inStream.Read(tBuffer, 0, 2);
                    Int16 spellId = BitConverter.ToInt16(tBuffer, 0);

                    inStream.Seek(5, SeekOrigin.Current);
                    Byte targetId = (Byte)inStream.ReadByte();

                    inStream.Read(tBuffer, 0, 2);
                    Int16 distance = NetHelper.FlipBytes(BitConverter.ToInt16(tBuffer, 0));

                    Byte[] relayBuffer = new Byte[34];
                    inStream.Seek(2, SeekOrigin.Begin);
                    inStream.Read(relayBuffer, 0, 34);

                    Spell spell = SpellManager.Spells[spellId];
                    if (spell == null) return;

                    ArenaPlayer targetArenaPlayer = player.ActiveArena.ArenaPlayers.FindById(targetId);
                    Bolt bolt = new Bolt(player.ActiveArenaPlayer, targetArenaPlayer, spell, distance);

                    if (player.ActiveArena.CastBolt(player.ActiveArenaPlayer, bolt))
                    {
                        Network.SendToArena(player.ActiveArenaPlayer, Outgoing.Arena.CastBolt(player.ActiveArenaPlayer, relayBuffer), false);
                    }
                }
                public static void CastProjectile(MageServer.Player player, MemoryStream inStream)
                {
                    if (player.ActiveArena == null || player.ActiveArenaPlayer == null) return;

                    Byte[] tBuffer = new Byte[2];
                    inStream.Seek(2, SeekOrigin.Begin);

                    inStream.Read(tBuffer, 0, 2);
                    Int16 spellId = NetHelper.FlipBytes(BitConverter.ToInt16(tBuffer, 0));

                    inStream.Read(tBuffer, 0, 2);
                    Int16 xPos = NetHelper.FlipBytes(BitConverter.ToInt16(tBuffer, 0));

                    inStream.Read(tBuffer, 0, 2);
                    Int16 yPos = NetHelper.FlipBytes(BitConverter.ToInt16(tBuffer, 0));

                    inStream.Read(tBuffer, 0, 2);
                    Int16 zPos = NetHelper.FlipBytes(BitConverter.ToInt16(tBuffer, 0));
                    zPos = zPos > 0x7FF ? (Int16) (-((zPos & 0x7FF) ^ 0x7FF)) : zPos;

                    inStream.Read(tBuffer, 0, 2);
                    Single fDirection = MathHelper.DirectionToRadians(NetHelper.FlipBytes(BitConverter.ToInt16(tBuffer, 0)));

                    inStream.Seek(2, SeekOrigin.Current);
                    Int32 angle = (Byte)inStream.ReadByte();
                    angle = angle > 0x7F ? (angle & 0x1F) ^ 0x1F : -angle;
                    Single fAngle = angle;

                    Byte[] relayBuffer = new Byte[16];
                    inStream.Seek(2, SeekOrigin.Begin);
                    inStream.Read(relayBuffer, 0, 16);

                    Spell spell = SpellManager.Spells[spellId];
                    if (spell == null) return;

                    ProjectileGroup projectileGroup = new ProjectileGroup(player.ActiveArenaPlayer, spell, new Vector3(xPos, yPos, zPos), fDirection, fAngle);

                    if (player.ActiveArena.CastProjectile(player.ActiveArenaPlayer, spell, projectileGroup))
                    {
                        Network.SendToArena(player.ActiveArenaPlayer, Outgoing.Arena.CastProjectile(player.ActiveArenaPlayer, relayBuffer), false);
                    }
                }
                public static void CastWall(MageServer.Player player, MemoryStream inStream)
                {
                    if (player.ActiveArena == null || player.ActiveArenaPlayer == null) return;

                    Byte[] tBuffer = new Byte[2];
                    inStream.Seek(2, SeekOrigin.Begin);

                    inStream.Read(tBuffer, 0, 2);
                    Int16 spellId = NetHelper.FlipBytes(BitConverter.ToInt16(tBuffer, 0));

                    inStream.Read(tBuffer, 0, 2);
                    Int16 objectId = NetHelper.FlipBytes(BitConverter.ToInt16(tBuffer, 0));

                    inStream.Read(tBuffer, 0, 2);
                    Int16 xPos = NetHelper.FlipBytes(BitConverter.ToInt16(tBuffer, 0));

                    inStream.Read(tBuffer, 0, 2);
                    Int16 yPos = NetHelper.FlipBytes(BitConverter.ToInt16(tBuffer, 0));

                    inStream.Read(tBuffer, 0, 2);
                    Int16 zPos = NetHelper.FlipBytes(BitConverter.ToInt16(tBuffer, 0));
                    zPos = zPos > 0x7FF ? (Int16) (-((zPos & 0x7FF) ^ 0x7FF)) : zPos;

                    inStream.Read(tBuffer, 0, 2);
                    Single fDirection = MathHelper.DirectionToRadians(NetHelper.FlipBytes(BitConverter.ToInt16(tBuffer, 0)));

                    Byte[] relayBuffer = new Byte[18];
                    inStream.Seek(2, SeekOrigin.Begin);
                    inStream.Read(relayBuffer, 0, 18);

                    Spell spell = SpellManager.Spells[spellId];
                    if (spell == null) return;

                    Wall wall = new Wall(objectId, player.ActiveArenaPlayer, spell, new Vector3(xPos, yPos, zPos), fDirection, relayBuffer);

                    if (player.ActiveArena.CastWall(player.ActiveArenaPlayer, wall))
                    {
                        Network.SendToArena(player.ActiveArenaPlayer, Outgoing.Arena.CastWall(relayBuffer), false);
                    }
                }
                public static void CastDispell(MageServer.Player player, MemoryStream inStream)
                {
                    if (player.ActiveArena == null || player.ActiveArenaPlayer == null) return;

                    Byte[] tBuffer = new Byte[2];
                    inStream.Seek(2, SeekOrigin.Begin);

                    inStream.Read(tBuffer, 0, 2);
                    Int16 objectId = NetHelper.FlipBytes(BitConverter.ToInt16(tBuffer, 0));

                    inStream.Read(tBuffer, 0, 2);
                    Int16 spellId = NetHelper.FlipBytes(BitConverter.ToInt16(tBuffer, 0));

                    Spell spell = SpellManager.Spells[spellId];
                    if (spell == null) return;

                    Wall wall = player.ActiveArena.Walls.FindById(objectId);
                    
                    if (wall == null)
                    {
                        Network.Send(player, Outgoing.Arena.ThinDamage(objectId, 1000));
                        return;
                    }

                    player.ActiveArena.CastDispell(player.ActiveArenaPlayer, wall, spell);
                }
                public static void ThinDamage(MageServer.Player player, MemoryStream inStream)
                {
                    if (player.ActiveArena == null || player.ActiveArenaPlayer == null) return;

                    Byte[] tBuffer = new Byte[2];
                    inStream.Seek(4, SeekOrigin.Begin);

                    inStream.Read(tBuffer, 0, 2);
                    Int16 thinId = NetHelper.FlipBytes(BitConverter.ToInt16(tBuffer, 0));

                    inStream.Read(tBuffer, 0, 2);
                    Int16 damage = NetHelper.FlipBytes(BitConverter.ToInt16(tBuffer, 0));

                    player.ActiveArena.ThinDamage(player.ActiveArenaPlayer, thinId, damage);
                }
                public static void ActivatedTrigger(MageServer.Player player, MemoryStream inStream)
                {
                    if (player.ActiveArena == null || player.ActiveArenaPlayer == null) return;

                    inStream.Seek(5, SeekOrigin.Begin);
                    Byte triggerId = (Byte)inStream.ReadByte();

                    Trigger trigger = player.ActiveArena.Grid.Triggers[triggerId];
                    if (trigger == null) return;

                    player.ActiveArena.ActivatedTrigger(player.ActiveArenaPlayer, trigger);
                }
            }   

            public static class Character
            {
                public static void Save(MageServer.Player player, MemoryStream inStream)
                {
                    MageServer.Character.Save(player, new MageServer.Character(inStream));
                }
                public static void Delete(MageServer.Player player, MemoryStream inStream)
                {
                    if (player.TableId != 0) return;

                    Byte[] dbuffer = new Byte[20];
                    inStream.Seek(2, SeekOrigin.Current);
                    inStream.Read(dbuffer, 0, 20);
                    String name = Encoding.ASCII.GetString(dbuffer).Split((Char) 0)[0].Escape();

                    MageServer.Character deleteCharacter = MageServer.Character.LoadByNameAndAccountId(player, name);

                    if (deleteCharacter != null)
                    {
                        Program.ServerForm.MiscLog.WriteMessage(String.Format("[Character Delete] {{{0}}} {1} ({2}), Level: {3}, Class: {4}, EXP: {5}, IP: {6}, Serial: {7}", player.AccountId, player.Username, deleteCharacter.Name, deleteCharacter.Level, deleteCharacter.Class, deleteCharacter.Experience, player.IpAddress, player.Serial), Color.Blue);
                        
						MySQL.CharacterStatistics.OverallDeleteByCharId((deleteCharacter.CharacterId));
	                    MySQL.CharacterStatistics.WeeklyDeleteByCharId((deleteCharacter.CharacterId));
	                    MySQL.Character.Delete(player.AccountId, name);
                    }

                    player.ActiveCharacter = null;
                }
            }

            public static class Login
            {
                public static void Authenticate(MageServer.Player player, MemoryStream inStream)
                {
                    Byte[] version = new Byte[4], loginBuffer = new Byte[20], serialBuffer = new Byte[32];

                    inStream.Seek(3, SeekOrigin.Current);
                    inStream.Read(version, 0, 4);
                    inStream.Read(loginBuffer, 0, 20);
                    String password = Encoding.ASCII.GetString(loginBuffer).Split((Char)0)[0];
                    inStream.Read(serialBuffer, 0, 32);
                    String serial = Encoding.ASCII.GetString(serialBuffer).Split((Char)0)[0].Escape();
                    inStream.Seek(18, SeekOrigin.Current);
                    inStream.Read(loginBuffer, 0, 20);
                    String username = Encoding.ASCII.GetString(loginBuffer).Split((Char)0)[0];

                    Subscription.Authenticate(player, username, password, serial, version);
                }

                public static void Disconnect(MageServer.Player player)
                {
                    player.DisconnectReason = Resources.Strings_Disconnect.Logoff;
                    player.Disconnect = true;
                }
            }

            public static class Player
            {
                public static void Heartbeat(MageServer.Player player, MemoryStream inStream)
                {
                    Byte[] heartbeatBuffer = new Byte[4];

                    inStream.Seek(2, SeekOrigin.Current);
                    inStream.Read(heartbeatBuffer, 0, 4);

                    player.Heartbeat(NetHelper.FlipBytes(BitConverter.ToUInt32(heartbeatBuffer, 0)));
                }
                public static void HasEnteredWorld(MageServer.Player player)
                {
                    Network.Send(player, Outgoing.Player.HasEnteredWorld());
                }
                public static void EnterWorld(MageServer.Player player, MemoryStream inStream)
                {
                    inStream.Seek(4, SeekOrigin.Current);
                    Byte worldId = (Byte)inStream.ReadByte();
                    Team team = (Team) (Byte)inStream.ReadByte();
                    Byte[] nameBuffer = new Byte[12];
                    inStream.Read(nameBuffer, 0, 12);
                    String charName = Encoding.ASCII.GetString(nameBuffer).Split((Char) 0)[0];

                    MageServer.World.PlayerEnteredWorld(player, worldId, team, charName);
                }
                public static void ExitWorld(MageServer.Player player)
                {
                    if (player.ActiveArena == null || player.ActiveArenaPlayer == null) return;

                    MageServer.Arena arena = player.ActiveArena;

                    player.ActiveArena.PlayerLeft(player.ActiveArenaPlayer);

                    Thread.Sleep(2000);

                    for (Int32 i = 0; i < arena.ArenaPlayers.Count; i++)
                    {
                        Network.Send(player, Outgoing.Arena.PlayerState(arena.ArenaPlayers[i]));
                    }
                }
                public static void Chat(MageServer.Player player, MemoryStream inStream)
                {
                    Int32 tLen = Convert.ToInt32(inStream.Length) - 10;
                    Byte[] cBuffer = new Byte[tLen];
                    Byte[] tBuffer = new Byte[2];
                    inStream.Seek(4, SeekOrigin.Begin);
                    inStream.Read(tBuffer, 0, 2);
                    Int16 target = NetHelper.FlipBytes(BitConverter.ToInt16(tBuffer, 0));
                    ChatType targetType = (ChatType) (Byte)inStream.ReadByte();
                    inStream.Seek(3, SeekOrigin.Current);
                    inStream.Read(cBuffer, 0, tLen);

                    String message = Encoding.ASCII.GetString(cBuffer).Split((Char) 0)[0];

                    MageServer.World.ProcessChatMessage(player, target, targetType, message);
                }
                public static void SwitchedToTableOrArena(MageServer.Player player, MemoryStream inStream)
                {
                    inStream.Seek(5, SeekOrigin.Begin);
                    player.TableId = (Byte)inStream.ReadByte();
                }
                public static void InviteToTable(MemoryStream inStream)
                {
                    inStream.Seek(2, SeekOrigin.Begin);
                    Byte tableId = (Byte)inStream.ReadByte();
                    inStream.Seek(3, SeekOrigin.Current);

                    Byte[] inviteData = new Byte[64];
                    inStream.Read(inviteData, 0, 64);
                    BitArray bitArray = new BitArray(inviteData);

                    Table targetTable = TableManager.Tables.FindById(tableId);

                    for (Int16 i = 1; i < bitArray.Count; i++)
                    {
                        if (!bitArray[i]) continue;

                        MageServer.Player targetPlayer = PlayerManager.Players.FindById(i);
                        if (targetPlayer == null || targetTable == null) return;

                        targetTable.InvitePlayerToTable(targetPlayer, inviteData);
                    }
                }
            }

            public static class Study
            {
                public static void RequestCharacterInSlot(MageServer.Player player, MemoryStream inStream)
                {
                    inStream.Seek(22, SeekOrigin.Current);
                    Byte slot = (Byte)inStream.ReadByte();

                    Network.Send(player, Outgoing.Study.SendCharacterInSlot(player, slot, MySQL.Character.FindByAccountIdAndSlot(player.AccountId, slot)));
                }
                public static void IsNameTaken(MageServer.Player player, MemoryStream inStream)
                {
                    Byte[] tBuffer = new Byte[12];
                    inStream.Seek(2, SeekOrigin.Current);
                    inStream.Read(tBuffer, 0, 12);
                    String name = Encoding.ASCII.GetString(tBuffer).Split((Char) 0)[0].Escape();

                    Boolean isTaken = MageServer.Character.IsNameTaken(name);
                    Network.Send(player, Outgoing.Study.IsNameTaken(player, name, isTaken));
                }
                public static void IsNameValid(MageServer.Player player, MemoryStream inStream)
                {
                    Byte[] tBuffer = new Byte[12];
                    inStream.Seek(2, SeekOrigin.Current);
                    inStream.Read(tBuffer, 0, 12);
                    String name = Encoding.ASCII.GetString(tBuffer).Split((Char) 0)[0].Escape();

                    Boolean isValid = MageServer.Character.IsNameValid(name, false);
                    Network.Send(player, Outgoing.Study.IsNameValid(player, name, isValid));
                }
                public static void HighScores(MageServer.Player player, MemoryStream inStream)
                {
                    inStream.Seek(5, SeekOrigin.Begin);
                    Byte pClassId = (Byte)inStream.ReadByte();
					Network.Send(player, Outgoing.Study.HighScores(pClassId, MySQL.Character.GetHighScoreList(pClassId-1)));
                }
            }

            public static class World
            {
                public static void RequestedPlayer(MageServer.Player player, MemoryStream inStream)
                {
                    Byte[] tBuffer = new Byte[2];
                    inStream.Seek(4, SeekOrigin.Begin);

                    inStream.Read(tBuffer, 0, 2);
                    Int16 tPlayerId = NetHelper.FlipBytes(BitConverter.ToInt16(tBuffer, 0));

                    if (player.ActiveArena != null)
                    {
                        ArenaPlayer arenaPlayer = player.ActiveArena.ArenaPlayers.FindById((Byte)tPlayerId);

                        if (arenaPlayer != null)
                        {
                            Network.Send(player, Outgoing.Arena.PlayerJoin(arenaPlayer));
                        }
                    }
                    else
                    {
                        MageServer.Player tPlayer = PlayerManager.Players.FindById(tPlayerId);

                        if (tPlayer != null)
                        {
                            Network.Send(player, Outgoing.World.PlayerJoin(tPlayer));
                        }
                    }
                }
                public static void RequestedAllPlayers(MageServer.Player player)
                {
                    MemoryStream outStream = null;
                    Int32 j = 0;

                    if (player.IsInArena)
                    {
                        lock (player.ActiveArena.ArenaPlayers.SyncRoot)
                        {
                            for (Int32 i = 0; i < player.ActiveArena.ArenaPlayers.Count; i++)
                            {
                                ArenaPlayer arenaPlayer = player.ActiveArena.ArenaPlayers[i];
                                if (arenaPlayer == null || player.ActiveArenaPlayer == arenaPlayer || arenaPlayer.WorldPlayer.Flags.HasFlag(PlayerFlag.Hidden)) continue;

                                outStream = Outgoing.Arena.ArenaPlayerEnterLarge(arenaPlayer, outStream);

                                if (j++ < 10) continue;

                                Network.Send(player, outStream);
                                outStream = null;
                                j = 0;
                            }  
                        } 
                    }
                    else
                    {
                        lock (PlayerManager.Players.SyncRoot)
                        {
                            for (Int32 i = 0; i < PlayerManager.Players.Count; i++)
                            {
                                MageServer.Player p = PlayerManager.Players[i];
                                if (p == null || p == player) continue;

                                if (player.ActiveArena == null)
                                {
                                    if (p.TableId == 0 && p.ActiveArena == null) continue;
                                }
                                else
                                {
                                    if (p.ActiveArena != player.ActiveArena) continue;
                                }

                                if (p.Flags.HasFlag(PlayerFlag.Hidden)) continue;

                                outStream = Outgoing.World.PlayerEnterLarge(p, outStream);

                                if (++j < 10) continue;

                                Network.Send(player, outStream);
                                outStream = null;
                                j = 0;
                            }
                        }
                    }

                    if (j <= 0 || outStream == null) return;

                    for (Int32 x = 10 - j; x > 0; x--)
                    {
                        for (Int32 r = 1; r <= 20; r++)
                        {
                            outStream.WriteByte(0x00);
                        }
                    }

                    Network.Send(player, outStream);
                }
                public static void RequestedArena(MageServer.Player player, MemoryStream inStream)
                {
                    inStream.Seek(5, SeekOrigin.Begin);
                    Byte arenaId = (Byte)inStream.ReadByte();

                    MageServer.Arena arena = ArenaManager.Arenas.FindById(arenaId);

                    if (arena != null)
                    {
                        Network.Send(player, Outgoing.World.ArenaCreated(arena));
                    }
                }
                public static void RequestedAllArenas(MageServer.Player player)
                {
                    MemoryStream outStream = null;
                    Int32 j = 0;

                    lock (ArenaManager.Arenas.SyncRoot)
                    {
                        for (Int32 i = 0; i < ArenaManager.Arenas.Count; i++)
                        {
                            MageServer.Arena a = ArenaManager.Arenas[i];
                            if (a == null) continue;

                            outStream = Outgoing.World.WorldEnterLarge(a, outStream);

                            if (++j < 4) continue;

                            Network.Send(player, outStream);
                            outStream = null;
                            j = 0;
                        }
                    }

                    if (j <= 0 || outStream == null) return;

                    for (Int32 x = 4 - j; x > 0; x--)
                    {
                        for (Int32 r = 1; r <= 52; r++)
                        {
                            outStream.WriteByte(0x00);
                        }
                    }

                    Network.Send(player, outStream);
                }
                public static void RequestedArenaStatus(MageServer.Player player)
                {
                    for (int i = 0; i < ArenaManager.Arenas.Count; i++)
                    {
                        MageServer.Arena arena = ArenaManager.Arenas[i];
                        if (arena == null) continue;

                        Network.Send(player, Outgoing.World.ArenaState(arena, player));
                    }
                }
                public static void CreateTable(MageServer.Player player, MemoryStream inStream)
                {
                    inStream.Seek(2, SeekOrigin.Begin);
                    TableType tableType = (TableType) (Byte)inStream.ReadByte();     

                    new Table(player, tableType);
                }
                public static void DeleteTable(MemoryStream inStream)
                {
                    inStream.Seek(2, SeekOrigin.Begin);

                    Byte tableId = (Byte)inStream.ReadByte();

                    Table t = TableManager.Tables.FindById(tableId);

                    if (t == null) return;

                    t.Delete = true;
                }
                public static void RequestedAllTables(MageServer.Player player)
                {
                    lock (TableManager.Tables.SyncRoot)
                    {
                        for (Int32 i = 0; i < TableManager.Tables.Count; i++)
                        {
                            Table t = TableManager.Tables[i];
                            if (t == null) continue;

                            Network.Send(player, Outgoing.World.TableCreated(t));
                        }
                    }

                    TableManager.Tables.ProcessSavedInvites(player);

					MageServer.World.SendSystemMessage(player, String.Format("Message of the Day: {0}", Properties.Settings.Default.MessageOfTheDay));

                    if (player.ActiveCharacter.AvailableStatPoints > 0)
                    {
                        MageServer.World.SendSystemMessage(player, "You have unspent stat points.  Go to the website to spend them.");
                    }
                    
                    if (player.Flags.HasFlag(PlayerFlag.ChatDisabled))
                    {
                        MageServer.World.SendSystemMessage(player, "Your chat is currently disabled. Type !mute to toggle.");
                    }

                    if (player.Flags.HasFlag(PlayerFlag.ExpLocked))
                    {
                        MageServer.World.SendSystemMessage(player, "Your exp is currently locked. Type !lockexp to toggle.");
                    }

                    if (player.Flags.HasFlag(PlayerFlag.Muted))
                    {
                        MageServer.World.SendSystemMessage(player, "You are currently muted and may not chat in public channels.");
                    }

                    if (player.Flags.HasFlag(PlayerFlag.MusicDisabled))
                    {
                        MageServer.World.SendSystemMessage(player, "You currently have streaming music disabled. Type !togglemusic to toggle.");
                    }
                }
                public static void CreateArena(MageServer.Player player, MemoryStream inStream)
                {
                    inStream.Seek(2, SeekOrigin.Begin);

                    UInt32 gridId = (Byte)inStream.ReadByte();
                    Byte levelRange = (Byte)inStream.ReadByte();

                    Grid grid = GridManager.Grids.FindById(gridId);

                    if (grid != null)
                    {
                        if (player.PreferredArenaMode == ArenaRuleset.ArenaMode.Custom)
                        {
                            new MageServer.Arena(player, grid, levelRange, new ArenaRuleset(player.PreferredArenaRules));
                        }
                        else
                        {
                            new MageServer.Arena(player, grid, levelRange, new ArenaRuleset(player.PreferredArenaMode));
                        }
                    }
                }
                public static void DeleteArena(MageServer.Player player, MemoryStream inStream)
                {
                    inStream.Seek(2, SeekOrigin.Begin);

                    Byte arenaId = (Byte)inStream.ReadByte();
                    MageServer.Arena arena = ArenaManager.Arenas.FindById(arenaId);
                    if (arena == null) return;

                    lock (arena.SyncRoot)
                    {
                        if (arena.ArenaPlayers.Count > 0)
                        {
                            if (player.Admin == AdminLevel.Staff || player.Admin == AdminLevel.Developer)
                            {
                                arena.CurrentState = MageServer.Arena.State.Ended;
                            }
                        }
                        else
                        {
                            arena.CurrentState = MageServer.Arena.State.Ended;
                        }
                    }
                }
            }

            public static class MageHook
            {
                public static void HackNotification(MageServer.Player player, MemoryStream inStream)
                {
                    inStream.Seek(2, SeekOrigin.Begin);
                    Byte hackType = (Byte)inStream.ReadByte();

                    StringBuilder hackString = new StringBuilder();

                    switch (hackType)
                    {
                        case 0:
                        {
                            hackString.Append("[Debugger] ");
                            break;
                        }
                        case 1:
                        {
                            hackString.Append("[Memory Hack] ");
                            break;
                        }
                        default:
                        {
                            hackString.Append("[Unknown Hack] ");
                            break;
                        }
                    }

                    if (player.ActiveCharacter == null)
                    {
                        hackString.Append(String.Format("({0}) {1}", player.AccountId, player.Username));
                    }
                    else
                    {
                        hackString.Append(String.Format("({0}[{1}]) {2}({3})", player.AccountId, player.ActiveCharacter.CharacterId, player.Username, player.ActiveCharacter.Name));
                    }

                    Program.ServerForm.CheatLog.WriteMessage(hackString.ToString(), Color.Red);

					player.DisconnectReason = Resources.Strings_Disconnect.CheatProgram;
                    player.Disconnect = true;
                }
                public static void CheatProgramNotification(MageServer.Player player, MemoryStream inStream)
                {
                    inStream.Seek(2, SeekOrigin.Begin);
                    Byte cheatProgram = (Byte)inStream.ReadByte();
                    Byte cheatType = (Byte)inStream.ReadByte();

                    StringBuilder hackString = new StringBuilder();

                    String programName, cheatTypeName;

                    switch (cheatProgram)
                    {
                        case 0:
                        {
                            programName = "Cheat Engine";
                            break;
                        }
                        case 1:
                        {
                            programName = "Gamehack";
                            break;
                        }
                        case 2:
                        {
                            programName = "GameCheater";
                            break;
                        }
                        case 3:
                        {
                            programName = "TSearch";
                            break;
                        }
                        case 4:
                        {
                            programName = "OllyDBG";
                            break;
                        }
                        case 5:
                        {
                            programName = "WPE Pro";
                            break;
                        }
                        default:
                        {
                            programName = "Unknown";
                            break;
                        }
                    }

                    switch (cheatType)
                    {
                        case 0:
                        {
                            cheatTypeName = "Executable";
                            break;
                        }
                        case 1:
                        {
                            cheatTypeName = "Window";
                            break;
                        }
                        default:
                        {
                            cheatTypeName = "Unknown";
                            break;
                        }
                    }

                    hackString.Append(String.Format("[Cheat Program] "));

                    if (player.ActiveCharacter == null)
                    {
                        hackString.Append(String.Format("({0}) {1} Program: {2}, Type: {3}", player.AccountId, player.Username, programName, cheatTypeName));
                    }
                    else
                    {
                        hackString.Append(String.Format("({0}[{1}]) {2}({3}) Program: {4}, Type: {5}", player.AccountId, player.ActiveCharacter.CharacterId, player.Username, player.ActiveCharacter.Name, programName, cheatTypeName));
                    }

                    Program.ServerForm.CheatLog.WriteMessage(hackString.ToString(), Color.Red);

	                player.DisconnectReason = Resources.Strings_Disconnect.CheatProgram;
                    player.Disconnect = true;
                }
            }
        }

        namespace Outgoing
        {
            public static class Arena
            {
                public static MemoryStream ArenaPlayerEnterLarge(ArenaPlayer arenaPlayer, MemoryStream outStream)
                {
                    if (outStream == null)
                    {
                        outStream = new MemoryStream();
                        outStream.WriteByte(0x00);
                        outStream.WriteByte((Byte)PacketOutFunction.PlayerEnterLarge);
                    }

                    outStream.Write(Encoding.ASCII.GetBytes(arenaPlayer.ActiveCharacter.Name), 0, arenaPlayer.ActiveCharacter.Name.Length);
                    outStream.Seek((12 - arenaPlayer.ActiveCharacter.Name.Length), SeekOrigin.Current);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(arenaPlayer.ArenaPlayerId)), 0, 2);
                    outStream.WriteByte(arenaPlayer.OwnerArena.ArenaId);
                    outStream.WriteByte((Byte) arenaPlayer.ActiveTeam);
                    outStream.WriteByte((Byte) arenaPlayer.ActiveCharacter.Class);
                    outStream.WriteByte(arenaPlayer.ActiveCharacter.Level);
                    outStream.WriteByte(arenaPlayer.ActiveCharacter.OpLevel);
                    outStream.WriteByte(0x00);
                    return outStream;
                }
                public static MemoryStream PlayerJump(ArenaPlayer arenaPlayer, Int16 targetId)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(arenaPlayer.ArenaPlayerId);
                    outStream.WriteByte((Byte)PacketOutFunction.PlayerJump);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(targetId)), 0, 2);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    return outStream;
                }
                public static MemoryStream PlayerYank(ArenaPlayer arenaPlayer, Byte playerId, Vector3 location)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(arenaPlayer.ArenaPlayerId);
                    outStream.WriteByte((Byte)PacketOutFunction.PlayerYank);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(playerId)), 0, 2);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(Convert.ToInt16(location.X))), 0, 2);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(Convert.ToInt16(location.Y))), 0, 2);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(Convert.ToInt16(location.Z))), 0, 2);
                    return outStream;
                }
                public static MemoryStream PlayerGod(ArenaPlayer arenaPlayer, Boolean godStatus)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(arenaPlayer.ArenaPlayerId);
                    outStream.WriteByte((Byte)PacketOutFunction.PlayerGod);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(Convert.ToByte(godStatus))), 0, 2);
                    return outStream;
                }
                public static MemoryStream PlayerJoin(ArenaPlayer arenaPlayer)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(arenaPlayer.ArenaPlayerId);
                    outStream.WriteByte((Byte)PacketOutFunction.PlayerJoin);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(arenaPlayer.ArenaPlayerId)), 0, 2);
                    outStream.WriteByte(arenaPlayer.OwnerArena.ArenaId);
                    outStream.WriteByte((Byte) arenaPlayer.ActiveTeam);
                    outStream.Write(Encoding.ASCII.GetBytes(arenaPlayer.ActiveCharacter.Name), 0, arenaPlayer.ActiveCharacter.Name.Length);
                    outStream.Seek((12 - arenaPlayer.ActiveCharacter.Name.Length), SeekOrigin.Current);
                    outStream.WriteByte((Byte) arenaPlayer.ActiveCharacter.Class);
                    outStream.WriteByte(arenaPlayer.ActiveCharacter.Level);
                    outStream.WriteByte(arenaPlayer.ActiveCharacter.OpLevel);
                    outStream.WriteByte(0x00);
                    return outStream;
                }
                public static MemoryStream PlayerLeave(ArenaPlayer arenaPlayer)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(arenaPlayer.ArenaPlayerId);
                    outStream.WriteByte((Byte)PacketOutFunction.PlayerLeave);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(arenaPlayer.ArenaPlayerId)), 0, 2);
                    return outStream;
                }
                public static MemoryStream SuccessfulArenaEntry()
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.SuccessfulArenaEntry);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    return outStream;
                }
                public static MemoryStream PlayerState(ArenaPlayer arenaPlayer)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(arenaPlayer.ArenaPlayerId);
                    outStream.WriteByte((Byte)PacketOutFunction.PlayerState);

                    outStream.WriteByte(0xFF);
                    outStream.WriteByte(0xFF);

                    outStream.WriteByte((Byte) arenaPlayer.DeathCount);
                    outStream.WriteByte((Byte) arenaPlayer.KillCount);
                    outStream.WriteByte(0xFF);
                    outStream.WriteByte(0xFF);
                    outStream.WriteByte(arenaPlayer.IsAlive ? (Byte) 0 : (Byte) 1);
                    outStream.WriteByte((Byte) arenaPlayer.ActiveCharacter.Class);
                    outStream.WriteByte(arenaPlayer.ActiveCharacter.Level);
                    outStream.WriteByte((Byte) arenaPlayer.RaiseCount);
                    outStream.WriteByte(0xFF);
                    outStream.WriteByte(0xFF);

                    outStream.WriteByte(0xFF);
                    outStream.WriteByte(0xFF);

                    outStream.WriteByte(0xFF);
                    outStream.WriteByte(0xFF);

                    outStream.WriteByte(0xFF);
                    outStream.WriteByte(0xFF);
                    outStream.WriteByte(0xFF);
                    outStream.WriteByte(0xFF);
                    return outStream;
                }
                public static MemoryStream PlayerMoveState(ArenaPlayer arenaPlayer, Byte[] relayBuffer)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(arenaPlayer.ArenaPlayerId);
                    outStream.WriteByte((Byte)PacketOutFunction.PlayerMoveState);
                    outStream.Write(relayBuffer, 0, 12);
                    return outStream;
                }
                public static MemoryStream PlayerMoveStateShort(ArenaPlayer arenaPlayer, Byte[] relayBuffer)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(arenaPlayer.ArenaPlayerId);
                    outStream.WriteByte((Byte)PacketOutFunction.PlayerMoveStateShort);
                    outStream.Write(relayBuffer, 0, 8);
                    return outStream;
                }
                public static MemoryStream CastEffect(ArenaPlayer arenaPlayer, Int16 spellId)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(arenaPlayer.ArenaPlayerId);
                    outStream.WriteByte((Byte)PacketOutFunction.CastEffect);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(spellId)), 0, 2);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    return outStream;
                }
                public static MemoryStream CastTargeted(ArenaPlayer arenaPlayer, Byte[] relayBuffer)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(arenaPlayer.ArenaPlayerId);
                    outStream.WriteByte((Byte)PacketOutFunction.CastTargeted);
                    outStream.Write(relayBuffer, 0, 28);
                    return outStream;
                }
                public static MemoryStream CastTargetedEx(ArenaPlayer targetPlayer, ArenaPlayer sourcePlayer, Spell spell)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(targetPlayer.ArenaPlayerId);
                    outStream.WriteByte((Byte)PacketOutFunction.CastTargeted);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(spell.Id)), 0, 2);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(spell.Range)), 0, 2);

                    if (sourcePlayer == null)
                    {
                        outStream.WriteByte(0);
                        outStream.WriteByte(0);
                    }
                    else
                    {
                        outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(sourcePlayer.ArenaPlayerId)), 0, 2);
                    }
                   
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(targetPlayer.ArenaPlayerId)), 0, 2);
                    outStream.Write(new Byte[20], 0, 20);
                    return outStream;
                }
                public static MemoryStream CastSign(ArenaPlayer arenaPlayer, Byte[] relayBuffer)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(arenaPlayer.ArenaPlayerId);
                    outStream.WriteByte((Byte)PacketOutFunction.CastSign);
                    outStream.Write(relayBuffer, 0, 20);
                    return outStream;
                }
                public static MemoryStream CastSignEx(ArenaPlayer arenaPlayer, Sign sign)
                {
                    Int16 signDirection = (Int16)MathHelper.RadiansToDirection(sign.Direction);

                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(arenaPlayer.ArenaPlayerId);
                    outStream.WriteByte((Byte)PacketOutFunction.CastSign);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(sign.Spell.Id)), 0, 2);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(sign.ObjectId)), 0, 2);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes((Int16)sign.BoundingBox.Origin.X)), 0, 2);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes((Int16)sign.BoundingBox.Origin.Y)), 0, 2);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes((Int16)sign.BoundingBox.Origin.Z)), 0, 2);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(signDirection)), 0, 2);
                    outStream.Write(new Byte[4], 0, 4);
                    outStream.WriteByte((Byte)sign.Team);
                    outStream.Write(new Byte[3], 0, 3);
                    return outStream;
                }
                public static MemoryStream CastBolt(ArenaPlayer arenaPlayer, Byte[] relayBuffer)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(arenaPlayer.ArenaPlayerId);
                    outStream.WriteByte((Byte)PacketOutFunction.CastBolt);
                    outStream.Write(relayBuffer, 0, 34);
                    return outStream;
                }
                public static MemoryStream CastProjectile(ArenaPlayer arenaPlayer, Byte[] relayBuffer)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(arenaPlayer.ArenaPlayerId);
                    outStream.WriteByte((Byte)PacketOutFunction.CastProjectile);
                    outStream.Write(relayBuffer, 0, 16);
                    return outStream;
                }
                public static MemoryStream CastWall(Byte[] relayBuffer)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.CastWall);
                    outStream.Write(relayBuffer, 0, 18);
                    return outStream;
                }
                public static MemoryStream PlayerDamage(ArenaPlayer victimPlayer, ArenaPlayer attackingPlayer, SpellDamage damages)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.UpdateHealth);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(attackingPlayer == null ? (Byte) 0 : attackingPlayer.ArenaPlayerId);
                    outStream.WriteByte(Convert.ToByte(damages.Damage));
                    outStream.WriteByte(Convert.ToByte(damages.Power));
                    //victimPlayer.CurrentHp
                    outStream.Write(BitConverter.GetBytes(victimPlayer.CurrentHp), 0, 2);
                    return outStream;
                }
                public static MemoryStream PlayerHit(ArenaPlayer victimPlayer)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.PlayerHit);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(victimPlayer.ArenaPlayerId);
                    return outStream;
                }
                public static MemoryStream PlayerDeath(ArenaPlayer victimPlayer, ArenaPlayer attackingPlayer)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.PlayerDeath);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(victimPlayer.ArenaPlayerId);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(attackingPlayer == null ? (Byte) 0 : attackingPlayer.ArenaPlayerId);
                    return outStream;
                }
                public static MemoryStream PlayerResurrect(ArenaPlayer arenaPlayer, ArenaPlayer targetPlayer)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.PlayerResurrect);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(targetPlayer.ArenaPlayerId);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(arenaPlayer.ArenaPlayerId);
                    return outStream;
                }
                public static MemoryStream BiasedShrine(ArenaPlayer arenaPlayer, Shrine shrine, Byte biasAmount)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.BiasedShrine);
                    outStream.WriteByte(shrine.ShrineId);
                    outStream.WriteByte((Byte) shrine.Team);
                    outStream.WriteByte((Byte) shrine.CurrentBias);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(biasAmount);
                    outStream.WriteByte((Byte) shrine.Power);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(arenaPlayer.ArenaPlayerId);
                    return outStream;
                }
                public static MemoryStream BiasedPool(ArenaPlayer arenaPlayer, Pool pool, Byte biasAmount)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.BiasedPool);
                    outStream.WriteByte(pool.PoolId);
                    outStream.WriteByte((Byte) pool.Team);
                    outStream.WriteByte((Byte) pool.CurrentBias);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(biasAmount);
                    outStream.WriteByte((Byte) pool.Power);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(arenaPlayer.ArenaPlayerId);
                    return outStream;
                }
                public static MemoryStream UpdateShrinePoolState(MageServer.Arena arena)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.UpdateShrinePoolState);

                    // Chaos Shrine
                    outStream.WriteByte(arena.ArenaTeams.Chaos.Shrine.ShrineId);
                    outStream.WriteByte((Byte) arena.ArenaTeams.Chaos.Shrine.Team);
                    outStream.WriteByte((Byte) arena.ArenaTeams.Chaos.Shrine.CurrentBias);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);

                    if (arena.ArenaTeams.Chaos.Shrine.IsIndestructible)
                    {
                        outStream.WriteByte(0x00);
                    }
                    else
                    {
                        outStream.WriteByte((Byte)arena.ArenaTeams.Chaos.Shrine.Power);
                    }

                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);

                    // Balance Shrine
                    outStream.WriteByte(arena.ArenaTeams.Balance.Shrine.ShrineId);
                    outStream.WriteByte((Byte) arena.ArenaTeams.Balance.Shrine.Team);
                    outStream.WriteByte((Byte) arena.ArenaTeams.Balance.Shrine.CurrentBias);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);

                    if (arena.ArenaTeams.Balance.Shrine.IsIndestructible)
                    {
                        outStream.WriteByte(0x00);
                    }
                    else
                    {
                        outStream.WriteByte((Byte)arena.ArenaTeams.Balance.Shrine.Power);
                    }

                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);

                    // Order Shrine
                    outStream.WriteByte(arena.ArenaTeams.Order.Shrine.ShrineId);
                    outStream.WriteByte((Byte) arena.ArenaTeams.Order.Shrine.Team);
                    outStream.WriteByte((Byte) arena.ArenaTeams.Order.Shrine.CurrentBias);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);

                    if (arena.ArenaTeams.Order.Shrine.IsIndestructible)
                    {
                        outStream.WriteByte(0x00);
                    }
                    else
                    {
                        outStream.WriteByte((Byte)arena.ArenaTeams.Order.Shrine.Power);
                    }

                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);

                    for (Int32 i = 0; i < 20; i++)
                    {
                        if (arena.Grid.Pools[i] != null)
                        {
                            outStream.WriteByte(arena.Grid.Pools[i].PoolId);
                            outStream.WriteByte((Byte) arena.Grid.Pools[i].Team);
                            outStream.WriteByte((Byte) arena.Grid.Pools[i].CurrentBias);
                            outStream.WriteByte(0x00);
                            outStream.WriteByte(0x00);
                            outStream.WriteByte((Byte) arena.Grid.Pools[i].Power);
                            outStream.WriteByte(0x00);
                            outStream.WriteByte(0x00);
                        }
                        else
                        {
                            outStream.WriteByte(0xFF);
                            outStream.WriteByte(0x00);
                            outStream.WriteByte(0x00);
                            outStream.WriteByte(0x00);
                            outStream.WriteByte(0x00);
                            outStream.WriteByte(0x00);
                            outStream.WriteByte(0x00);
                            outStream.WriteByte(0x00);
                        }
                    }
                    return outStream;
                }
                public static MemoryStream UpdateExperience(ArenaPlayer arenaPlayer)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.UpdateExperience);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes((UInt32) arenaPlayer.CombatExp)), 0, 4);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes((UInt32) arenaPlayer.BonusExp)), 0, 4);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes((UInt32) arenaPlayer.ObjectiveExp)), 0, 4);
                    return outStream;
                }
                public static MemoryStream UpdateHealth(ArenaPlayer arenaPlayer)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.UpdateHealth);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.Write(BitConverter.GetBytes(arenaPlayer.CurrentHp), 0, 2);
                    return outStream;
                }
                public static MemoryStream CalledGhost(ArenaPlayer arenaPlayer, ArenaPlayer targetArenaPlayer)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.CalledGhost);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(arenaPlayer.ArenaPlayerId);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(targetArenaPlayer.ArenaPlayerId);
                    outStream.WriteByte(0x02);
                    return outStream;
                }
                public static MemoryStream TappedAtShrine(ArenaPlayer arenaPlayer, Boolean canRes)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.PlayerResurrect);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(arenaPlayer.ArenaPlayerId);
                    outStream.WriteByte(0xFF);
                    outStream.WriteByte(canRes ? (Byte) 0xFE : (Byte) 0xFF);
                    return outStream;
                }
                public static MemoryStream ActivatedTrigger(Trigger trigger)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.ActivatedTrigger);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(Convert.ToByte(trigger.TriggerId));
                    outStream.WriteByte((Byte) trigger.CurrentState);
                    return outStream;
                }
                public static MemoryStream ObjectDeath(ArenaPlayer arenaPlayer, Int16 objectId)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.ObjectDeath);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(objectId)), 0, 2);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(arenaPlayer.ArenaPlayerId);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    return outStream;
                }
                public static MemoryStream ObjectDeath(Int16 objectId)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.ObjectDeath);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(objectId)), 0, 2);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    return outStream;
                }
                public static MemoryStream ThinDamage(Int16 objectId, Int16 damage)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.ThinDamage);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(objectId)), 0, 2);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(damage)), 0, 2);
                    return outStream;
                }     
                public static MemoryStream PlaySound(GameSound.Sound sound, Int16 range, Int16 x, Int16 y)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.PlaySound);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes((Int16)sound)), 0, 2);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(range)), 0, 2);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(x)), 0, 2);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(y)), 0, 2);
                    return outStream;
                }
            }

            public static class Login
            {
                public static MemoryStream Connected(MageServer.Player player)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.LoginConnected);
                    outStream.WriteByte(0x00);
                    outStream.Write(Subscription.GameVersion, 0, 4);
                    outStream.Write(Encoding.ASCII.GetBytes(player.Username), 0, player.Username.Length);
                    outStream.Seek((12 - player.Username.Length), SeekOrigin.Current);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);

                    // Do Encryption? 0 = No, Anything Else = Yes
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);

                    // Unknown
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);

                    // Unknown
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    return outStream;
                }
                public static MemoryStream Error(Subscription.ErrorType loginError)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.LoginError);
                    outStream.WriteByte((Byte)loginError);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    return outStream;
                }
            }

            public static class Player
            {
                public static MemoryStream SendPlayerId(MageServer.Player player)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.SendPlayerId);
                    outStream.Write(BitConverter.GetBytes(player.PlayerId), 0, 2);
                    return outStream;
                }
                public static MemoryStream SendPlayerId(ArenaPlayer arenaPlayer)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.SendPlayerId);
                    outStream.WriteByte(arenaPlayer.ArenaPlayerId);
                    outStream.WriteByte(0x00);
                    return outStream;
                }
                public static MemoryStream HeartbeatReply(MageServer.Player player)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.HeartbeatReply);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(player.LastHeartbeat)), 0, 4);
                    return outStream;
                }
                public static MemoryStream SaveSuccess(MageServer.Player player, Byte slot)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.SaveSuccess);
                    outStream.Write(Encoding.ASCII.GetBytes(player.Username), 0, player.Username.Length);
                    outStream.Seek((20 - player.Username.Length), SeekOrigin.Current);
                    outStream.WriteByte(slot);
                    return outStream;
                }
                public static MemoryStream SwitchedToTable(MageServer.Player player)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.SwitchedToTable);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(player.PlayerId)), 0, 2);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(player.TableId);
                    return outStream;
                }
                public static MemoryStream HasEnteredWorld()
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.HasEnteredWorld);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    return outStream;
                }
                public static MemoryStream Chat(MageServer.Player player, Int16 target, ChatType targetType, String message)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.Chat);

                    if (player == null)
                    {
                        outStream.WriteByte(0x00);
                        outStream.WriteByte(0x00);
                    }
                    else
                    {
                        if (player.ActiveArena != null)
                        {
                            if (player.ActiveArenaPlayer != null)
                            {
                                outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(player.ActiveArenaPlayer.ArenaPlayerId)), 0, 2);
                            }
                            else
                            {
                                outStream.WriteByte(0x00);
                                outStream.WriteByte(0x00);
                            }
                        }
                        else
                        {
                            outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(player.PlayerId)), 0, 2);
                        }  
                    }      

                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(target)), 0, 2);
                    outStream.WriteByte((Byte) targetType);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
                    return outStream;
                }
                public static MemoryStream InviteToTable(Table table, Byte[] inviteData)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.InviteToTable);
                    outStream.WriteByte((Byte) table.TableId);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.Write(inviteData, 0, inviteData.Length);
                    return outStream;
                }
            }

            public static class Study
            {
                public static MemoryStream IsNameValid(MageServer.Player player, String name, Boolean valid)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.IsNameValid);
                    outStream.Write(Encoding.ASCII.GetBytes(name), 0, name.Length);
                    outStream.Seek((30 - name.Length), SeekOrigin.Current);
                    outStream.Write(Encoding.ASCII.GetBytes(player.Username), 0, player.Username.Length);
                    outStream.Seek((20 - player.Username.Length), SeekOrigin.Current);
                    outStream.WriteByte(Convert.ToByte(valid));
                    return outStream;
                }
                public static MemoryStream IsNameTaken(MageServer.Player player, String name, Boolean taken)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.IsNameTaken);
                    outStream.Write(Encoding.ASCII.GetBytes(name), 0, name.Length);
                    outStream.Seek((30 - name.Length), SeekOrigin.Current);
                    outStream.Write(Encoding.ASCII.GetBytes(player.Username), 0, player.Username.Length);
                    outStream.Seek((20 - player.Username.Length), SeekOrigin.Current);
                    outStream.WriteByte(Convert.ToByte(taken));
                    return outStream;
                }
                public static MemoryStream SendCharacterInSlot(MageServer.Player player, Byte slot, DataTable data)
                {
                    MemoryStream outStream = new MemoryStream();

                    outStream.WriteByte(0);
                    outStream.WriteByte((Byte)PacketOutFunction.SendCharacterInSlot);
                    outStream.Write(Encoding.ASCII.GetBytes(player.Username), 0, player.Username.Length);
                    outStream.Seek((20 - player.Username.Length), SeekOrigin.Current);
                    outStream.WriteByte(slot);
                    outStream.WriteByte(0);
                    outStream.WriteByte(0);
                    outStream.WriteByte(0);

                    if (data.Rows.Count > 0)
                    {
                        DataRow charData = data.Rows[0];

                        String name = charData.Field<String>("name");
                        outStream.Write(Encoding.ASCII.GetBytes(name), 0, name.Length);
                        outStream.Seek((20 - name.Length), SeekOrigin.Current);
                        outStream.WriteByte(charData.Field<Byte>("agility"));
                        outStream.WriteByte(charData.Field<Byte>("constitution"));
                        outStream.WriteByte(charData.Field<Byte>("memory"));
                        outStream.WriteByte(charData.Field<Byte>("reasoning"));
                        outStream.WriteByte(charData.Field<Byte>("discipline"));
                        outStream.WriteByte(charData.Field<Byte>("empathy"));
                        outStream.WriteByte(charData.Field<Byte>("intuition"));
                        outStream.WriteByte(charData.Field<Byte>("presence"));
                        outStream.WriteByte(charData.Field<Byte>("quickness"));
                        outStream.WriteByte(charData.Field<Byte>("strength"));
                        outStream.WriteByte(0x00);
                        outStream.WriteByte(0x00);
                        outStream.WriteByte(charData.Field<Byte>("agility"));
                        outStream.WriteByte(charData.Field<Byte>("constitution"));
                        outStream.WriteByte(charData.Field<Byte>("memory"));
                        outStream.WriteByte(charData.Field<Byte>("reasoning"));
                        outStream.WriteByte(charData.Field<Byte>("discipline"));
                        outStream.WriteByte(charData.Field<Byte>("empathy"));
                        outStream.WriteByte(charData.Field<Byte>("intuition"));
                        outStream.WriteByte(charData.Field<Byte>("presence"));
                        outStream.WriteByte(charData.Field<Byte>("quickness"));
                        outStream.WriteByte(charData.Field<Byte>("strength"));
                        outStream.WriteByte(0x00);
                        outStream.WriteByte(0x00);
                        outStream.WriteByte(charData.Field<Byte>("list_1"));
                        outStream.WriteByte(charData.Field<Byte>("list_2"));
                        outStream.WriteByte(charData.Field<Byte>("list_3"));
                        outStream.WriteByte(charData.Field<Byte>("list_4"));
                        outStream.WriteByte(charData.Field<Byte>("list_5"));
                        outStream.WriteByte(charData.Field<Byte>("list_6"));
                        outStream.WriteByte(charData.Field<Byte>("list_7"));
                        outStream.WriteByte(charData.Field<Byte>("list_8"));
                        outStream.WriteByte(charData.Field<Byte>("list_9"));
                        outStream.WriteByte(charData.Field<Byte>("list_10"));
                        outStream.WriteByte(0x00);
                        outStream.WriteByte(0x00);
                        outStream.WriteByte(charData.Field<Byte>("list_level_1"));
                        outStream.WriteByte(charData.Field<Byte>("list_level_2"));
                        outStream.WriteByte(charData.Field<Byte>("list_level_3"));
                        outStream.WriteByte(charData.Field<Byte>("list_level_4"));
                        outStream.WriteByte(charData.Field<Byte>("list_level_5"));
                        outStream.WriteByte(charData.Field<Byte>("list_level_6"));
                        outStream.WriteByte(charData.Field<Byte>("list_level_7"));
                        outStream.WriteByte(charData.Field<Byte>("list_level_8"));
                        outStream.WriteByte(charData.Field<Byte>("list_level_9"));
                        outStream.WriteByte(charData.Field<Byte>("list_level_10"));
                        outStream.WriteByte(0x00);
                        outStream.WriteByte(0x00);
                        outStream.WriteByte(charData.Field<Byte>("class"));
                        outStream.WriteByte(charData.Field<Byte>("level"));
                        outStream.WriteByte(charData.Field<Byte>("spell_picks"));
                        outStream.WriteByte(0x00);
                        outStream.WriteByte(charData.Field<Byte>("model"));
                        outStream.WriteByte(0x00);
                        outStream.WriteByte(0x00);
                        outStream.WriteByte(0x00);
                        outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes((UInt32)(charData.Field<UInt64>("experience")))), 0, 4);

                        byte[] kBuffer = BitConverter.GetBytes(NetHelper.FlipBytes(charData.Field<UInt16>("spell_key_1")));
                        outStream.Write(kBuffer, 0, 2);
                        kBuffer = BitConverter.GetBytes(NetHelper.FlipBytes(charData.Field<UInt16>("spell_key_2")));
                        outStream.Write(kBuffer, 0, 2);
                        kBuffer = BitConverter.GetBytes(NetHelper.FlipBytes(charData.Field<UInt16>("spell_key_3")));
                        outStream.Write(kBuffer, 0, 2);
                        kBuffer = BitConverter.GetBytes(NetHelper.FlipBytes(charData.Field<UInt16>("spell_key_4")));
                        outStream.Write(kBuffer, 0, 2);
                        kBuffer = BitConverter.GetBytes(NetHelper.FlipBytes(charData.Field<UInt16>("spell_key_5")));
                        outStream.Write(kBuffer, 0, 2);
                        kBuffer = BitConverter.GetBytes(NetHelper.FlipBytes(charData.Field<UInt16>("spell_key_6")));
                        outStream.Write(kBuffer, 0, 2);
                        kBuffer = BitConverter.GetBytes(NetHelper.FlipBytes(charData.Field<UInt16>("spell_key_7")));
                        outStream.Write(kBuffer, 0, 2);
                        kBuffer = BitConverter.GetBytes(NetHelper.FlipBytes(charData.Field<UInt16>("spell_key_8")));
                        outStream.Write(kBuffer, 0, 2);
                        kBuffer = BitConverter.GetBytes(NetHelper.FlipBytes(charData.Field<UInt16>("spell_key_9")));
                        outStream.Write(kBuffer, 0, 2);
                        kBuffer = BitConverter.GetBytes(NetHelper.FlipBytes(charData.Field<UInt16>("spell_key_10")));
                        outStream.Write(kBuffer, 0, 2);
                        kBuffer = BitConverter.GetBytes(NetHelper.FlipBytes(charData.Field<UInt16>("spell_key_11")));
                        outStream.Write(kBuffer, 0, 2);
                        kBuffer = BitConverter.GetBytes(NetHelper.FlipBytes(charData.Field<UInt16>("spell_key_12")));
                        outStream.Write(kBuffer, 0, 2);

                        outStream.WriteByte(0x00);
                        outStream.WriteByte(0x00);
                        outStream.WriteByte(0x00);
                        outStream.WriteByte(0x00);
                        outStream.WriteByte(0x00);
                        outStream.WriteByte(0x00);
                        outStream.WriteByte(0x00);
                        outStream.WriteByte(0x00);
                        outStream.WriteByte(0x00);
                        outStream.WriteByte(0x00);
                        outStream.WriteByte(0x00);
                        outStream.WriteByte(0x00);

                        outStream.WriteByte(charData.Field<Byte>("oplevel"));
                        outStream.WriteByte(charData.Field<Byte>("oplevel"));
                        outStream.WriteByte(charData.Field<Byte>("oplevel"));
                        outStream.WriteByte(charData.Field<Byte>("oplevel"));
                    }
                    return outStream;
                }
                public static MemoryStream HighScores(Int32 classId, DataTable dataTable)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.HighScores);

                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(classId)), 0, 4); // List (Class ID + 1)
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(dataTable.Rows.Count)), 0, 4); // Player Count

                    for (Int32 i = 1; i <= dataTable.Rows.Count; i++)
                    {
                        String name = dataTable.Rows[i - 1].Field<String>("name");
                        outStream.Write(Encoding.ASCII.GetBytes(name), 0, name.Length);
                        outStream.Seek((60 - name.Length), SeekOrigin.Current);
                        outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes((Int32) dataTable.Rows[i - 1].Field<Byte>("level"))), 0, 4);
                        outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(dataTable.Rows[i - 1].Field<UInt64>("experience"))), 0, 4);
                    }

                    return outStream;
                }
            }

            public static class System
            {
				public static void PlaySoundToArena(MageServer.Arena arena, GameSound.Sound sound)
                {
                    if (arena == null) return;

                    lock (arena.SyncRoot)
                    {
                        for (Int32 i = 0; i < arena.ArenaPlayers.Count; i++)
                        {
                            Network.Send(arena.ArenaPlayers[i].WorldPlayer, Arena.PlaySound(sound, 16000, 0, 0));
                        }
                    }
                }
                public static MemoryStream PlayWebMusic(String songName)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.PlayWebMusic);
                    outStream.Write(Encoding.ASCII.GetBytes(songName), 0, songName.Length);
                    outStream.WriteByte(0x00);
                    return outStream;
                }
                public static MemoryStream DirectTextMessage(MageServer.Player player, String message)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.Chat);

                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);

                    if (player == null)
                    {
                        outStream.WriteByte(0x00);
                        outStream.WriteByte(0x00);
                    }
                    else
                    {
                        if (player.ActiveArena != null)
                        {
                            if (player.ActiveArenaPlayer != null)
                            {
                                outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(player.ActiveArenaPlayer.ArenaPlayerId)), 0, 2);
                            }
                            else
                            {
                                outStream.WriteByte(0x00);
                                outStream.WriteByte(0xFF);
                            }
                        }
                        else
                        {
                            outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(player.PlayerId)), 0, 2);
                        }
                    }
                    
                    outStream.WriteByte((Byte) ChatType.Whisper);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
                    return outStream;
                }
                public static void DrawBoundingBox(ArenaPlayer arenaPlayer, OrientedBoundingBox boundingBox)
                {
                    const Int16 spellId = 216;

                    if (arenaPlayer == null) return;
                    MemoryStream outStream;

                    for (Int32 i = 0; i < boundingBox.Corners.Length; i++)
                    {
                        outStream = new MemoryStream();
                        outStream.WriteByte(arenaPlayer.ArenaPlayerId);
                        outStream.WriteByte((Byte)PacketOutFunction.CastProjectile);
                        outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(spellId)), 0, 2);
                        outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes((Int16) boundingBox.Corners[i].X)), 0, 2);
                        outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes((Int16) boundingBox.Corners[i].Y)), 0, 2);
                        outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes((Int16) boundingBox.Corners[i].Z)), 0, 2);
                        outStream.WriteByte(0x0C);
                        outStream.WriteByte(0x23);
                        outStream.WriteByte(0x00);
                        outStream.WriteByte(0x00);
                        outStream.WriteByte(0x00);
                        outStream.WriteByte(0x00);
                        outStream.WriteByte(0x00);
                        outStream.WriteByte(0x00);
                        Network.Send(arenaPlayer.WorldPlayer, outStream);
                    }

                    outStream = new MemoryStream();
                    outStream.WriteByte(arenaPlayer.ArenaPlayerId);
                    outStream.WriteByte((Byte)PacketOutFunction.CastProjectile);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(spellId)), 0, 2);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes((Int16)boundingBox.Origin.X)), 0, 2);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes((Int16)boundingBox.Origin.Y)), 0, 2);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes((Int16)boundingBox.Origin.Z)), 0, 2);
                    outStream.WriteByte(0x0C);
                    outStream.WriteByte(0x23);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    Network.Send(arenaPlayer.WorldPlayer, outStream);
                }
            }

            public static class World
            {
                public static MemoryStream PlayerJoin(MageServer.Player player)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte((Byte) player.PlayerId);
                    outStream.WriteByte((Byte)PacketOutFunction.PlayerJoin);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(player.PlayerId)), 0, 2);
                    outStream.WriteByte(player.TableId > 0 ? player.TableId : player.ActiveArena.ArenaId);
                    outStream.WriteByte((Byte) player.ActiveTeam);
                    outStream.Write(Encoding.ASCII.GetBytes(player.ActiveCharacter.Name), 0, player.ActiveCharacter.Name.Length);
                    outStream.Seek((12 - player.ActiveCharacter.Name.Length), SeekOrigin.Current);
                    outStream.WriteByte((Byte) player.ActiveCharacter.Class);
                    outStream.WriteByte(player.ActiveCharacter.Level);
                    outStream.WriteByte(player.ActiveCharacter.OpLevel);
                    outStream.WriteByte(0x00);
                    return outStream;
                }
                public static MemoryStream PlayerLeave(MageServer.Player player)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte((Byte) player.PlayerId);
                    outStream.WriteByte((Byte)PacketOutFunction.PlayerLeave);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(player.PlayerId)), 0, 2);
                    return outStream;
                }
                public static MemoryStream PlayerEnterLarge(MageServer.Player player, MemoryStream outStream)
                {
                    if (outStream == null)
                    {
                        outStream = new MemoryStream();
                        outStream.WriteByte(0x00);
                        outStream.WriteByte((Byte)PacketOutFunction.PlayerEnterLarge);                        
                    }

                    outStream.Write(Encoding.ASCII.GetBytes(player.ActiveCharacter.Name), 0, player.ActiveCharacter.Name.Length);
                    outStream.Seek((12 - player.ActiveCharacter.Name.Length), SeekOrigin.Current);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(player.PlayerId)), 0, 2);
                    outStream.WriteByte(player.ActiveArena != null ? player.ActiveArena.ArenaId : player.TableId);
                    outStream.WriteByte((Byte) player.ActiveTeam);
                    outStream.WriteByte((Byte) player.ActiveCharacter.Class);
                    outStream.WriteByte(player.ActiveCharacter.Level);
                    outStream.WriteByte(player.ActiveCharacter.OpLevel);
                    outStream.WriteByte(0x00);
                    return outStream;
                }
                public static MemoryStream WorldEnterLarge(MageServer.Arena arena, MemoryStream outStream)
                {
                    if (outStream == null)
                    {
                        outStream = new MemoryStream();
                        outStream.WriteByte(0x00);
                        outStream.WriteByte((Byte)PacketOutFunction.WorldEnterLarge);
                    }

                    outStream.WriteByte(arena.ArenaId);
                    outStream.WriteByte(0xFF);
                    outStream.Write(Encoding.ASCII.GetBytes(arena.GameName), 0, arena.GameName.Length);
                    outStream.Seek(20 - arena.GameName.Length, SeekOrigin.Current);
                    outStream.Write(Encoding.ASCII.GetBytes(arena.Grid.Name), 0, arena.Grid.Name.Length);
                    outStream.Seek(10 - arena.Grid.Name.Length, SeekOrigin.Current);
                    outStream.Write(Encoding.ASCII.GetBytes(arena.Founder), 0, arena.Founder.Length);
                    outStream.Seek(10 - arena.Founder.Length, SeekOrigin.Current);
                    outStream.Write(Encoding.ASCII.GetBytes(arena.ShortGameName), 0, arena.ShortGameName.Length);
                    outStream.Seek(10 - arena.ShortGameName.Length, SeekOrigin.Current);
                    return outStream;
                }
                public static MemoryStream TableCreated(Table table)
                {
                    MemoryStream outStream = new MemoryStream();
                    
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.TableCreated);
                    outStream.WriteByte((Byte) table.TableId);
                    outStream.WriteByte((Byte) table.Type);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.Write(Encoding.ASCII.GetBytes(table.Name), 0, table.Name.Length);
                    outStream.Seek((20 - table.Name.Length), SeekOrigin.Current);
                    outStream.Write(Encoding.ASCII.GetBytes(table.Founder), 0, table.Founder.Length);
                    outStream.Seek((10 - table.Founder.Length), SeekOrigin.Current);
                    outStream.WriteByte(0x01);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    return outStream;
                }
                public static MemoryStream TableDeleted(Table table)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.TableDeleted);
                    outStream.WriteByte((Byte) table.TableId);
                    return outStream;
                }
                public static MemoryStream ArenaCreated(MageServer.Arena arena)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.ArenaCreated);
                    outStream.WriteByte(arena.ArenaId);
                    outStream.WriteByte(0x00);
                    outStream.Write(Encoding.ASCII.GetBytes(arena.GameName), 0, arena.GameName.Length);
                    outStream.Seek(20 - arena.GameName.Length, SeekOrigin.Current);
                    outStream.Write(Encoding.ASCII.GetBytes(arena.Grid.Name), 0, arena.Grid.Name.Length);
                    outStream.Seek(10 - arena.Grid.Name.Length, SeekOrigin.Current);
                    outStream.Write(Encoding.ASCII.GetBytes(arena.Founder), 0, arena.Founder.Length);
                    outStream.Seek(10 - arena.Founder.Length, SeekOrigin.Current);
                    outStream.Write(Encoding.ASCII.GetBytes(arena.ShortGameName), 0, arena.ShortGameName.Length);
                    return outStream;
                }
                public static MemoryStream ArenaState(MageServer.Arena arena, MageServer.Player player)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.ArenaState);
                    outStream.WriteByte(arena.ArenaId);
                    outStream.WriteByte(0x01); // Enables the game
                    outStream.WriteByte(arena.MaxPlayers);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte) arena.EndState); // Winner
                    outStream.WriteByte(arena.LevelRange);
                    outStream.WriteByte(arena.TableId);
                    outStream.WriteByte(Convert.ToByte(arena.ArenaPlayers.Count));
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(Convert.ToByte(!arena.ArenaTeams.Chaos.Shrine.IsDisabled)); // Enables Chaos Team
                    outStream.WriteByte(Convert.ToByte(!arena.ArenaTeams.Order.Shrine.IsDisabled)); // Enables Order Team
                    outStream.WriteByte(Convert.ToByte(!arena.ArenaTeams.Balance.Shrine.IsDisabled)); // Enables Balance Team
                    outStream.WriteByte(0x00); // Enables Rogue Team
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(arena.TimeLimit)), 0, 2);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes((Int16)arena.Duration.ElapsedSeconds)), 0, 2);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)arena.CurrentState);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(arena.CountdownTick == null ? (Byte) 0x00 : (Byte) (119 - (arena.CountdownTick.ElapsedSeconds*4)));
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(player.LastArenaId);
                    return outStream;
                }
                public static MemoryStream ArenaForceEndState(MageServer.Arena arena, MageServer.Player player)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.ArenaState);
                    outStream.WriteByte(arena.ArenaId);
                    outStream.WriteByte(0x01);
                    outStream.WriteByte(arena.MaxPlayers);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)MageServer.Arena.State.Ended);
                    outStream.WriteByte(arena.LevelRange);
                    outStream.WriteByte(arena.TableId);
                    outStream.WriteByte(Convert.ToByte(arena.ArenaPlayers.Count));
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(Convert.ToByte(!arena.ArenaTeams.Chaos.Shrine.IsDisabled));
                    outStream.WriteByte(Convert.ToByte(!arena.ArenaTeams.Order.Shrine.IsDisabled));
                    outStream.WriteByte(Convert.ToByte(!arena.ArenaTeams.Balance.Shrine.IsDisabled)); 
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes(arena.TimeLimit)), 0, 2);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.Write(BitConverter.GetBytes(NetHelper.FlipBytes((Int16)arena.Duration.ElapsedSeconds)), 0, 2);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)MageServer.Arena.State.Ended);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(arena.CountdownTick == null ? (Byte)0x00 : (Byte)(119 - (arena.CountdownTick.ElapsedSeconds * 4)));
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(0x00);
                    outStream.WriteByte(player.LastArenaId);
                    return outStream;
                }
                public static MemoryStream ArenaDeleted(MageServer.Arena arena)
                {
                    MemoryStream outStream = new MemoryStream();
                    outStream.WriteByte(0x00);
                    outStream.WriteByte((Byte)PacketOutFunction.ArenaDeleted);
                    outStream.WriteByte(arena.ArenaId);
                    return outStream;
                }
            }
        }
    }
}