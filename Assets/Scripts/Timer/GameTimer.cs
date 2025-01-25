using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance;

    [SerializeField] int StartTime;
    [SerializeField] TMP_Text TimerText;

    private int lapStart;
    //private int lastLapTime;
    private int bestLapTime = int.MaxValue;

    private int currentLap = 1;
    [SerializeField] int totalLaps;

    private int _timer;
    public int Timer {
        get { return _timer; }
        set {
            _timer = value;
            UpdateUI();
        }
    }

    public bool ShouldTick;
    public bool RaceMode;

    [SerializeField] List<Checkpoint> Checkpoints;
    int lastCheckpoint = -1;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Setup
        if (!RaceMode) Timer = StartTime * 100;
        UpdateUI();

        InvokeRepeating("TimerTick", 0.01f, 0.01f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TimerTick()
    {
        if (!ShouldTick) return;
        if (Timer <= 0 && !RaceMode)
        {
            ShouldTick = false;
            Debug.Log("NO TIMEEEEEE!!!!!!!!!!!!!");
        }

        Timer += RaceMode ? 1 : -1;
    }

    void UpdateUI()
    {
        TimerText.text = currentLap + "/" + totalLaps +"\n" + FormatTime(Timer - lapStart) + "\nBest: " + FormatTime(bestLapTime == int.MaxValue ? 0 : bestLapTime);
    }

    string FormatTime(int time)
    {
        int m = Mathf.FloorToInt(time / 100 / 60);
        int s = Mathf.FloorToInt((time - (m * 60 * 100)) / 100);
        int ms = time % 100;

        return m.ToString().PadLeft(2, '0') + ":" + s.ToString().PadLeft(2, '0') + "." + ms.ToString().PadLeft(2, '0');
    }

    internal void DoLap()
    {
        if (!RaceMode) return;

        int lapTime = Timer - lapStart;
        lapStart = Timer;

        //int lapTimeDiff = lapTime - lastLapTime;

        if (lapTime < bestLapTime)
        {
            bestLapTime = lapTime;
        }

        currentLap = Math.Min(currentLap + 1, totalLaps);

        UpdateUI();
    }

    internal void DoCheckpoint(Checkpoint checkpoint)
    {
        int i = Checkpoints.IndexOf(checkpoint);

        if (i != lastCheckpoint + 1) return;

        lastCheckpoint = i;

        if (i == Checkpoints.Count - 1)
        {
            DoLap();
            lastCheckpoint = -1;
        }
    }

}
