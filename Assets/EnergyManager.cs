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
    RectTransform EnergyBarTransf;

    RectTransform MaxEnergyBarTransf;

    //-1000 is min transform

    // Start is called before the first frame update
    void Start()
    {
        MaxEnergyBarTransf = EnergyBarTransf;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentEnergy = Mathf.Min(MaxEnergy, CurrentEnergy + EnergyRegenPerSec * Time.deltaTime);
        // print(CurrentEnergy);

        float EnergyPercent = Mathf.Clamp(MaxEnergy/ CurrentEnergy, 0, 1);

        float yPos = Mathf.Lerp(-1000, MaxEnergyBarTransf.anchoredPosition.y, EnergyPercent);

        EnergyBarTransf.anchoredPosition = new Vector2(EnergyBarTransf.anchoredPosition.x, yPos);
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
