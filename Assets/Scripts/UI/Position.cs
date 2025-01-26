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

    public List<Tuple<string, int>> FinishedResults;

    // Start is called before the first frame update
    void Awake()
    {
        FinishedResults = new List<Tuple<string, int>>();
        // Get car controllers
        cars = FindObjectsOfType<GameTimer>().ToArray();

        totalPos.text = "/" + cars.Length;

        StartCoroutine(CheckPosition());
    }
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

        for (int i = 0; i < FinishedResults.Count; i++)
        {
            positions[FinishedResults[i].Item2-1].text = FinishedResults[i].Item1.Replace(' ', '\n');
            if (FinishedResults[i].Item1.Equals(player.name))
            {
                playerPos = i + 1;
                positions[FinishedResults[i].Item2 -1].transform.parent.GetComponent<Image>().color = HighlightColor;
            }
            else
            {
                positions[FinishedResults[i].Item2 -1].transform.parent.GetComponent<Image>().color = BaseColor;
            }
        }
        var r = carPct.OrderByDescending(e => e.Item2).Select(x => x.Item1).ToArray();

        for (int pos = FinishedResults.Count; pos < r.Length; pos++)
        {            //update all ui
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

    public int GetFinishedPosition(Transform t)
    {
        var r = Array.IndexOf(carPct.OrderByDescending(e => e.Item2).Select(x => x.Item1).ToArray(), t) + 1;
        Debug.Log($"{t.name} {r}");
        FinishedResults.Add(new(t.name, r));

        return r;

    }
}
