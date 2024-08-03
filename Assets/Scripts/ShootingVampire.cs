using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingVampire : AbstractEnemy
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform bulletPoint;
    [SerializeField] AudioSource myAudio;
    [SerializeField] float bulletSpeed;
    [SerializeField] float reloadTime;
    private bool isRealoading = false;

    protected override void AttackPlayer()
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, player.transform.position - transform.position, Vector2.Distance(player.transform.position, transform.position));
        bool seesPlayer = true;
        foreach (var h in hit)
        {
            if (h.collider.CompareTag("Wall"))
            {
                seesPlayer = false;
                break;
            }
        }
        if(seesPlayer)
        {
            path.enableRotation = false;
            path.canMove = false;
            myRig.velocity = Vector3.zero;
            transform.rotation = GlobalUtils.LookAt(transform.position, player.transform.position, -90);
            if(isRealoading == false)
            {
                GameObject bullet = Instantiate(projectilePrefab, transform.position, transform.rotation);
                bullet.GetComponent<Rigidbody2D>().AddForce(bulletPoint.up * bulletSpeed, ForceMode2D.Impulse);
                myAudio.Play();
                isRealoading = true;
                StartCoroutine(Reload());
            }   
        }
        else
        {
            path.enableRotation = true;
            path.canMove = true;
        }

    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);
        isRealoading = false;
    }
}
