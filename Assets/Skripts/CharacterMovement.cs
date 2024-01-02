using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 10f;
    [HideInInspector] public Vector3 direction;

    [SerializeField] float groundOff;
    [SerializeField] LayerMask groundMask;
    Vector3 ballPos;

    [SerializeField] float gravity = -9.81f;
    Vector3 velocity;

    void Update()
    {
       GetDataMove();
        Gravity();
    }

    void GetDataMove()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        direction = transform.forward * vertical + transform.right * horizontal;
        controller.Move(direction * speed * Time.deltaTime);
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
