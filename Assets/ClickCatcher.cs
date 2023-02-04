using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCatcher : MonoBehaviour
{
    Camera m_Camera;
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
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 10);
            if (hit)
            {
                // Use the hit variable to determine what was clicked on.
                print(hit.collider.gameObject.name);
            }
        }
    }
}
