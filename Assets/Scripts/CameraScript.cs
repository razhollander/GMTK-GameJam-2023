using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CameraScript : GaneEventListener
{
    public Transform camera_head;
    [SerializeField] private LineRenderer _chosenPath;
    public GameObject light_spot;
    private int currPointIndex = 0;
    [SerializeField] private float lightSpeed = 0.4f;
    [SerializeField] private float minDist = 0.01f;
    bool power_out = false;

    public override void OnGameEvent(GameEvent gameEvent)
    {
        if(gameEvent == GameEvent.Electricity)
        {
            power_out = true;
        }
        else
        {
            power_out = false;
        }
    }

    private void Update()
    {
        if (!power_out)
        {
            LerpLightToNextCurrPoint();
            Vector3 diff = camera_head.position - light_spot.transform.position;
            diff.Normalize();

            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            camera_head.rotation = Quaternion.Euler(0f, 0f, rot_z);
        }
    }

    private void LerpLightToNextCurrPoint()
    {
        var currPos = _chosenPath.transform.TransformPoint(_chosenPath.GetPosition(currPointIndex));
        light_spot.transform.position = Vector3.Lerp(light_spot.transform.position, currPos, lightSpeed * Time.deltaTime);

        if (Vector3.Distance(light_spot.transform.position, currPos) < minDist)
        {
            currPointIndex++;

            if (currPointIndex >= _chosenPath.positionCount)
            {
                currPointIndex = 0;
            }
        }
    }
        
    
    
}
