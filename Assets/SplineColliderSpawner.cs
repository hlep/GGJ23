using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class SplineColliderSpawner : MonoBehaviour
{
    [SerializeField]
    SpriteShapeController controller;

    [SerializeField]
    float radius;

    [SerializeField]
    bool bFirstRoot = false;

    CircleCollider2D secondCollider;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 firstPoint = controller.spline.GetPosition(0);
        Vector3 secondPoint = controller.spline.GetPosition(1);

        //CircleCollider2D firstCollider = gameObject.AddComponent<CircleCollider2D>();

        //firstCollider.radius = radius;
        //firstCollider.offset = firstPoint;

        secondCollider = gameObject.AddComponent<CircleCollider2D>();

        //secondCollider.includeLayers = LayerMask.GetMask("Root");

        secondCollider.radius = radius;
        secondCollider.offset = secondPoint;

        if (!bFirstRoot)
        {
            secondCollider.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //we need to update the end collider, because it may move while growing
        secondCollider.offset = controller.spline.GetPosition(1);
        /*//secondCollider.
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = 6;
        Collider2D[] Collided = new Collider2D[10];
        secondCollider.OverlapCollider(contactFilter, Collided);
        //if(Collided.Length > 0) { print("it collides!"); };*/
    }

    public void EnableSecondCollider()
    {
        secondCollider.offset = controller.spline.GetPosition(1);
        secondCollider.enabled = true;
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("CollideSplineColliderSpawner!!!!!");
    }
}
