using System;
using System.Linq;
using System.Threading;
using Helper;

namespace MageServer
{
    public enum TableType
    {
        Public,
        Private,
    }

    public class TableManager : ListCollection<Table>
    {
        public static readonly TableManager Tables = new TableManager();
        private readonly Thread _workerThread;

        public Table FindById(Int32 tableId)
        {
            return this.FirstOrDefault(t => tableId == t.TableId);
        }

        public Int16 GetAvailableTableId()
        {
            for (Int16 i = 50; i <= 70; i++)
            {
                if (FindById(i) == null)
                {
                    return i;
                }
            }
            return 0;
        }

	    private TableManager()
        {
            _workerThread = new Thread(ProcessTables);
            _workerThread.Start();
        } 

        private void ProcessTables()
        {
            while (_workerThread != null)
            {
                lock (Tables.SyncRoot)
                {
                    for (Int32 i = Tables.Count - 1; i >= 0; i--)
                    {
                        Table table = Tables[i];
                        if (table == null) continue;

                        if (table.Duration.HasElapsed || table.Delete)
                        {
                            table.Delete = true;

                            if (PlayerManager.Players.Any(p => p.TableId == table.TableId) || ArenaManager.Arenas.Any(a => a.TableId == table.TableId))
                            {
                                table.Duration.Reset();
                                table.Delete = false;
                            }

                            if (table.Delete)
                            {
                                Network.SendTo(GamePacket.Outgoing.World.TableDeleted(table), Network.SendToType.Tavern);
                                Tables.RemoveAt(i);  
                            }
                        }
                    }
                }

                Thread.Sleep(200);
            }
        }

        public void ProcessSavedInvites(Player player)
        {
            lock (Tables.SyncRoot)
            {
                for (Int32 i = Tables.Count - 1; i >= 0; i--)
                {
                    Table table = Tables[i];
                    if (table == null) continue;

                    if (table.IsPlayerInvited(player)) table.InvitePlayerToTable(player, null);
                }
            }
        }

        public void ClearSavedInvites(Player player)
        {
            lock (Tables.SyncRoot)
            {
                for (Int32 i = Tables.Count - 1; i >= 0; i--)
                {
                    Table table = Tables[i];
                    if (table == null || player.ActiveCharacter == null) continue;

                    table.InvitedCharacterIds.Remove(player.ActiveCharacter.CharacterId);
                }
            }
        }
    }  
}