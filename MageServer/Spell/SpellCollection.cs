using Helper;

namespace MageServer
{
    public class SpellCollection : ListCollection<Spell>
    {
        public SpellCollection()
        {
            Add(new Spell());
        }
    }
}
