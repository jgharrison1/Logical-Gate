using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Animator animator;
    public GameObject dialogueBox;
    private GameObject CM;
    public float textWaitTime = 0.05f;

    private Queue<string> sentences;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        CM = GameObject.Find("CutsceneManager");
    }

    public void StartDialogue(Dialogue dialogue) {
        dialogueBox.SetActive(true);
        animator.SetBool("IsOpen", true);
        Debug.Log("Starting Dialogue with " + dialogue.name); //Remove Later
        nameText.text = dialogue.name;

        sentences.Clear(); 

        foreach (string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence() {
        //check if there are more sentences left in the queue
        if (sentences.Count == 0) {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        Debug.Log(sentence); //Remove Later
        StopAllCoroutines(); //stopallcoroutines will stop current dialogue if next one is triggered before it finishes.
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence) {
        //the purpose of this async function is to append dialogue letter by letter instead of all at once,
        //which is more asthetically pleasing to read.
        dialogueText.text = "";
        //ToCharArray() converts string to char array, then append letters to dialogueText
        foreach (char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            if(letter == '.')
                yield return new WaitForSeconds(2.0f);
            else if (letter == ',')
                yield return new WaitForSeconds(0.5f);
            //yield return null; // this waits for 1 frame after appending letter
            else
                yield return new WaitForSeconds(textWaitTime);
        }
    }

    public void EndDialogue() {
        dialogueText.text = "";
        if(!CM.GetComponent<CutsceneManager>().sceneActive){
            Debug.Log("End of Conversation");
            animator.SetBool("IsOpen", false);
            //dialogueBox.SetActive(false);
        }
        FindObjectOfType<CutsceneManager>().NextAction();
    }

}
