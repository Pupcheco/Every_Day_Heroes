using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedLogic : MonoBehaviour
{
    public UnityEngine.UI.Text numberShadowText;
    public UnityEngine.UI.Text numberText;

    public GameLogic gameLogic;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var text = gameLogic.Score.ToString();
        numberShadowText.text = text;
        numberText.text = text;    
    }
}
