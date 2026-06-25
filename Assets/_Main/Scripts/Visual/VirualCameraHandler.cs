using Unity.Cinemachine;
using UnityEngine;

public class VirualCameraHandler : MonoBehaviour
{
    private CinemachineInputAxisController camController;

    private void Awake()
    {
        camController = GetComponent<CinemachineInputAxisController>();
    }

    private void OnEnable()
    {
        Typewriter.OnMessageDisplay += DisableCamera;
        Typewriter.OnMessageStop += EnableCamera;
    }

    private void OnDisable()
    {
        Typewriter.OnMessageDisplay -= DisableCamera;
        Typewriter.OnMessageStop -= EnableCamera;
    }

    private void DisableCamera()
    {
        if(camController != null)
        {
            camController.enabled = false;
        }
    }

    private void EnableCamera()
    {
        if (camController != null)
        {
            camController.enabled = true;
        }
    }

}
