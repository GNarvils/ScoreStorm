using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraAim : MonoBehaviour
{
    AimBaseState currentState;
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

    void Start()
    {
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
        currentState.UpdateState(this);
    }
    private void LateUpdate() {
        camFollowPos.localEulerAngles = new Vector3(-yAxis, camFollowPos.localEulerAngles.y, camFollowPos.localEulerAngles.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, xAxis, transform.eulerAngles.z);
    }

    public void SwitchState(AimBaseState state) {
        currentState = state;
        currentState.EnterState(this);
    }
}
