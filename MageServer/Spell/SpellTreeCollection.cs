using System;
using System.Linq;
using Helper;

namespace MageServer
{
    public class SpellTreeCollection : ListCollection<SpellTree>
    {
        public SpellTree FindById(Int32 treeId)
        {
            return this.FirstOrDefault(spellTree => (spellTree != null && treeId == spellTree.Id));
        }

        public SpellTree FindByIdAndClass(Int32 treeId, Character.PlayerClass  playerClass)
        {
            return this.FirstOrDefault(spellTree => (spellTree != null && treeId == spellTree.Id && spellTree.PlayerClass == playerClass));
        }

        public SpellTree FindBySlotAndClass(Int32 listSlot, Character.PlayerClass playerClass)
        {
            return this.FirstOrDefault(spellTree => (spellTree != null && listSlot == spellTree.ListSlot && spellTree.PlayerClass == playerClass));
        }

		public void SetLevelById(Int32 treeId, Int32 level)
		{
			SpellTree tree = this.FirstOrDefault(spellTree => (spellTree != null && treeId == spellTree.Id));
			if (tree != null) tree.Level = level;
		}
    }
}
