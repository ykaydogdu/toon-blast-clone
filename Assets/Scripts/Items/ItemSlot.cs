using UnityEngine;

/// <summary>
/// This class holds the necessary information to create an item.
/// </summary>
public class ItemSlot : MonoBehaviour
{
    public ItemType ItemType;
    public bool Clickable = true;
    public bool CanFall = true;
    public int Health = 1;
    public bool SurroundDamage = false;
}
