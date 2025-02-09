using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventPopupScript : MonoBehaviour
{
    [SerializeField] private PopupType popupType;
    [SerializeField] private GameObject popupObject;

    private const string loadSceneButtonTagName = "Load Scene Button";
    private const string pauseMenuAnimationBool = "Show";

    private void Start()
    {
        switch (popupType)
        {
            case PopupType.PlayerDeath:
                HealthController playerHealthController = GameManager.Instance.playerObject.GetComponent<HealthController>();
                playerHealthController.deathEvents.AddListener(() =>
                {
                    popupObject.SetActive(true);
                });
                Button transitionButton = null;
                foreach (Transform child in popupObject.transform)
                {
                    if (child.tag == loadSceneButtonTagName)
                    {
                        child.TryGetComponent(out transitionButton);
                        break;
                    }
                }
                if (transitionButton != null)
                {
                    transitionButton.onClick.AddListener(() => TransitionManager.Instance.ReloadCurrentScene());
                }
                break;
            case PopupType.LevelComplete:
                UIManager.Instance.SetLevelCompleteScreen(popupObject);
                break;
            case PopupType.Pause:
                UIManager.Instance.pauseEvent += () =>
                {
                    popupObject.SetActive(true);
                    popupObject.TryGetComponent(out Animator pauseMenuAnimator);
                    if (pauseMenuAnimator != null)
                        pauseMenuAnimator.SetBool(pauseMenuAnimationBool, true);
                    GameManager.Instance.EnableOrDisablePlayer(false);
                };
                break;
            case PopupType.UpgradeShop:
                UIManager.Instance.upgradeShopScreen = popupObject;
                break;
            default: 
                break;
        }
    }

    private enum PopupType
    {
        PlayerDeath,
        LevelComplete,
        Pause,
        UpgradeShop
    }
}
