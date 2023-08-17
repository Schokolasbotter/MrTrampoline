using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class trampolineControlScript : MonoBehaviour
{
    #region
    [Header("References")]
    [SerializeReference] private touchManager touchManager;
    [SerializeReference] private effectPlayer effectPlayer;
    [SerializeReference] private sliderScript barScript;

    [Header("Trampoline Settings")]
    [Range(0f, 2.0f)][SerializeField]
    private float maxChargingRate = 0.05f;
    private float chargingRate = 0f;
    [Range(5.0f, 10.0f)][SerializeField]
    public float minChargeForce = 5f;
    [Range(5.0f, 100.0f)][SerializeField]
    private float maxChargeForce = 10f;
    [SerializeField]
    private float chargeForce;
    [Range(0.01f, 0.5f)][SerializeField]
    private float bounceTimeWindow = 0.2f;
    public float outputForce;
    private Vector2 startingPosition;
    private bool isCharging;
    #endregion

    private void Start()
    {
        //Set the minimum Force at start
        chargeForce = minChargeForce;
        outputForce = minChargeForce;

        //Subscribe to Event
        touchManager.TouchStart += getStartingPosition;
        touchManager.TouchStart += () => isCharging = true;
        touchManager.TouchStop += releaseCharge;
        touchManager.TouchStop += () => isCharging = false;
    }

    private void Update()
    {
        //Charge per frame with the set charging Rate
        chargeForce = Mathf.Clamp(chargeForce + chargingRate, minChargeForce, maxChargeForce);

        //Get touchPosition in Screen Coords
        Vector2 touchposition = touchManager.getPosition();
        
        //Trampoline
        moveTrampoline(touchposition);
        if (isCharging)
        {
            chargeTrampoline(touchposition);         
        }

        //Map onto color
        float normal = Mathf.InverseLerp(minChargeForce, maxChargeForce, chargeForce);
        float colorValue = Mathf.Lerp(255f, 0f, normal);
        barScript.sliderValue = normal;
        Color chargeColor = new Color(255, colorValue, colorValue);
        this.GetComponent<SpriteRenderer>().color = chargeColor;
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

    public void chargeTrampoline(Vector2 touchPosition)
    {
        //Manage StaringPosition
        Vector3 startingVector = new Vector3(0f, startingPosition.y, 0f);
        Vector3 startingWorldVector = utils.ScreenToWorld(Camera.main, startingVector);

        //Change from Screen Coordinates to World Coordinates
        Vector3 inputVector = new Vector3(0f, touchPosition.y, 0f);
        Vector3 worldVector = utils.ScreenToWorld(Camera.main, inputVector);
        //if input is below the height 
        if(worldVector.y < startingWorldVector.y) 
        {
            //Position Difference
            float yDelta = startingWorldVector.y - worldVector.y;
            //Map onto ChangeRate Scale
            float normal = Mathf.InverseLerp(0f, 1.5f, yDelta);
            chargingRate = Mathf.Lerp(0, maxChargingRate, normal);
            //Audio
            if (!effectPlayer.isTrampolinePlaying() && chargeForce < maxChargeForce)
            {
                effectPlayer.playCharging();
            }
        }
        else
        {
            //Reset to not charging
            chargeForce = minChargeForce;
            effectPlayer.stopTrampolineAudio();
            chargingRate = 0f;
        }
    }

    public void getStartingPosition()
    {
         startingPosition = touchManager.getStartingPosition();
    }

    public void releaseCharge ()
    {
        //Set Output Force
        StartCoroutine(changeOutputForce());
        //Reset Charged Force
        chargeForce = minChargeForce;
        chargingRate = 0f;
    }

    IEnumerator changeOutputForce()
    {
        //Set output force to the charged Force for x time
        outputForce = chargeForce;
        yield return new WaitForSeconds(bounceTimeWindow);
        outputForce = minChargeForce;
    }
}
