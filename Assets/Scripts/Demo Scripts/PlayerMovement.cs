using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

  [Header("Walk / Run Setting")] public float walkSpeed;
  public float runSpeed;

  [Header("Jump Settings")] public float playerJumpForce;
  public ForceMode appliedForceMode;

  [Header("Jumping State")] public bool playerIsJumping;

  [Header("Current Player Speed")] public float currentSpeed;

  [Space] public float timeBetweenDamage = 0.5f;

  public float FollowerSpeedDecay = 0.985f;

  private float _xAxis;
  private float _zAxis;
  private Rigidbody _rb;
  private RaycastHit _hit;
  private Vector3 _groundLocation;
  private bool _isShiftPressedDown;
  private float _nextDamageTime = -10f;
  private void Start()
  {
    _rb = GetComponent<Rigidbody>();
  }

  private void Update()
  {
    _xAxis = Input.GetAxis("Horizontal");
    _zAxis = Input.GetAxis("Vertical");

    // Calculate Player Speed 
    currentSpeed = _isShiftPressedDown ? runSpeed : walkSpeed;
    CalculateFollowerToPlayerSpeed();

    // Find Player Jump Status 
    playerIsJumping = Input.GetButton("Jump");


    // Disable Multiple Jumping
    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 10f, Color.blue);
    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out _hit,
        Mathf.Infinity))
    {
      if (string.Compare(_hit.collider.tag, "Ground", System.StringComparison.Ordinal) == 0)
      {
        _groundLocation = _hit.point;
      }

      var distanceFromPlayerToGround = Vector3.Distance(transform.position, _groundLocation);
      if (distanceFromPlayerToGround > 1f)
        playerIsJumping = false;
    }

  }

  private void FixedUpdate()
  {
    //Move Player
    _rb.MovePosition(transform.position + Time.deltaTime * currentSpeed *
                     transform.TransformDirection(_xAxis, 0f, _zAxis));

    // Player Jump 
    if (playerIsJumping)
      PlayerJump(playerJumpForce, appliedForceMode);
  }

  private void OnGUI()
  {
    _isShiftPressedDown = Input.GetButton("Run");
  }

  private void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.layer == 10 && Time.time > _nextDamageTime)
    {
      var count = 1;  //TODO(Rastal): This actually needs to be calculated based on the impact.
      FollowerManager.LoseFollowers(count);
      _nextDamageTime = Time.time + timeBetweenDamage;
    }
  }

  private void PlayerJump(float jumpForce, ForceMode forceMode)
  {
    _rb.AddForce(jumpForce * _rb.mass * Time.deltaTime * Vector3.up, forceMode);
  }

  private void CalculateFollowerToPlayerSpeed()
  {
    currentSpeed *= Mathf.Pow(FollowerSpeedDecay, FollowerManager.Followers.Count);
  }

}