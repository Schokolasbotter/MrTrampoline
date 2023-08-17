using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screenSetup : MonoBehaviour
{
    [Header("References")]
    public Transform leftWall;
    public Transform rightWall;

    private void Awake()
    {
        //Get Screen Width
        float screenWidth =  Screen.width;
        //Convert to world coordinates
        Vector3 newLeftWallPosition = utils.ScreenToWorld(Camera.main, new Vector3(0f, 0f, 0f));
        Vector3 newRightWallPosition = utils.ScreenToWorld(Camera.main, new Vector3(screenWidth, 0f, 0f));
        //Adjust on Y-Axis
        newLeftWallPosition.y += leftWall.localScale.y / 2;
        newRightWallPosition.y += leftWall.localScale.y / 2;
        //Adjust on X Axis
        newLeftWallPosition.x -= leftWall.localScale.x / 2;
        newRightWallPosition.x += leftWall.localScale.x / 2;
        //Set new positions
        leftWall.position = newLeftWallPosition;
        rightWall.position = newRightWallPosition;
    }
}
