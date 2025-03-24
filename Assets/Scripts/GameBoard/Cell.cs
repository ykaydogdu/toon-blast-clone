using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class represents a single cell.
/// Holds position info, item info and neighbour info.
/// </summary>
public class Cell : MonoBehaviour
{
    [HideInInspector] public int R;
    [HideInInspector] public int C;

    public List<Cell> Neighbours { get; private set; }

    public GameBoard Board { get; private set; }
    public Cell CellBelow { get; set; }

    private Item _item;
    public Item item { get => _item; set => _item = value; }

    public void Initialize(int r, int c, GameBoard board, ItemType itemType, bool isSpawnCell = false)
    {
        Board = board;
        R = r;
        C = c;
        transform.localPosition = !isSpawnCell ? new Vector3(C, R) : new Vector3(C, R + 1);

        // place item
        if (itemType != ItemType.None)
        {
            Item item = ItemFactory.Instance.CreateItem(itemType, transform);
            item.Cell = this;
            item.IsFalling = false;
            item.transform.position = transform.position;
        }

        // cellbelow
        if (R > 0)
        {
            CellBelow = Board.Cells[R - 1, C];
        }

        if (!isSpawnCell)
        {
            // neighbours
            Neighbours = new List<Cell>();
            if (C > 0) Neighbours.Add(Board.Cells[R, C - 1]);
            if (C < Board.ColumnCount - 1) Neighbours.Add(Board.Cells[R, C + 1]);
            if (R > 0) Neighbours.Add(Board.Cells[R - 1, C]);
            if (R < Board.RowCount - 1) Neighbours.Add(Board.Cells[R + 1, C]);
        }
        
    }

    public void GiveItem(Item item)
    {
        if (this.item != null) return;
        this.item = item;
    }

    public void DestroyItem()
    {
        if (item == null) return;
        Destroy(item.gameObject);
        item = null;
    }

    public void ItemFall()
    {
        if (item == null || !item.CanFall) return;
        item.Cell = GetFallTarget();
        item = null;
    }

    Cell GetFallTarget()
    {
        Cell target = CellBelow;
        while (target.CellBelow != null && target.CellBelow.item == null)
        {
            target = target.CellBelow;
        }
        return target;
    }

    public void OnTouch()
    {
        if (!item.Clickable) return;

        switch (item.ItemType)
        {
            case ItemType.RedCube:
            case ItemType.GreenCube:
            case ItemType.BlueCube:
            case ItemType.YellowCube:
                ItemMatcher.Instance.CubeClick(this);
                break;
            case ItemType.VerticalRocket:
            case ItemType.HorizontalRocket:
                RocketExecuter.Instance.TryExecute(this);
                break;
        }
    }

    public List<Cell> GetNeighbours(params Direction[] directions)
    {
        List<Cell> neighbours = new List<Cell>();
        foreach (Direction direction in directions)
        {
            switch (direction)
            {
                case Direction.Left:
                    if (C > 0) neighbours.Add(Board.Cells[R, C - 1]);
                    break;
                case Direction.Right:
                    if (C < Board.ColumnCount - 1) neighbours.Add(Board.Cells[R, C + 1]);
                    break;
                case Direction.Up:
                    if (R < Board.RowCount - 1) neighbours.Add(Board.Cells[R + 1, C]);
                    break;
                case Direction.Down:
                    if (R > 0) neighbours.Add(Board.Cells[R - 1, C]);
                    break;
            }
        }
        return neighbours;
    }
}
