using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    float MaxEnergy = 100;
    public float CurrentEnergy = 30;
    float EnergyRegenPerSec = 3;
    float EnergyConsumptionPerDistance = 30;

    [SerializeField]
    RectTransform CurrentEnergyBarTransf;

    [SerializeField]
    RectTransform FutureEnergyBarTransf;

    float MaxEnergyBarY;

    public bool IsPreviewing = false;

    public float PreviewEnergy;


    //-1000 is min transform

    // Start is called before the first frame update
    void Start()
    {
        MaxEnergyBarY = CurrentEnergyBarTransf.anchoredPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentEnergy = Mathf.Min(MaxEnergy, CurrentEnergy + EnergyRegenPerSec * Time.deltaTime);
        // print(CurrentEnergy);

        float EnergyPercent = Mathf.Clamp(CurrentEnergy/MaxEnergy, 0, 1);

        float yPos = Mathf.Lerp(-1000, MaxEnergyBarY, EnergyPercent);

        CurrentEnergyBarTransf.anchoredPosition = new Vector2(CurrentEnergyBarTransf.anchoredPosition.x, yPos);


        if (IsPreviewing)
        {
            float PreviewEnergyPercent = Mathf.Clamp(PreviewEnergy / MaxEnergy, 0, 1);
            float previewYPos = Mathf.Lerp(-1000, MaxEnergyBarY, EnergyPercent);
            FutureEnergyBarTransf.anchoredPosition = new Vector2(FutureEnergyBarTransf.anchoredPosition.x, previewYPos);
        }
        else
        {
            FutureEnergyBarTransf.anchoredPosition = new Vector2(FutureEnergyBarTransf.anchoredPosition.x, yPos);
        }
    }

    public void ConsumeEnergy(float energy)
    {
        CurrentEnergy -= energy;
    }

    public float GrowthEnergyCalc(float distance) 
    {
        return EnergyConsumptionPerDistance * distance;
    }

    public float PreviewEnergyConsumption(float energy)
    {
        float EnergyLeft = CurrentEnergy - energy;
        return EnergyLeft;
    }
}
