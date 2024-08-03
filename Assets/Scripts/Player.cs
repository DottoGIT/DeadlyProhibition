using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float MovementSpeed;
    [SerializeField] float rotOffset;
    [SerializeField] float legOffset;
    [SerializeField] Animator legsAnim;
    [SerializeField] Animator torsoAnim;
    [SerializeField] Transform bulletPoint;
    [SerializeField] Transform legs;
    [SerializeField] Transform torso;
    [SerializeField] Animator HealthAnimator;
    [SerializeField] AudioSource shooting_audio;
    [SerializeField] AudioSource axe_audio;
    [SerializeField] AudioSource noAmmo_audio;
    [SerializeField] AudioSource footstep_audio;
    [SerializeField] AudioSource hurt_audio;
    [SerializeField] float footstepFrequency;
    [SerializeField] float flashingTime;
    [SerializeField] float healthRegenTime;
    float footStepTimer = 0;
    float flashingTimer = 0;
    float healthRegenTimer = 0;
    bool canTakeDamage = true;
    int tookDamage = 0;

    Rigidbody2D myRigid;
    
    [Header("Gun stats")]

    [SerializeField] float bulletSpeed;
    [SerializeField] int bulletDamage;
    [SerializeField] float reloadTime;
    [SerializeField] int spread;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] bool canShoot = true;
    public bool isShooting = false;
    public int maxBullets;
    public int currentBullets { get; private set; } = 0;
    bool isRealoading = false;
    float bulletPointOffset = 47.7f;

    private void Start()
    {
        flashingTimer = flashingTime;
        healthRegenTimer = healthRegenTime;
        currentBullets = maxBullets;
        myRigid = GetComponent<Rigidbody2D>();
        torsoAnim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        // Movement
        Vector2 inputVector = InputManager.instance.inputActions.Player.Movement.ReadValue<Vector2>();
        Vector2 destination = new Vector2(gameObject.transform.position.x + inputVector.x * MovementSpeed * Time.deltaTime, gameObject.transform.position.y + inputVector.y * MovementSpeed * Time.deltaTime);
        myRigid.MovePosition(destination);

        //Handle Sound

        if(inputVector != Vector2.zero)
        {
            footStepTimer -= Time.deltaTime;
            if (footStepTimer <= 0)
            {
                footstep_audio.pitch = 1 + Random.Range(-0.1f, 0.1f);
                footstep_audio.Play();
                footStepTimer = footstepFrequency;
            }
        }

        // Anim things

        if(inputVector != Vector2.zero)
        {
            legsAnim.SetBool("isWalking", true);
        }
        else
        {
            legsAnim.SetBool("isWalking", false);
        }

        //Flashing time
        if(canTakeDamage == false)
        {
            flashingTimer -= Time.deltaTime;
            if(flashingTimer <= 0)
            {
                canTakeDamage = true;
                flashingTimer = flashingTime;
            }
        }

       //Regeneration
       healthRegenTimer -= Time.deltaTime;
       if(healthRegenTimer <= 0)
       {
           Heal();
           healthRegenTimer = healthRegenTime;
       }
    }

    private void Update()
    {
        if(Time.timeScale == 1)
        {
            CalculatePlayerRotation();

            if (InputManager.instance.inputActions.Player.Shoot.IsPressed() && canShoot)
            {
                Shoot();
                isShooting = true;
            }
            else
            {
                isShooting = false;
            }

            if (InputManager.instance.inputActions.Player.AxeSwing.IsPressed())
            {
                torsoAnim.SetBool("holdsRightButton", true);
            }
            else
            {
                torsoAnim.SetBool("holdsRightButton", false);
            }
        }
    }

    void CalculatePlayerRotation()
    {

        Vector3 cameraPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        torso.rotation = GlobalUtils.LookAt(transform.position, cameraPos, rotOffset);

        Vector2 inputVector = InputManager.instance.inputActions.Player.Movement.ReadValue<Vector2>();
        float LegsRotZ = Mathf.Rad2Deg * Mathf.Asin(((inputVector.y) / Mathf.Sqrt(Mathf.Pow(inputVector.x, 2) + Mathf.Pow(inputVector.y, 2))));

        if (inputVector == Vector2.zero)
        {
            legs.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if(inputVector.x >= 0)
        {
            legs.rotation = Quaternion.Euler(0, 0, LegsRotZ + legOffset);
        }
        else
        {
            legs.rotation = Quaternion.Euler(0, 0, -LegsRotZ + legOffset + 180);
        }

    }

    public void PlayAxeSwingEffect()
    {
        axe_audio.Play();
    }

    private void Shoot()
    {
        if(isRealoading == false && currentBullets > 0 && Time.timeScale == 1)
        {
            float randSpread = Random.Range(-spread, spread);
            Quaternion myRot = bulletPoint.localRotation;
            bulletPoint.localRotation = Quaternion.Euler(0, 0, bulletPointOffset + randSpread);

            GameObject bullet = Instantiate(bulletPrefab, bulletPoint.position, bulletPoint.rotation);
            bullet.GetComponent<Rigidbody2D>().AddForce(bulletPoint.up * bulletSpeed, ForceMode2D.Impulse);
            bullet.GetComponent<Bullet>().damage = bulletDamage;

            bulletPoint.localRotation = myRot;
            currentBullets--;

            shooting_audio.Play();

            isRealoading = true;
            StartCoroutine(LoadNextBullet());
        }
        else if(isRealoading == false && Time.timeScale == 1)
        {
            noAmmo_audio.Play();

            isRealoading = true;
            StartCoroutine(LoadNextBullet());
        }
    }

    IEnumerator LoadNextBullet()
    {
        yield return new WaitForSeconds(reloadTime);
        isRealoading = false;
    }

    public void Heal()
    {
        if(tookDamage > 0)
        {
            HealthAnimator.SetTrigger("heal");
            tookDamage -= 1;
        }
    }

    public void TakeDamage()
    {
        if(canTakeDamage)
        {
            HealthAnimator.SetTrigger("tookDmg");
            canTakeDamage = false;
            tookDamage += 1;
            hurt_audio.Play();
        }
        healthRegenTimer = healthRegenTime;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage();
        }
    }
}
