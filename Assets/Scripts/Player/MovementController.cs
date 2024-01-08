using System.Collections;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static CameraSwitch;
using static Stamina;
using static LadderPosition;

// [RequireComponent(typeof(CharacterController))]
public class MovementController : MonoBehaviour
{
    public static MovementController movementInstance;
    private void Awake() => movementInstance = this;
    public GameObject player;
    float rotationX = 0;
    public bool canMove = true;
    public CharacterController characterController;
    public float moveSpeed = 25.0f;
    public float sprintSpeed = 75.0f;
    private Vector2 movement;
    public float gravity = -9.81f;
    public Vector3 velocity;
    public float jumpPower = 5f;
    public bool isJumping = false;
    private Vector3 direction;
    public float climbSpeed;
    public bool isOnLadder = false;
    public bool isSliding=false;
    public float groundCheckDistance = 0.01f;
    public LayerMask groundLayer;
    public float ladderSprintMultiplier = 1.5f;
    public bool ctrlPressed = false; // A flag to track if the Ctrl key is pressed

    // Camera rotation variables
    private Vector2 _inputLook;
    private bool LockCameraPosition = false;
    private float _cinemachineTargetYaw = 0f;
    private float _cinemachineTargetPitch = 0f;
    private const bool IsCurrentDeviceMouse = true;
    private float BottomClamp = -5f;
    private float TopClamp = 60f;
    public Transform CinemachineCameraTarget;
    private float ladderHorizontalRotationLimit = 35f; // Define the rotation limit
    private float currentLadderYaw = 0f; // A variable to track the current Yaw rotation

    void Start()
    {
        player.SetActive(false);
        player.SetActive(true);
        characterController = GetComponent<CharacterController>();
        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
        Cursor.visible = false;
        //Debug.Log(PlayerPrefs.GetInt("LoadSavedGame", 0));
        if (PlayerPrefs.GetInt("LoadSavedGame", 0) == 1)
        {
            Load();
        }
    }

    void Update()
    {
        if (isOnLadder)
            LadderMovement();
    }

    private void LateUpdate()
    {
        
        if (!isOnLadder)
            FppCameraRotationNormal();
        else
            FppCameraRotationLadder();
    }

    IEnumerator Jump()
    {
        yield return new WaitForSeconds(0.2f);
        isJumping = false;
    }

