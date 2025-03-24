using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class ItemMatcher : Singleton<ItemMatcher>
{
    [SerializeField] private GameBoard gameBoard;
    [SerializeField] private ParticleSystem hintParticle;

    private HashSet<Cell> rocketHintedCells = new HashSet<Cell>();

    private bool[,] visited;

    private const int MIN_MATCH = 2;
    private const int MIN_MATCH_HINT = 4;

    private void Start()
    {
        visited = new bool[gameBoard.RowCount, gameBoard.ColumnCount];
    }

    private void Update()
    {
        HintCells();
        HintRockets();
    }

    private void HintCells()
    {
        var hintedCells = new HashSet<Cell>();
        for (int r = 0; r < gameBoard.RowCount; r++)
        {
            for (int c = 0; c < gameBoard.ColumnCount; c++)
            {
                var cell = gameBoard.Cells[r, c];
                if (hintedCells.Contains(cell) || cell.item == null || cell.item.IsFalling || cell.item is not Cube) continue;
                var matchList = findMatches(gameBoard.Cells[r, c]);
                if (matchList.Count >= MIN_MATCH_HINT)
                {
                    foreach (var match in matchList)
                    {
                        var matchCube = match.item as Cube;
                        if (matchCube != null)
                        {
                            matchCube.SetHinted(true);
                            hintedCells.Add(match);
                        }
                    }
                }
                else
                {
                    if (cell.item is Cube cube)
                    {
                        cube.SetHinted(false);
                    }
                }
            }
        }
    }

    private void HintRockets()
    { 
        for (int r = 0;r < gameBoard.RowCount; r++)
        {
            for (int c = 0; c < gameBoard.ColumnCount; c++)
            {
                var cell = gameBoard.Cells[r, c];
                if (cell.item == null || cell.item.IsFalling || cell.item is not Rocket)
                {
                    if (rocketHintedCells.Contains(cell))
                    {
                        // remove particle system child
                        foreach (Transform child in cell.transform)
                        {
                            if (child.name.Contains("RocketComboHint"))
                            {
                                Destroy(child.gameObject);
                            }
                        }
                    }
                }
                var matchList = findMatches(gameBoard.Cells[r, c]);
                if (matchList.Count >= MIN_MATCH)
                {
                    if (rocketHintedCells.Contains(cell)) continue;
                    foreach (var match in matchList)
                    {
                        var matchRocket = match.item as Rocket;
                        if (matchRocket != null)
                        {
                            var particle = Instantiate(hintParticle, match.transform.position, Quaternion.identity, match.transform);
                            rocketHintedCells.Add(match);
                        }
                    }
                }
                else
                {
                    if (!rocketHintedCells.Contains(cell)) continue;
                    if (cell.item is Rocket rocket)
                    {
                        // remove particle system child
                        foreach (Transform child in cell.transform)
                        {
                            if (child.name.Contains("RocketComboHint"))
                            {
                                Destroy(child.gameObject);
                            }
                        }
                    }
                }
            }
        }
    }

    public void CubeClick(Cell cell)
    {
        var matches = findMatches(cell);
        if (matches.Count < MIN_MATCH) return;

        _ = MoveManager.Instance.DecreaseMovesAsync();

        if (matches.Count >= MIN_MATCH_HINT)
        { 
            Vector3 targetPos = cell.transform.position;
            TouchHandler.Instance.DisableTouch();
            Sequence sequence = DOTween.Sequence();
            foreach (var match in matches)
            {
                if (match == cell) continue;
                var matchCube = match.item;
                sequence.Join(matchCube.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.25f));
                sequence.Join(matchCube.transform.DOMove(targetPos, 0.25f));
            }
            sequence.OnComplete(() =>
            { 
                ExecuteCubeClick(cell, matches);
                TouchHandler.Instance.EnableTouch();
            });
        }
        else
        {
            ExecuteCubeClick(cell, matches);
        }
    }

    private void ExecuteCubeClick(Cell cell, List<Cell> matches)
    {
        // destroy matches and damage 
        HashSet<Cell> cellsToDamage = new HashSet<Cell>();
        foreach (var match in matches)
        {
            foreach (var neighbour in match.Neighbours)
            {
                var item = neighbour.item;
                if (item != null && item.SurroundDamage)
                {
                    cellsToDamage.Add(neighbour);
                }
            }
            match.item.TakeDamage();
        }
        foreach (var cellToDamage in cellsToDamage)
        {
            cellToDamage.item.TakeDamage();
        }

        // spawn rocket
        if (matches.Count >= MIN_MATCH_HINT)
        {
            int orientation = UnityEngine.Random.Range(0, 2);
            var rocket = ItemFactory.Instance.CreateItem(orientation == 0 ? ItemType.HorizontalRocket : ItemType.VerticalRocket, cell.transform);
            rocket.Cell = cell;
            rocket.transform.position = cell.transform.position;
        }
    }

    public List<Cell> findMatches(Cell cell)
    {
        if (cell.item == null) return new List<Cell>();
        var cellList = new List<Cell>();
        clearVisited();
        var type = cell.item.ItemType;
        findMatchesRecursive(cell, type, cellList);
        return cellList;
    }

    private void findMatchesRecursive(Cell cell, ItemType type, List<Cell> cellList)
    {
        if (cell == null || cell.item == null || visited[cell.R, cell.C] || cell.item.IsFalling)
        {
            return;
        }
        visited[cell.R, cell.C] = true;

        var item = cell.item;
        if (type == ItemType.HorizontalRocket || type == ItemType.VerticalRocket)
        {
            if (item.ItemType != ItemType.HorizontalRocket && item.ItemType != ItemType.VerticalRocket) return;
        }else if (item.ItemType != type) return;

        cellList.Add(cell);
        foreach (var neighbour in cell.Neighbours)
        {
            findMatchesRecursive(neighbour, type, cellList);
        }
    }

    private void clearVisited()
    {
        for (int i = 0; i < gameBoard.RowCount; i++)
        {
            for (int j = 0; j < gameBoard.ColumnCount; j++)
            {
                visited[i, j] = false;
            }
        }
    }
}
