using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;


    public List<Checkpoint> Checkpoints;

    private void Awake()
    {
        Instance = this;
    }
}
