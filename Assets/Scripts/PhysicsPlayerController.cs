using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsPlayerController : MonoBehaviour
{
    public float standupTorque;

    public float speed;
    public float jumpSpeed;

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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        standupOrientation = this.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
        moveJumped = Input.GetKeyDown(KeyCode.Space) ? 0 : 1;

        currentSpeed = speed;

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * autoCorrectHeight, Color.blue);

        //added layermask for those dealing with complex ground objects.
        if (Physics.Raycast(
                transform.position,
                transform.TransformDirection(Vector3.down),
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
            transform.TransformDirection(
                moveHorizontal,
                0f,
                moveVertical));

        // Jump Force
        rb.AddForce(
            new Vector3(0f, moveJumped * jumpSpeed, 0f) * Time.deltaTime, ForceMode.Force);

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
}
