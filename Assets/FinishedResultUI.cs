using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinishedResultUI : MonoBehaviour
{
    public void SetFinishedString(int position)
    {
        string p = position + "th";

        switch (position)
        {
            case 1:
                p = "first";
                break;
            case 2:
                p = "second";
                break;
            case 3:
                p = "third";
                break;
            case 4:
                p = "last";
                break;
        }
        GetComponent<TextMeshProUGUI>().text = $"You finished {p}!";
        gameObject.SetActive(true);
    }
}
