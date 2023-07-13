using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeDisplay : MonoBehaviour
{
    private Text timeDisplay;

    private void Awake()
    {
        timeDisplay = GetComponent<Text>();
        timeDisplay.text = $"Time: {GameManager.DisplayTimeWithFormat()}";
    }
}
