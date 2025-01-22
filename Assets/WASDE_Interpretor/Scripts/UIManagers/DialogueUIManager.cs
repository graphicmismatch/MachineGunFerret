using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.UIElements;
using System.Collections;
public class DialogueUIManager : MonoBehaviour
{
    public TMP_Text dialogueBox;
    public Transform optionParent;
    public GameObject optionObject;
    public GameObject panel;
    public ScrollRect view;
    public static UnityEvent destroySelf = new UnityEvent();
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void CloseDisplay() {
        dialogueBox.text += "\n==========\n";
        StartCoroutine(ScrollToBottom());
        destroySelf.Invoke();
    }
    public void UpdateDisplay(DialogueData dd)
    {
        if (!panel.activeInHierarchy)
        {
            panel.SetActive(true);
        }
        destroySelf.Invoke();
        if (dialogueBox.text != "")
        {
            dialogueBox.text += "\n";
        }
        dialogueBox.text += DialogueTreeInterpreter.currentlyPlaying.chars[dd.charIDs[dd.charCurrentlySpeaking]].Name+":"+dd.line;
        StartCoroutine(ScrollToBottom());
        if (dd.options.Length == 0)
        {
            GameObject g = Instantiate(optionObject, optionParent);
            OptionData d = new OptionData();
            d.id = -1;
            d.title = DialogueDataReference.inst.defaultResponse;
            g.GetComponent<DialogueOptionManager>().init(d);
        }
        else
        {
            foreach (OptionData d in dd.options)
            {
                GameObject g = Instantiate(optionObject, optionParent);
                g.GetComponent<DialogueOptionManager>().init(d);
            }
        }
    }
    public IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();
        view.verticalNormalizedPosition = 0;
    }

}
