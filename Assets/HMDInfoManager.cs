using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HMDInfoManager : MonoBehaviour
{
    public GameObject xrOrigin;
    public GameObject desktopCharacter;
    // Start is called before the first frame update
    void Start()
    {
        if (!XRSettings.isDeviceActive)
        {
            desktopCharacter.SetActive(true);
            xrOrigin.SetActive(false);
        } 
        else if (XRSettings.isDeviceActive && XRSettings.loadedDeviceName == "Mock HMD" || XRSettings.loadedDeviceName == "MockHMDDisplay")
        {
            desktopCharacter.SetActive(true);
            xrOrigin.SetActive(false);
        }
        else
        {
            xrOrigin.SetActive(true);
            desktopCharacter.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
