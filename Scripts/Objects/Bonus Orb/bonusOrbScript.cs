using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bonusOrbScript : MonoBehaviour
{
    #region
    [Header("Spawn Settings")]
    [Range(0.1f, 1.5f)][SerializeField] private float maxScale = 0.5f;
    [Range(0.0f, 0.1f)][SerializeField] private float growthRate = 0.01f;

    [Header("Movement Settings")]
    [Range(-2.5f, 0f)] [SerializeField] private float leftBorder = -2.3f;
    [Range(0f, 2.5f)] [SerializeField] private float rightBorder = 2.3f;
    [Range(2.5f, 5f)] [SerializeField] private float topBorder = 4f;
    [Range(0f, 2.5f)] [SerializeField] private float bottomBorder = 1f;
    [Range(0f, 5f)] [SerializeField] private float horizontalSpeed = 1f;
    [Range(0f, 5f)] [SerializeField] private float verticalSpeed = 0.5f;

    [Header("LifeTime Settings")]
    [Range(0f, 60f)] [SerializeField] private float lifeTime = 30f;
    #endregion

    // Update is called once per frame
    void Update()
    {
        //grow on spawn
        if(transform.localScale.x < maxScale && transform.localScale.y < maxScale)
        {
            transform.localScale += new Vector3(growthRate, growthRate);
        }

        //Movement
        float horizontalPosition = ((rightBorder - leftBorder) / 2 * Mathf.Sin(Time.time*horizontalSpeed));
        float verticalPosition = bottomBorder + (topBorder - bottomBorder) / 2 + (topBorder - bottomBorder) / 2 * (Mathf.Sin(Time.time*verticalSpeed));
        transform.position = new Vector3(horizontalPosition, verticalPosition, 0f);

        //LifeTime
        if (lifeTime <= 0f)
        {
            Destroy(this.gameObject);
        }
        else
        {
            lifeTime -= Time.deltaTime;
        }
    }

    //When Collected
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "MrT")
        {
            Destroy(this.gameObject);
        }
    }

    //Show spawn and movement area in Engine
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        utils.drawRectangleGizmo(leftBorder, rightBorder, topBorder, bottomBorder);
    }    
}
