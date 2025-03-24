using UnityEngine;

public class Box : Item
{ 
    public new void Initialize(ItemSlot itemSlot)
    {
        base.Initialize(itemSlot);
        Clickable = false;
        SurroundDamage = true;
    }

    protected override Sprite getSprite()
    {
        var imgLibrary = ItemImageLibrary.Instance;
        return imgLibrary.Box;
    }
}
