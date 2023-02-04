using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;

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
            SpriteShapeController shapeC = hit.collider.gameObject.GetComponent<SpriteShapeController>();
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

    public void FindClosestPoint()
    {

    }
}
