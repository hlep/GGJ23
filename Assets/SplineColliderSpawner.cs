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

    CircleCollider2D secondCollider;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 firstPoint = controller.spline.GetPosition(0);
        Vector3 secondPoint = controller.spline.GetPosition(1);

        CircleCollider2D firstCollider = gameObject.AddComponent<CircleCollider2D>();


        firstCollider.radius = radius;
        firstCollider.offset = firstPoint;

        secondCollider = gameObject.AddComponent<CircleCollider2D>();

        secondCollider.radius = radius;
        secondCollider.offset = secondPoint;
    }

    // Update is called once per frame
    void Update()
    {
        secondCollider.offset = controller.spline.GetPosition(1);
    }
}
