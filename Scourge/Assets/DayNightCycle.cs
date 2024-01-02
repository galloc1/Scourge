using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public int secondsPerCycle;
    public int startOfDay;
    public int endOfDay;

    private float secondsPassed;
    private bool isDay;

    // Start is called before the first frame update
    void Start()
    {
        secondsPassed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        secondsPassed += Time.deltaTime;
        if (secondsPassed > startOfDay && secondsPassed < endOfDay)
        {
            isDay = true;
        }
        else
        {
            isDay = false;
        }

        if(secondsPassed>=secondsPerCycle)
        {
            secondsPassed -= secondsPerCycle;
        }
    }

    public bool IsDay() { return isDay; }
}
