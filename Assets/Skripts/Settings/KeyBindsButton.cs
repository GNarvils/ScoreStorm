using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyBindsButton : MonoBehaviour
{
    Transform keybinds;
    Event keyEvent;
    TMP_Text buttonText;
    KeyCode newKey;
    bool waitingKey;
    void Start()
    {
        // Find the KeyBinds GameObject directly under the Canvas
        keybinds = transform.Find("KeyBinds");
        waitingKey = false;

        if (keybinds != null)
        {
            // Loop through child elements only if keybinds is not null
            for (int i = 0; i < 9; i++)
            {
                // Check if the child exists
                Transform child = keybinds.Find(keybinds.GetChild(i).name);

                if (child != null)
                {
                    TMP_Text textComponent = child.GetComponentInChildren<TMP_Text>();
                    if (textComponent != null)
                    {
                        switch (child.name)
                        {
                            case "Forward":
                                textComponent.text = KeyBinds.manager.forward.ToString();
                                break;
                            case "Backward":
                                textComponent.text = KeyBinds.manager.backward.ToString();
                                break;
                            case "Left":
                                textComponent.text = KeyBinds.manager.left.ToString();
                                break;
                            case "Right":
                                textComponent.text = KeyBinds.manager.right.ToString();
                                break;
                            case "Run":
                                textComponent.text = KeyBinds.manager.run.ToString();
                                break;
                            case "Reload":
                                textComponent.text = KeyBinds.manager.reload.ToString();
                                break;
                            case "Guard":
                                textComponent.text = KeyBinds.manager.guard.ToString();
                                break;
                            case "Interact":
                                textComponent.text = KeyBinds.manager.interact.ToString();
                                break;
                            case "Swap":
                                textComponent.text = KeyBinds.manager.swap.ToString();
                                break;
                        }
                    }
                }
            }
        }
        else
        {
            Debug.LogError("KeyBinds GameObject not found!");
        }
    }


    void OnGUI()
    {
        keyEvent = Event.current;
        if (keyEvent.isKey && waitingKey) {
            newKey = keyEvent.keyCode;
            waitingKey = false;
        }
    }

    public void StartAssignment(string keyName) {
        if (!waitingKey)
            StartCoroutine(AssignKey(keyName));
    }

    public void SendText(TMP_Text text) {
        buttonText = text;
    }

    IEnumerator WaitForKey() {
        while (!keyEvent.isKey)
            yield return null;
    }

    public IEnumerator AssignKey(string keyName)
    {
        waitingKey = true;

        yield return WaitForKey();

        switch (keyName) {
            case "forward":
                KeyBinds.manager.forward = newKey;
                buttonText.text = KeyBinds.manager.forward.ToString();
                PlayerPrefs.SetString("forwardKey", KeyBinds.manager.forward.ToString());
                break;
            case "backward":
                KeyBinds.manager.backward = newKey;
                buttonText.text = KeyBinds.manager.backward.ToString();
                PlayerPrefs.SetString("backwardKey", KeyBinds.manager.backward.ToString());
                break;
            case "left":
                KeyBinds.manager.left = newKey;
                buttonText.text = KeyBinds.manager.left.ToString();
                PlayerPrefs.SetString("leftKey", KeyBinds.manager.left.ToString());
                break;
            case "right":
                KeyBinds.manager.right = newKey;
                buttonText.text = KeyBinds.manager.right.ToString();
                PlayerPrefs.SetString("rightKey", KeyBinds.manager.right.ToString());
                break;
            case "run":
                KeyBinds.manager.run = newKey;
                buttonText.text = KeyBinds.manager.run.ToString();
                PlayerPrefs.SetString("runKey", KeyBinds.manager.run.ToString());
                break;
            case "reload":
                KeyBinds.manager.reload = newKey;
                buttonText.text = KeyBinds.manager.reload.ToString();
                PlayerPrefs.SetString("reloadKey", KeyBinds.manager.reload.ToString());
                break;
            case "guard":
                KeyBinds.manager.guard = newKey;
                buttonText.text = KeyBinds.manager.guard.ToString();
                PlayerPrefs.SetString("guardKey", KeyBinds.manager.guard.ToString());
                break;
            case "interact":
                KeyBinds.manager.interact = newKey;
                buttonText.text = KeyBinds.manager.interact.ToString();
                PlayerPrefs.SetString("interactKey", KeyBinds.manager.interact.ToString());
                break;
            case "swap":
                KeyBinds.manager.swap = newKey;
                buttonText.text = KeyBinds.manager.swap.ToString();
                PlayerPrefs.SetString("swapKey", KeyBinds.manager.swap.ToString());
                break;
        }
        yield return null;
    }
}
