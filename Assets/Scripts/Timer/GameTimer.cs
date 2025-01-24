using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance;

    [SerializeField] int StartTime;
    private TMP_Text Text;

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

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        // Setup
        Text = GetComponent<TMP_Text>();
        if (!RaceMode) Timer = StartTime;
        UpdateUI();

        InvokeRepeating("TimerTick", 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TimerTick()
    {
        if (!ShouldTick) return;
        if (Timer <= 0)
        {
            ShouldTick = false;
            Debug.Log("NO TIMEEEEEE!!!!!!!!!!!!!");
        }

        Timer += RaceMode ? 1 : -1;
    }

    void UpdateUI()
    {
        int m = Mathf.FloorToInt(Timer / 60);
        int s = Timer % 60;

        Text.text = m.ToString().PadLeft(2, '0') + ":" + s.ToString().PadLeft(2, '0');
    }
}
