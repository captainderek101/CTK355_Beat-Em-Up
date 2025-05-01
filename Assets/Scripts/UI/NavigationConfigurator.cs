using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class NavigationConfigurator : MonoBehaviour
{
    [SerializeField] private UINavigationDirections directionToControl;
    [SerializeField] private bool returnToLastSelected = true;
    private Selectable selectable;
    private void Start()
    {
        selectable = GetComponent<Selectable>();
        UIManager.Instance.selectionChanged.AddListener(OnSelectionChanged);
    }

    private void OnSelectionChanged(GameObject previous, GameObject current)
    {
        if (returnToLastSelected && current == gameObject && previous.activeSelf && previous.TryGetComponent(out Selectable previousSelectable) && previous.transform.parent.name != "DialogueBox")
        {
            Navigation newNavigation = new Navigation();
            newNavigation.mode = selectable.navigation.mode;
            newNavigation.wrapAround = selectable.navigation.wrapAround;
            switch(directionToControl)
            {
                case UINavigationDirections.Up:
                    newNavigation.selectOnUp = previousSelectable;
                    break;
                case UINavigationDirections.Down:
                    newNavigation.selectOnDown = previousSelectable;
                    break;
                case UINavigationDirections.Left:
                    newNavigation.selectOnLeft = previousSelectable;
                    break;
                case UINavigationDirections.Right:
                    newNavigation.selectOnRight = previousSelectable;
                    break;
                default:
                    break;
            }
            selectable.navigation = newNavigation;
        }
    }

    public enum UINavigationDirections
    {
        Up, Down, Left, Right
    }
}
