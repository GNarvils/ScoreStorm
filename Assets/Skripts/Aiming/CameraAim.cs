using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Scripting;

public class CameraAim : MonoBehaviour
{
    public AimBaseState currentState;
    public NoAimS Hip = new NoAimS();
    public AimS Aim = new AimS();

    [SerializeField] float sensitivity = 1;
    float xAxis, yAxis;
    [SerializeField] Transform camFollowPos;


    [HideInInspector] public CinemachineVirtualCamera vCam;
    public float aimFov = 40;
    [HideInInspector] public float Fov;
    [HideInInspector] public float currentFov;
    public float fovSmooth;
    public Animator anim;

    public Transform aimPos;
    [SerializeField] float aimSmooth = 20;
    [SerializeField] LayerMask aimMask;
    public GameObject redDot;
    public GameObject blackDot;

   
    void Start()
    {
        Cursor.visible = false;
        vCam = GetComponentInChildren<CinemachineVirtualCamera>();
        Fov = vCam.m_Lens.FieldOfView;
        SwitchState(Hip);
    }
    void Update()
    {
        xAxis += Input.GetAxisRaw("Mouse X") * sensitivity;
        yAxis += Input.GetAxisRaw("Mouse Y") * sensitivity;
        yAxis = Mathf.Clamp(yAxis, -80, 80);

        vCam.m_Lens.FieldOfView = Mathf.Lerp(vCam.m_Lens.FieldOfView, currentFov, fovSmooth * Time.deltaTime);

        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimMask))
            aimPos.position = Vector3.Lerp(aimPos.position, hit.point, aimSmooth * Time.deltaTime);

        currentState.UpdateState(this);
    }
    private void LateUpdate()
    {
        camFollowPos.localEulerAngles = new Vector3(-yAxis, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis, transform.eulerAngles.z);
    }

    public void SwitchState(AimBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
}
