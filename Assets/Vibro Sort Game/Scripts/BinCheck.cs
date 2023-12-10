using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinCheck : MonoBehaviour
{
    public OVRInput.Controller controller;
    public HapticsManager hM;
    // Start is called before the first frame update
    public ObjectCategory binCategory = ObjectCategory.Concrete;
    public Scoreboard sb;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Haptic")) {
            hM.SetHapticFeedback(1, 0);
            hM.SetHapticFeedback(1, 1);
            sb.DecreaseObjectsLeft();
            if(other.gameObject.GetComponent<SortInteractable>().GetCategory() == binCategory) {
                sb.IncreaseCorrectScore();
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Haptic")) {
            hM.SetHapticFeedback(1, 0);
            hM.SetHapticFeedback(1, 1);
            sb.IncreaseObjectsLeft();
            if(other.gameObject.GetComponent<SortInteractable>().GetCategory() == binCategory) {
                sb.DecreaseCorrectScore();
            }
        }
    }

}
