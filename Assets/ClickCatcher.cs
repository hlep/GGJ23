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
            ContactFilter2D rootFilter = new ContactFilter2D();
            rootFilter.layerMask = LayerMask.GetMask("Root");
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            RaycastHit2D[] secondhit = new RaycastHit2D[20];
            Physics2D.Raycast(ray.origin, ray.direction, rootFilter, secondhit);
            if (hit)
            {
                // Use the hit variable to determine what was clicked on.
                print(hit.collider.gameObject.name);

                if(hit.collider.gameObject.tag == "Earth")
                {
                    if (m_ClickActive) //click can be active only when we build roots?
                    {
                        //finish building root

                        GameObject SpawnedRoot = Instantiate(rootPrefab, Vector3.zero, Quaternion.identity);

                        Vector3 pos = SpawnedRoot.transform.position;
                        SpawnedRoot.transform.position.Set(pos.x, pos.y, m_LatestLayer++);

                        SpriteShapeController shapeController = SpawnedRoot.GetComponent<SpriteShapeController>();
                        shapeController.spline.SetPosition(0, m_SavedClick);
                        shapeController.spline.SetPosition(1, ray.origin);
                        
                        m_ClickActive = false;
                        m_SavedClick = Vector3.zero;
                    }
                    else
                    {
                        m_SavedClick = mousePosition;
                    }
                }

                if (hit.collider.gameObject.tag == "Root")
                {
                    if (!m_ClickActive)
                    {
                        m_ClickActive = true;
                        m_SavedClick = ray.origin;
                        //start showing how much energy would we lose
                    }
                }
            }
            else
            {
                m_ClickActive = false;
                m_SavedClick = Vector3.zero;
            }

            
        }

        if (Input.GetMouseButtonDown(1))
        {
            m_ClickActive = false;
            m_SavedClick = Vector3.zero;
        }
    }
}
