using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TimerDisplay : MonoBehaviour
{
    [SerializeField] GameTimer playerGameTimer;
    [SerializeField] TMP_Text TimerText;

    private int lapStart;
    private int bestLapTime = int.MaxValue;

    private int timer;

    public bool ShouldTick;
 
    public bool IsFinalLap => playerGameTimer.CurrentLap == totalLaps; 
    public static int totalLaps = 3;

    // Start is called before the first frame update
    void Start()
    {
        if (playerGameTimer == null)
        {
            var p = FindObjectOfType<PlayerInputController>();
            playerGameTimer = p.GetComponent<GameTimer>();
        }
        playerGameTimer.DoLapEvent.AddListener(DoLap);

        InvokeRepeating("TimerTick", 0.01f, 0.01f);
    }

    // Update is called once per frame
    void Update()
    {
        TimerText.text = $"Lap {Mathf.Clamp(playerGameTimer.CurrentLap, 1, totalLaps)}/{totalLaps} \n {FormatTime(timer - lapStart)}\nBest: {FormatTime(bestLapTime == int.MaxValue ? 0 : bestLapTime)}";
    }

    void TimerTick()
    {
        if (!ShouldTick) return;

        timer += 1;
    }

    string FormatTime(int time)
    {
        int m = Mathf.FloorToInt(time / 100 / 60);
        int s = Mathf.FloorToInt((time - (m * 60 * 100)) / 100);
        int ms = time % 100;

        return m.ToString().PadLeft(2, '0') + ":" + s.ToString().PadLeft(2, '0') + "." + ms.ToString().PadLeft(2, '0');
    }

    void DoLap()
    {
        // Ignore lap 0 -> 1
        if (playerGameTimer.CurrentLap == 1) return;

        int lapTime = timer - lapStart;
        lapStart = timer;

        if (lapTime < bestLapTime)
        {
            bestLapTime = lapTime;
        }
    }
}
