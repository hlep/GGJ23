using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class EarthRandomizer : MonoBehaviour
{
    [SerializeField] private SpriteShapeController m_EarthShape;

    // Start is called before the first frame update
    void Start()
    {
        m_EarthShape.spline.InsertPointAt(2, Vector3.zero);
        m_EarthShape.spline.InsertPointAt(2, new Vector3(5,2,0));
        m_EarthShape.spline.InsertPointAt(2, Vector3.zero);
        //m_EarthShape.spline.SetLeftTangent(3, )
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
