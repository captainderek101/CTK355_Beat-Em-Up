using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

/// <summary>
/// This component controls how the Water level of the player is displayed
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private Image blackScreen;
    [SerializeField] private AnimationCurve blackScreenCurve;
    [SerializeField] public TMP_Text coinUI;
    private GameObject levelCompleteScreen;
    [HideInInspector] public GameObject upgradeShopScreen;
    public UIGroupControl abilityHUD;

    private const string loadSceneButtonTagName = "Load Scene Button";

    private GameObject lastSelected;
    public UnityEvent<GameObject, GameObject> selectionChanged;
    [HideInInspector] public bool paused = false;
    public UnityEvent pauseEvent;
    public UnityEvent pauseExitEvent;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        TransitionManager.Instance.transitionEvent += (float duration) => { ControlBlackScreen(true, duration); };
        if (GameManager.Instance.inFirstLoadedScene == false)
        {
            Color screenColor = blackScreen.color;
            screenColor.a = 1;
            blackScreen.color = screenColor;
            ControlBlackScreen(false, TransitionManager.Instance.defaultTransitionTime);
        }
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != lastSelected && selectionChanged != null)
        {
            foreach (MultiplayerEventSystem eventSystem in FindObjectsOfType<MultiplayerEventSystem>())
            {
                eventSystem.SetSelectedGameObject(EventSystem.current.currentSelectedGameObject);
            }
            selectionChanged.Invoke(lastSelected, EventSystem.current.currentSelectedGameObject);
            lastSelected = EventSystem.current.currentSelectedGameObject;
        }
    }

    public void ShowLevelCompleteScreen()
    {
        SetUIActionMap();
        if(levelCompleteScreen.activeSelf)
        {
            levelCompleteScreen.GetComponent<Menu>().OpenMenu();
        }
        else
        {
            levelCompleteScreen.SetActive(true);
        }
    }

    public bool SetLevelCompleteScreen(GameObject screenObject)
    {
        bool wasSet = levelCompleteScreen != null;
        levelCompleteScreen = screenObject;
        return wasSet;
    }

    public void SetCoinUI(int coins)
    {
        coinUI.text = coins.ToString();
    }

    public void OpenUpgradeShop()
    {
        SetUIActionMap();
        upgradeShopScreen.SetActive(true);
    }

    public void SetPlayerActionMap()
    {
        foreach (PlayerInput player in PlayerInputController.Instance.players)
        {
            player.SwitchCurrentActionMap("Player");
        }
    }

    public void SetUIActionMap()
    {
        foreach (PlayerInput player in PlayerInputController.Instance.players)
        {
            player.SwitchCurrentActionMap("UI");
        }
    }
    public void CloseGame()
    {
        Application.Quit();
    }

    private void ControlBlackScreen(bool makeVisible, float transitionTime)
    {
        if(makeVisible)
        {
            StartCoroutine(ChangeBlackScreenCoroutine(transitionTime, 1));
        }
        else
        {
            StartCoroutine(ChangeBlackScreenCoroutine(transitionTime, 0));
        }
    }

    private IEnumerator ChangeBlackScreenCoroutine(float transitionTime, float targetAlpha)
    {
        float timeSinceStart = 0;
        float current;
        float startingAlpha = blackScreen.color.a;
        float startingAlphaAboveTarget = startingAlpha - targetAlpha;
        Color newColor = blackScreen.color;
        while (timeSinceStart < transitionTime)
        {
            current = blackScreenCurve.Evaluate(timeSinceStart / transitionTime);
            yield return new WaitForEndOfFrame();
            newColor.a = startingAlpha - (current * startingAlphaAboveTarget);
            blackScreen.color = newColor;
            timeSinceStart += Time.deltaTime;
        }
    }
}
