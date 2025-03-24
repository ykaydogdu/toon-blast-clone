/// <summary>
/// Represents a goal for the level.
/// Defined by (ItemType, count) pair. Which means the player has to break count number of ItemType.
/// </summary>
public class Goal
{
    public ItemType ItemType;
    public int Count;

    public Goal(ItemType itemType, int count)
    {
        ItemType = itemType;
        Count = count;
    }
}
