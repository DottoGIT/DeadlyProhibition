using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform Player;
    [SerializeField] Transform pointer;
    [SerializeField] float cameraSpeed;
    [SerializeField] float cameraLookoutSpeed;
    static bool LookoutModeOn = false;


    void FixedUpdate()
    {
        Vector3 desiredPosition;
        if (LookoutModeOn == false)
        {
            desiredPosition = new Vector3(Player.position.x, Player.position.y, -10);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, cameraSpeed);
        }
        else
        {
            desiredPosition = new Vector3((Player.position.x + pointer.position.x)/2, (Player.position.y + pointer.position.y) / 2, -10);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, cameraLookoutSpeed);
        }
    }

    public static void DisableLookout()
    {
        LookoutModeOn = false;
    }

    public static void EnableLookout()
    {
        LookoutModeOn = true;
    }
}
