using UnityEngine;

public class Rocket : Item
{
    public RocketOrientation RocketType;

    public void Initialize(ItemSlot slot, RocketOrientation type)
    {
        this.RocketType = type;
        base.Initialize(slot);
    }

    protected override Sprite getSprite()
    {
        var imgLibrary = ItemImageLibrary.Instance;
        switch (RocketType)
        {
            case RocketOrientation.Horizontal: return imgLibrary.HorizontalRocket;
            case RocketOrientation.Vertical: return imgLibrary.VerticalRocket;
            default: return null;
        }
    }
}
