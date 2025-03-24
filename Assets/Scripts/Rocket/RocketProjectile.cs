using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class RocketProjectile : MonoBehaviour
{
    [SerializeField] private Direction direction;
    private HashSet<Cell> _collidingCells = new HashSet<Cell>();

    private void Start()
    {
        Vector3 target;
        float distance = 10;
        float exitDistance = 10;
        switch (direction)
        {
            case Direction.Left:
                target = new Vector3(-15, transform.position.y, 0);
                distance = transform.position.x - target.x;
                exitDistance = transform.position.x + 10;
                break;
            case Direction.Right:
                target = new Vector3(15, transform.position.y, 0);
                distance = target.x - transform.position.x;
                exitDistance = 10 - transform.position.x;
                break;
            case Direction.Up:
                target = new Vector3(transform.position.x, 15, 0);
                distance = target.y - transform.position.y;
                exitDistance = 10 - transform.position.y;
                break;
            case Direction.Down:
                target = new Vector3(transform.position.x, -15, 0);
                distance = transform.position.y - target.y;
                exitDistance = transform.position.y + 10;
                break;
            default:
                target = new Vector3(0, 0, 0);
                break;
        }

        var tween = transform.DOMove(target, 0.05f * distance);
        bool exited = false;
        tween.OnUpdate(() =>
        {
            if (!exited && tween.position >= 0.05f * exitDistance)
            {
                RocketExecuter.Instance.FallingControl(-1);
                exited = true;
            }
        });
        tween.OnComplete(() =>
        { 
            Destroy(gameObject);
        });
    }

    private void Update()
    {
        List<Cell> collidingCells = CheckCollisions();
        foreach (Cell cell in collidingCells)
        {
            if (cell.item != null && (cell.item.ItemType == ItemType.HorizontalRocket || cell.item.ItemType == ItemType.VerticalRocket))
                RocketExecuter.Instance.TryExecute(cell, false);
            if (cell.item != null) cell.item.TakeDamage();
        }
    }

    private List<Cell> CheckCollisions()
    {
        // check for colliding cells
        List<Cell> collidingCells = new List<Cell>();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.25f);

        foreach (Collider2D collider in colliders)
        {
            Cell cell = collider.GetComponent<Cell>();
            if (cell != null && !_collidingCells.Contains(cell))
            {
                collidingCells.Add(cell);
                _collidingCells.Add(cell);
            }
        }
        return collidingCells;
    }
}
