using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBinds : MonoBehaviour
{
    public static KeyBinds manager;

    //Darbības pogas mainīgie.
    public KeyCode forward {get; set;}
    public KeyCode backward { get; set; }
    public KeyCode left { get; set; }
    public KeyCode right { get; set; }
    public KeyCode interact { get; set; }
    public KeyCode run{ get; set; }
    public KeyCode reload { get; set; }
    public KeyCode guard { get; set; }

    public KeyCode swap { get; set; }

    void Awake()
    {
        //Dabū kontroles vadītāja skriptu
        if (manager == null) { 
            manager = this;
        } 
        else if (manager != this) { 
            Destroy(gameObject);
        }

        //Dabū katras darbības pogu, kura ja nav tad tiek iestatīta noklusējuma vērtība.
        forward = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("forwardKey", "W"));
        backward = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("backwardKey", "S"));
        left = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("leftKey", "A"));
        right = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("rightKey", "D"));
        interact = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("interactKey", "E"));
        run = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("runKey", "LeftShift"));
        reload = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("reloadKey", "R"));
        guard = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("guardKey", "Space"));
        swap = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("swapKey", "Q"));
    }
}
