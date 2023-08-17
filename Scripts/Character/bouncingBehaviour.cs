using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bouncingBehaviour : MonoBehaviour
{
    #region
    [Header ("References")]
    [SerializeReference] private gameManager gameManager;
    [SerializeReference] private effectPlayer effectPlayer;
    [SerializeReference] private Transform trampoline;
    private Rigidbody2D rb;

    [Header ("Settings")]
    //Variables
    [Range (1f,20f)]
    public float maxUpwardVelocityY = 15f;
    [Range(-1, -20f)]
    public float maxDownwardVelocityY = -10f;
    [Range(0.1f, 1f)]
    public float minBounceVariance = 0.5f;
    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        //Start of the game until player starts game
        if(gameManager.gameState == gameManager.GameState.start)
        {
            //No Velocity
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            //Set Transform to stay over trampoline
            transform.position = new Vector3(trampoline.position.x, transform.position.y, transform.position.z);
        }
        //During Gameplay
        else
        {
            rb.gravityScale = 1;
            //Limit Velocity
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, maxDownwardVelocityY, maxUpwardVelocityY));
        }
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If gamestate is Not start,then bounce
        if(gameManager.gameState != gameManager.GameState.start)
        {
            //if not in starting state
            //If hit the trampoline
            if(collision.gameObject.tag.Equals("Player"))
            {
                //Increase Multiplier
                gameManager.increaseMultiplier();

                //Get Trampoline Transform and Script
                trampolineControlScript trampolineScript = collision.gameObject.GetComponent<trampolineControlScript>();
                Transform trampolineTransform = collision.gameObject.transform;
                //Map the Difference on X axis onto an angle in Degrees
                float xDifference = trampolineTransform.position.x - gameObject.transform.position.x;  //get the Distance to centre of trampoline
                float trampolineSizeX = collision.gameObject.GetComponent<BoxCollider2D>().size.x;            
                float normal = Mathf.InverseLerp(-trampolineSizeX, trampolineSizeX, xDifference);
                float bounceAngle = Mathf.Lerp(60, 120, normal);
                //Calculate Vector
                Vector2 bounceVector = new Vector2(Mathf.Cos(bounceAngle * Mathf.Deg2Rad), Mathf.Sin(bounceAngle * Mathf.Deg2Rad));
                //Bounce
                //get the power of the trampoline charge
                float bounceForce = trampolineScript.outputForce;
                if(bounceForce == trampolineScript.minChargeForce)
                {
                    bounceForce = bounceForce * Random.Range(1-minBounceVariance, 1 + minBounceVariance);
                    //Play regular bounce effect
                    effectPlayer.playBounce();
                }
                else
                {
                    //Otherwise play charged bounce effect
                    effectPlayer.playChargedBounce();
                }
                rb.AddForce(bounceVector * bounceForce, ForceMode2D.Impulse);
                
            }
            else if (collision.gameObject.tag.Equals("Ground"))
            {
                StartCoroutine(effectPlayer.playFloorHit());
                //EndGame
                gameManager.endGame();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("LeftWall"))
        {
            //Reverse X Velocity
            rb.velocity = new Vector2(-0.9f * rb.velocity.x, rb.velocity.y);
        }
        else if (collision.gameObject.tag.Equals("RightWall"))
        {
            //Reverse X Velocity
            rb.velocity = new Vector2(- 0.9f * rb.velocity.x, rb.velocity.y);
        }
        else if (collision.gameObject.tag.Equals("BonusOrb"))
        {
            //Activate Bonus Multiplier
            gameManager.activateBonus();
        }
        else if (collision.gameObject.tag.Equals("ObstacleOrb"))
        {
            //EndGame
            gameManager.endGame();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("LeftWall"))
        {
            //Make sure the Velocity is positive -> move right
            if(rb.velocity.x < 0)
            {
                //Reverse X Velocity
                rb.velocity = new Vector2(-1f * rb.velocity.x, rb.velocity.y);
            }
        }
        else if (collision.gameObject.tag.Equals("RightWall"))
        {
            //Make sure the Velocity is negative -> move left
            if (rb.velocity.x > 0)
            {
                //Reverse X Velocity
                rb.velocity = new Vector2(-1f * rb.velocity.x, rb.velocity.y);
            }
        }
    }

    //When player starts the game
    public void startBounce()
    {
        //AddForce
        rb.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
        //Play regular bounce effect
        effectPlayer.playBounce();
        this.gameObject.GetComponent<CircleCollider2D>().enabled = true;
    }
}


