using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCommands : MonoBehaviour
{

    public void EndGame()
    {
        GameManager.instance.KillPlayer();
    }

}
