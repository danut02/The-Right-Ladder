using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public static CameraSwitch cameraInstance;
    private void Awake() => cameraInstance = this;
    public GameObject enableTppCamera; // Enables or disables the third person perspective camera
    public GameObject enableFppCamera; // Enables or disables the first person perspective camera
    public Camera tppCamera; // Third Person Perspective Camera
    public Camera fppCamera; // First Person Perspective Camera
    public Camera playerCamera; // Camera that is currently being used
    private bool switchCamera = true;
    
    // Start is called before the first frame update
    void Start()
    {
        // We start in third person perspective
        enableFppCamera.SetActive(false); 
        playerCamera = tppCamera;
    }

    // Update is called once per frame
    void Update()
    {
        // Press V to switch between first person and third person perspective
        if(Input.GetKeyDown(KeyCode.V)) 
        {
            if(switchCamera)
            {
                // Switch to first person perspective
                enableFppCamera.SetActive(true);
                enableTppCamera.SetActive(false);
                playerCamera = fppCamera;
                switchCamera = false;
            }
            else
            {
                // Switch to third person perspective
                enableFppCamera.SetActive(false);
                enableTppCamera.SetActive(true);
                playerCamera = tppCamera;
                switchCamera = true;
            }
        }
    }
}
