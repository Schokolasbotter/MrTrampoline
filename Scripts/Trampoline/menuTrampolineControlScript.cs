using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class menuTrampolineControlScript : MonoBehaviour
{
    #region
    public Transform character;

    [Header("Trampoline Settings")]
    public float outputForce;
    #endregion

    private void Update()
    {
        float xDistance = 0;
        //Set Position
        transform.position = new Vector2(transform.position.x + xDistance * 0.01f,transform.position.y); 
    }

    public void moveTrampoline(Vector2 touchPosition)
    {
        //change to  Vector3
        Vector3 touchVector = new Vector3(touchPosition.x, touchPosition.y, 0f);
        //From Screen to world coordinates
        float xPosition = utils.ScreenToWorld(Camera.main, touchVector).x;
        //Set Position
        gameObject.transform.position = new Vector2(xPosition,gameObject.transform.position.y);
    }
}
