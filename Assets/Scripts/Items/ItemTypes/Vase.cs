using UnityEngine;

public class Vase : Item
{
    private const int MAX_HEALTH = 2;

    public new void Initialize(ItemSlot itemSlot)
    {
        base.Initialize(itemSlot);
        Clickable = false;
        SurroundDamage = true;
        Health = MAX_HEALTH;
        UpdateSprite();
    }

    protected override Sprite getSprite()
    {
        var imgLibrary = ItemImageLibrary.Instance;
        switch (Health)
        {
            case 2:
                return imgLibrary.Vase;
            case 1:
                return imgLibrary.VaseDamaged;
            default:
                return imgLibrary.Vase;
        }
    }
}
