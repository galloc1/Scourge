using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDayNightCycle : MonoBehaviour
{
    public DayNightCycle dayNightCycleScript;

    private bool isHome;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHome&&dayNightCycleScript.IsDay())
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Burnt to a crisp in the light of day");
        gameObject.SetActive(false);
    }

    public void ReturnHome()
    {
        isHome = true;
    }
}
