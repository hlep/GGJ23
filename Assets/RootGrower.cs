using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class RootGrower : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // actualtime = dist/speed
        // deltaprogress = deltatime/actualtime
        //  progress+=deltaprogress
        // Mathf.SmoothStep(float a, float b, float progress);

        if (m_rootEnd != Vector3.zero && controller.spline.GetPosition(1) != m_rootEnd)
        {
            Progress += Time.deltaTime / MovementTime;

            Vector3 NewPos = Vector3.Lerp(controller.spline.GetPosition(1), m_rootEnd, Mathf.SmoothStep(0f, 1f, Progress));

            controller.spline.SetPosition(1, NewPos);

            //Vector3 NewPos = Vector3.Lerp(controller.spline.GetPosition(1), m_rootEnd, MaxGrowSpeed*Time.deltaTime);


        } 
        else if(!m_finishedGrowing && m_rootEnd != Vector3.zero)
        {
            colliderSpawner.EnableSecondCollider();
            m_finishedGrowing = true;
        }
    }

    public void SetRootEndpoint(Vector3 p)
    {
        m_rootEnd = p;
        MovementTime = Vector3.Distance(m_rootEnd, controller.spline.GetPosition(1)) / MaxGrowSpeed;

        //some offset to avoid hitting other root when splitting from the same root
        Vector3 StartPos = Vector3.Lerp(controller.spline.GetPosition(1), p, 0.1f);

        RaycastHit2D hit = Physics2D.Raycast(StartPos, p, 99999.0f, LayerMask.GetMask("Root"), 0.0f);

        if (hit && hit.collider.gameObject != gameObject)
        {
            m_rootEnd = hit.point;
            print("collision hit!");
            print(hit.collider.gameObject.name);
        }
    }
   
}
