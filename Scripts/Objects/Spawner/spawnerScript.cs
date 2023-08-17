using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerScript : MonoBehaviour
{
    #region
    [Header("Spawn  Settings")]
    [SerializeReference] private GameObject bonusOrbPrefab;
    [SerializeReference] private GameObject obstacleOrbPrefab;
    [Range(-2.5f, 0f)] [SerializeField] private float leftBorder = -2.3f;
    [Range(0f, 2.5f)] [SerializeField] private float rightBorder = 2.3f;
    [Range(2.5f, 5f)] [SerializeField] private float topBorder = 4f;
    [Range(0f, 2.5f)] [SerializeField] private float bottomBorder = 1f;
    #endregion
 
    public void spawnBonusOrb()
    {
        //Create a rando position within te spawn Rectangle
        Vector3 spawnLocation = new Vector3(Random.Range(leftBorder, rightBorder), Random.Range(bottomBorder, topBorder), 0f);
        Instantiate(bonusOrbPrefab, spawnLocation, new Quaternion(0, 0, 0, 0),this.gameObject.transform);
    }

    public void spawnObstacleOrb()
    {
        //Create a random position within te spawn Rectangle
        Vector3 spawnLocation = new Vector3(Random.Range(leftBorder, rightBorder), Random.Range(bottomBorder, topBorder), 0f);
        Instantiate(obstacleOrbPrefab, spawnLocation, new Quaternion(0, 0, 0, 0), this.gameObject.transform);
    }

    public void destroyObjects()
    {
        if(transform.childCount > 0) //If there are children
        {
            //Loop through Children in reverse to destroy all
            for(int i = transform.childCount; i > 0; i--)
            {
                Destroy(this.transform.GetChild(i-1).gameObject);
            }
        }
    }

    public bool isBonusOrbActive()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            if (this.transform.GetChild(i).gameObject.tag.Equals("BonusOrb"))
            {
                return true; // If BonusOrb is active return true
            }
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        utils.drawRectangleGizmo(leftBorder, rightBorder, topBorder, bottomBorder);
    }
}
