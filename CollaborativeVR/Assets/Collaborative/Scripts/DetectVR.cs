using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Management;

public class DetectVR : MonoBehaviour
{
    public bool startInVR;
    
    public GameObject xrOrigin;
    public GameObject xrController;

    public GameObject desktopCharacter;
    
    // Start is called before the first frame update
    void Start()
    {
        if (startInVR == true)
        {
            var xrSettings = XRGeneralSettings.Instance;
            if (xrSettings == null)
            {
                Debug.Log("XRGeneralSettings is null");
                return;
            }

            var xrManager = xrSettings.Manager;
            if (xrManager == null)
            {
                Debug.Log("XRManagerSettings is null");
                return;
            }

            var xrLoader = xrManager.activeLoader;
            if (xrLoader == null)
            {
                Debug.Log("XRLoader is null");
                SwapToDesktopCharacter();
                return;
            }

            Debug.Log("XRLoader is NOT null");
            xrOrigin.SetActive(true);
            desktopCharacter.SetActive(false);
        }
        else
        {
            SwapToDesktopCharacter();
        }
    }

    void SwapToDesktopCharacter()
    {
        xrOrigin.SetActive(false);
        xrController.SetActive(false);
        desktopCharacter.SetActive(true);
    }
}
