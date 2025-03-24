using UnityEngine;

public class TouchHandler : Singleton<TouchHandler>
{
    private const string celltag = "Cell";

    [SerializeField] private new Camera camera;
    [SerializeField] private GameBoard gameBoard;

    private void Update()
    {
        if (!enabled) return;
#if UNITY_EDITOR
        GetTouchEditor();
#else
        GetTouchMobile();
#endif
    }

    private void GetTouchEditor()
    { 
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 touchPosition = camera.ScreenToWorldPoint(Input.mousePosition);
            ExecuteTouch(touchPosition);
        }
    }

    private void GetTouchMobile()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                Vector3 touchPosition = camera.ScreenToWorldPoint(touch.position);
                ExecuteTouch(touchPosition);
            }
        }
    }

    private void ExecuteTouch(Vector3 touchPosition)
    {
        var overlap = Physics2D.OverlapPoint(touchPosition) as BoxCollider2D;
        if ( overlap != null && overlap.CompareTag(celltag))
        { 
            overlap.GetComponent<Cell>().OnTouch();
        }
    }

    public void EnableTouch()
    {
        enabled = true;
    }

    public void DisableTouch()
    {
        enabled = false;
    }

    private void OnEnable()
    {
        MoveManager.Instance.OnMovesFinished += DisableTouch;
        GoalManager.Instance.OnGoalsCompleted += DisableTouch;
    }

    private void OnDisable()
    {
        MoveManager.Instance.OnMovesFinished -= DisableTouch;
        GoalManager.Instance.OnGoalsCompleted -= DisableTouch;
    }
}
