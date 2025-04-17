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
    public Button continueButton;
    public float textWaitTime = 0.25f;
    private int spamClick = 0;
    string sentence = "";
    private Queue<string> sentences;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        CM = GameObject.Find("CutsceneManager");
    }

    public void StartDialogue(Dialogue dialogue) {
        continueButton.interactable = true;
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
        spamClick++;
        StopAllCoroutines(); //stopallcoroutines will stop current dialogue if next one is triggered before it finishes.
        if(sentences.Count == 0 & spamClick > 1) {
            continueButton.interactable = false;
            dialogueText.text = sentence;
            spamClick = 0;
            Invoke("reactivateContinueButton", 0.75f);
        }
        else if (sentences.Count == 0) {
            spamClick = 0;
            EndDialogue();
            return;
        }

        if(spamClick == 1) {
            sentence = sentences.Dequeue();
            StartCoroutine(TypeSentence(sentence));
        }

        else if(spamClick > 1) {
            continueButton.interactable = false;
            dialogueText.text = sentence;
            spamClick = 0;
            Invoke("reactivateContinueButton", 0.75f); // Waits .75 seconds before allowing user to hit continue again.
        }
    }


    IEnumerator TypeSentence(string sentence) {
        //the purpose of this async function is to append dialogue letter by letter instead of all at once,
        //which is more asthetically pleasing to read.
        dialogueText.text = "";
        string[] tokens = sentence.Split(" "); // split sentence into tokens using space character as delimiter.

        foreach(string word in tokens) {
            if(word == "<color=\"red\">" | word == "<color=\"yellow\">" | word == "<color=\"white\">"  | word == "</color>") {
                dialogueText.text += word; //if word is a text color change, appends all at once so that player wont see the code print out. 
            }
            else {
                //ToCharArray() converts string to char array, then appends letters to dialogueText
                foreach (char letter in word.ToCharArray()) {
                    dialogueText.text += letter;
                    //token += letter;
                    if(letter == '.')
                        yield return new WaitForSeconds(0.5f);
                    else if (letter == ',')
                        yield return new WaitForSeconds(0.25f);
                    //yield return null; // this waits for 1 frame after appending letter
                    else
                        yield return new WaitForSeconds(textWaitTime);
                }
                dialogueText.text += " "; // reappend the space character to each word.
            }
        }
        spamClick = 0; //if coroutine finishes printing the whole line, resets spamclick to 0.
    }


    public void EndDialogue() {
        dialogueText.text = "";
        if(!CM.GetComponent<CutsceneManager>().sceneActive){
            Debug.Log("End of Conversation");
            animator.SetBool("IsOpen", false);
            //dialogueBox.SetActive(false); //removed because this makes dialogue box instantly disappear
        }
        else {
            continueButton.interactable = false;
            dialogueText.text = "...";
            FindObjectOfType<CutsceneManager>().NextAction();
        }
    }

    public void reactivateContinueButton() {
        continueButton.interactable = true;
    }
}
