using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sliderScript : MonoBehaviour
{
    [Range(0f, 1f)] public float sliderValue = 0.5f;
    private RectTransform colorTransform;

    // Start is called before the first frame update
    void Start()
    {
        //Get Information on start
        colorTransform = transform.GetChild(0).GetComponent<RectTransform>();
        colorTransform.position = colorTransform.position = new Vector3(colorTransform.position.x, -880f, colorTransform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        //Map the Charged power to create the power bar movement
        float yPosition = Mathf.Lerp(-880f, -363f, sliderValue);
        float height = Mathf.Lerp(0f, 1700f, sliderValue);
        colorTransform.sizeDelta= new Vector2(colorTransform.rect.width,height);
        colorTransform.anchoredPosition = new Vector2(colorTransform.anchoredPosition.x, yPosition);
    }

}
