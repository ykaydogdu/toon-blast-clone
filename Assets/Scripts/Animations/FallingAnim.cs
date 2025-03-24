using UnityEngine;
using DG.Tweening;

public class FallingAnim : MonoBehaviour
{
    public Item item;
    [HideInInspector] public Cell targetCell;

    [SerializeField] private float duration = 0.35f;
    private Vector3 targetPos;

    public void Awake()
    {
        DOTween.SetTweensCapacity(1000, 100);
    }

    public void FallTo(Cell targetCell)
    { 
        this.targetCell = targetCell;
        this.targetPos = targetCell.transform.position;
        item.transform.DOMoveY(targetPos.y, duration).SetEase(Ease.InCubic).OnComplete(() =>
        { 
            this.targetCell = null;
            targetCell.GiveItem(item);
            item.IsFalling = false;
        });
    }

    public void TerminateFalling()
    {
        item.transform.DOKill();
        item.IsFalling = false;
    }
}
