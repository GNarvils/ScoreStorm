using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    public GameObject infoPanel;

    public GameObject screen1;
    public GameObject screen2;
    public GameObject screen3;

    // Metode, kas parāda info paneli
    public void ShowInfoPanel()
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(true);
            screen1.SetActive(true);
            screen2.SetActive(false);
            screen3.SetActive(false);
        }
    }

    // Metode, kas paslēpj info paneli
    public void HideInfoPanel()
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(false);
            screen1.SetActive(false);
            screen2.SetActive(false);
            screen3.SetActive(false);
        }
    }
    //Parāda pirmo ekrānu
    public void showScreen1() 
    {
        screen1.SetActive(true);
        screen2.SetActive(false);
        screen3.SetActive(false);
    }
    //Parāda otro ekrānu
    public void showScreen2()
    {
        screen1.SetActive(false);
        screen2.SetActive(true);
        screen3.SetActive(false);
    }
    //Parāda trešo ekrānu
    public void showScreen3()
    {
        screen1.SetActive(false);
        screen2.SetActive(false);
        screen3.SetActive(true);
    }
    void Start()
    {
        // Sākotnēji info paneli noslēpjam
        if (infoPanel != null)
        {
            infoPanel.SetActive(false);
        }
    }

    }

