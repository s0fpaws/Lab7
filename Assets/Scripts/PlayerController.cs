using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // -- Components --
    private Transform playerTx;
    private CharacterController controller;

    public Transform cameraTx;

    [Header("Inputs")]
    public string axisLookHorzizontal = "Mouse X";
    public string axisLookVertical = "Mouse Y";
    private string axisMoveHorizontal = "Horizontal";
    private string axisMoveVertical = "Vertical";

    // -- Input Variables --
    [HideInInspector] public float inputLookX = 0;
    [HideInInspector] public float inputLookY = 0;
    [HideInInspector] public float inputMoveX;
    [HideInInspector] public float inputMoveY;
    [HideInInspector] public bool inputKeyDownCursor = false;
    public KeyCode keyToggleCursor = KeyCode.BackQuote;

    [Header("Look Settings")]
    public float mouseSensitivityX = 2f;
    public float mouseSensitivityY = 2f;
    public float mouseSnappiness = 20f;
    public float clampLookY = 90f;

    [Header("Move Settings")]
    public float speed = 10f;
    public float gravity = -9.81f;
    private Vector3 lastPos = Vector3.zero;

    [Header("Reference Variables")]
    public float xRotation = -12.33f;
    private Vector3 fauxGravity = Vector3.zero;
    private float accMouseX = 0;
    private float accMouseY = 0;
    public bool cursorActive = false;

    void Start()
    {
        Initialize();
        RefreshCursor();
    }

    void Initialize()
    {
        playerTx = transform;
        lastPos = playerTx.position;
        controller = GetComponent<CharacterController>();
        fauxGravity = Vector3.up * gravity;
        cameraTx.localPosition = new Vector3(0f, 2.88f, -4.66f);
    }

    void Update()
    {
        //cameraTx.position = new Vector3(0, cameraTx.position.y, cameraTx.position.z);
        Inputs();
        Look();
        Movement();
        FallenDown();
    }

    void Inputs()
    {
        inputKeyDownCursor = Input.GetKeyDown(keyToggleCursor);
        inputLookX = Input.GetAxis(axisLookHorzizontal);
        inputLookY = Input.GetAxis(axisLookVertical);
        if (inputKeyDownCursor)
        {
            ToggleLockCursor();
        }
    }

    void ToggleLockCursor()
    {
        cursorActive = !cursorActive;
        RefreshCursor();
    }

    void RefreshCursor()
    {
        if (!cursorActive && Cursor.lockState != CursorLockMode.Locked) { Cursor.lockState = CursorLockMode.Locked; }
        if (cursorActive && Cursor.lockState != CursorLockMode.None) { Cursor.lockState = CursorLockMode.None; }
    }

    void Look()
    {
        accMouseX = Mathf.Lerp(accMouseX, inputLookX, mouseSnappiness * Time.deltaTime);
        accMouseY = Mathf.Lerp(accMouseY, inputLookY, mouseSnappiness * Time.deltaTime);

        float mouseX = accMouseX * mouseSensitivityX * 100f * Time.deltaTime;
        float mouseY = accMouseY * mouseSensitivityY * 100f * Time.deltaTime;

        // rotate camera X
        xRotation += -mouseY;
        xRotation = Mathf.Clamp(xRotation, -clampLookY, clampLookY);

        cameraTx.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        //cameraTx.localPosition = new Vector3(0f, 2.88f, -4.66f);

        if (xRotation + 12.33f >= 12.33f)
        {
            //cameraTx.localPosition = new Vector3(0f, cameraTx.localPosition.y, Mathf.Lerp(-4.66f, 0.5f, xRotation/90));
            cameraTx.localPosition = new Vector3(0f, Mathf.Lerp(1.8f, 4f, xRotation / 90), Mathf.Lerp(-4.66f, 0.5f, xRotation / 90));
        }
        else if (xRotation + 12.33f < 12.33f)
        {
            cameraTx.localPosition = new Vector3(0f, Mathf.Lerp(1.8f, -1.2f, xRotation / -90), Mathf.Lerp(-4.66f, -0.5f, xRotation / -90));
        }

        //cameraTx.position = new Vector3(0, cameraTx.position.y, cameraTx.position.z);
        // rotate player Y
        playerTx.Rotate(Vector3.up * mouseX);
    }

    void Movement()
    {
        inputMoveX = Input.GetAxis(axisMoveHorizontal);
        inputMoveY = Input.GetAxis(axisMoveVertical);

        Vector3 calc;
        Vector3 move;

        float curSpeed = (playerTx.position - lastPos).magnitude / Time.deltaTime;
        curSpeed = (curSpeed < 0 ? 0 - curSpeed : curSpeed);
        move = (playerTx.right * inputMoveX) + (playerTx.forward * inputMoveY);

        if (move.magnitude > 1f)
        {
            move = move.normalized;
        }
        if (fauxGravity.y < 0)
        {
            fauxGravity.y = Mathf.Lerp(fauxGravity.y, gravity, 1.5f * Time.deltaTime);
        }
        fauxGravity.y += gravity * Time.deltaTime;

        calc = move * speed * Time.deltaTime;
        calc += fauxGravity * Time.deltaTime;

        controller.Move(calc);
    }

    void FallenDown()
    {
        if (playerTx.position.y < -10)
        {
            playerTx.position = new Vector3(5, 1.3f, 5);
        }
    }
}
