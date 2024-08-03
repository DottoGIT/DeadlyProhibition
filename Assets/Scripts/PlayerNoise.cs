using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNoise : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] CircleCollider2D coll;
    [SerializeField] float walkingDetectionRange;
    [SerializeField] float shootingDetectionRange;

    private void Update()
    {
        if(player.isShooting)
        {
            coll.radius = shootingDetectionRange;
        }
        else
        {
            coll.radius = walkingDetectionRange;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        AbstractEnemy enemy = collision.gameObject.GetComponent<AbstractEnemy>();
        if (enemy != null)
        {
            RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, enemy.transform.position - transform.position, Vector2.Distance(enemy.transform.position, transform.position));

            bool wallBetween = false;
            foreach(var h in hit)
            {
                if(h.collider.CompareTag("Wall"))
                {
                    wallBetween = true;
                    break;
                }
            }

            if(wallBetween)
                Debug.DrawRay(transform.position, enemy.transform.position - transform.position, Color.red);
            else
                Debug.DrawRay(transform.position, enemy.transform.position - transform.position, Color.green);

            if(wallBetween == false || player.isShooting)
            {
                enemy.TriggerAgro();
            }

        }


    }
}
