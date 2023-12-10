using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HapticsManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI debugText;
    private int hapticFrameMilli = 100; // Each frame is 50 milliseconds.

    private float[] leftHapticsBuffer_amp;
    public float[] LeftHapticsBuffer_amp {
        get { return leftHapticsBuffer_amp;  }
        set {
            leftHapticsBuffer_amp = value;
            leftDuration = value.Length * hapticFrameMilli;
        }
    }
    private float[] leftHapticsBuffer_freq;
    public float[] LeftHapticsBuffer_freq {
        get { return leftHapticsBuffer_freq;  }
        set {
            leftHapticsBuffer_freq = value;
            leftDuration = value.Length * hapticFrameMilli;
        }
    }
    private int leftDuration = 0;
    public bool leftHapticLoop = true;
    public float leftHapticStartTime = 0f;

    private float[] rightHapticsBuffer_amp;
    public float[] RightHapticsBuffer_amp {
        get { return rightHapticsBuffer_amp;  }
        set {
            rightHapticsBuffer_amp = value;
            rightDuration = value.Length * hapticFrameMilli;
        }
    }
    private float[] rightHapticsBuffer_freq;
    public float[] RightHapticsBuffer_freq {
        get { return rightHapticsBuffer_freq;  }
        set {
            rightHapticsBuffer_freq = value;
            rightDuration = value.Length * hapticFrameMilli;
        }
    }
    private int rightDuration = 0;
    public bool rightHapticLoop = true;
    public float rightHapticStartTime = 0f;

    private float[] grabHapticValues = new float[] { 0f, 0.1f, 0.25f, 0.45f, 0.7f, 1f, 0f, 0f, 0f, 0f, 0f };
    private float[] putDownHapticValues = new float[] { 0.5f, 1f, 0.85f, 0.65f, 0.4f, 0.1f, 0f, 0f, 0f, 0f, 0f };
    private float[] collisionHapticValues = new float[] { 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f };


    void Start()
    {
        LeftHapticsBuffer_amp = new float[] {0f};
        LeftHapticsBuffer_freq = new float[] {0f};

        RightHapticsBuffer_amp = new float[] {0f};
        RightHapticsBuffer_freq = new float[] {0f};

        InitializeColorToHapticMapping();
    }

    // Need a function that takes a relative Haptic Buffer in seconds and returns an Absolute Haptic Buffer in milliseconds.


    private void FixedUpdate() {
        if (leftHapticLoop) {
            int currentHapticFrame_left = ((int)((Time.time - leftHapticStartTime) * 1000) % leftDuration) / hapticFrameMilli;
            OVRInput.SetControllerVibration(leftHapticsBuffer_freq[currentHapticFrame_left], leftHapticsBuffer_amp[currentHapticFrame_left], OVRInput.Controller.LTouch);
        } else {
            if ((Time.time - leftHapticStartTime) * 1000 < leftDuration) {
                int currentHapticFrame_left = ((int)((Time.time - leftHapticStartTime) * 1000) % leftDuration) / hapticFrameMilli;
                OVRInput.SetControllerVibration(leftHapticsBuffer_freq[currentHapticFrame_left], leftHapticsBuffer_amp[currentHapticFrame_left], OVRInput.Controller.LTouch);
            }
        }

        if (rightHapticLoop) {
            int currentHapticFrame_right = ((int)((Time.time - rightHapticStartTime) * 1000) % rightDuration) / hapticFrameMilli;
            OVRInput.SetControllerVibration(rightHapticsBuffer_freq[currentHapticFrame_right], rightHapticsBuffer_amp[currentHapticFrame_right], OVRInput.Controller.RTouch);
        } else {
            if ((Time.time - rightHapticStartTime) * 1000 < rightDuration) {
                int currentHapticFrame_right = ((int)((Time.time - rightHapticStartTime) * 1000) % rightDuration) / hapticFrameMilli;
                OVRInput.SetControllerVibration(rightHapticsBuffer_freq[currentHapticFrame_right], rightHapticsBuffer_amp[currentHapticFrame_right], OVRInput.Controller.RTouch);
            }
        }

        debugText.text = string.Join(" ", rightHapticsBuffer_freq) + "\n" + rightDuration.ToString(); // + " " + currentHapticFrame_right.ToString();//rightHapticsBuffer_freq[currentHapticFrame_right].ToString() + " " + rightHapticsBuffer_amp[currentHapticFrame_right].ToString();
    }

    public void End(OVRInput.Controller c) {
        if (c == OVRInput.Controller.LTouch) {
            LeftHapticsBuffer_amp = new float[] {0f};
            LeftHapticsBuffer_freq = new float[] {0f};
        } else if (c == OVRInput.Controller.RTouch) {
            RightHapticsBuffer_amp = new float[] {0f};
            RightHapticsBuffer_freq = new float[] {0f};
        }
    }

    public void SetHapticFeedback(int type, int side)
    // type=0 means grab, 1=putdown, 2=collision with objects
    // side=0=right, 1=left
    {
        //right
        if (side == 0) {
            switch (type)
            {
                case 0
                    SetRightHapticValues(grabHapticValues);
                    break;
                case 1:
                    SetRightHapticValues(putDownHapticValues);
                    break;
                case 2:
                    SetRightHapticValues(collisionHapticValues);
                    break;
            }
        }
        //left
        if (side != 0) {
            switch (type)
            {
                case 0
                    SetLeftHapticValues(grabHapticValues);
                    break;
                case 1:
                    SetLeftHapticValues(putDownHapticValues);
                    break;
                case 2:
                    SetLeftHapticValues(collisionHapticValues);
                    break;
            }
        }
    }

    // To set the values for putting down, grabbing, and collision
    private void SetRightHapticValues(float[] hapticValues)
    {
        RightHapticsBuffer_freq = new float[hapticValues.Length];
        RightHapticsBuffer_amp = hapticValues;
        RightDuration = LeftHapticsBuffer_freq.Length * hapticFrameMilli;
    }

    private void SetLeftHapticValues(float[] hapticValues)
    {
        LeftHapticsBuffer_freq = new float[hapticValues.Length];
        LeftHapticsBuffer_amp = hapticValues;
        leftDuration = LeftHapticsBuffer_freq.Length * hapticFrameMilli;
    }

    // To initialize the different category amp values
    private void InitializeColorToHapticMapping()
    {
        colorToHapticMapping = new Dictionary<ObjectCategory, float[]>
        {
            { ObjectCategory.Concrete, new float[] { 1f, 1f, 0.7f, 0.5f, 0.2f, 0f, 0f, 0f, 0f, 0f, 0f } },
            { ObjectCategory.Squishy, new float[] { 0f, 0.2f, 0.5f, 0.7f, 1f, 1f, 0.7f, 0.5f, 0.2f, 0f, 0f } },
            { ObjectCategory.Chalky, new float[] { 1f, 0f, 1f, 0f, 1f, 0f, 1f, 0f, 1f, 0f, 1f } },
            { ObjectCategory.Bruisy, new float[] { 1f, 0.5f, 0.1f, 1f, 0.5f, 0.1f, 1f, 0.5f, 0.1f, 0f } },
            { ObjectCategory.Alien, new float[] { 0.2f, 1f, 0.3f, 0.9f, 0.4f, 0.8f, 0.5f, 0.7f, 0.6, 0.7f 0.8f } }
        };
    }

    public void LeftSetColorHaptic(ObjectCategory category)
    {
        LeftHapticsBuffer_freq = new float[LeftHapticsBuffer_freq.Length];
        LeftHapticsBuffer_amp = colorToHapticMapping[category];
        leftDuration = LeftHapticsBuffer_freq.Length * hapticFrameMilli;
    }

    public void RightSetColorHaptic(ObjectCategory category)
    {
        RightHapticsBuffer_freq = new float[RightHapticsBuffer_freq.Length];
        RightHapticsBuffer_amp = colorToHapticMapping[category];
        RightDuration = RightHapticsBuffer_freq.Length * hapticFrameMilli;
    }

    
}


// When the task is done, screen turns on.