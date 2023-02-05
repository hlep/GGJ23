using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    float MaxEnergy = 100;
    public float CurrentEnergy = 0;
    float EnergyRegenPerSec = 7;
    float EnergyConsumptionPerDistance = 10;

    // Start is called before the first frame update
    void Start()
    {
        CurrentEnergy = MaxEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        CurrentEnergy = Mathf.Min(MaxEnergy, CurrentEnergy + EnergyRegenPerSec * Time.deltaTime);
        // print(CurrentEnergy);
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
