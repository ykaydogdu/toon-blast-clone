using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.U2D;

public class Item : MonoBehaviour
{
    private const int BaseSortingOrder = 10;
    private static int childSpriteOrder = 0;

    public ItemType ItemType;
    public bool Clickable;
    public bool CanFall;
    public int Health;

    public bool SurroundDamage = false;
    
    public bool IsFalling = false;

    private Cell cell;
    public Cell Cell
    {
        get => cell;
        set
        {
            cell = value;
            transform.SetParent(cell.transform);
            cell.GiveItem(this);
            if (CanFall)
            {
                var anim = GetComponent<FallingAnim>();
                IsFalling = true;
                anim.FallTo(cell);
            }
        }
    }

    public void Initialize(ItemSlot itemSlot)
    {
        ItemType = itemSlot.ItemType;
        Clickable = itemSlot.Clickable;
        CanFall = itemSlot.CanFall;
        if (CanFall)
        {
            gameObject.AddComponent<FallingAnim>().item = this;
        }
        Health = itemSlot.Health;
        SurroundDamage = itemSlot.SurroundDamage;
        var spriteRenderer = new GameObject($"Sprite_{childSpriteOrder}").AddComponent<SpriteRenderer>();
        spriteRenderer.transform.SetParent(transform);
        spriteRenderer.transform.localPosition = Vector3.zero;
        spriteRenderer.transform.localScale = Vector3.one;
        spriteRenderer.sprite = getSprite();
        spriteRenderer.sortingLayerID = SortingLayer.NameToID("Cell");
        spriteRenderer.sortingOrder = BaseSortingOrder + childSpriteOrder++;
    }

    protected virtual Sprite getSprite() { return null; }

    protected void UpdateSprite()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = getSprite();
    }

    public void TakeDamage(int damage = 1)
    {
        Health -= damage;
        ParticleManager.Instance.PlayParticle(this);
        UpdateSprite();
        if (Health <= 0)
        {
            if (IsFalling)
            {
                GetComponent<FallingAnim>().TerminateFalling();
            }
            GoalManager.Instance.UpdateLevelGoal(ItemType);
            DestroyItem();
        }
    }

    private void DestroyItem()
    {
        cell.DestroyItem();
    }
}
