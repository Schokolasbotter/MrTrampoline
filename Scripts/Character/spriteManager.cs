using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class spriteManager : MonoBehaviour
{
    public Sprite idle, up, down, left, right, falling;
    private SpriteRenderer characterSpriteRenderer;
    private void Start()
    {
        characterSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    //Set Sprites Regularly
    public void setIdleSprite()
    {
        characterSpriteRenderer.sprite = idle;
    }

    public void setFallingSprite()
    {
        characterSpriteRenderer.sprite = falling;
    }

    //Set Sprites based on arrow
    public void setSprite(Arrow arrow)
    {
        switch (arrow._direction)
        {
            default:
                break;
            case Arrow.Direction.up:
                //Up
                transform.localScale = new Vector3(transform.localScale.x, Mathf.Abs(transform.localScale.y), transform.localScale.z);
                characterSpriteRenderer.sprite = up;
                break;
            case Arrow.Direction.down:
                //down
                transform.localScale = new Vector3(transform.localScale.x, Mathf.Abs(transform.localScale.y), transform.localScale.z);
                characterSpriteRenderer.sprite = down;
                break;
            case Arrow.Direction.left:
                //Left
                transform.localScale = new Vector3(transform.localScale.x, -Mathf.Abs(transform.localScale.y), transform.localScale.z);
                characterSpriteRenderer.sprite = right;
                break;
            case Arrow.Direction.right:
                //Right
                transform.localScale = new Vector3(transform.localScale.x, Mathf.Abs(transform.localScale.y), transform.localScale.z);
                characterSpriteRenderer.sprite = right;
                break;
        }
        
    }

    //Random Sprite
    public void setRandomSprite()
    {
        //Change sprite Randomly
        float randomNumber = Random.Range(0f, 1f);
        switch (randomNumber)
        {
            case < 0.2f:
                characterSpriteRenderer.sprite = idle;
                break;
            case < 0.4f:
                transform.localScale = new Vector3(transform.localScale.x, Mathf.Abs(transform.localScale.y), transform.localScale.z);
                characterSpriteRenderer.sprite = up;
                break;
            case < 0.6f:
                transform.localScale = new Vector3(transform.localScale.x, Mathf.Abs(transform.localScale.y), transform.localScale.z);
                characterSpriteRenderer.sprite = down;
                break;
            case < 0.8f:
                transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
                characterSpriteRenderer.sprite = right;
                break;
            case < 1f:
                transform.localScale = new Vector3(transform.localScale.x, Mathf.Abs(transform.localScale.y), transform.localScale.z);
                characterSpriteRenderer.sprite = right;
                break;
        }
    }
    

}
