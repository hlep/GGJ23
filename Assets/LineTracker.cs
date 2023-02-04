using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTracker : MonoBehaviour
{
    // Start is called before the first frame update
    List<KeyValuePair<Vector3, Vector3>> startEndPair;

    void Start()
    {
  /*      foreach(var TwoPoints in keyValuePairs)
        {
            
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckLineIntersect(Vector3 startPoint, Vector3 endPoint)
    {
        foreach (var TwoPoints in startEndPair)
        {

        }
    }

    public void AddNewLine(Vector3 startPoint, Vector3 endPoint)
    {
        startEndPair.Add(new KeyValuePair<Vector3, Vector3>(startPoint, endPoint));
    }
}
