using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ClickCatcher : MonoBehaviour
{
    Camera m_Camera;
    Vector3 m_RootBuildingStartPosition;
    bool m_RootBuildingStarted = false;
    public GameObject rootPrefab;

    [SerializeField]
    LineTracker m_LineTracker;

    [SerializeField]
    ActionsTracker m_ActionsTracker;

    [SerializeField]
    EnergyManager m_EnergyManager;

    [SerializeField]
    GameObject ActiveRootSprite;

    [SerializeField]
    TMPro.TextMeshProUGUI m_Text;

    GameObject CreatedSprite = null;

    Collider2D ColliderWithSprite = null;


    void Awake()
    {
        m_Camera = Camera.main;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            mousePosition.z = 0.0f;

            var collider = Physics2D.OverlapPoint(mousePosition, 1 <<
            LayerMask.NameToLayer(!m_RootBuildingStarted ? "Root" : "Earth"));

            if (collider)
            {
                if (m_RootBuildingStarted && collider.gameObject.tag == "Earth")
                {
                    if (!m_ActionsTracker.HasFreeActions())
                    {
                        return;
                    }

                    float PreviewEnergy = m_EnergyManager.CurrentEnergy - m_EnergyManager.GrowthEnergyCalc(Vector3.Distance(mousePosition, m_RootBuildingStartPosition));

                    if (PreviewEnergy < 0)
                    {
                        return;
                    }

                    Vector3 startPosWithOffset = Vector3.Lerp(m_RootBuildingStartPosition, mousePosition, 0.2f);

                    Vector3 lineIntersect;
                    if (m_LineTracker.CheckLineIntersect(startPosWithOffset, mousePosition, out lineIntersect))
                    {
                        //do some warning?
                        StopRootBuilding();
                        print("Line intersect fail...");
                        print(lineIntersect);
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
        else
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            mousePosition.z = 0.0f;

            var collider = Physics2D.OverlapPoint(mousePosition, 1 << LayerMask.NameToLayer("Root"));

            if (collider && collider != ColliderWithSprite && !m_RootBuildingStarted)
            {
                Destroy(CreatedSprite);
                CreatedSprite = Instantiate(ActiveRootSprite, mousePosition, Quaternion.identity);
                //ColliderWithSprite = collider;
            }
            else if ((!m_RootBuildingStarted && !collider) && CreatedSprite)
            {
                Destroy(CreatedSprite);
            }

            collider = Physics2D.OverlapPoint(mousePosition, 1 << LayerMask.NameToLayer("Earth"));
            if(m_RootBuildingStarted && collider)
            {
                m_EnergyManager.IsPreviewing = true;

                float PreviewEnergy = m_EnergyManager.CurrentEnergy - m_EnergyManager.GrowthEnergyCalc(Vector3.Distance(mousePosition, m_RootBuildingStartPosition));
                m_Text.text = PreviewEnergy.ToString();

                m_EnergyManager.PreviewEnergy = PreviewEnergy;
            }
            else
            {
                m_EnergyManager.IsPreviewing = false;
                m_Text.text = "";
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
        Destroy(CreatedSprite);
    }

    private void EndGrow()
    {
        m_ActionsTracker.UpdateActions(1);
    }
}
