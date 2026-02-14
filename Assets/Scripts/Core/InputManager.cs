using UnityEngine;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{
    [Header("Input Schemes")]
    [SerializeField] private string controlScheme = "KeyboardMouse";
    [SerializeField] private bool invertLookY = false;

    [Header "Key Bindings")]
    [SerializeField] private string moveForward = "w";
    [SerializeField] private string moveBackward = "s";
    [SerializeField] private string moveLeft = "a";
    [SerializeField] private string moveRight = "d";
    [SerializeField] private string jump = "space";
    [SerializeField] private string interact = "e";
    [SerializeField] private string sprint = "left shift";
    [SerializeField] private string crouch = "c";

    [Header "Input Settings"]
    [SerializeField] private float lookSensitivity = 1f;
    [SerializeField] private float smoothLookTime = 0.1f;

    private static InputManager instance;
    public static InputManager Instance => instance;

    private Vector2 mouseInput;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool jumpPressed;
    private bool interactPressed;
    private bool sprintPressed;
    private bool crouchPressed;

    private Dictionary<string, string> keyBindings;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeKeyBindings();
    }

    private void InitializeKeyBindings()
    {
        keyBindings = new Dictionary<string, string>
        {
            {"MoveForward", moveForward},
            {"MoveBackward", moveBackward},
            {"MoveLeft", moveLeft},
            {"MoveRight", moveRight},
            {"Jump", jump},
            {"Interact", interact},
            {"Sprint", sprint},
            {"Crouch", crouch}
        };
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        // Movement input
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // Mouse input
        if (controlScheme == "KeyboardMouse")
        {
            mouseInput.x = Input.GetAxis("Mouse X");
            mouseInput.y = Input.GetAxis("Mouse Y");
        }

        // Action inputs
        jumpPressed = Input.GetButtonDown("Jump");
        interactPressed = Input.GetKeyDown(GetKeyCode(keyBindings["Interact"]));
        sprintPressed = Input.GetKey(GetKeyCode(keyBindings["Sprint"]));
        crouchPressed = Input.GetKeyDown(GetKeyCode(keyBindings["Crouch"]));
    }

    private KeyCode GetKeyCode(string binding)
    {
        switch (binding.ToLower())
        {
            case "w": return KeyCode.W;
            case "s": return KeyCode.S;
            case "a": return KeyCode.A;
            case "d": return KeyCode.D;
            case "space": return KeyCode.Space;
            case "left shift": return KeyCode.LeftShift;
            case "e": return KeyCode.E;
            case "c": return KeyCode.C;
            default: return KeyCode.None;
        }
    }

    #region Public Accessors

    public Vector2 GetMoveInput()
    {
        return moveInput.normalized;
    }

    public Vector2 GetMouseInput()
    {
        if (invertLookY)
        {
            return new Vector2(mouseInput.x, -mouseInput.y);
        }
        return mouseInput;
    }

    public bool GetJumpPressed()
    {
        if (jumpPressed)
        {
            jumpPressed = false;
            return true;
        }
        return false;
    }

    public bool GetInteractPressed()
    {
        if (interactPressed)
        {
            interactPressed = false;
            return true;
        }
        return false;
    }

    public bool GetSpressed()
    {
        return sprintPressed;
    }

    public bool GetCrouchPressed()
    {
        if (crouchPressed)
        {
            crouchPressed = false;
            return true;
        }
        return false;
    }

    #endregion

    #region Settings

    public void SetControlScheme(string scheme)
    {
        controlScheme = scheme;
        PlayerPrefs.SetString("ControlScheme", scheme);
        PlayerPrefs.Save();
    }

    public string GetControlScheme()
    {
        return controlScheme;
    }

    public void SetInvertLookY(bool invert)
    {
        invertLookY = invert;
        PlayerPrefs.SetInt("InvertLookY", invert ? 1 : 0);
        PlayerPrefs.Save();
    }

    public bool GetInvertLookY()
    {
        return invertLookY;
    }

    public void SetLookSensitivity(float sensitivity)
    {
        lookSensitivity = sensitivity;
        PlayerPrefs.SetFloat("LookSensitivity", sensitivity);
        PlayerPrefs.Save();
    }

    public float GetLookSensitivity()
    {
        return lookSensitivity;
    }

    public void SetKeyBinding(string action, string key)
    {
        if (keyBindings.ContainsKey(action))
        {
            keyBindings[action] = key;
        }
    }

    public string GetKeyBinding(string action)
    {
        if (keyBindings.ContainsKey(action))
        {
            return keyBindings[action];
        }
        return "";
    }

    #endregion

    #region UI Input

    public bool GetSubmit()
    {
        return Input.GetButtonDown("Submit") || Input.GetKeyDown(KeyCode.Return);
    }

    public bool GetCancel()
    {
        return Input.GetButtonDown("Cancel") || Input.GetKeyDown(KeyCode.Escape);
    }

    public Vector2 GetUIInput()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    #endregion

    private void OnApplicationQuit()
    {
        SaveSettings();
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetString("ControlScheme", controlScheme);
        PlayerPrefs.SetInt("InvertLookY", invertLookY ? 1 : 0);
        PlayerPrefs.SetFloat("LookSensitivity", lookSensitivity);

        // Save key bindings
        foreach (var binding in keyBindings)
        {
            PlayerPrefs.SetString(binding.Key, binding.Value);
        }

        PlayerPrefs.Save();
    }
}