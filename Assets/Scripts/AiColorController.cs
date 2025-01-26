using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiColorController : MonoBehaviour
{
    public FishColorOptions FishColorOptions;
    public CarColorOptions CarColorOptions;
    public MeshRenderer Car;
    public SkinnedMeshRenderer Fish;

    // MaterialPropertyBlocks for changing materials independently
    private MaterialPropertyBlock carMaterialBlock;
    private MaterialPropertyBlock fishMaterialBlock1;
    private MaterialPropertyBlock fishMaterialBlock2;

    void Start()
    {
        // Initialize the MaterialPropertyBlocks
        carMaterialBlock = new MaterialPropertyBlock();
        fishMaterialBlock1 = new MaterialPropertyBlock();
        fishMaterialBlock2 = new MaterialPropertyBlock();

        SetCar();

        SetFish();
    }
    [ContextMenu("SetFish")]
    private void SetFish()
    {
        // Set fish color using MaterialPropertyBlock
        ColorPairing fishColors = FishColorOptions.GetRandom();
        Fish.GetPropertyBlock(fishMaterialBlock1, 0);
        fishMaterialBlock1.SetColor("_Color", fishColors.PrimaryColor);
        Fish.SetPropertyBlock(fishMaterialBlock1, 0);

        Fish.GetPropertyBlock(fishMaterialBlock2, 1);
        fishMaterialBlock2.SetColor("_Color", fishColors.SecondaryColor);
        Fish.SetPropertyBlock(fishMaterialBlock2, 1);
    }
    [ContextMenu("SetCar")]
    private void SetCar()
    {
        // Set car color using the MaterialPropertyBlock
        Color carColor = CarColorOptions.GetRandom();
        Car.GetPropertyBlock(carMaterialBlock, 0);
        carMaterialBlock.SetColor("_Color", carColor);
        Car.SetPropertyBlock(carMaterialBlock, 0);
    }
}

[CreateAssetMenu(fileName = "FishColors", menuName = "ScriptableObjects/FishColors", order = 1)]
public class FishColorOptions : ScriptableObject
{
    public List<ColorPairing> Colors;
    public ColorPairing GetRandom()
    {
        return Colors[Random.Range(0, Colors.Count)];
    }

}
[CreateAssetMenu(fileName = "CarColors", menuName = "ScriptableObjects/CarColors", order = 1)]
public class CarColorOptions : ScriptableObject
{
    public List<Color> Colors;

    public Color GetRandom()
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