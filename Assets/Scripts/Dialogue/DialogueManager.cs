using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

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
        playerTransform = GameManager.Instance.playerObject.transform;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        EventSystem.current.SetSelectedGameObject(firstSelected);
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
            DisplayNextDialogueLine();
        }

        isDialogueActive = true;
    }

    public void DisplayNextDialogueLine()
    {
        if(currentDialogue == null || currentDialogueLine >= currentDialogue.dialogueLines.Count)
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
        characterName.text = currentDialogue.dialogueLines[currentDialogueLine].character.name;

        currentDialogue.dialogueLines[currentDialogueLine].line = TextPreprocessor.Instance.PreprocessText(currentDialogue.dialogueLines[currentDialogueLine].line);

        StopAllCoroutines();

        StartCoroutine(TypeSentence(currentDialogue.dialogueLines[currentDialogueLine]));

        currentDialogueLine++;
    }

    private IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
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