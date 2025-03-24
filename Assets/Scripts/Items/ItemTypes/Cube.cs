using UnityEngine;

public class Cube : Item
{
    public CubeColor Color;
    private bool hinted = false;

    public void Initialize(ItemSlot slot, CubeColor color)
    {
        this.Color = color;
        base.Initialize(slot);
    }

    public void SetHinted(bool hinted)
    {
        this.hinted = hinted;
        var spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = getSprite();
    }

    protected override Sprite getSprite()
    {
        if (hinted) return getHintSprite();

        var imgLibrary = ItemImageLibrary.Instance;
        switch (Color)
        {
            case CubeColor.Red: return imgLibrary.RedCube;
            case CubeColor.Green: return imgLibrary.GreenCube;
            case CubeColor.Blue: return imgLibrary.BlueCube;
            case CubeColor.Yellow: return imgLibrary.YellowCube;
            default: return null;
        }
    }

    private Sprite getHintSprite()
    {
        var imgLibrary = ItemImageLibrary.Instance;
        switch (Color)
        {
            case CubeColor.Red: return imgLibrary.RedCubeHint;
            case CubeColor.Green: return imgLibrary.GreenCubeHint;
            case CubeColor.Blue: return imgLibrary.BlueCubeHint;
            case CubeColor.Yellow: return imgLibrary.YellowCubeHint;
            default: return null;
        }
    }
}
