using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

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

    public AudioClip clickSound;
    public AudioClip hoverSound;
    private AudioSource audioSource; 

    private float hoverScale = 1.1f;
    private Vector3 normalScale1; 
    private Vector3 normalScale2; 
    private Vector3 normalScale3;
    private Vector3 normalScale4;
    public TMP_Text p1l1Score;
    public TMP_Text p2l1Score;
    public TMP_Text p1l2Score;
    public TMP_Text p2l2Score;

    public TMP_Text help;

    void Start()
    {
        playerSelection.SetActive(true);
        levelSelection.SetActive(false);
        normalScale1 = playerImage1.transform.localScale;
        normalScale2 = playerImage2.transform.localScale;
        normalScale3 = levelImage1.transform.localScale;
        normalScale4 = levelImage2.transform.localScale;
        p1l1Score.text = PlayerPrefs.GetInt("Score_Player_1_Level_1", 0).ToString();
        p2l1Score.text = PlayerPrefs.GetInt("Score_Player_2_Level_1", 0).ToString();
        p1l2Score.text = PlayerPrefs.GetInt("Score_Player_1_Level_2", 0).ToString();
        p2l2Score.text = PlayerPrefs.GetInt("Score_Player_2_Level_2", 0).ToString();

        audioSource = gameObject.AddComponent<AudioSource>();
        float volume = PlayerPrefs.GetFloat("Sound", 1.0f);
        audioSource.volume = volume;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayHoverSound();

        if (eventData.pointerEnter == playerImage1.gameObject)
        {
            playerImage1.transform.localScale = normalScale1 * hoverScale;
            help.text = "Šim spēlētājam kustības spējas ir ātras un ieroči var izdarīt bojājumus pret pretiniekiem, bet izturība ir maza.";
        }
        else if (eventData.pointerEnter == playerImage2.gameObject)
        {
            playerImage2.transform.localScale = normalScale2 * hoverScale;
            help.text = "Šis spēlētājs var izturēt daudzas lietas un viņa ekipējums ir ļoti stiprs, bet kustības ātrums ir ļoti lēns.";
        }
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
        help.text = "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayClickSound(); 

        if (eventData.pointerEnter == playerImage1.gameObject)
            SelectPlayer1();
        else if (eventData.pointerEnter == playerImage2.gameObject)
            SelectPlayer2();
        else if (eventData.pointerEnter == levelImage1.gameObject)
            SelectLevel1();
        else if (eventData.pointerEnter == levelImage2.gameObject)
            SelectLevel2();
    }


    private void PlayClickSound()
    {
        if (clickSound != null)
            audioSource.PlayOneShot(clickSound); 
    }
    private void PlayHoverSound()
    {
        if (hoverSound != null)
            audioSource.PlayOneShot(hoverSound);
    }

    public void SelectPlayer1()
    {
        selectedPlayer = 1;
        PlayerPrefs.SetInt("SelectedPlayer", selectedPlayer);
        Debug.Log("Spēlētājs 1 izvēlēts.");
        playerSelection.SetActive(false);
        levelSelection.SetActive(true);
    }

    public void SelectPlayer2()
    {
        selectedPlayer = 2;
        PlayerPrefs.SetInt("SelectedPlayer", selectedPlayer);
        Debug.Log("Spēlētājs 2 izvēlēts.");
        playerSelection.SetActive(false);
        levelSelection.SetActive(true);
    }

    public void SelectLevel1()
    {
        selectedLevel = 1;
        PlayerPrefs.SetInt("SelectedLevel", selectedLevel);
        Debug.Log("Līmenis 1 izvēlēts.");
        playerSelection.SetActive(false);
        levelSelection.SetActive(false);
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
    public void SelectLevel2()
    {
        selectedLevel = 2;
        PlayerPrefs.SetInt("SelectedLevel", selectedLevel);
        Debug.Log("Līmenis 2 izvēlēts.");
        playerSelection.SetActive(false);
        levelSelection.SetActive(false);
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}
