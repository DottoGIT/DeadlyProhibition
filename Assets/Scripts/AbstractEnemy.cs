using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public abstract class AbstractEnemy : MonoBehaviour
{
    [SerializeField] int maxHealth;
    [SerializeField] int knockbackForce;
    [SerializeField] GameObject bloodStain;
    [SerializeField] GameObject deathParticle;
    [SerializeField] GameObject deadVampire;
    [SerializeField] AudioSource Trigger_Audio;
    [SerializeField] float movementSpeed;
    public AudioClip deathClip;

    protected GameObject player;
    protected Rigidbody2D myRig;
    protected AIDestinationSetter destSetter;
    protected AIPath path;

    int currentHealth;
    bool isTriggered = false;

    private void Start()
    {
        myRig = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>().gameObject;
        path = GetComponent<AIPath>();
        destSetter = GetComponent<AIDestinationSetter>();
        destSetter.target = player.transform;
        path.maxSpeed = movementSpeed;
    }

    private void Update()
    {
        if (isTriggered)
        {
            AttackPlayer();
        }
    }

    protected abstract void AttackPlayer();
    
    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public void TriggerAgro()
    {
        Trigger_Audio.enabled = true;
        isTriggered = true;
    }

    void Die()
    {
        transform.rotation = GlobalUtils.LookAt(transform.position, player.transform.position, 90);
        Instantiate(bloodStain, transform.position, transform.rotation);
        GameObject enemyObj = Instantiate(deadVampire, transform.position, transform.rotation);
        enemyObj.GetComponent<Rigidbody2D>().AddForce(transform.up * knockbackForce, ForceMode2D.Impulse);
        enemyObj.GetComponent<AudioSource>().clip = deathClip;
        enemyObj.GetComponent<AudioSource>().Play();
        Instantiate(deathParticle, transform.position, transform.rotation);
        UIManager.IncreaseComboCount();
        Destroy(gameObject);
    }
}
