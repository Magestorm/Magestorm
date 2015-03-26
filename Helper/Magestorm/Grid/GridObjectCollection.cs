namespace Helper
{
    public class GridObjectCollection : ListCollection<GridObject>
    {
        public GridObjectCollection()
        {
            Add(new GridObject());
        }
    }
}