    public void JumpOnLadder()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && staminaInstance.stamina >= 33f)
        {
            Debug.Log("tried to jump");
            isJumping = true;
            StartCoroutine(Jump());
            staminaInstance.stamina -= 33f;
            direction = new Vector3(0, 150, 0);
            characterController.Move(transform.TransformDirection(direction) * (climbSpeed * Time.deltaTime));
        }
    }

    public void SlideOnLadder()
    {
        
        if (Input.GetKey(KeyCode.LeftControl))
        {
            isSliding=true;
            ctrlPressed = true;
            velocity.y -= 0.02f;
            velocity.y += gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
            staminaInstance.grip -= 0.05f;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isSliding=false;
            ctrlPressed = false;
            velocity.y = 0f;
        }
    }

    // public void NormalMovement()
    // {
    //     if (IsCharacterGrounded())
    //     {
    //         velocity.y = -0.02f; // Small downward force to ensure character stays on the ground
    //         if (Input.GetKey(KeyCode.Space))
    //         {
    //             isJumping = true;
    //             StartCoroutine(Jump());
    //             velocity.y = jumpPower;
    //             velocity.y += gravity * Time.deltaTime;
    //         }
    //     }
    //     else
    //     {
    //         if (isJumping == false)
    //         {
    //             velocity.y -= 0.06f;
    //             // Apply gravity to the velocity
    //             // Since gravity is an acceleration, it affects the velocity over time, not instantaneously.
    //             velocity.y += gravity * Time.deltaTime;
    //         }
    //     }
    //
    //     if (isJumping)
    //     {
    //         velocity.y += 0.1f;
    //         velocity.y += gravity * Time.deltaTime;
    //     }
    //
    //     // Move the character with the accumulated velocity
    //     characterController.Move(velocity * Time.deltaTime);
    //
    //     // Check for sprint input (Shift key).
    //     float currentSpeed = moveSpeed;
    //     if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W))
    //         if (Input.GetKey(KeyCode.LeftShift))
    //         {
    //             currentSpeed = sprintSpeed;
    //             staminaInstance.stamina -= 10f * Time.deltaTime;
    //         }
    //         else
    //         {
    //             currentSpeed = moveSpeed;
    //             staminaInstance.stamina -= 4f * Time.deltaTime;
    //         }
    //
    //     movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    //     Vector3 direction = new Vector3(movement.x, 0, movement.y).normalized;
    //
    //     if (canMove && direction.magnitude >= 0.1f)
    //     {
    //         characterController.Move(transform.TransformDirection(direction) * currentSpeed * Time.deltaTime);
    //     }
    //
    //     transform.rotation = Quaternion.Euler(0, _cinemachineTargetYaw, 0);
    //
    //     BottomClamp = -5f;
    // }

    public void LadderMovement()
    {
        
        if (!canMove)
            return;
        float vertInput = Input.GetAxis("Vertical");
        direction = new Vector3(0, vertInput, 0);
        // TMP_SpriteAnimator.SetFloat("ClimbSpeed", Mathf.Abs(vertInput));
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W))
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                characterController.Move(transform.TransformDirection(direction) * (climbSpeed * Time.deltaTime));
                ladderPosition.PositionHandler();
                staminaInstance.stamina -= 5f * Time.deltaTime;
            }
            else
            {
                characterController.Move(transform.TransformDirection(direction) *
                                         (climbSpeed * Time.deltaTime * ladderSprintMultiplier));
                ladderPosition.PositionHandler();
                staminaInstance.stamina -= 10f * Time.deltaTime * ladderSprintMultiplier;
            }
        else staminaInstance.stamina -= 0.1f * Time.deltaTime * ladderSprintMultiplier;

        JumpOnLadder();
        SlideOnLadder();

        BottomClamp = -60f;
    }

    private void FppCameraRotationNormal()
    {
        rotationX += -Input.GetAxis("Mouse Y");
        rotationX = Mathf.Clamp(rotationX, -90f, 45f);
        cameraInstance.playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }

    private void FppCameraRotationLadder()
    {
        // Handling vertical rotation (same as before)
        rotationX += -Input.GetAxis("Mouse Y");
        rotationX = Mathf.Clamp(rotationX, -90f, 45f);

        // Handling horizontal rotation
        currentLadderYaw += Input.GetAxis("Mouse X");
        currentLadderYaw = Mathf.Clamp(currentLadderYaw, -ladderHorizontalRotationLimit, ladderHorizontalRotationLimit);

        // Apply the rotations
        cameraInstance.playerCamera.transform.localRotation = Quaternion.Euler(rotationX, currentLadderYaw, 0);
    }

    // private bool IsCharacterGrounded()
    // {
    //     // Cast a ray downward to check for ground contact with tolerance
    //     Vector3 origin = transform.position + characterController.center;
    //     Vector3 offset1 = new Vector3(0, 0, 1f);
    //     Vector3 frontCorner = transform.position + characterController.center + offset1;
    //     Vector3 offset2 = new Vector3(0, 0, -1f);
    //     Vector3 backCorner = transform.position + characterController.center + offset2;
    //     Vector3 offset3 = new Vector3(1f, 0, 0);
    //     Vector3 rightCorner = transform.position + characterController.center + offset3;
    //     Vector3 offset4 = new Vector3(-1f, 0, 0);
    //     Vector3 leftCorner = transform.position + characterController.center + offset4;
    //     float radius = 0.4f;
    //     float distance = groundCheckDistance + radius; // Add the radius as tolerance
    //
    //     if (Physics.SphereCast(origin, radius, Vector3.down, out RaycastHit hitInfo, distance, groundLayer) ||
    //         Physics.SphereCast(frontCorner, radius, Vector3.down, out RaycastHit hitInfo1, distance, groundLayer) ||
    //         Physics.SphereCast(backCorner, radius, Vector3.down, out RaycastHit hitInfo2, distance, groundLayer) ||
    //         Physics.SphereCast(rightCorner, radius, Vector3.down, out RaycastHit hitInfo3, distance, groundLayer) ||
    //         Physics.SphereCast(leftCorner, radius, Vector3.down, out RaycastHit hitInfo4, distance, groundLayer))
    //     {
    //         return true;
    //     }
    //     else return false;
    // }

    public void Load()
    {
        Data data = SaveSystem.LoadData();
        Debug.Log(data.xPosition);
        Debug.Log(data.yPosition);
        Debug.Log(data.zPosition);
        if (data != null)
        {
            for (int i = 1; i <= data.level; i++)
            {
                string objectName = "Level" + i;
                GameObject objectToDestroy = GameObject.Find(objectName);
                if (objectToDestroy != null)
                {
                    Destroy(objectToDestroy);
                }
            }

            StartCoroutine(LoadWait(data));
        }

        PlayerPrefs.SetInt("LoadSavedGame", 0);
    }

    IEnumerator LoadWait(Data data)
    {
        yield return new WaitForSeconds(0.1f);
        Vector3 savedPosition = new Vector3(data.xPosition, data.yPosition, data.zPosition);
        characterController.enabled = false;
        transform.position = savedPosition;
        characterController.enabled = true;
        Levels.Instance.setLevel(data.level);
        isOnLadder = true;
        LadderMovement();
    }
}