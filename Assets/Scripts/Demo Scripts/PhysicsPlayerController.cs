using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsPlayerController : MonoBehaviour
{
  public float standupTorque;
  public float speed;
  public float jumpSpeed;
  public float followerToSpeedDecay = 0.985f;

  [Header("Vertical Auto Correction Height")] public float autoCorrectHeight;
  [Header("Ground Layer Mask")] public LayerMask groundLayerMask;

  private Rigidbody rb;
  private float moveHorizontal;
  private float moveVertical;
  private float moveJumped;
  private float currentSpeed;
  private Quaternion standupOrientation;

  private RaycastHit m_hit;
  private Vector3 m_groundLocation;
  private float m_distanceFromPlayerToGround;

  void Start()
  {
    rb = GetComponent<Rigidbody>();
    standupOrientation = this.transform.rotation;
  }

  void Update()
  {
    moveHorizontal = Input.GetAxis("Horizontal");
    moveVertical = Input.GetAxis("Vertical");
    moveJumped = Input.GetKeyDown(KeyCode.Space) ? 0 : 1;

    currentSpeed = CalculateSpeed();

    Debug.DrawRay(transform.position, Vector3.down * autoCorrectHeight, Color.blue);

    //added layermask for those dealing with complex ground objects.
    if (Physics.Raycast(
            transform.position,
            Vector3.down,
            out m_hit,
            autoCorrectHeight,
            groundLayerMask))
    {
      m_groundLocation = m_hit.point;
      m_distanceFromPlayerToGround = transform.position.y - m_groundLocation.y;
    }

  }

  void FixedUpdate()
  {
    // Move position
    rb.MovePosition(transform.position +
        Time.deltaTime *
        currentSpeed *
        //transform.TransformDirection(
          new Vector3(
            moveHorizontal,
            0f,
            moveVertical));

    // Jump Force
    if (Input.GetKeyDown(KeyCode.Space))
    {
      rb.AddForce(
new Vector3(0f, moveJumped * jumpSpeed, 0f) * Time.deltaTime, ForceMode.Force);
    }


    // Standup torque
    var standupRotation = standupOrientation * Quaternion.Inverse(this.transform.rotation);
    rb.AddRelativeTorque(
        standupRotation.x * standupTorque,
        standupRotation.y * standupTorque,
        standupRotation.z * standupTorque,
        ForceMode.Force);
  }

  void OnMouseDown()
  {
    rb.AddForce(100.0f, 100.0f, 100.0f, ForceMode.Acceleration);
  }

  // Calculate Player To Follower Speed Ratio
  private float CalculateSpeed()
  {
    return speed * Mathf.Pow(followerToSpeedDecay, FollowerManager.Followers.Count);
  }
}

