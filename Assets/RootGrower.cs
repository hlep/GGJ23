using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class RootGrower : MonoBehaviour
{
    [SerializeField]
    SpriteShapeController controller;

    [SerializeField]
    float MaxGrowSpeed = 0.3f;

    Vector3 m_rootEnd = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_rootEnd != Vector3.zero && controller.spline.GetPosition(1) != m_rootEnd)
        {
            Vector3 NewPos = Vector3.Lerp(controller.spline.GetPosition(1), m_rootEnd, MaxGrowSpeed*Time.deltaTime);
            controller.spline.SetPosition(1, NewPos);
        }
    }

    public void SetRootEndpoint(Vector3 p)
    {
        m_rootEnd = p;
    }
}
