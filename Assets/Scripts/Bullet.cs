using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float lifeTime;
    [SerializeField] GameObject particleSpawnPoint;
    [SerializeField] GameObject particle;
    [HideInInspector] public int damage;


    void Start()
    {
        StartCoroutine(CountToDie());
    }

    void DestroyBullet()
    {
        Instantiate(particle, particleSpawnPoint.transform.position, particleSpawnPoint.transform.rotation);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Wall"))
        {
            DestroyBullet();
        }
        if(collision.gameObject.CompareTag("Enemy"))
        {
            AbstractEnemy enemy = collision.gameObject.GetComponent<AbstractEnemy>();
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    IEnumerator CountToDie()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }


}
