using UnityEngine;

public class testCamera : MonoBehaviour
{
    public float MouseSensitivity = 3;
    public Camera shoulderCamera;
    public Transform cameraPivot;
    float xRotation = 0f;
    public float WalkSpeed = 10;
    Vector3 move;
    private void Start()
    {
        shoulderCamera.transform.localRotation = Quaternion.identity;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        playerMove();
        cameraMove();
    }

    void playerMove()
    {
        if (WalkSpeed > 0)
        {
            //My temp velocity variable


            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            //transform.forward/right are relative to the direction my body is facing
            Vector3 move = transform.forward * z + transform.right * x;
            //I reduce my total movement to 1 and then multiply it by my speed
            move = move * WalkSpeed;

            /* //If I hit jump and am on the ground, I jump
             if (JumpPower > 0 && Input.GetKeyDown(KeyCode.Space) && OnGround())
                 move.y = JumpPower;
             else  //Otherwise, my Y velocity is whatever it was last frame
                 move.y = RB.linearVelocity.y;

             //Plug my calculated velocity into the rigidbody
             RB.linearVelocity = move;

             float speed = new Vector2(x, z).magnitude;
             animator.SetFloat("speed", speed);*/
        }
    }
    void cameraMove()
    {

        //If my mouse goes left/right my body moves left/right
        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
        float mouseY = -Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;



        transform.Rotate(0, mouseX, 0);

        xRotation += mouseY * MouseSensitivity / 2 * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -30, 30);

        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        shoulderCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //arm_right.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
