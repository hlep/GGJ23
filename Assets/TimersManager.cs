using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimersManager : MonoBehaviour
{


    void StartPreparation()
    {

    }

    private IEnumerator StartGame(float timeBeforStart)
    {
        StartPreparation();
        yield return new WaitForSeconds(timeBeforStart);
        print("Started");
    }


    private IEnumerator StartGameAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        print("Started");
    }

    

    // Start is called before the first frame update
    void Start()
    {
        /*IEnumerator coroutine = StartGameAfterTime(10);
        StartCoroutine(coroutine);*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
