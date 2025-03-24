using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ButtonShrink : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float shrinkScale = 0.95f; // Scale to shrink to
    public float animationDuration = 0.1f; // Duration of the animation

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOScale(originalScale * shrinkScale, animationDuration);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(originalScale, animationDuration);
    }
}