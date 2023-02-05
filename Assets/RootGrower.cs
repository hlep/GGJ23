using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class RootGrower : MonoBehaviour
{

    public delegate void GrowEnded();
    public GrowEnded growEndDelegate;

    [SerializeField]
    SpriteShapeController controller;

    [SerializeField]
    SplineColliderSpawner colliderSpawner;
    [SerializeField]
    float MaxGrowSpeed = 0.09f;

    Vector3 m_rootEnd = Vector3.zero;

    bool m_finishedGrowing = false;

    float Progress = 0.0f;

    float MovementTime = 0.0f;

    public EnergyManager m_EnergyManager;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (m_rootEnd != Vector3.zero && controller.spline.GetPosition(1) != m_rootEnd)
        {
            Progress += Time.deltaTime / MovementTime;

            Vector3 NewPos = Vector3.Lerp(controller.spline.GetPosition(1), m_rootEnd, Mathf.SmoothStep(0f, 1f, Progress));

            float EnergyConsumed = m_EnergyManager.GrowthEnergyCalc(Vector3.Distance(controller.spline.GetPosition(1), NewPos));

            m_EnergyManager.ConsumeEnergy(EnergyConsumed);

            controller.spline.SetPosition(1, NewPos); 
        } 
        else if(!m_finishedGrowing && m_rootEnd != Vector3.zero)
        {
            //return an action
            colliderSpawner.EnableSecondCollider();
            m_finishedGrowing = true;
            growEndDelegate();
        }
    }

    public void SetRootEndpoint(Vector3 p)
    {
        m_rootEnd = p;
        MovementTime = Vector3.Distance(m_rootEnd, controller.spline.GetPosition(1)) / MaxGrowSpeed;
    }
   
}
