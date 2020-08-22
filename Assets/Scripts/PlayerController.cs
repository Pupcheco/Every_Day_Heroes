using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public CharacterController controller;
  public float smoothingTime = 0.1f;
  private float smoothingVelocity;
  public float moveSpeed = 6f;
  public Transform cam;
  public float gravity = -9.81f;
  public float jumpHeight = 3;
  private Vector3 velocity;
  private bool isGrounded;
  public Transform groundCheck;
  public float groundDistance = 0.4f;
  public LayerMask groundLayer;
  
  private void FixedUpdate()
  {
    float horizontalInput = Input.GetAxisRaw("Horizontal");
    float verticalInput = Input.GetAxisRaw("Vertical");

    // Calculate Player Direction 
    Vector3 dir = new Vector3(horizontalInput, 0f, verticalInput).normalized;

    if (dir.magnitude >= 0.01f)
    {
      // Calculate Player Rotation
      float targetRotation = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
      // Smooth Player Rotation
      float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref smoothingVelocity, smoothingTime);
      transform.rotation = Quaternion.Euler(0, rotation, 0f);
      // Adjust Player Movement To Camera Orientation 
      Vector3 moveDir = Quaternion.Euler(0f, targetRotation, 0f) * Vector3.forward;
      controller.Move(moveDir.normalized * moveSpeed * Time.deltaTime);

      // Check Whether Player Is On Ground
      isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);
      if (isGrounded && velocity.y < 0)
      {
        velocity.y = -2f;
      }
      // Player Jump 
      if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
      {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
      }
      velocity.y += gravity * Time.deltaTime;
      controller.Move(velocity * Time.deltaTime);
    }

  }

}
