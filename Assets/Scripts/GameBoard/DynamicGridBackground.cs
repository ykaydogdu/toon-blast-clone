using UnityEngine;

/// <summary>
/// Dynamically sets the size of the grid background based on the level info. 
/// </summary>
public class DynamicGridBackground : MonoBehaviour
{
    [SerializeField] private GameBoard board;
    private const float WIDTH_PADDING = 0.35f;
    private const float HEIGHT_PADDING = 0.5f;

    private void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            Debug.LogError("SpriteRenderer not found!");
            return;
        }

        float width = board.ColumnCount + WIDTH_PADDING;
        float height = board.RowCount + HEIGHT_PADDING;

        sr.size = new Vector2(width, height);
    }

}
