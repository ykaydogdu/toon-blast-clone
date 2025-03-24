using System.Collections.Generic;
using UnityEngine;

public class CubeFallingManager : Singleton<CubeFallingManager>
{
    private bool isActive;
    private GameBoard board;

    public void Initialize(GameBoard board)
    {
        this.board = board;

        EnableFall();
    }

    public void CubeFalls()
    {
        for (int r = 0; r < board.RowCount; r++)
        {
            for (int c = 0; c < board.ColumnCount; c++)
            {
                var cell = board.Cells[r, c];
                if (cell.item != null && cell.CellBelow != null && cell.CellBelow.item == null)
                    cell.ItemFall();
            }
        }
        for (int c = 0; c < board.ColumnCount; c++)
        {
            var cell = board.SpawnCells[c];
            if (cell.item != null && cell.CellBelow != null && cell.CellBelow.item == null)
                cell.ItemFall();
        }
    }

    public void TopRowFill()
    {
        int topRow = board.RowCount - 1;
        for (int c = 0; c < board.ColumnCount; c++)
        {
            var cell = board.Cells[topRow, c];
            if (cell.item == null)
            {
                var spawnCell = board.SpawnCells[c];
                Item item = ItemFactory.Instance.CreateRandomCube(spawnCell.transform);
                spawnCell.GiveItem(item);
                item.transform.position = spawnCell.transform.position;
            }
        }
    }

    public void EnableFall() { isActive = true; }
    public void DisableFall() { isActive = false; }

    private void Update()
    {
        if (!isActive) return;

        CubeFalls();
        TopRowFill();
    }
}
