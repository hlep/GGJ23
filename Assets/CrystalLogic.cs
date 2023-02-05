using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalLogic : MonoBehaviour
{
    [SerializeField]
    GameObject ActiveConsumptionSprite;

    public EnergyManager m_EnergyManager;

    public ActionsTracker m_ActionsTracker;

    public float EnergyStore = 100;
    public float EnergyConsumptionPerSecond = 10;
    public bool IsTouched = false;
    public bool IsConsumingEnergy = false;

    GameObject CreatedSprite = null;

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

            if (EnergyStore <= 0)
            {
                StopConsumingEnergy();
                Destroy(gameObject);
            }
        }
    }

    public void StartConsumingEnergy()
    {
        IsConsumingEnergy = true;
        m_ActionsTracker.UpdateActions(-1);
        CreatedSprite = Instantiate(ActiveConsumptionSprite, gameObject.transform);
        CreatedSprite.gameObject.transform.localScale = Vector3.one;
    }

    public void StopConsumingEnergy()
    {
        IsConsumingEnergy = false;
        m_ActionsTracker.UpdateActions(1);
        Destroy(CreatedSprite);
    }

    public bool CanStartConsumingEnergy()
    {
        return EnergyStore > 0 && m_ActionsTracker.HasFreeActions() && IsTouched;
    }
}
