using System.Collections.Generic;
using UnityEngine;

public class Level
{
    public ItemType[,] BoardData { get; private set; }
    public int RowCount { get { return BoardData.GetLength(0); } }
    public int ColumnCount { get { return BoardData.GetLength(1); } }
    public List<Goal> GoalList { get; private set; }
    public int MoveLimit { get; private set; }

    public Level(LevelData levelData)
    {
        // obstacle counters
        int boxCount = 0;
        int stoneCount = 0;
        int vaseCount = 0;

        BoardData = new ItemType[levelData.grid_height, levelData.grid_width];

        int index = 0;
        for (int r = 0; r < levelData.grid_height; r++)
        {
            for (int c = 0; c < levelData.grid_width; c++)
            { 
                switch (levelData.grid[index++])
                {
                    case "r":
                        BoardData[r, c] = ItemType.RedCube;
                        break;
                    case "g":
                        BoardData[r, c] = ItemType.GreenCube;
                        break;
                    case "b":
                        BoardData[r, c] = ItemType.BlueCube;
                        break;
                    case "y":
                        BoardData[r, c] = ItemType.YellowCube;
                        break;
                    case "rand":
                        BoardData[r, c] = GenerateRandomCube();
                        break;
                    case "vro":
                        BoardData[r, c] = ItemType.VerticalRocket;
                        break;
                    case "hro":
                        BoardData[r, c] = ItemType.HorizontalRocket;
                        break;
                    case "bo":
                        BoardData[r, c] = ItemType.Box;
                        boxCount++;
                        break;
                    case "s":
                        BoardData[r, c] = ItemType.Stone;
                        stoneCount++;
                        break;
                    case "v":
                        BoardData[r, c] = ItemType.Vase;
                        vaseCount++;
                        break;
                    default:
                        BoardData[r, c] = ItemType.None;
                        break;

                }
            }
        }

        // generate goals
        GoalList = new List<Goal>();
        if (boxCount > 0)
        {
            GoalList.Add(new Goal(ItemType.Box, boxCount));
        }
        if (stoneCount > 0)
        {
            GoalList.Add(new Goal(ItemType.Stone, stoneCount));
        }
        if (vaseCount > 0)
        {
            GoalList.Add(new Goal(ItemType.Vase, vaseCount));
        }

        // move limit
        MoveLimit = levelData.move_count;
    }

    ItemType GenerateRandomCube()
    {
        return (ItemType)Random.Range(1, 5);
    }
}
