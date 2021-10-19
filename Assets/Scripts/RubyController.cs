using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    public float speed = 3f;

    public int maxHealth = 5;
    public float timeInvincible = 2.0f;

    public int health {get {return currentHealth;}}
    int currentHealth;

    bool isInvincible;
    float invincibleTimer;


    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);

    AudioSource audioSource;

    public GameObject projectilePrefab;

    public AudioClip ThrowCog;
    public AudioClip RubyHit;

    // Start is called before the first frame update
    void Start()
    { 

        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;

        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");

        Vector2 move = new Vector2(horizontal, vertical);

        if(!Mathf.Approximately(move.x,0.0f)|| !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x,move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if(isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if(invincibleTimer < 0)
                isInvincible = false;
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }


        if(Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if(hit.collider != null)
            {
                NonPlayable character = hit.collider.GetComponent<NonPlayable>();

                if(character != null)
                {
                    character.DisplayDialog();
                }

            }

        }
    }
   
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + 3.0f * horizontal * Time.deltaTime * speed;
        position.y = position.y + 3.0f * vertical * Time.deltaTime * speed;

        rigidbody2d.MovePosition(position);

    }


    public void ChangeHealth(int amount)
    {

        if(amount<0)
        {
            if(isInvincible)
                return;
            isInvincible = true;
            invincibleTimer = timeInvincible;
        }

        PlaySound(RubyHit);
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        
        UIHealthBar.instance.SetValue(currentHealth/ (float)maxHealth);
    }


    void Launch()
    {
        GameObject projectileObject =  Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        PlaySound(ThrowCog);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection,300);

        animator.SetTrigger("Launch");

    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
