using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class SpriteClicker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnMouseDown()
    {
        // Code here is called when the GameObject is clicked on.
        print("CLICK!!!!");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        if (hit)
        {
            print("hit!");
            print(hit.collider.gameObject.name);
            //print(hit.collider);
            // Use the hit variable to determine what was clicked on.
        }
        else
        {
            print("no hit");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
