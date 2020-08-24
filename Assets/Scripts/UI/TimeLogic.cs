using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLogic : MonoBehaviour
{
    public UnityEngine.UI.Text numberShadowText;
    public UnityEngine.UI.Text numberText;

    public int secondsLeft;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var minutes = secondsLeft / 60;
        var seconds = secondsLeft % 60;
        var text = minutes.ToString()  + ":" + seconds.ToString();
        numberShadowText.text = text;    
        numberText.text = text;    
    }
}
