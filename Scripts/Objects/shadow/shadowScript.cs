using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shadowScript : MonoBehaviour
{
    #region
    public Transform characterTransform;
    public Vector3 offSet;
    #endregion

    // Update is called once per frame
    void Update()
    {
        //Position
        transform.position = characterTransform.position + offSet;
        transform.rotation = characterTransform.rotation;
        //get Sprite from character
        this.GetComponent<SpriteRenderer>().sprite = characterTransform.gameObject.GetComponent<SpriteRenderer>().sprite;
    }
}
