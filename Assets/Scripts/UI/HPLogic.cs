﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPLogic : MonoBehaviour
{
    public UnityEngine.UI.Text numberText;
    public UnityEngine.UI.Slider slider;

    private int currentHp;
    public int maxHp;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentHp = FollowerManager.Followers.Count;
        numberText.text = currentHp.ToString();
        slider.value = currentHp / ((float) maxHp);
    }
}
