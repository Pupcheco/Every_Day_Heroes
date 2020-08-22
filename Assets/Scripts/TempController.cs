using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempController : MonoBehaviour {

    CharacterController _controller;

    void OnEnable() {
        _controller = this.GetComponent<CharacterController>();
    }

    void Update() {
        var moveX = Input.GetAxis("Horizontal");
        var moveY = Input.GetAxis("Vertical");
        _controller.Move(new Vector3(moveX, 0f, moveY) * Time.deltaTime * 5f);
    }
}
