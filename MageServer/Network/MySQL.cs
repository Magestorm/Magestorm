using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using Helper.Timing;
using MageServer.Properties;
using MySql.Data.MySqlClient;

namespace MageServer
{
	[SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
	public static class MySQL
    {
	    private static readonly String ConnectionString;

        static MySQL()
        {
            ConnectionString = String.Format(Resources.Strings_MySQL.ConnectionString, Settings.Default.DatabaseName, Settings.Default.DatabaseHost, Settings.Default.DatabasePort, Settings.Default.DatabaseUsername, Settings.Default.DatabasePassword);
        }

		public static class ServerSettings
		{
			public static Boolean SetExpMultiplier(Single expMultiplier)
			{
				try
				{
					using (MySqlConnection sqlConnection = new MySqlConnection(ConnectionString))
					{
						sqlConnection.Open();

						if (sqlConnection.State == ConnectionState.Open)
						{
							MySqlCommand sqlCommand = new MySqlCommand
							{
								Connection = sqlConnection,
								CommandText = Resources.Strings_MySQL.NonQuery_Update_ServerSettings_SetExpMultiplier
							};

							sqlCommand.Parameters.AddWithValue("@exp_multiplier", expMultiplier);
							return sqlCommand.ExecuteNonQuery() >= 1;
						}

						throw new Exception(Resources.Strings_MySQL.Error_Connecting);
					}
				}
				catch (Exception ex)
				{
					Program.ServerForm.MainLog.WriteMessage(ex.Message, Color.Red);
				}

				return false;
			}
		}

		public static class OnlineAccounts
	    {
			public static void SetAllOffline()
			{
				try
				{
					using (MySqlConnection sqlConnection = new MySqlConnection(ConnectionString))
					{
						sqlConnection.Open();

						if (sqlConnection.State == ConnectionState.Open)
						{
							MySqlCommand sqlCommand = new MySqlCommand
							{
								Connection = sqlConnection,
								CommandText = Resources.Strings_MySQL.NonQuery_Delete_OnlineAccounts_SetAllOffline
							};

							sqlCommand.ExecuteNonQuery();
							return;
						}

						throw new Exception(Resources.Strings_MySQL.Error_Connecting);
					}
				}
				catch (Exception ex)
				{
					Program.ServerForm.MainLog.WriteMessage(ex.Message, Color.Red);
				}
			}

			public static Boolean SetOnline(Int32 accountId)
			{
				try
				{
					using (MySqlConnection sqlConnection = new MySqlConnection(ConnectionString))
					{
						sqlConnection.Open();

						if (sqlConnection.State == ConnectionState.Open)
						{
							MySqlCommand sqlCommand = new MySqlCommand
							{
								Connection = sqlConnection,
								CommandText = Resources.Strings_MySQL.NonQuery_Insert_OnlineAccounts_SetOnline
							};

							sqlCommand.Parameters.AddWithValue("@accountid", accountId);
							return sqlCommand.ExecuteNonQuery() >= 1;
						}

						throw new Exception(Resources.Strings_MySQL.Error_Connecting);
					}
				}
				catch (Exception ex)
				{
					Program.ServerForm.MainLog.WriteMessage(ex.Message, Color.Red);
				}

				return false;
			}

			public static Boolean SetOffline(Int32 accountId)
			{
				try
				{
					using (MySqlConnection sqlConnection = new MySqlConnection(ConnectionString))
					{
						sqlConnection.Open();

						if (sqlConnection.State == ConnectionState.Open)
						{
							MySqlCommand sqlCommand = new MySqlCommand
							{
								Connection = sqlConnection,
								CommandText = Resources.Strings_MySQL.NonQuery_Delete_OnlineAccounts_SetOffline
							};

							sqlCommand.Parameters.AddWithValue("@accountid", accountId);
							return sqlCommand.ExecuteNonQuery() >= 1;
						}

						throw new Exception(Resources.Strings_MySQL.Error_Connecting);
					}
				}
				catch (Exception ex)
				{
					Program.ServerForm.MainLog.WriteMessage(ex.Message, Color.Red);
				}

				return false;
			}
	    }

		public static class BannedSerials
		{
			public static Boolean IsBanned(String serial)
			{
				try
				{
					using (MySqlConnection sqlConnection = new MySqlConnection(ConnectionString))
					{
						sqlConnection.Open();

						if (sqlConnection.State == ConnectionState.Open)
						{
							MySqlCommand sqlCommand = new MySqlCommand
							{
								Connection = sqlConnection,
								CommandText = Resources.Strings_MySQL.Query_Select_Account_IsSerialBanned
							};

							sqlCommand.Parameters.AddWithValue("@serial", serial);
							return sqlCommand.ExecuteScalar() != null;
						}

						throw new Exception(Resources.Strings_MySQL.Error_Connecting);
					}
				}
				catch (Exception ex)
				{
					Program.ServerForm.MainLog.WriteMessage(ex.Message, Color.Red);
				}

				return false;
			}
		}

	    public static class OnlineCharacters
	    {
			public static void SetAllOffline()
			{
				try
				{
					using (MySqlConnection sqlConnection = new MySqlConnection(ConnectionString))
					{
						sqlConnection.Open();

						if (sqlConnection.State == ConnectionState.Open)
						{
							MySqlCommand sqlCommand = new MySqlCommand
							{
								Connection = sqlConnection,
								CommandText = Resources.Strings_MySQL.NonQuery_Delete_OnlineCharacters_SetAllOffline
							};

							sqlCommand.ExecuteNonQuery();
							return;
						}

						throw new Exception(Resources.Strings_MySQL.Error_Connecting);
					}
				}
				catch (Exception ex)
				{
					Program.ServerForm.MainLog.WriteMessage(ex.Message, Color.Red);
				}
			}

			public static Boolean SetOnline(Int32 characterId, Byte tableId, Byte arenaId, String shortGameName)
			{
				try
				{
					using (MySqlConnection sqlConnection = new MySqlConnection(ConnectionString))
					{
						sqlConnection.Open();

						if (sqlConnection.State == ConnectionState.Open)
						{
							MySqlCommand sqlCommand = new MySqlCommand
							{
								Connection = sqlConnection,
								CommandText = Resources.Strings_MySQL.NonQuery_InsertUpdate_OnlineCharacters_SetOnline
							};

							sqlCommand.Parameters.AddWithValue("@charid", characterId);
							sqlCommand.Parameters.AddWithValue("@arenaid", arenaId);
							sqlCommand.Parameters.AddWithValue("@tableid", tableId);
							sqlCommand.Parameters.AddWithValue("@arenashortname", shortGameName);

							return sqlCommand.ExecuteNonQuery() >= 1;
						}

						throw new Exception(Resources.Strings_MySQL.Error_Connecting);
					}
				}
				catch (Exception ex)
				{
					Program.ServerForm.MainLog.WriteMessage(ex.Message, Color.Red);
				}

				return false;
			}

			public static Boolean SetOffline(Int32 characterId)
			{
				try
				{
					using (MySqlConnection sqlConnection = new MySqlConnection(ConnectionString))
					{
						sqlConnection.Open();

						if (sqlConnection.State == ConnectionState.Open)
						{
							MySqlCommand sqlCommand = new MySqlCommand
							{
								Connection = sqlConnection,
								CommandText = Resources.Strings_MySQL.NonQuery_Delete_Character_SetOffline
							};

							sqlCommand.Parameters.AddWithValue("@charid", characterId);

							return sqlCommand.ExecuteNonQuery() >= 1;
						}

						throw new Exception(Resources.Strings_MySQL.Error_Connecting);
					}
				}
				catch (Exception ex)
				{
					Program.ServerForm.MainLog.WriteMessage(ex.Message, Color.Red);
				}

				return false;
			}
	    }

	    public static class Character
	    {
			public static DataTable GetHighScoreList(Int32 playerClass)
			{
				try
				{
					using (MySqlConnection sqlConnection = new MySqlConnection(ConnectionString))
					{
						sqlConnection.Open();

						if (sqlConnection.State == ConnectionState.Open)
						{
							MySqlCommand sqlCommand = new MySqlCommand
							{
								Connection = sqlConnection,
								CommandText = Resources.Strings_MySQL.Query_Select_Study_GetHighScoreList
							};

							sqlCommand.Parameters.AddWithValue("@class", (Byte)playerClass);

							MySqlDataAdapter sqlAdapter = new MySqlDataAdapter();
							DataTable result = new DataTable();

							sqlAdapter.SelectCommand = sqlCommand;
							sqlAdapter.Fill(result);

							return result;
						}

						throw new Exception(Resources.Strings_MySQL.Error_Connecting);
					}
				}
				catch (Exception ex)
				{
					Program.ServerForm.MainLog.WriteMessage(ex.Message, Color.Red);
				}

				return null;
			}

			public static Boolean Save(MageServer.Character character, Boolean isNew, PlayerFlag flags)
			{
				try
				{
					if (character == null) return false;

					using (MySqlConnection sqlConnection = new MySqlConnection(ConnectionString))
					{
						sqlConnection.Open();

						if (sqlConnection.State == ConnectionState.Open)
						{
							MySqlCommand sqlCommand = new MySqlCommand
							{
								Connection = sqlConnection,
								CommandText = isNew ? Resources.Strings_MySQL.NonQuery_Insert_Character_SaveNew : Resources.Strings_MySQL.NonQuery_Update_Character_SaveExisting
							};

							sqlCommand.Parameters.AddWithValue("@accountid", character.AccountId);
							sqlCommand.Parameters.AddWithValue("@slot", character.Slot);
							sqlCommand.Parameters.AddWithValue("@name", character.Name);
							sqlCommand.Parameters.AddWithValue("@agility", character.Agility);
							sqlCommand.Parameters.AddWithValue("@constitution", character.Constitution);
							sqlCommand.Parameters.AddWithValue("@memory", character.Memory);
							sqlCommand.Parameters.AddWithValue("@reasoning", character.Reasoning);
							sqlCommand.Parameters.AddWithValue("@discipline", character.Discipline);
							sqlCommand.Parameters.AddWithValue("@empathy", character.Empathy);
							sqlCommand.Parameters.AddWithValue("@intuition", character.Intuition);
							sqlCommand.Parameters.AddWithValue("@presence", character.Presence);
							sqlCommand.Parameters.AddWithValue("@quickness", character.Quickness);
							sqlCommand.Parameters.AddWithValue("@strength", character.Strength);
							sqlCommand.Parameters.AddWithValue("@spent_stat", character.SpentStatPoints);
							sqlCommand.Parameters.AddWithValue("@bonus_stat", character.BonusStatPoints);
							sqlCommand.Parameters.AddWithValue("@bonus_spent", character.BonusStatPointsSpent);
							sqlCommand.Parameters.AddWithValue("@list_1", character.List1);
							sqlCommand.Parameters.AddWithValue("@list_2", character.List2);
							sqlCommand.Parameters.AddWithValue("@list_3", character.List3);
							sqlCommand.Parameters.AddWithValue("@list_4", character.List4);
							sqlCommand.Parameters.AddWithValue("@list_5", character.List5);
							sqlCommand.Parameters.AddWithValue("@list_6", character.List6);
							sqlCommand.Parameters.AddWithValue("@list_7", character.List7);
							sqlCommand.Parameters.AddWithValue("@list_8", character.List8);
							sqlCommand.Parameters.AddWithValue("@list_9", character.List9);
							sqlCommand.Parameters.AddWithValue("@list_10", character.List10);
							sqlCommand.Parameters.AddWithValue("@list_level_1", character.ListLevel1);
							sqlCommand.Parameters.AddWithValue("@list_level_2", character.ListLevel2);
							sqlCommand.Parameters.AddWithValue("@list_level_3", character.ListLevel3);
							sqlCommand.Parameters.AddWithValue("@list_level_4", character.ListLevel4);
							sqlCommand.Parameters.AddWithValue("@list_level_5", character.ListLevel5);
							sqlCommand.Parameters.AddWithValue("@list_level_6", character.ListLevel6);
							sqlCommand.Parameters.AddWithValue("@list_level_7", character.ListLevel7);
							sqlCommand.Parameters.AddWithValue("@list_level_8", character.ListLevel8);
							sqlCommand.Parameters.AddWithValue("@list_level_9", character.ListLevel9);
							sqlCommand.Parameters.AddWithValue("@list_level_10", character.ListLevel10);
							sqlCommand.Parameters.AddWithValue("@experience", character.Experience);
							sqlCommand.Parameters.AddWithValue("@class", (Byte)character.Class);
							sqlCommand.Parameters.AddWithValue("@level", character.Level);
							sqlCommand.Parameters.AddWithValue("@spell_picks", character.SpellPicks);
							sqlCommand.Parameters.AddWithValue("@model", character.Model);
							sqlCommand.Parameters.AddWithValue("@spell_key_1", character.SpellKey1);
							sqlCommand.Parameters.AddWithValue("@spell_key_2", character.SpellKey2);
							sqlCommand.Parameters.AddWithValue("@spell_key_3", character.SpellKey3);
							sqlCommand.Parameters.AddWithValue("@spell_key_4", character.SpellKey4);
							sqlCommand.Parameters.AddWithValue("@spell_key_5", character.SpellKey5);
							sqlCommand.Parameters.AddWithValue("@spell_key_6", character.SpellKey6);
							sqlCommand.Parameters.AddWithValue("@spell_key_7", character.SpellKey7);
							sqlCommand.Parameters.AddWithValue("@spell_key_8", character.SpellKey8);
							sqlCommand.Parameters.AddWithValue("@spell_key_9", character.SpellKey9);
							sqlCommand.Parameters.AddWithValue("@spell_key_10", character.SpellKey10);
							sqlCommand.Parameters.AddWithValue("@spell_key_11", character.SpellKey11);
							sqlCommand.Parameters.AddWithValue("@spell_key_12", character.SpellKey12);
							sqlCommand.Parameters.AddWithValue("@oplevel", character.OpLevel);
							sqlCommand.Parameters.AddWithValue("@flags", flags);

							return sqlCommand.ExecuteNonQuery() >= 1;
						}

						throw new Exception(Resources.Strings_MySQL.Error_Connecting);
					}
				}
				catch (Exception ex)
				{
					Program.ServerForm.MainLog.WriteMessage(ex.Message, Color.Red);
				}

				return false;
			}

			public static DataTable FindByName(String name)
			{
				try
				{
					using (MySqlConnection sqlConnection = new MySqlConnection(ConnectionString))
					{
						sqlConnection.Open();

						if (sqlConnection.State == ConnectionState.Open)
						{
							MySqlCommand sqlCommand = new MySqlCommand
							{
								Connection = sqlConnection,
								CommandText = Resources.Strings_MySQL.Query_Select_Character_FindByName
							};

							sqlCommand.Parameters.AddWithValue("@name", name);

							MySqlDataAdapter sqlAdapter = new MySqlDataAdapter();
							DataTable result = new DataTable();

							sqlAdapter.SelectCommand = sqlCommand;
							sqlAdapter.Fill(result);

							return result;
						}

						throw new Exception(Resources.Strings_MySQL.Error_Connecting);
					}
				}
				catch (Exception ex)
				{
					Program.ServerForm.MainLog.WriteMessage(ex.Message, Color.Red);
				}

				return null;
			}

			public static DataTable FindByNameAndAccountId(String name, Int32 accountId)
			{
				try
				{
					using (MySqlConnection sqlConnection = new MySqlConnection(ConnectionString))
					{
						sqlConnection.Open();

						if (sqlConnection.State == ConnectionState.Open)
						{
							MySqlCommand sqlCommand = new MySqlCommand
							{
								Connection = sqlConnection,
								CommandText = Resources.Strings_MySQL.Query_Select_Character_FindByNameAndAccountId
							};

							sqlCommand.Parameters.AddWithValue("@name", name);
							sqlCommand.Parameters.AddWithValue("@accountid", accountId);

							MySqlDataAdapter sqlAdapter = new MySqlDataAdapter();
							DataTable result = new DataTable();

							sqlAdapter.SelectCommand = sqlCommand;
							sqlAdapter.Fill(result);

							return result;
						}

						throw new Exception(Resources.Strings_MySQL.Error_Connecting);
					}
				}
				catch (Exception ex)
				{
					Program.ServerForm.MainLog.WriteMessage(ex.Message, Color.Red);
				}

				return null;
			}

			public static DataTable FindByAccountIdAndSlot(Int32 accountId, Byte slot)
			{
				try
				{
					using (MySqlConnection sqlConnection = new MySqlConnection(ConnectionString))
					{
						sqlConnection.Open();

						if (sqlConnection.State == ConnectionState.Open)
						{
							MySqlCommand sqlCommand = new MySqlCommand
							{
								Connection = sqlConnection,
								CommandText = Resources.Strings_MySQL.Query_Select_Character_FindByAccountIdAndSlot
							};

							sqlCommand.Parameters.AddWithValue("@accountid", accountId);
							sqlCommand.Parameters.AddWithValue("@slot", slot);

							MySqlDataAdapter sqlAdapter = new MySqlDataAdapter();
							DataTable result = new DataTable();

							sqlAdapter.SelectCommand = sqlCommand;
							sqlAdapter.Fill(result);

							return result;
						}

						throw new Exception(Resources.Strings_MySQL.Error_Connecting);
					}
				}
				catch (Exception ex)
				{
					Program.ServerForm.MainLog.WriteMessage(ex.Message, Color.Red);
				}

				return null;
			}

			public static Boolean Delete(Int32 accountId, String name)
			{
				try
				{
					using (MySqlConnection sqlConnection = new MySqlConnection(ConnectionString))
					{
						sqlConnection.Open();

						if (sqlConnection.State == ConnectionState.Open)
						{
							MySqlCommand sqlCommand = new MySqlCommand
							{
								Connection = sqlConnection,
								CommandText = Resources.Strings_MySQL.NonQuery_Delete_Character_Delete
							};

							sqlCommand.Parameters.AddWithValue("@accountid", accountId);
							sqlCommand.Parameters.AddWithValue("@name", name);

							return sqlCommand.ExecuteNonQuery() >= 1;
						}

						throw new Exception(Resources.Strings_MySQL.Error_Connecting);
					}
				}
				catch (Exception ex)
				{
					Program.ServerForm.MainLog.WriteMessage(ex.Message, Color.Red);
				}

				return false;
			}
	    }

	    public static class Matches
	    {
			public static Int64 Created(Int32 arenaId, Int32 tableId, Int32 creationTime, Int32 playerCount, Int32 highestPlayerCount, Int32 maxPlayers, Arena.State currentState, Arena.State endState, String shortName, String longName, Int32 founderCharId, Int32 duration, Int32 levelRange, ArenaRuleset.ArenaMode mode, ArenaRuleset.ArenaRule rules)
			{
				try
				{
					using (MySqlConnection sqlConnection = new MySqlConnection(ConnectionString))
					{
						sqlConnection.Open();

						if (sqlConnection.State == ConnectionState.Open)
						{
							MySqlCommand sqlCommand = new MySqlCommand
							{
								Connection = sqlConnection,
								CommandText = Resources.Strings_MySQL.NonQuery_Insert_Matches_New
							};

							sqlCommand.Parameters.AddWithValue("@arenaid", arenaId);
							sqlCommand.Parameters.AddWithValue("@tableid", tableId);
							sqlCommand.Parameters.AddWithValue("@creation_time", creationTime);
							sqlCommand.Parameters.AddWithValue("@player_count", playerCount);
							sqlCommand.Parameters.AddWithValue("@highest_player_count", highestPlayerCount);
							sqlCommand.Parameters.AddWithValue("@max_players", maxPlayers);
							sqlCommand.Parameters.AddWithValue("@current_state", (Int32)currentState);
							sqlCommand.Parameters.AddWithValue("@end_state", (Int32)endState);
							sqlCommand.Parameters.AddWithValue("@short_name", shortName);
							sqlCommand.Parameters.AddWithValue("@long_name", longName);
							sqlCommand.Parameters.AddWithValue("@founder_charid", founderCharId);
							sqlCommand.Parameters.AddWithValue("@duration", duration);
							sqlCommand.Parameters.AddWithValue("@level_range", levelRange);
							sqlCommand.Parameters.AddWithValue("@mode", (Int32)mode);
							sqlCommand.Parameters.AddWithValue("@rules", (Int32)rules);

							Int64 sqlResult = sqlCommand.ExecuteNonQuery();

							if (sqlResult >= 1) return sqlCommand.LastInsertedId;
							return 0;
						}

						throw new Exception(Resources.Strings_MySQL.Error_Connecting);
					}
				}
				catch (Exception ex)
				{
					Program.ServerForm.MainLog.WriteMessage(ex.Message, Color.Red);
				}

				return 0;
			}
	    }

	    public static class CharacterStatistics
	    {
			public static Boolean OverallUpdate(Statistics.StatisticSheet statisticSheet)
			{
				try
				{
					using (MySqlConnection sqlConnection = new MySqlConnection(ConnectionString))
					{
						sqlConnection.Open();

						if (sqlConnection.State == ConnectionState.Open)
						{
							MySqlCommand sqlCommand = new MySqlCommand
							{
								Connection = sqlConnection,
								CommandText = Resources.Strings_MySQL.NonQuery_InsertUpdate_StatisticsOverall_Update
							};

							sqlCommand.Parameters.AddWithValue("@charid", statisticSheet.CharacterId);
							sqlCommand.Parameters.AddWithValue("@hidden", statisticSheet.Hidden);
							sqlCommand.Parameters.AddWithValue("@kills", statisticSheet.Kills);
							sqlCommand.Parameters.AddWithValue("@deaths", statisticSheet.Deaths);
							sqlCommand.Parameters.AddWithValue("@raises", statisticSheet.Raises);
							sqlCommand.Parameters.AddWithValue("@damagedone", statisticSheet.DamageDone);
							sqlCommand.Parameters.AddWithValue("@damagetaken", statisticSheet.DamageTaken);
							sqlCommand.Parameters.AddWithValue("@healingdone", statisticSheet.HealingDone);
							sqlCommand.Parameters.AddWithValue("@healingtaken", statisticSheet.HealingTaken);
							sqlCommand.Parameters.AddWithValue("@wins", statisticSheet.Wins);
							sqlCommand.Parameters.AddWithValue("@losses", statisticSheet.Losses);

							return sqlCommand.ExecuteNonQuery() >= 1;
						}

						throw new Exception(Resources.Strings_MySQL.Error_Connecting);
					}
				}
				catch (Exception ex)
				{
					Program.ServerForm.MainLog.WriteMessage(ex.Message, Color.Red);
				}

				return false;
			}

			public static Boolean WeeklyUpdate(Statistics.StatisticSheet statisticSheet)
			{
				try
				{
					using (MySqlConnection sqlConnection = new MySqlConnection(ConnectionString))
					{
						sqlConnection.Open();

						if (sqlConnection.State == ConnectionState.Open)
						{
							MySqlCommand sqlCommand = new MySqlCommand
							{
								Connection = sqlConnection,
								CommandText = Resources.Strings_MySQL.NonQuery_InsertUpdate_StatisticsWeekly_Update
							};

							sqlCommand.Parameters.AddWithValue("@charid", statisticSheet.CharacterId);
							sqlCommand.Parameters.AddWithValue("@date", TimeHelper.GetStartOfWeekUnixTime());
							sqlCommand.Parameters.AddWithValue("@hidden", statisticSheet.Hidden);
							sqlCommand.Parameters.AddWithValue("@kills", statisticSheet.Kills);
							sqlCommand.Parameters.AddWithValue("@deaths", statisticSheet.Deaths);
							sqlCommand.Parameters.AddWithValue("@raises", statisticSheet.Raises);
							sqlCommand.Parameters.AddWithValue("@damagedone", statisticSheet.DamageDone);
							sqlCommand.Parameters.AddWithValue("@damagetaken", statisticSheet.DamageTaken);
							sqlCommand.Parameters.AddWithValue("@healingdone", statisticSheet.HealingDone);
							sqlCommand.Parameters.AddWithValue("@healingtaken", statisticSheet.HealingTaken);
							sqlCommand.Parameters.AddWithValue("@wins", statisticSheet.Wins);
							sqlCommand.Parameters.AddWithValue("@losses", statisticSheet.Losses);

							return sqlCommand.ExecuteNonQuery() >= 1;
						}

						throw new Exception(Resources.Strings_MySQL.Error_Connecting);
					}
				}
				catch (Exception ex)
				{
					Program.ServerForm.MainLog.WriteMessage(ex.Message, Color.Red);
				}

				return false;
			}

			public static Boolean OverallDeleteByCharId(Int32 characterId)
			{
				try
				{
					using (MySqlConnection sqlConnection = new MySqlConnection(ConnectionString))
					{
						sqlConnection.Open();

						if (sqlConnection.State == ConnectionState.Open)
						{
							MySqlCommand sqlCommand = new MySqlCommand
							{
								Connection = sqlConnection,
								CommandText = Resources.Strings_MySQL.NonQuery_Delete_StatisticsOverall_DeleteByCharId
							};

							sqlCommand.Parameters.AddWithValue("@charid", characterId);


							return sqlCommand.ExecuteNonQuery() >= 1;
						}

						throw new Exception(Resources.Strings_MySQL.Error_Connecting);
					}
				}
				catch (Exception ex)
				{
					Program.ServerForm.MainLog.WriteMessage(ex.Message, Color.Red);
				}

				return false;
			}

			public static Boolean WeeklyDeleteByCharId(Int32 characterId)
			{
				try
				{
					using (MySqlConnection sqlConnection = new MySqlConnection(ConnectionString))
					{
						sqlConnection.Open();

						if (sqlConnection.State == ConnectionState.Open)
						{
							MySqlCommand sqlCommand = new MySqlCommand
							{
								Connection = sqlConnection,
								CommandText = Resources.Strings_MySQL.NonQuery_Delete_StatisticsWeekly_DeleteByCharId
							};

							sqlCommand.Parameters.AddWithValue("@charid", characterId);


							return sqlCommand.ExecuteNonQuery() >= 1;
						}

						throw new Exception(Resources.Strings_MySQL.Error_Connecting);
					}
				}
				catch (Exception ex)
				{
					Program.ServerForm.MainLog.WriteMessage(ex.Message, Color.Red);
				}

				return false;
			}
	    }
    }
}