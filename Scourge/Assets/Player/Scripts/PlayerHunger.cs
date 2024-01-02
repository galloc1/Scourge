using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHunger : MonoBehaviour
{
    [Header("Hunger")]
    [Tooltip("The maximum volume of blood the player can store")]
    public float bloodCapacity; //The maximum volume of blood the player can store

    public TextMeshProUGUI bloodVolumeDisplay;

    private float bloodVolume;   //The current volume of blood the player has


    // Start is called before the first frame update
    void Start()
    {
        bloodVolume = bloodCapacity;
        bloodVolumeDisplay.text = Mathf.Round(bloodVolume).ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DrinkBlood(float volumeDrank)
    {
        bloodVolume = Mathf.Min(bloodVolume + volumeDrank, bloodCapacity);
        bloodVolumeDisplay.text = Mathf.Round(bloodVolume).ToString();
    }

    public void UseBlood(float volumeUsed)
    {
        bloodVolume = Mathf.Max(bloodVolume-volumeUsed, 0);
        bloodVolumeDisplay.text = Mathf.Round(bloodVolume).ToString();
        if(bloodVolume <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Succumbed to hunger");
        gameObject.SetActive(false);
    }
}
