using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class discoScript : MonoBehaviour
{
    #region
    public Transform characterTransform;
    public GameObject spotlight;
    private bool isOn = false;
    //Lights
    private GameObject[] lights = new GameObject[5];
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i < transform.childCount; i++)
        { 
            lights[i]= transform.GetChild(i).gameObject;
            lights[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Follow Character at all time
        transform.position = characterTransform.position;
    }

    public void startDisco()
    {
        isOn = true;
        spotlight.SetActive(false);
        StartCoroutine(StartDisco());
    }

    public void stopDisco()
    {
        isOn = false;
        spotlight.SetActive(true);
        foreach (GameObject light in lights){
            light.SetActive(false);
        }
    }

    private IEnumerator StartDisco()
    {
        //if disco is On
        while (isOn)
        {
            //Every 0.2 seconds
            //Get one of the lights randomly
            //Switch it to opposite state on->off off->on
            GameObject light = lights[Random.Range(0, 4)];
            light.SetActive(!light.activeSelf);
            yield return new WaitForSeconds(0.2f);
        }
        
    }
}
