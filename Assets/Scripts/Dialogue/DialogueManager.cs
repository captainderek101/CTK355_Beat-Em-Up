using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using System.Net.NetworkInformation;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public Image characterIcon;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;

    private Queue<Dialogue> queue;
    private Dialogue currentDialogue;
    private int currentDialogueLine = 0;
    private Vector3 dialogueStartLocation = Vector3.zero;
    private Transform playerTransform;

    public bool isDialogueActive = false;
    private bool typing = false;

    public float typingSpeed = 0.2f;

    public Animator animator;
    [SerializeField] private GameObject firstSelected;

    [SerializeField] private bool enablePlayerOnDialogueEnd = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        queue = new Queue<Dialogue>();
    }

    private void Start()
    {
        playerTransform = GameManager.Instance.playerObjects[0].transform;
    }

    public void StartDialogue(Dialogue dialogue, Transform actor)
    {
        playerTransform = actor;
        StartDialogue(dialogue);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        foreach(MultiplayerEventSystem eventSystem in FindObjectsOfType< MultiplayerEventSystem>())
        {
            eventSystem.SetSelectedGameObject(firstSelected);
        }
        UIManager.Instance.SetUIActionMap();
        if (dialogue.endCurrentDialogue)
        {
            currentDialogue = null;
            queue.Clear();
        }
        dialogueStartLocation = playerTransform.position;
        queue.Enqueue(dialogue);

        if (!isDialogueActive)
        {
            animator.Play("DialogueBoxShow");
        }
        if (dialogue.endCurrentDialogue || !isDialogueActive)
        {
            typing = false;
            DisplayNextDialogueLine();
        }

        isDialogueActive = true;
    }

    public void DisplayNextDialogueLine()
    {
        if (typing)
        {
            typing = false;
            return;
        }

        if (currentDialogue == null || currentDialogueLine >= currentDialogue.dialogueLines.Count)
        {
            if(currentDialogue != null && currentDialogue.dialogueEnded != null)
            {
                currentDialogue.dialogueEnded.Invoke();
            }

            currentDialogueLine = 0;
            if (queue.Count == 0)
            {
                HideDialogue();
                return;
            }
            else
            {
                currentDialogue = queue.Dequeue();
            }
        }

        characterIcon.sprite = currentDialogue.dialogueLines[currentDialogueLine].character.icon;
        characterIcon.gameObject.SetActive(characterIcon.sprite != null);
        characterName.text = currentDialogue.dialogueLines[currentDialogueLine].character.name;

        currentDialogue.dialogueLines[currentDialogueLine].line = TextPreprocessor.Instance.PreprocessText(currentDialogue.dialogueLines[currentDialogueLine].line);

        StopAllCoroutines();

        StartCoroutine(TypeSentence(currentDialogue.dialogueLines[currentDialogueLine]));

        currentDialogueLine++;
    }

    private IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        typing = true;
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            if (typing)
                yield return new WaitForSeconds(typingSpeed);
        }
        typing = false;
    }

    private void HideDialogue()
    {
        if (enablePlayerOnDialogueEnd)
        {
            UIManager.Instance.SetPlayerActionMap();
        }
        isDialogueActive = false;
        animator.Play("DialogueBoxHide");
    }

    private void FixedUpdate()
    {
        if (isDialogueActive && currentDialogue.distanceToEnd < (playerTransform.position - dialogueStartLocation).magnitude)
        {
            currentDialogue.dialogueEnded.Invoke();
            currentDialogue = null;
            DisplayNextDialogueLine();
        }
    }
}