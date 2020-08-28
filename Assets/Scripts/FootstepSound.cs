using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSound : MonoBehaviour {
    
    [FMODUnity.EventRef] public string Footstep = "";
    
    public void PlayFootstepSound() {
        if (Footstep != "") {
            FMODUnity.RuntimeManager.PlayOneShot(Footstep, this.transform.position);
        }
    }    
}
