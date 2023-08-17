using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuBouncingBehaviour : MonoBehaviour
{
    #region
    [Header ("References")]
    [SerializeReference] private Transform trampoline;
    private spriteManager spriteManager;
    private Rigidbody2D rb;

    [Header ("Settings")]
    //Variables
    [Range (1f,20f)]
    public float maxUpwardVelocityY = 15f;
    [Range(-1, -20f)]
    public float maxDownrdVelocityY = -10f;
    [Range(0.1f, 1f)]
    public float minBounceVariance = 0.5f;
    public float stdBounceForce = 5;
    public float maxAngularVelocity = 200f;
    private float timer = 2f;
    #endregion

    private void Start()
    {
        //get components
        spriteManager = GetComponent<spriteManager>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //Change Sprite randomly
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            spriteManager.setRandomSprite();
            timer = 2f;
        }
    }


    private void FixedUpdate()
    {
        //Limit Velocity
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, maxDownrdVelocityY, maxUpwardVelocityY));
        //Fix position
        transform.position = new Vector2(0f, transform.position.y);
        rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, -maxAngularVelocity, maxAngularVelocity);
    }

    //Bouncing behaviour when touching the trampoline
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag.Equals("Player"))
        {
            float bounceForce = stdBounceForce * Random.Range(1-minBounceVariance, 1 + minBounceVariance);
            rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
            rb.AddForce(new Vector2(Random.Range(-0.1f,0.1f),0), ForceMode2D.Impulse);
        }
    }
}


