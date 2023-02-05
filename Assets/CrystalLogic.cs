using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalLogic : MonoBehaviour
{

    public EnergyManager m_EnergyManager;

    public ActionsTracker m_ActionsTracker;

    public float EnergyStore = 100;
    public float EnergyConsumptionPerSecond = 10;
    public bool IsTouched = false;
    bool IsConsumingEnergy = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(IsConsumingEnergy) 
        {
            //adding energy here
            float EnergyChange = Time.deltaTime * EnergyConsumptionPerSecond;
            EnergyStore -= EnergyChange;
            m_EnergyManager.ConsumeEnergy(-EnergyChange);
        }

        if(EnergyStore <= 0)
        {
            IsConsumingEnergy = false;
            m_ActionsTracker.UpdateActions(1);
        }
    }

    public void StartConsumingEnergy()
    {
        IsConsumingEnergy = true;
        m_ActionsTracker.UpdateActions(-1);
    }
}
