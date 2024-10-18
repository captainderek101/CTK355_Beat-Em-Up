using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This component controls how the Water level of the player is displayed
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [SerializeField] private Slider playerHealthSlider;
    [SerializeField] private Button closeGameButton;
    private GameObject levelCompleteScreen;

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
        closeGameButton.onClick.AddListener(CloseGame);
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

    public void CloseGame()
    {
        Application.Quit();
    }
}
