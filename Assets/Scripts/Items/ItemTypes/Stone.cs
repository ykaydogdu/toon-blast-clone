using UnityEngine;

public class Stone : Item
{
    public new void Initialize(ItemSlot itemSlot)
    {
        base.Initialize(itemSlot);
        Clickable = false;
        CanFall = false;
    }

    protected override Sprite getSprite()
    {
        var imgLibrary = ItemImageLibrary.Instance;
        return imgLibrary.Stone;
    }
}
