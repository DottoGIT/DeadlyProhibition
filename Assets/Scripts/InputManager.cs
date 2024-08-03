using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance { get; private set; }
    public InputActions inputActions;
    UIManager uiManag;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        inputActions = new InputActions();
        inputActions.Player.Enable();
        inputActions.Player.Lookout.performed += Lookout_performed;
        inputActions.Player.Lookout.canceled += Lookout_canceled;
        inputActions.Player.Pause.performed += Pause_performed;
    }

    private void Start()
    {
        uiManag = GetComponent<UIManager>();
    }

    private void Pause_performed(InputAction.CallbackContext obj)
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
            uiManag.PauseGameButtonClicked();
    }

    private void Lookout_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
            CameraFollow.DisableLookout();
    }

    private void Lookout_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
            CameraFollow.EnableLookout();
    }
}
