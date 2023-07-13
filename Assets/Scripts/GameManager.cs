using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static float s_timer;
    [SerializeField] private Text timeText;

    private void Awake()
    {
        if (timeText == null) return;
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (timeText == null) return;

        s_timer += Time.deltaTime;
        timeText.text = DisplayTimeWithFormat();
    }

    public static string DisplayTimeWithFormat()
    {
        int remainder = (int)s_timer % 60;
        return $"{(int)(s_timer / 60)}:{remainder / 10}{remainder % 10}";
    }

    //Call this on the Start Game Button
    public void ResetTimer()
    {
        s_timer = 0;
    }
}
