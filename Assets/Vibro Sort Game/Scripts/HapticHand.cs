using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HapticHand : MonoBehaviour
{

    public HapticsManager hM;
    public OVRInput.Controller controller;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Haptic"))
        {
            HapticInteractable currentHapticObject = other.gameObject.GetComponent<HapticInteractable>();
            ObjectCategory category = other.gameObject.GetComponent<SortInteractable>().GetCategory();

            if (controller == OVRInput.Controller.LTouch)
            {
                hM.LeftSetColorHaptic(category);
                hM.LeftHapticBuffer_amp = currentHapticObject.GetHapticBufferAmplitudes();
                hM.LeftHapticBuffer_freq = currentHapticObject.GetHapticBufferFrequencies();
                hM.leftHapticLoop = currentHapticObject.GetIsLoopable();
                hM.leftHapticStartTime = Time.time;
            }
            else if (controller == OVRInput.Controller.RTouch)
            {
                hM.RightSetColorHaptic(category);
                hM.RightHapticBuffer_amp = currentHapticObject.GetHapticBufferAmplitudes();
                hM.RightHapticBuffer_freq = currentHapticObject.GetHapticBufferFrequencies();
                hM.rightHapticLoop = currentHapticObject.GetIsLoopable();
                hM.rightHapticStartTime = Time.time;
            }

            // Grabbing Mechanism
            if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controller)) {
                if (controller == OVRInput.Controller.LTouch) {
                    hM.SetHapticFeedback(0, 1);
                } else if (controller == OVRInput.Controller.RTouch) {
                    hM.SetHapticFeedback(0, 0);
                }
            }

            // 
        }
    }
    private void OnTriggerExit(Collider other) {
        hM.End(controller);
    }
}
