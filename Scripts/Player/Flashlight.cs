using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flashlight : MonoBehaviour
{
    [SerializeField] GameObject FlashlightLight;
    
    private bool flashlightActive = false;
    public Slider flashlightSlider;
    public float maxCapacity;
    float currentCapacity;

    // Start is called before the first frame update
    void Start()
    {
        FlashlightLight.gameObject.SetActive(false);
        flashlightSlider.maxValue = maxCapacity;
        currentCapacity = maxCapacity;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!flashlightActive && currentCapacity > 0)
            {
                FlashlightLight.gameObject.SetActive(true);
                flashlightActive = true;
            }
            else
            {
                FlashlightLight.gameObject.SetActive(false);
                flashlightActive = false;
            }
        }

        // Turn off flashliight when battery is empty, and recharge battery when turned off
        flashlightSlider.value = currentCapacity;
        if (flashlightActive && currentCapacity > 0)
        {
            currentCapacity = Mathf.Clamp(currentCapacity - 1f * Time.deltaTime, 0f, maxCapacity);
            CancelInvoke("BatteryRecharge");
        }
        else
        {
            FlashlightLight.gameObject.SetActive(false);
            flashlightActive = false;
            Invoke("BatteryRecharge", 1f);
        }
    }

    void BatteryRecharge()
    {
        currentCapacity = Mathf.Clamp(currentCapacity + 1f * Time.deltaTime, 0f, maxCapacity);
    }
}
