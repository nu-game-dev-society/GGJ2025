using UnityEngine;

public interface IBoostProvider
{
    float BoostProvided { get; }
}

public class BoostBubbleComponent : MonoBehaviour, IBoostProvider
{
    public float BoostProvided => this.boostProvided;
    [SerializeField]
    private float boostProvided;
}
