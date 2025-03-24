using UnityEngine;

/// <summary>
/// This class represents the game board as a grid of cells.
/// Manages the cells in the board.
/// </summary>
public class GameBoard : MonoBehaviour
{
    private const float X_OFFSET = 0.5f;
    private const float Y_OFFSET = -2.5f;


    public Transform cellsParent;
    public Transform particlesParent;
    [SerializeField] private Cell cellPrefab;

    public Level Level;

    public int RowCount { get; private set; }
    public int ColumnCount { get; private set; }

    public Cell[,] Cells { get; private set; }
    public Cell[] SpawnCells { get; private set; }

    private void Awake()
    {
        LoadLevelInfo();
        createCells();
        CubeFallingManager.Instance.Initialize(this);
        MoveManager.Instance.Initialize(Level.MoveLimit);
        GoalManager.Instance.Initialize(Level.GoalList);
    }

    private void LoadLevelInfo()
    {
        LevelManager.Instance.Initialize();
        Level = LevelManager.Instance.GetLevel();
        RowCount = Level.RowCount;
        ColumnCount = Level.ColumnCount;
        float x = (-ColumnCount * 0.5f) + X_OFFSET;
        float y = (-RowCount * 0.5f) + Y_OFFSET;
        cellsParent.transform.position = new Vector3(x, y, 0);
    }

    private void createCells()
    {
        Cells = new Cell[RowCount, ColumnCount];
        for (int r = 0; r < RowCount; r++)
        {
            for (int c = 0; c < ColumnCount; c++)
            {
                Cell cell = Instantiate(cellPrefab, new Vector3(c, r, 0), Quaternion.identity, cellsParent);
                cell.name = $"Cell ({r}, {c})";
                Cells[r, c] = cell;
            }
        }
        SpawnCells = new Cell[ColumnCount];
        for (int c = 0; c < ColumnCount; c++)
        {
            Cell spawnCell = Instantiate(cellPrefab, new Vector3(c, RowCount, 0), Quaternion.identity, cellsParent);
            Destroy(spawnCell.GetComponent<BoxCollider2D>()); // prevent collisions with projectiles
            spawnCell.name = $"SpawnCell ({RowCount}, {c})";
            SpawnCells[c] = spawnCell;

        }
        initializeCells();
    }

    private void initializeCells()
    {
        for (int r = 0; r < RowCount; r++)
        {
            for (int c = 0; c < ColumnCount; c++)
            {
                Cell cell = Cells[r, c];
                ItemType type = Level.BoardData[r, c];
                cell.Initialize(r, c, this, type);
            }
        }
        for (int c = 0, r = RowCount; c < ColumnCount; c++)
        {
            Cell cell = SpawnCells[c];
            cell.Initialize(r, c, this, ItemType.None, true);
        }
    }
}
