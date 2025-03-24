using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketExecuter : Singleton<RocketExecuter>
{
    private const int MIN_COMBO = 2;

    public GameBoard board;
    [SerializeField] private RocketProjectile rightProjectile;
    [SerializeField] private RocketProjectile leftProjectile;
    [SerializeField] private RocketProjectile upProjectile;
    [SerializeField] private RocketProjectile downProjectile;

    private int projectileCount = 0;

    public void TryExecute(Cell cell, bool click = true)
    {
        List<RocketProjectile> projectileList = new List<RocketProjectile>();
        // check for combo
        List<Cell> matches = click ? ItemMatcher.Instance.findMatches(cell) : new List<Cell> { cell };
        var count = matches.Count;

        if (count < MIN_COMBO)
        {
            RocketOrientation rocketType = ((Rocket)cell.item).RocketType;
            cell.item.TakeDamage();

            // give damage to neighbors
            List<Cell> neighbours;
            if (rocketType == RocketOrientation.Vertical)
            {
                neighbours = cell.GetNeighbours(new Direction[]{ Direction.Right, Direction.Left});
            } else
            {
                neighbours = cell.GetNeighbours(new Direction[] { Direction.Up, Direction.Down });
            }

            foreach (var neighbour in neighbours) 
                neighbour.item?.TakeDamage();

            FallingControl(2);
            projectileList.AddRange(GetProjectileList(cell, rocketType));
        }
        else
        {
            Cell center = DetermineCenter(cell);
            Vector3 targetPos = center.transform.position;
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
                // destroy rockets to prevent multiple shooting
                foreach (var match in matches)
                {
                    match.item.TakeDamage();
                }
                // spawn projectiles
                FallingControl(12);
                for (int dr = -1; dr <= 1; dr++)
                {
                    var currentCell = board.Cells[center.R + dr, center.C];
                    projectileList.AddRange(GetProjectileList(currentCell, RocketOrientation.Horizontal));
                }
                for (int dc = -1; dc <= 1; dc++)
                {
                    var currentCell = board.Cells[center.R, center.C + dc];
                    projectileList.AddRange(GetProjectileList(currentCell, RocketOrientation.Vertical));
                }
            });
        }
    }

    public void FallingControl(int change)
    {
        projectileCount += change;
        if (projectileCount == 0)
        {
            CubeFallingManager.Instance.EnableFall();
            TouchHandler.Instance.EnableTouch();
        } else
        { 
            CubeFallingManager.Instance.DisableFall();
            TouchHandler.Instance.DisableTouch();
        }
    }

    private List<RocketProjectile> GetProjectileList(Cell cell, RocketOrientation orientation)
    { 
        Vector3 pos = cell.transform.position;

        // spawn projectiles
        if (orientation == RocketOrientation.Horizontal)
        {
            var right = Instantiate(rightProjectile, pos + new Vector3(0.5f, 0, 0), Quaternion.identity, cell.transform);
            var left = Instantiate(leftProjectile, pos + new Vector3(-0.5f, 0, 0), Quaternion.identity, cell.transform);
            return new List<RocketProjectile> { right, left };
        }
        else if (orientation == RocketOrientation.Vertical)
        {
            var up = Instantiate(upProjectile, pos + new Vector3(0, 0.5f, 0), Quaternion.identity, cell.transform);
            var down = Instantiate(downProjectile, pos + new Vector3(0, -0.5f, 0), Quaternion.identity, cell.transform);
            return new List<RocketProjectile> { up, down };
        }
        else throw new System.Exception("Invalid rocket type");
    }

    private Cell DetermineCenter(Cell cell)
    {
        int centerR = cell.R;
        int centerC = cell.C;
        if (centerR == 0) centerR++;
        if (centerR == board.RowCount - 1) centerR--;
        if (centerC == 0) centerC++;
        if (centerC == board.ColumnCount - 1) centerC--;
        return board.Cells[centerR, centerC];
    }
}
