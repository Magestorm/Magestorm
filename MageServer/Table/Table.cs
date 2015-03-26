using System;
using System.Collections;
using Helper;
using Helper.Timing;

namespace MageServer
{
    public class Table
    {
        public Boolean Delete;
        public readonly Interval Duration;
        public readonly String Founder;
        public readonly String Name;
        public readonly Int16 TableId;
        public readonly TableType Type;
        public readonly ListCollection<Int32> InvitedCharacterIds; 

        public Table(Player player, TableType type)
        {
            lock (TableManager.Tables.SyncRoot)
            {
                if ((TableId = TableManager.Tables.GetAvailableTableId()) == 0) return;

                Type = type;
                Delete = false;
                Founder = player.ActiveCharacter.Name;
                Name = String.Format("{0}'s Table", Founder); 
                Duration = new Interval(600000, false);
                InvitedCharacterIds = new ListCollection<Int32>();

                TableManager.Tables.Add(this);
                Network.SendTo(GamePacket.Outgoing.World.TableCreated(this), Network.SendToType.Tavern);
            }
        }

        public void InvitePlayerToTable(Player player, Byte[] inviteData)
        {
            if (player.ActiveCharacter == null) return;

            if (inviteData == null)
            {
                inviteData = new Byte[64];
                BitArray bitArray = new BitArray(inviteData);
                bitArray[player.PlayerId] = true;
                bitArray.CopyTo(inviteData, 0);
            }

            if (!InvitedCharacterIds.Contains(player.ActiveCharacter.CharacterId))
            {
                InvitedCharacterIds.Add(player.ActiveCharacter.CharacterId);
            }

            Network.Send(player, GamePacket.Outgoing.Player.InviteToTable(this, inviteData));
        }

        public Boolean IsPlayerInvited(Player player)
        {
            return player.ActiveCharacter != null && InvitedCharacterIds.Contains(player.ActiveCharacter.CharacterId);
        }
    }
}
