using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameTimer : MonoBehaviour
{
    public int LastCheckpoint { get; private set; } = -1;
    public int CurrentLap { get; private set; } = 0;
    public UnityEvent DoLapEvent;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void DoCheckpoint(Checkpoint checkpoint)
    {
        int i = CheckpointManager.Instance.Checkpoints.IndexOf(checkpoint);

        Debug.Log(i);
        Debug.Log(checkpoint);

        if (i != LastCheckpoint + 1 && LastCheckpoint != CheckpointManager.Instance.Checkpoints.Count - 1) return;

        LastCheckpoint = i;

        if (i == 0)
        {
            CurrentLap += 1;
            DoLapEvent.Invoke();
        }
    }

}
