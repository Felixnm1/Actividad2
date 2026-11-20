using UnityEngine;
using UnityEngine.InputSystem;

public class FPS_Controller : MonoBehaviour
{
    
    [SerializeField] private float velocity;
    [SerializeField] private float runVelocityMultiplier;
    [SerializeField] private float mouseSensivity;
    [SerializeField] private Transform cam;

    [Header("Controls")]
    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference run;
    [SerializeField] private InputActionReference jump;
    [SerializeField] private InputActionReference moveCamera;

    [Header("Detección de suelo")]
    private bool isGrounded;
    private bool isWatered;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float radioGroundCheck = 0.2f;
    [SerializeField] private LayerMask capaSuelo;
    [SerializeField] private LayerMask capaAgua;


    private Vector3 verticalVelocity;

    
    private float inputH;
    private float inputV;

    [Header("Salto")]
    
    //[SerializeField] private float jumpForce = 8f;
    [SerializeField] private float jumpHeigth = 3f;
    public float gravityFactor;

    private CharacterController charCon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        charCon = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private float rotation = 0f;
    // Update is called once per frame
    void Update()
    {
        Vector2 inputVector = move.action.ReadValue<Vector2>();
        inputH = inputVector.x;
        inputV = inputVector.y;

        Vector3 dirMov = transform.forward * inputV + transform.right * inputH;
        dirMov.y = 0;

        charCon.Move(dirMov.normalized * velocity * Time.deltaTime);
        CheckWater();


        dirMov.Normalize();
        Vector3 movement = dirMov * velocity * Time.deltaTime;
        if (run.action.IsPressed() && !isWatered)
        {
            movement = movement * runVelocityMultiplier;
            
        }
            

        charCon.Move(movement);

        if (jump.action.WasPressedThisFrame() && isGrounded && !isWatered)
        {

            Jump();

                
        }
        CheckGround();


        //mouse movement reading
        Vector2 mouseInputVector = moveCamera.action.ReadValue<Vector2>() * mouseSensivity * Time.deltaTime;
        rotation -= mouseInputVector.y;
        rotation = Mathf.Clamp(rotation, -90, 90);
        cam.transform.localRotation = Quaternion.Euler(rotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseInputVector.x);

    }
    private void CheckGround() {

        isGrounded = Physics.CheckSphere(groundCheck.position, radioGroundCheck, capaSuelo);
        ApplyGravity();
    }
    private void CheckWater()
    {

        isWatered =  Physics.CheckSphere(groundCheck.position, radioGroundCheck, capaAgua);
    }

    private void Jump() {

        float jumpForce = Mathf.Sqrt(2f * gravityFactor * jumpHeigth);
        verticalVelocity.y = jumpForce;
        charCon.Move(Vector3.up * verticalVelocity.y * Time.deltaTime);
    }

    private void ApplyGravity() {

        verticalVelocity.y -= gravityFactor * Time.deltaTime;
        charCon.Move(Vector3.up * verticalVelocity.y * Time.deltaTime);
    }

}
