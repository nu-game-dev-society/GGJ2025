using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class WrongWay : MonoBehaviour
{
    [SerializeField] SplineContainer spline;
    [SerializeField] Transform player;

    [SerializeField] GameObject display;

    private bool isMovingBackwards = false;
    private float lastSplinePct = 0f;
    private float backwardsStartTime = 0f;
    [SerializeField] private float thresholdBackwardsTime = 2f;
    [SerializeField] private float forwardThreshold = 0.002f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckWrongWay());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CheckWrongWay()
    {
        yield return new WaitForSeconds(0.5f);

        // Get the player's position on the spline
        SplineUtility.GetNearestPoint(spline.Spline, player.position, out float3 nearest, out float splinePct);

        // Check if the player is moving backwards
        if (splinePct < lastSplinePct)
        {
            // Start tracking backwards time if not already in backwards state
            if (!isMovingBackwards)
            {
                backwardsStartTime = Time.time;
                isMovingBackwards = true;
            }

            // If the player is moving back for an amount of time, toggle on
            if (Time.time - backwardsStartTime >= thresholdBackwardsTime)
            {
                isMovingBackwards = true;
                display.SetActive(true);
            }
        }
        else
        {
            // If the player has moved forward by the threshold, toggle off
            if (isMovingBackwards && splinePct - lastSplinePct >= forwardThreshold)
            {
                isMovingBackwards = false;
                display.SetActive(false);
            }
        }

        // Update last spline percentage for next loop
        lastSplinePct = splinePct;

        StartCoroutine(CheckWrongWay());
    }
}
