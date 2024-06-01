using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public BaseState currentState; // Pašreizējais stāvoklis
    public IdleS idle = new IdleS(); // Miera stāvoklis
    public WalkS walk = new WalkS(); // Gājiena stāvoklis
    public SprintS sprint = new SprintS(); // Skriešanas stāvoklis

    public PlayerHealth health; 
    public KeyBinds key; 
    public CharacterController controller; 

    public float speed; // Kopējais kustības ātrums
    public float walkSpeed = 3, walkBackwardsS = 2; // Ātrums pastaigāšanās un atpakaļgaitā
    public float sprintSpeed = 7, sprintBackwardsS = 5; // Skriešanas un atpakaļgaitas ātrums
    [HideInInspector] public float hzInput, vInput; // Horizontālais un vertikālais ievades mainīgie
    [HideInInspector] public Vector3 direction; // Kustības virziens

    [SerializeField] float groundOff; // Attālums līdz grīdai
    [SerializeField] LayerMask groundMask; // Slānis, kas apzīmē zemi
    Vector3 ballPos;

    [SerializeField] float gravity = -9.81f; 
    Vector3 velocity; 

    public Animator anim; 
    public GameTime gameTime; 

    void Start()
    {
        // Uzstāda sākotnējo stāvokli uz miera stāvokli
        SwitchState(idle);

        // Iegūst vajadzīgos komponentus
        health = GetComponent<PlayerHealth>();
        key = GetComponentInParent<KeyBinds>();

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
            // Iegūst datus par kustību
            GetDataMove();

            // Uzstāda gravitāciju un apstrādā pašreizējo stāvokli
            Gravity();

            // Nosaka horizontālo un vertikālo vērtību animācijai
            anim.SetFloat("horizontal", hzInput);
            anim.SetFloat("vertical", vInput);

            // Atjauno pašreizējo stāvokli
            currentState.UpdateState(this);
        }
        else
        {
            // Aptur horizontālo un vertikālo kustību animācijā
            anim.SetFloat("horizontal", 0f);
            anim.SetFloat("vertical", 0f);
        }
    }

    // Metode, lai pārslēgtu stāvokli
    public void SwitchState(BaseState state)
    {
        currentState = state; // Uzstāda jauno pašreizējo stāvokli
        currentState.EnterState(this); // Ievada jauno stāvokli
    }

    // Metode, lai iegūtu datus par kustību
    void GetDataMove()
    {
        hzInput = 0f; // Norāda horizontālo ievadi kā 0
        vInput = 0f; // Norāda vertikālo ievadi kā 0

        // Iegūst horizontālo un vertikālo ievadi no taustiņiem
        if (Input.GetKey(KeyBinds.manager.left))
            hzInput -= 1f;
        if (Input.GetKey(KeyBinds.manager.right))
            hzInput += 1f;
        if (Input.GetKey(KeyBinds.manager.forward))
            vInput += 1f;
        if (Input.GetKey(KeyBinds.manager.backward))
            vInput -= 1f;

        // Ja ir kāda horizontāla vai vertikāla ievade
        if (hzInput != 0f || vInput != 0f)
        {
            // Noteik virzienu, kurā pārvietoties
            direction = (transform.forward * vInput + transform.right * hzInput).normalized;

            // Pārvieto spēlētāju uz norādīto virzienu un ātrumu
            controller.Move(direction * speed * Time.deltaTime);
        }
    }

    // Metode, lai noteiktu, vai atrodas uz zemes
    bool IsGrounded()
    {
        ballPos = new Vector3(transform.position.x, transform.position.y - groundOff, transform.position.z);
        if (Physics.CheckSphere(ballPos, controller.radius - 0.05f, groundMask))
            return true;
        return false;
    }

    // Metode, lai apstrādātu gravitāciju
    void Gravity()
    {
        // Ja spēlētājs nav uz zemes, palielina leņķi, ar kuru spēlētājs kritīs
        if (!IsGrounded())
            velocity.y += gravity * Time.deltaTime;
        else if (velocity.y < 0) 
            velocity.y = -2;

        // Pārvieto spēlētāju, ietekmējot gravitāciju
        controller.Move(velocity * Time.deltaTime);
    }
}

