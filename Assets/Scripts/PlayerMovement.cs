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

  [Header("Current Player Speed")] public Transform followPoint;
  public float FollowerSpeedDecay = 0.985f;

  private float _xAxis;
  private float _zAxis;
  private Rigidbody _rb;
  private Animator _animator;
  private SpriteRenderer _sprite;
  private RaycastHit _hit;
  private Vector3 _groundLocation;
  private bool _isShiftPressedDown;
  private float _nextDamageTime = -10f;
  private int jumpCounter = 0;
  private void Start()
  {
    _rb = GetComponent<Rigidbody>();
    _animator = GetComponentInChildren<Animator>();
    _sprite = GetComponentInChildren<SpriteRenderer>();
    _rb.isKinematic = true;
    _rb.isKinematic = false;
  }

  private void Update()
  {
    _xAxis = Input.GetAxis("Horizontal");
    _zAxis = Input.GetAxis("Vertical");

    // Calculate Player Speed 
    currentSpeed = _isShiftPressedDown ? runSpeed : walkSpeed;
    CalculateFollowerToPlayerSpeed();

    // Find Player Jump Status 
    playerIsJumping = Input.GetButtonDown("Jump");

    //TODO (shaux): ROTATE FollowPoint around the player so that it is always behind the player depending on the move velocity
    // Vector3 movement = new Vector3(_xAxis, 0.0f, _zAxis);
    // followPoint.rotation = Quaternion.RotateTowards(movement);
    // followPoint.Translate(movement * currentSpeed * Time.deltaTime, Space.World);

    // Disable Multiple Jumping
    // Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 10f, Color.blue);
    // if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out _hit,
    //     Mathf.Infinity))
    // {
    //   if (string.Compare(_hit.collider.tag, "Ground", System.StringComparison.Ordinal) == 0)
    //   {
    //     _groundLocation = _hit.point;
    //   }

    //   float distanceFromPlayerToGround = Vector3.Distance(transform.position, _groundLocation);
    //   Debug.Log("dist" + distanceFromPlayerToGround);
    //   if (distanceFromPlayerToGround > 0.1f)
    //   {
    //     playerIsJumping = false;
    //   }
    // }

    // Rotate player object (added by Rastal)
    var moveInput = new Vector2(_xAxis, _zAxis);
    if (moveInput.magnitude > 0.1f)
    {
      var angle = Vector2.SignedAngle(Vector2.up, moveInput);
      this.transform.rotation = Quaternion.Euler(0f, angle * 1, 0f);
    }

    // Animator stuff
    var moveSideways = false;
    if (_xAxis < -0.15f || _xAxis > 0.15f)
    {
      moveSideways = true;
    }
    if (_xAxis < -0.15f && moveSideways)
    {
      _sprite.flipX = true;
    }
    else
    {
      _sprite.flipX = false;
    }
    _animator.SetBool("MoveSideways", moveSideways);
    _animator.SetFloat("MoveZ", _zAxis);
  }

  private void FixedUpdate()
  {
    //Move Player
    _rb.MovePosition(transform.position + Time.deltaTime * currentSpeed * new Vector3(_xAxis, 0f, _zAxis));

    Debug.Log(jumpCounter);
    // Player Jump 
    if (playerIsJumping && jumpCounter == 0)
    {
      PlayerJump(playerJumpForce, appliedForceMode);
      jumpCounter++;
    }
  }

  private void OnGUI()
  {
    _isShiftPressedDown = Input.GetButton("Run");
  }

  private void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.layer == 10 && Time.time > _nextDamageTime &&
        collision.impulse.magnitude > 30f && FollowerManager.Followers.Count > 0)
    {
      var count = 1;  //TODO(Rastal): This actually needs to be calculated based on the impact.
      FollowerManager.LoseFollowers(count);
      _nextDamageTime = Time.time + timeBetweenDamage;
    }

    if (collision.gameObject.layer == 9 && jumpCounter != 0)
    {
      jumpCounter = 0;
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