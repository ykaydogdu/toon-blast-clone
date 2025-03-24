using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory : Singleton<ItemFactory>
{
    public ItemSlot ItemPrefab;

    private Dictionary<ItemType, Func<ItemSlot, Item>> itemCreators
        = new Dictionary<ItemType, Func<ItemSlot, Item>>
        {
            { ItemType.RedCube, (itemSlot) => createCube(itemSlot, CubeColor.Red) },
            { ItemType.GreenCube, (itemSlot) => createCube(itemSlot, CubeColor.Green) },
            { ItemType.BlueCube, (itemSlot) => createCube(itemSlot, CubeColor.Blue) },
            { ItemType.YellowCube, (itemSlot) => createCube(itemSlot, CubeColor.Yellow) },
            { ItemType.HorizontalRocket, (itemSlot) => createRocket(itemSlot, RocketOrientation.Horizontal) },
            { ItemType.VerticalRocket, (itemSlot) => createRocket(itemSlot, RocketOrientation.Vertical) },
            { ItemType.Box, createBox },
            { ItemType.Stone, createStone },
            { ItemType.Vase, createVase }
        };

    public Item CreateItem(ItemType itemType, Transform parent)
    {
        if (itemType == ItemType.None) return null;

        var itemSlot = Instantiate(ItemPrefab, Vector3.zero, Quaternion.identity, parent);
        itemSlot.ItemType = itemType;

        if (!itemCreators.TryGetValue(itemType, out var createFunc))
        {
            Debug.LogWarning($"No creator for item type {itemType}");
            return null;
        }
        
        return createFunc(itemSlot);
    }

    private static Item createCube(ItemSlot itemSlot, CubeColor color)
    {
        var item = itemSlot.gameObject.AddComponent<Cube>();
        item.Initialize(itemSlot, color);
        return item;
    }

    private static Item createRocket(ItemSlot itemSlot, RocketOrientation type)
    {
        var item = itemSlot.gameObject.AddComponent<Rocket>();
        item.Initialize(itemSlot, type);
        return item;
    }

    private static Item createBox(ItemSlot itemSlot)
    {
        var item = itemSlot.gameObject.AddComponent<Box>();
        item.Initialize(itemSlot);
        return item;
    }

    private static Item createStone(ItemSlot itemSlot)
    {
        var item = itemSlot.gameObject.AddComponent<Stone>();
        item.Initialize(itemSlot);
        return item;
    }

    private static Item createVase(ItemSlot itemSlot)
    {
        var item = itemSlot.gameObject.AddComponent<Vase>();
        item.Initialize(itemSlot);
        return item;
    }

    public Item CreateRandomCube(Transform parent)
    {
        ItemType itemType = (ItemType)UnityEngine.Random.Range(1, 5);
        return CreateItem(itemType, parent);
    }
}
