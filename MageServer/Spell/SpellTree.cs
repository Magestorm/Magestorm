using System;

namespace MageServer
{
    public class SpellTree
    {
        public Int32 Id;
        public Int32 ListSlot;
	    public Int32 Level;
        public String Name;
        public Character.PlayerClass PlayerClass;
        public readonly SpellCollection TreeSpells;

        public SpellTree()
        {
            TreeSpells = new SpellCollection();
        }

		public SpellTree(SpellTree spellTree)
		{
			if (spellTree == null)
			{
				Id = -1;
				ListSlot = -1;
				Level = -1;
				Name = "None";
				PlayerClass = Character.PlayerClass.Arcanist;
				TreeSpells = null;
				return;
			}

			Id = spellTree.Id;
			ListSlot = spellTree.ListSlot;
			Level = spellTree.Level;
			Name = spellTree.Name;
			PlayerClass = spellTree.PlayerClass;
			TreeSpells = spellTree.TreeSpells;
		}
		public SpellTree(SpellTree spellTree, Int32 level)
		{
			if (spellTree == null)
			{
				Id = -1;
				ListSlot = -1;
				Level = -1;
				Name = "None";
				PlayerClass = Character.PlayerClass.Arcanist;
				TreeSpells = null;
				return;
			}

			Id = spellTree.Id;
			ListSlot = spellTree.ListSlot;
			Level = level;
			Name = spellTree.Name;
			PlayerClass = spellTree.PlayerClass;
			TreeSpells = spellTree.TreeSpells;
		}
    }
}
