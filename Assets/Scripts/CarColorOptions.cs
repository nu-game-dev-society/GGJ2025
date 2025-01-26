using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CarColors", menuName = "ScriptableObjects/CarColors", order = 1)]
public class CarColorOptions : ScriptableObject
{
    public List<Color> Colors;

    public Color GetRandom()
    {
        return Colors[Random.Range(0, Colors.Count)];
    }
}