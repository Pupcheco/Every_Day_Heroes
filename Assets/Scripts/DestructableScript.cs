using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableScript : MonoBehaviour
{
    public GameObject destroyedVersion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        TriggerDestruction(); 
    }

    void TriggerDestruction()
    {
        Object.Instantiate(destroyedVersion, this.transform.position, this.transform.rotation);
        Object.Destroy(this.gameObject);
    }
}
