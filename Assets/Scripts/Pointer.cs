using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    private void Update()
    {
        Vector3 cameraPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        transform.position = new Vector3(cameraPos.x, cameraPos.y, 0);
    }
}
