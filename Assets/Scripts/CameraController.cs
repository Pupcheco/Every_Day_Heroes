using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject subject;
    public Vector3 positionOffset;

    // Start is called before the first frame update
    void Start()
    {
        //positionOffset = this.transform.position - subject.transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = subject.transform.position + positionOffset; 
    }
}
