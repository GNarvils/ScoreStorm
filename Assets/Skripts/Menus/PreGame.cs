using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PreGame : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image playerImage1;
    public Image playerImage2; 
    public GameObject playerSelection;

    public Image levelImage1;
    public Image levelImage2;
    public GameObject levelSelection;

    private int selectedPlayer = 0; 
    private int selectedLevel = 0;

    private float hoverScale = 1.1f;
    private Vector3 normalScale1; 
    private Vector3 normalScale2; 
    private Vector3 normalScale3;
    private Vector3 normalScale4;

    void Start()
    {
        playerSelection.SetActive(true);
        levelSelection.SetActive(false);
        normalScale1 = playerImage1.transform.localScale;
        normalScale2 = playerImage2.transform.localScale;
        normalScale3 = levelImage1.transform.localScale;
        normalScale4 = levelImage2.transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerEnter == playerImage1.gameObject)
            playerImage1.transform.localScale = normalScale1 * hoverScale;
        else if (eventData.pointerEnter == playerImage2.gameObject)
            playerImage2.transform.localScale = normalScale2 * hoverScale;
        else if (eventData.pointerEnter == levelImage1.gameObject)
            levelImage1.transform.localScale = normalScale3 * hoverScale;
        else if (eventData.pointerEnter == levelImage2.gameObject)
            levelImage2.transform.localScale = normalScale4 * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerEnter == playerImage1.gameObject)
            playerImage1.transform.localScale = normalScale1;
        else if (eventData.pointerEnter == playerImage2.gameObject)
            playerImage2.transform.localScale = normalScale2;
        else if (eventData.pointerEnter == levelImage1.gameObject)
            levelImage1.transform.localScale = normalScale3;
        else if (eventData.pointerEnter == levelImage2.gameObject)
            levelImage2.transform.localScale = normalScale4;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerEnter == playerImage1.gameObject)
            SelectPlayer1();
        else if (eventData.pointerEnter == playerImage2.gameObject)
            SelectPlayer2();
        else if (eventData.pointerEnter == levelImage1.gameObject)
            SelectLevel1();
        else if (eventData.pointerEnter == levelImage2.gameObject)
            SelectLevel2();
    }

    public void SelectPlayer1()
    {
        selectedPlayer = 1;
        PlayerPrefs.SetInt("SelectedPlayer", selectedPlayer);
        Debug.Log("Player 1 selected.");
        playerSelection.SetActive(false);
        levelSelection.SetActive(true);
    }

    public void SelectPlayer2()
    {
        selectedPlayer = 2;
        PlayerPrefs.SetInt("SelectedPlayer", selectedPlayer);
        Debug.Log("Player 2 selected.");
        playerSelection.SetActive(false);
        levelSelection.SetActive(true);
    }

    public void SelectLevel1()
    {
        selectedLevel = 1;
        PlayerPrefs.SetInt("SelectedLevel", selectedLevel);
        Debug.Log("Level 1 selected.");
        playerSelection.SetActive(false);
        levelSelection.SetActive(false);
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
    public void SelectLevel2()
    {
        selectedLevel = 2;
        PlayerPrefs.SetInt("SelectedLevel", selectedLevel);
        Debug.Log("Level 2 selected.");
        playerSelection.SetActive(false);
        levelSelection.SetActive(false);
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}
