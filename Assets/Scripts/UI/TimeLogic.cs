using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLogic : MonoBehaviour
{
    public UnityEngine.UI.Text numberShadowText;
    public UnityEngine.UI.Text numberText;
    
    void Update() {
        var minutes = (int)(Time.time / 60);
        var minutesText = "";
        if (minutes < 10) {
            minutesText = "0" + minutes.ToString();
        } else {
            minutesText = minutes.ToString();
        }

        var seconds = (int)(Time.time % 60);
        var secondsText = "";
        if (seconds < 10) {
            secondsText = "0" + seconds.ToString();
        } else {
            secondsText = seconds.ToString();
        }
        
        var text = minutesText + ":" + secondsText;
        numberShadowText.text = text;    
        numberText.text = text;
    }
}
