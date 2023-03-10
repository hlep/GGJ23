using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class LineTracker : MonoBehaviour
{
    [SerializeField]
    TreeController treeController;

    // Start is called before the first frame update
    List<KeyValuePair<Vector3, Vector3>> startEndPair = new List<KeyValuePair<Vector3, Vector3>>();
    public List<KeyValuePair<GameObject, Vector3>> CirlePairs = new List<KeyValuePair<GameObject, Vector3>>();
    const float MineralRadius = 0.22f;
    const float DiscoveryRadius = 0.5f;

    const float MaxDepth = 3.61f;
    const float MaxDepthForce = 10;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool CheckLineIntersect(Vector3 startPoint, Vector3 endPoint, out Vector3 intersectionPoint)
    {
        foreach (var TwoPoints in startEndPair)
        {
            if (doIntersect(startPoint, endPoint, TwoPoints.Key, TwoPoints.Value))
            {
                intersectionPoint = lineLineIntersection(startPoint, endPoint, TwoPoints.Key, TwoPoints.Value);
                return true;
            }
        }

        intersectionPoint = Vector3.zero;

        return false;
    }

    public void AddNewLine(Vector3 startPoint, Vector3 endPoint)
    {
        startEndPair.Add(new KeyValuePair<Vector3, Vector3>(startPoint, endPoint));

        float Depth = Mathf.Max(endPoint.y - startPoint.y, 0);

        treeController.compensatedWeight += Depth / MaxDepth * MaxDepthForce;

        foreach (var CirclePair in CirlePairs)
        {
            if (CheckMineralIntersection(startPoint, endPoint, CirclePair.Value, MineralRadius))
            {
                var Crystal = CirclePair.Key.GetComponent<CrystalLogic>();
                Crystal.IsTouched = true;
            }

            if (CheckPointInsideRadius(endPoint, CirclePair.Value, DiscoveryRadius))
            {
                var Crystal = CirclePair.Key.GetComponent<CrystalLogic>();
                Crystal.SetDiscovered();
            }
        }
    }

    // Given three collinear Vector3s p, q, r, the function checks if
    // Vector3 q lies on line segment 'pr'
    static Boolean onSegment(Vector3 p, Vector3 q, Vector3 r)
    {
        if (q.x <= Math.Max(p.x, r.x) && q.x >= Math.Min(p.x, r.x) &&
            q.y <= Math.Max(p.y, r.y) && q.y >= Math.Min(p.y, r.y))
            return true;

        return false;
    }

    // To find orientation of ordered triplet (p, q, r).
    // The function returns following values
    // 0 --> p, q and r are collinear
    // 1 --> Clockwise
    // 2 --> Counterclockwise
    static int orientation(Vector3 p, Vector3 q, Vector3 r)
    {
        // See https://www.geeksforgeeks.org/orientation-3-ordered-Vector3s/
        // for details of below formula.
        float val = (q.y - p.y) * (r.x - q.x) -
                (q.x - p.x) * (r.y - q.y);

        if (val == 0) return 0; // collinear

        return (val > 0) ? 1 : 2; // clock or counterclock wise
    }

    // The main function that returns true if line segment 'p1q1'
    // and 'p2q2' intersect.
    static Boolean doIntersect(Vector3 p1, Vector3 q1, Vector3 p2, Vector3 q2)
    {
        // Find the four orientations needed for general and
        // special cases
        int o1 = orientation(p1, q1, p2);
        int o2 = orientation(p1, q1, q2);
        int o3 = orientation(p2, q2, p1);
        int o4 = orientation(p2, q2, q1);

        // General case
        if (o1 != o2 && o3 != o4)
            return true;

        // Special Cases
        // p1, q1 and p2 are collinear and p2 lies on segment p1q1
        if (o1 == 0 && onSegment(p1, p2, q1)) return true;

        // p1, q1 and q2 are collinear and q2 lies on segment p1q1
        if (o2 == 0 && onSegment(p1, q2, q1)) return true;

        // p2, q2 and p1 are collinear and p1 lies on segment p2q2
        if (o3 == 0 && onSegment(p2, p1, q2)) return true;

        // p2, q2 and q1 are collinear and q1 lies on segment p2q2
        if (o4 == 0 && onSegment(p2, q1, q2)) return true;

        return false; // Doesn't fall in any of the above cases
    }


    public Vector3 lineLineIntersection(Vector3 A, Vector3 B, Vector3 C, Vector3 D)
    {
        // Line AB represented as a1x + b1y = c1
        float a1 = B.y - A.y;
        float b1 = A.x - B.x;
        float c1 = a1 * (A.x) + b1 * (A.y);

        // Line CD represented as a2x + b2y = c2
        float a2 = D.y - C.y;
        float b2 = C.x - D.x;
        float c2 = a2 * (C.x) + b2 * (C.y);

        float determinant = a1 * b2 - a2 * b1;

        if (determinant == 0)
        {
            // The lines are parallel. This is simplified
            // by returning a pair of FLT_MAX
            return Vector3.zero;
        }
        else
        {
            float x = (b2 * c1 - b1 * c2) / determinant;
            float y = (a1 * c2 - a2 * c1) / determinant;
            return new Vector3(x, y);
        }
    }

    bool CheckMineralIntersection(Vector3 startPoint, Vector3 endPoint, Vector3 mineralLoc, float mineralRadius)
    {
        // Calculate the slope and intercept of the line
        float slope = (endPoint.y - startPoint.y) / (endPoint.x - startPoint.x);
        float intercept = startPoint.y - slope * startPoint.x;

        // Calculate the coefficients of the quadratic equation
        float a = 1 + slope * slope;
        float b = 2 * (slope * intercept - mineralLoc.x - slope * mineralLoc.y);
        float c = mineralLoc.x * mineralLoc.x + intercept * intercept - 2 * intercept * mineralLoc.y + mineralLoc.y * mineralLoc.y - mineralRadius * mineralRadius;

        // Calculate the discriminant
        float discriminant = b * b - 4 * a * c;

        if (discriminant >= 0)
        {
            // There are two real roots
            double root1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
            double root2 = (-b - Math.Sqrt(discriminant)) / (2 * a);

            //it's the line of intersection
            new Vector3((float)root1, (slope * (float)root1 + intercept), 0);

            return true;
        }

        return false;
    }

    bool CheckPointInsideRadius(Vector3 Point, Vector3 mineralLoc, float Radius)
    {
        bool isInside = (Math.Pow((Point.x - mineralLoc.x), 2) + Math.Pow((Point.y - mineralLoc.y), 2)) <= Math.Pow(Radius, 2);
        return isInside;
    }

}
