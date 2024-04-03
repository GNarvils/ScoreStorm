using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
public class CharacterMovement : MonoBehaviour
{

    public BaseState currentState;
    public IdleS idle = new IdleS();
    public WalkS walk = new WalkS();
    public SprintS sprint = new SprintS();

    public PlayerHealth health;
    public CharacterController controller;
    public float speed;
    public float walkSpeed = 3, walkBackwardsS = 2;
    public float sprintSpeed = 7, sprintBackwardsS = 5;
    [HideInInspector] public float hzInput, vInput;
    [HideInInspector] public Vector3 direction;

    [SerializeField] float groundOff;
    [SerializeField] LayerMask groundMask;
    Vector3 ballPos;

    [SerializeField] float gravity = -9.81f;
    Vector3 velocity;

    public Animator anim;

    void Start() {
        SwitchState(idle);
        health = GetComponent<PlayerHealth>();
    }
    void Update()
    {
        if (!health.isDead)
        {
            GetDataMove();
            Gravity();

            anim.SetFloat("horizontal", hzInput);
            anim.SetFloat("vertical", vInput);

            currentState.UpdateState(this);
        }
    }

    public void SwitchState(BaseState state) {
        currentState = state;
        currentState.EnterState(this);
    }
    void GetDataMove()
    {
        hzInput = 0f;
        vInput = 0f;

        if (Input.GetKey(KeyCode.A)) 
            hzInput -= 1f;
        if (Input.GetKey(KeyCode.D)) 
            hzInput += 1f;
        if (Input.GetKey(KeyCode.W)) 
            vInput += 1f;
        if (Input.GetKey(KeyCode.S)) 
            vInput -= 1f;

        if (hzInput != 0f || vInput != 0f)
        {
            direction = (transform.forward * vInput + transform.right * hzInput).normalized;
            controller.Move(direction * speed * Time.deltaTime);
        }
    }
    bool IsGrounded() {
        ballPos = new Vector3(transform.position.x, transform.position.y - groundOff, transform.position.z);
        if (Physics.CheckSphere(ballPos, controller.radius - 0.05f, groundMask)) return true;
        return false;
    }

    void Gravity() {
        if (!IsGrounded()) velocity.y += gravity * Time.deltaTime;
        else if (velocity.y < 0) velocity.y = -2;

        controller.Move(velocity * Time.deltaTime);
    }
    private void OnDrawGizmo() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(ballPos, controller.radius - 0.05f);
    }
}
