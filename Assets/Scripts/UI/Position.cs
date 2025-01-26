using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;

public class Position : MonoBehaviour
{
    [SerializeField] SplineContainer spline;
    public Transform player;

    [SerializeField] TMP_Text currentPos;
    [SerializeField] TMP_Text totalPos;

    GameTimer[] cars;
    public int playerPos;

    [SerializeField] TMP_Text[] positions;

    // Start is called before the first frame update
    void Awake()
    {
        // Get car controllers
        cars = FindObjectsOfType<GameTimer>().ToArray();

        totalPos.text = "/" + cars.Length;

        StartCoroutine(CheckPosition());
    }
    public string DebugNames;
    public List<Tuple<Transform, float>> carPct = new List<Tuple<Transform, float>>();
    public Color HighlightColor;
    public Color BaseColor;
    IEnumerator CheckPosition()
    {

        carPct = new List<Tuple<Transform, float>>();

        foreach (GameTimer car in cars)
        {
            SplineUtility.GetNearestPoint(spline.Spline, car.transform.position, out float3 nearest, out float splinePct);

            carPct.Add(new Tuple<Transform, float>(car.transform, splinePct + (car.CurrentLap * 100)));
        }

        var r = carPct.OrderByDescending(e => e.Item2).Select(x => x.Item1).ToArray();
        DebugNames = string.Join(", ", r.Select(x => x.name));
        for (int pos = 0; pos < r.Length; pos++)
        {
            //update all ui
            if (pos < positions.Length)
            {
                positions[pos].text = r[pos].name.Replace(' ', '\n');
            }

            if (r[pos].Equals(player))
            {
                playerPos = pos + 1;
                positions[pos].transform.parent.GetComponent<Image>().color = HighlightColor;
            }
            else
            {
                positions[pos].transform.parent.GetComponent<Image>().color = BaseColor;
            }
        }

        currentPos.text = playerPos.ToString();
        yield return new WaitForSeconds(0.1f);

        StartCoroutine(CheckPosition());
    }

    public int GetPosition(Transform t)
    {
        var r = Array.IndexOf(carPct.OrderByDescending(e => e.Item2).Select(x => x.Item1).ToArray(), t) + 1;
        Debug.Log($"{t.name} {r}");
        return r;

    }
}
