using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;

public class ClickCatcher : MonoBehaviour
{
    Camera m_Camera;
    Vector3 m_RootBuildingStartPosition;
    bool m_RootBuildingStarted = false;
    public GameObject rootPrefab;
    //int m_LatestLayer = 0;

    [SerializeField]
    LineTracker m_LineTracker;

    [SerializeField]
    ActionsTracker m_ActionsTracker;

    [SerializeField]
    EnergyManager m_EnergyManager;


    void Awake()
    {
        m_Camera = Camera.main;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 0.0f;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            var collider = Physics2D.OverlapPoint(mousePosition);

          
            if (collider)
            {
                if (m_RootBuildingStarted && collider.gameObject.tag == "Earth")
                {
                    if (!m_ActionsTracker.HasFreeActions())
                    {
                        return;
                    }

                    if (m_LineTracker.CheckLineIntersect(m_RootBuildingStartPosition, mousePosition))
                    {
                        //do some warning?
                        StopRootBuilding();
                        print("Line intersect fail...");
                        return;
                    }

                    //finish building root

                    GameObject SpawnedRoot = Instantiate(rootPrefab, Vector3.zero, Quaternion.identity);

                    SpriteShapeController shapeController = SpawnedRoot.GetComponent<SpriteShapeController>();
                    shapeController.spline.SetPosition(0, m_RootBuildingStartPosition);
                    //we can't spawn two points at the same place, so here's that
                    shapeController.spline.SetPosition(1, Vector3.Lerp(m_RootBuildingStartPosition, mousePosition, 0.2f));

                    RootGrower grower = SpawnedRoot.GetComponent<RootGrower>();
                    grower.SetRootEndpoint(mousePosition);
                    grower.growEndDelegate = EndGrow;

                    grower.m_EnergyManager = m_EnergyManager;

                    m_LineTracker.AddNewLine(m_RootBuildingStartPosition, mousePosition);

                    m_ActionsTracker.UpdateActions(-1);

                    StopRootBuilding();
                }

                if (collider.gameObject.tag == "Root")
                {
                    Debug.Log("Start root building");
                    m_RootBuildingStarted = true;
                    m_RootBuildingStartPosition = mousePosition;
                    //start showing how much energy would we lose?
                }
            }
            else
            {
                if (m_RootBuildingStarted)
                {
                    StopRootBuilding();
                    Debug.Log("Stop root building - clicked outside earth");
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            StopRootBuilding();
        }
    }

    private void StopRootBuilding()
    {
        m_RootBuildingStarted = false;
        m_RootBuildingStartPosition = Vector3.zero;
    }

    private void EndGrow()
    {
        m_ActionsTracker.UpdateActions(1);
    }
}
