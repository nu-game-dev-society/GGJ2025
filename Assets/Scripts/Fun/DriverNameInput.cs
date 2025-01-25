using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DriverNameInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField input;
    void Start()
    {
        if (input == null)
            input = GetComponent<TMP_InputField>();
        input.onValueChanged.AddListener(Save);
        input.text = DriverNames.GetPlayerDriverName();
        Save();
    }

    private void Save(string inp)
    {
        Debug.Log("Saved key");
        DriverNames.SetPlayerDriverName(inp);
    }

    public void Regen()
    {
        input.text = DriverNames.GetRandom();
        Save();
    }
    public void Save()
    {
        Debug.Log($"Saved key {input.text}");
        DriverNames.SetPlayerDriverName(input.text);
    }
}
