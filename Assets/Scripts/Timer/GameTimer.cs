using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class IntEvent : UnityEvent<int> { }
public class GameTimer : MonoBehaviour
{
    public int LastCheckpoint { get; private set; } = -1;
    public int CurrentLap { get; private set; } = 0;
    public UnityEvent DoLapEvent;

    public IntEvent LastLapComplete;
    Position Position;
    private void Start()
    {
        Position = FindObjectOfType<Position>();
    }


    internal void DoCheckpoint(Checkpoint checkpoint)
    {
        int i = CheckpointManager.Instance.Checkpoints.IndexOf(checkpoint);

        if (i != LastCheckpoint + 1 && LastCheckpoint != CheckpointManager.Instance.Checkpoints.Count - 1) return;

        LastCheckpoint = i;

        if (i == 0)
        {
            CurrentLap += 1;
            DoLapEvent.Invoke();
            if (CurrentLap == TimerDisplay.totalLaps + 1)
            {
                //get position
                int pos = Position.GetPosition(transform);
                LastLapComplete.Invoke(pos);
            }
        }
    }

}
