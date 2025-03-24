using UnityEngine;

public class ItemImageLibrary : Singleton<ItemImageLibrary>
{
    public Sprite RedCube;
    public Sprite GreenCube;
    public Sprite BlueCube;
    public Sprite YellowCube;

    public Sprite RedCubeHint;
    public Sprite GreenCubeHint;
    public Sprite BlueCubeHint;
    public Sprite YellowCubeHint;

    public Sprite HorizontalRocket;
    public Sprite VerticalRocket;

    public Sprite Box;
    public Sprite Stone;
    public Sprite Vase;
    public Sprite VaseDamaged;

    public Sprite GetSprite(ItemType type)
    {
        switch (type)
        {
            case ItemType.RedCube:
                return RedCube;
            case ItemType.GreenCube:
                return GreenCube;
            case ItemType.BlueCube:
                return BlueCube;
            case ItemType.YellowCube:
                return YellowCube;
            case ItemType.HorizontalRocket:
                return HorizontalRocket;
            case ItemType.VerticalRocket:
                return VerticalRocket;
            case ItemType.Box:
                return Box;
            case ItemType.Stone:
                return Stone;
            case ItemType.Vase:
                return Vase;
            default:
                return null;
        }
    }
}