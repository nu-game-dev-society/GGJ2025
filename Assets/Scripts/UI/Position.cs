using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class Position : MonoBehaviour
{
    [SerializeField] SplineContainer spline;
    [SerializeField] Transform player;

    [SerializeField] TMP_Text currentPos;
    [SerializeField] TMP_Text totalPos;

    GameTimer[] cars;

    // Start is called before the first frame update
    void Start()
    {
        // Get car controllers
        cars = FindObjectsOfType<GameTimer>().ToArray();

        totalPos.text = "/" + cars.Length;

        StartCoroutine(CheckPosition());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator CheckPosition()
    {
        yield return new WaitForSeconds(0.1f);

        Dictionary<Transform, float> carPct = new Dictionary<Transform, float>();

        foreach (GameTimer car in cars)
        {
            SplineUtility.GetNearestPoint(spline.Spline, car.transform.position, out float3 nearest, out float splinePct);

            carPct.Add(car.transform, splinePct + (car.CurrentLap * 100));
        }

        int playerPos = 0;

        foreach (var car in carPct.OrderBy(e => e.Value).Reverse())
        {
            playerPos++;
            if (car.Key.Equals(player))
            {
                break;
            }
        }

        currentPos.text = playerPos.ToString();

        StartCoroutine(CheckPosition());
    }
}
