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
    KeyBinds keyBindsManager;

    void Start()
    {
        keybinds = transform.Find("KeyBinds");
        waitingKey = false;
        keyBindsManager = KeyBinds.manager;

            for (int i = 0; i < 9; i++)
            {
                Transform child = keybinds.Find(keybinds.GetChild(i).name);

                if (child != null)
                {
                    TMP_Text textComponent = child.GetComponentInChildren<TMP_Text>();
                    if (textComponent != null)
                    {
                        SetKeyText(child.name, textComponent);
                    }
                }
            }
    }

    void SetKeyText(string actionName, TMP_Text textComponent)
    {
        switch (actionName)
        {
            case "Forward":
                textComponent.text = keyBindsManager.forward.ToString();
                break;
            case "Backward":
                textComponent.text = keyBindsManager.backward.ToString();
                break;
            case "Left":
                textComponent.text = keyBindsManager.left.ToString();
                break;
            case "Right":
                textComponent.text = keyBindsManager.right.ToString();
                break;
            case "Run":
                textComponent.text = keyBindsManager.run.ToString();
                break;
            case "Reload":
                textComponent.text = keyBindsManager.reload.ToString();
                break;
            case "Guard":
                textComponent.text = keyBindsManager.guard.ToString();
                break;
            case "Interact":
                textComponent.text = keyBindsManager.interact.ToString();
                break;
            case "Swap":
                textComponent.text = keyBindsManager.swap.ToString();
                break;
        }
    }

    void OnGUI()
    {
        keyEvent = Event.current;
        if (keyEvent.isKey && waitingKey)
        {
            newKey = keyEvent.keyCode;
            waitingKey = false;
            StartCoroutine(AssignKey());
        }
    }

    public void StartAssignment(string keyName)
    {
        if (!waitingKey)
            StartCoroutine(AssignKey());
    }

    public void SendText(TMP_Text text)
    {
        buttonText = text;
    }

    IEnumerator WaitForKey()
    {
        while (!keyEvent.isKey)
            yield return null;
    }

    IEnumerator AssignKey()
    {
        waitingKey = true;
        yield return WaitForKey();

        if (!IsKeyAlreadyUsed(newKey))
        {
            AssignKeyToAction(newKey);
        }
        else
        {
            AssignKeyToAction(GetDefaultValue(buttonText.transform.parent.name));
        }
    }

    bool IsKeyAlreadyUsed(KeyCode key)
    {
        foreach (var property in typeof(KeyBinds).GetProperties())
        {
            if (property.PropertyType == typeof(KeyCode))
            {
                if ((KeyCode)property.GetValue(keyBindsManager) == key)
                {
                    return true;
                }
            }
        }
        return false;
    }

    void AssignKeyToAction(KeyCode key)
    {
        buttonText.text = key.ToString();
        string actionName = buttonText.transform.parent.name;
        typeof(KeyBinds).GetProperty(actionName.ToLower()).SetValue(keyBindsManager, key);
        PlayerPrefs.SetString(actionName.ToLower() + "Key", key.ToString());
        PlayerPrefs.Save();
    }

    KeyCode GetDefaultValue(string actionName)
    {
        switch (actionName)
        {
            case "Forward":
                return KeyCode.W;
            case "Backward":
                return KeyCode.S;
            case "Left":
                return KeyCode.A;
            case "Right":
                return KeyCode.D;
            case "Run":
                return KeyCode.LeftShift;
            case "Reload":
                return KeyCode.R;
            case "Guard":
                return KeyCode.Space;
            case "Interact":
                return KeyCode.E;
            case "Swap":
                return KeyCode.Q;
            default:
                return KeyCode.None;
        }
    }
}
