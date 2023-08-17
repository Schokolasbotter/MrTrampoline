using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacleOrbScript : MonoBehaviour
{
    #region
    [Header("Spawn Settings")]
    [Range(0.1f, 1.5f)] [SerializeField] private float maxScale = 0.5f;
    [Range(0.0f, 0.1f)] [SerializeField] private float growthRate = 0.01f;

    [Header("LifeTime Settings")]
    [Range(0f, 60f)] [SerializeField] private float lifeTime = 30f;
    #endregion


    // Update is called once per frame
    void Update()
    {
        //grow on spawn
        if (transform.localScale.x < maxScale && transform.localScale.y < maxScale)
        {
            transform.localScale += new Vector3(growthRate, growthRate);
        }

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

    //Destroy on contact
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MrT")
        {
            Destroy(this.gameObject);
        }
    }
}
