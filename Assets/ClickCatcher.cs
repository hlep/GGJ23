using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ClickCatcher : MonoBehaviour
{
    Camera m_Camera;
    Vector3 m_SavedClick;
    bool m_ClickActive = false;
    public GameObject rootPrefab;
    int m_LatestLayer = 0;

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
            Ray ray = m_Camera.ScreenPointToRay(mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit)
            {
                if (m_ClickActive && hit.collider.gameObject.tag == "Earth")
                {
                    if (!m_ActionsTracker.HasFreeActions())
                    {
                        return;
                    }

                    if (m_LineTracker.CheckLineIntersect(m_SavedClick, ray.origin))
                    {
                        //do some warning?
                        StopClick();
                        return;
                    }

                    //finish building root

                    GameObject SpawnedRoot = Instantiate(rootPrefab, Vector3.zero, Quaternion.identity);

                    Vector3 pos = SpawnedRoot.transform.position;
                    SpawnedRoot.transform.position.Set(pos.x, pos.y, m_LatestLayer++);

                    SpriteShapeController shapeController = SpawnedRoot.GetComponent<SpriteShapeController>();
                    shapeController.spline.SetPosition(0, m_SavedClick);

                    //we can't spawn two points at the same place, so here's that
                    shapeController.spline.SetPosition(1, Vector3.Lerp(m_SavedClick, ray.origin, 0.2f));

                    RootGrower grower = SpawnedRoot.GetComponent<RootGrower>();
                    grower.SetRootEndpoint(ray.origin);
                    grower.growEndDelegate = EndGrow;

                    grower.m_EnergyManager = m_EnergyManager;

                    m_LineTracker.AddNewLine(m_SavedClick, ray.origin);

                    m_ActionsTracker.UpdateActions(-1);

                    StopClick();
                }

                if (hit.collider.gameObject.tag == "Root")
                {

                    m_ClickActive = true;
                    m_SavedClick = ray.origin;
                    //start showing how much energy would we lose?
                }
            }
            else
            {
                //StopClick();
                //TODO: remove?
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            StopClick();
        }
    }

    private void StopClick()
    {
        m_ClickActive = false;
        m_SavedClick = Vector3.zero;
    }

    private void EndGrow()
    {
        m_ActionsTracker.UpdateActions(1);
    }
}
