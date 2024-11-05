using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private float dragIntensity = 0.1f;  // Drag intensity
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private Transform cameraTransform;

    private bool isOnGround;
    private float xRotation = 0f;
    private Rigidbody rb;
    private Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isOnGround = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Mouse input for looking around
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        float xMove = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical");

        moveDirection = (transform.right * xMove + transform.forward * zMove).normalized;
        rb.AddForce(moveDirection * speedMultiplier * Time.deltaTime);

        // Apply drag
        Vector3 dragForce = -new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z) * dragIntensity;
        rb.AddForce(dragForce);

        rb.linearVelocity = new Vector3(Mathf.Clamp(rb.linearVelocity.x, -100000 * Time.deltaTime, 100000 * Time.deltaTime), rb.linearVelocity.y, Mathf.Clamp(rb.linearVelocity.z, -100000 * Time.deltaTime, 100000 * Time.deltaTime));

        // Jumping
        if (isOnGround && Input.GetKeyDown(jumpKey))
        {
            rb.AddForce(Vector3.up * jumpMultiplier, ForceMode.Impulse);
            isOnGround = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = false;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }
}
