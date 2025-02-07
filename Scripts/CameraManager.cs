using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera idleCamera;
    [SerializeField] private CinemachineVirtualCamera followCamera;

    private void Awake()
    {
        SwitchIdleCam();
    }
    
    public void SwitchIdleCam()
    {
        idleCamera.enabled = true;
        followCamera.enabled = false;
    }

    public void SwitchFollowCam(Transform followTransfrom)
    {
        followCamera.Follow = followTransfrom;
        idleCamera.enabled = false;
        followCamera.enabled = true;
    }
}
