using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeScript : MonoBehaviour
{
    public AudioClip deathClip;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AbstractEnemy enemy = collision.gameObject.GetComponent<AbstractEnemy>();
        if(enemy != null)
        {
            enemy.deathClip = deathClip;
            enemy.TakeDamage(99);
        }
    }
}
