using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FishColors", menuName = "ScriptableObjects/FishColors", order = 1)]
public class FishColorOptions : ScriptableObject
{
    public List<ColorPairing> Colors;
    public ColorPairing GetRandom()
    {
        return Colors[Random.Range(0, Colors.Count)];
    }

}


[System.Serializable]
public class ColorPairing
{
    public Color PrimaryColor;
    public Color SecondaryColor;
}