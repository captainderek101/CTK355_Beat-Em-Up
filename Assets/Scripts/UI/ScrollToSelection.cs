using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollToSelection : MonoBehaviour
{
    private Selectable selectable;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform scrollContent;
    private RectTransform buttonRect;

    private void Start()
    {
        selectable = GetComponent<Selectable>();
        buttonRect = GetComponent<RectTransform>();
        UIManager.Instance.selectionChanged.AddListener(OnSelectionChanged);
    }
    private void OnSelectionChanged(GameObject previous, GameObject current)
    {
        if (current == selectable.gameObject)
        {
            Canvas.ForceUpdateCanvases();
            float deltaY = scrollRect.transform.InverseTransformPoint(transform.position).y;
            if (deltaY > 0)
            {
                deltaY += buttonRect.rect.height / 2;
            }
            else
            {
                deltaY -= buttonRect.rect.height / 2;
            }

            if (Mathf.Abs(deltaY) > scrollRect.viewport.rect.height / 2)
            {
                Vector2 newPosition = scrollContent.anchoredPosition;
                if (deltaY > 0)
                {
                    newPosition.y -= deltaY - (scrollRect.viewport.rect.height / 2);
                }
                else
                {
                    newPosition.y -= deltaY + (scrollRect.viewport.rect.height / 2);
                }
                scrollContent.anchoredPosition = newPosition;
            }
        }
    }
}
