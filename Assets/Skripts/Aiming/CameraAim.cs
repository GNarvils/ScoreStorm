using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Scripting;

public class CameraAim : MonoBehaviour
{
    public AimBaseState currentState; // Pašreizējais mērķa stāvoklis
    public NoAimS Hip = new NoAimS(); // Miera stāvoklis
    public AimS Aim = new AimS(); // Mērķa stāvoklis

    [SerializeField] float sensitivity = 1; // Peles jūtības koeficients
    float xAxis, yAxis; // Horizontālā un vertikālā kustība

    [SerializeField] Transform camFollowPos; // Kameras sekotāja pozīcija

    [HideInInspector] public CinemachineVirtualCamera vCam;
    public float aimFov = 35; // Mērķa redzes lauks
    [HideInInspector] public float Fov; // Pamata redzes lauks
    [HideInInspector] public float currentFov; // Pašreizējais redzes lauks
    public float fovSmooth; // Redzes lauka vienmērīgais pārejas koeficients

    public Animator anim;

    public Transform aimPos; // Mērķa pozīcija
    [SerializeField] float aimSmooth = 20; // Mērķa pozīcijas vienmērīgās pārejas koeficients
    [SerializeField] LayerMask aimMask; 

    public PlayerHealth health; 
    public GameTime gameTime; 
    public GameObject redDot; // Sarkanais punkts, kas parādas ekrāna vidū

    void Start()
    {
        health = GetComponent<PlayerHealth>();
        //Uzstāda kursoru par neredzamu
        Cursor.visible = false;


        vCam = GetComponentInChildren<CinemachineVirtualCamera>();
        Fov = vCam.m_Lens.FieldOfView; // Saglabā pamata redzes lauku
        SwitchState(Hip); // Uzstāda sākotnējo stāvokli uz miera stāvokli

        // Iegūst jutības koeficientu no iepriekš saglabātajiem iestatījumiem
        sensitivity = PlayerPrefs.GetFloat("Sensitivity", 1f);

        // Meklē UI objektu un iegūst spēles laika komponenti no tā
        GameObject uiGameObject = GameObject.Find("UI");
        if (uiGameObject != null)
        {
            gameTime = uiGameObject.GetComponent<GameTime>();
            if (gameTime == null)
            {
                Debug.LogError("GameTime skripts nav atrasts");
            }
        }
        else
        {
            Debug.LogError("UI GameObject nav atrasts.");
        }
    }

    void Update()
    {
        // Ja spēlētājs nav miris un spēle nav beigusies
        if (!health.isDead && !gameTime.gameIsOver)
        {
            // Parāda vai paslēpj sarkano punktu atkarībā no pašreizējā stāvokļa
            if (currentState == Aim)
            {
                redDot.SetActive(true);
            }
            else
            {
                redDot.SetActive(false);
            }

            // Iegūst horizontālo un vertikālo kustību no peles
            xAxis += Input.GetAxisRaw("Mouse X") * sensitivity;
            yAxis += Input.GetAxisRaw("Mouse Y") * sensitivity;
            yAxis = Mathf.Clamp(yAxis, -80, 80); // Ierobežo vertikālo kustību

            // Iestata mērķa redzes lauku, vienmērīgi pārejot starp pašreizējo un nākamo redzes lauku
            vCam.m_Lens.FieldOfView = Mathf.Lerp(vCam.m_Lens.FieldOfView, currentFov, fovSmooth * Time.deltaTime);

            Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            Ray ray = Camera.main.ScreenPointToRay(screenCenter);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimMask))
                aimPos.position = Vector3.Lerp(aimPos.position, hit.point, aimSmooth * Time.deltaTime);

            // Atjauno pašreizējo stāvokli
            currentState.UpdateState(this);
        }
    }

    private void LateUpdate()
    {
        // Atjauno kameras leņķi un rakstura rotāciju
        camFollowPos.localEulerAngles = new Vector3(-yAxis, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis, transform.eulerAngles.z);
    }

    // Metode, lai pārslēgtu stāvokli
    public void SwitchState(AimBaseState state)
    {
        currentState = state; // Uzstāda jauno pašreizējo stāvokli
        currentState.EnterState(this); // Ievada jauno stā
        currentState.EnterState(this); // Ievada jauno stāvokli
    }
}
