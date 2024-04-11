using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    public Image statusBar;
    public float maxCharge = 100f;
    public float chargeRate = 10f; // Adjust this to change the speed of charging
    public float currentCharge;
    public float duration;

    void Start()
    {
        currentCharge = maxCharge;
        chargeRate = maxCharge/duration;
    }

    void Update()
    {
        // Decrease the current charge over time
        currentCharge -= chargeRate * Time.deltaTime;

        // Clamp the charge to ensure it doesn't go below 0
        currentCharge = Mathf.Clamp(currentCharge, 0f, maxCharge);

        // Calculate the fill amount based on the current charge
        float fillAmount = currentCharge / maxCharge;

        // Set the fill amount of the status bar
        statusBar.fillAmount = fillAmount;
    }
    public void ResetFill(){
        currentCharge = maxCharge;
        statusBar.fillAmount = maxCharge;
    }
}
