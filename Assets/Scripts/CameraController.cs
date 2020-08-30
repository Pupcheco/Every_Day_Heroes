using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject subject;
    public Vector3 positionOffset;

    void LateUpdate()  // Changing position in LateUpdate avoids jittery camera movement.
    {
        this.transform.position = subject.transform.position + positionOffset; 
    }
}
