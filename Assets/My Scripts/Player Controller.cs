using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;

    public Rigidbody rb;
    public Camera camera;

    public float MouseSensitivity = 5f;
    private float xRotation = 0f;

    public float jumpForce = 6f;
    private int jumpCount = 0;

    public float KillRange = 1.5f;
    private HumanNPC targetHuman;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Physics.gravity = new Vector3(0, -20f, 0);
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CameraMove();
        Kill();
    }

    void Move()
    {
        float x = Input.GetAxis("Vertical");
        float z = Input.GetAxis("Horizontal");

        Vector3 move = transform.forward * x + transform.right * z;
        move *= speed;

        rb.linearVelocity = new Vector3(move.x,rb.linearVelocity.y,move.z);
        Jump();
    }

    void CameraMove()
    {
        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity;
        float mouseY = -Input.GetAxis("Mouse Y") * MouseSensitivity;

        // Rotate the player horizontally
        transform.Rotate(0f, mouseX, 0f);

        // Rotate the camera vertically
        xRotation += mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2)
        {
            rb.linearVelocity = new Vector3(
                rb.linearVelocity.x,
                0f,
                rb.linearVelocity.z
            );
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpCount++;

            if (jumpCount >= 1)
            {
                jumpForce = 8f;
            }
            else
            {
                jumpForce = 6f;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Player touched the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
        }
    }

    public void Kill()
    {
        Collider[] nearbyHumans = Physics.OverlapSphere(transform.position, KillRange);

        targetHuman = null;

        float closestDistance = KillRange;

        foreach (Collider col in nearbyHumans)
        {
            HumanNPC human = col.GetComponentInParent<HumanNPC>();

            if (human == null || !human.isAlive)
            {
                continue;
            }

            if (!human.CompareTag("Human"))
            {
                Debug.Log("HumanNPC found, but tag is not Human: " + human.name);
                continue;
            }

            float currentDistance = Vector3.Distance(
                transform.position,
                col.ClosestPoint(transform.position)
            );

            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                targetHuman = human;
            }
        }

        // Press E after finding a valid target.
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (targetHuman != null)
            {
                targetHuman.Die();
                Debug.Log("Human Killed: " + targetHuman.name);
            }
            else
            {
                Debug.Log("No living human in kill range!");
            }
        }
    }
}
