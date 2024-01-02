using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class HumanHealth : MonoBehaviour
{
    [Header("Blood")]
    [Tooltip("The maximum volume of blood this human can store")]
    public float bloodCapacity; //The maximum volume of blood this human can hold
    [Tooltip("The minimum volume of blood required for the human to live")]
    public float fatalityThreshold; //When blood volume drops to this level, the human dies

    private float bloodVolume;   //Acts similarly to a health bar

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        bloodVolume = bloodCapacity;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public float Hurt(float damage)
    {
        bloodVolume -= damage;
        if (bloodVolume <= fatalityThreshold)
        {
            Die();
        }
        return damage;
    }

    private void Die()
    {

    }
}
