using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This component controls how the Water level of the player is displayed
/// </summary>
public delegate void UIEvent();
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public UIEvent pauseEvent;
    [SerializeField] private Slider playerHealthSlider;
    [SerializeField] private Image blackScreen;
    [SerializeField] private AnimationCurve blackScreenCurve;
    [SerializeField] public TMP_Text coinUI;
    private GameObject levelCompleteScreen;
    [HideInInspector] public GameObject upgradeShopScreen;

    private const string loadSceneButtonTagName = "Load Scene Button";

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
        if(playerHealthSlider == null)
        {
            Debug.LogWarning(nameof(UIManager) + " is missing player health slider!");
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

    public void SetPlayerHealthUI(float percentWaterLevel)
    {
        playerHealthSlider.value = percentWaterLevel;
    }

    public void ShowLevelCompleteScreen(string sceneToLoad)
    {
        Button transitionButton = null;
        foreach(Transform child in levelCompleteScreen.transform)
        {
            if (child.tag == loadSceneButtonTagName)
            {
                child.TryGetComponent(out transitionButton);
                break;
            }
        }
        if(transitionButton != null)
        {
            transitionButton.onClick.RemoveListener(() => TransitionManager.Instance.LoadSceneAsync(sceneToLoad)); // in case we did this already...
            transitionButton.onClick.AddListener(() => TransitionManager.Instance.LoadSceneAsync(sceneToLoad));
        }
        levelCompleteScreen.SetActive(true);
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
        upgradeShopScreen.SetActive(true);
        GameManager.Instance.EnableOrDisablePlayer(false);
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
