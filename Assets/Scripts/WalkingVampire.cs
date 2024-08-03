using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingVampire : AbstractEnemy
{
    protected override void AttackPlayer()
    {
        path.canMove = true;
        path.enableRotation = true;
    }
}
