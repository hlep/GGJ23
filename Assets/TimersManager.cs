using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimersManager : MonoBehaviour
{
    [SerializeField] public float[] TreeStagesDurations;
    private IEnumerator TreeStageCoroutine = null;


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

    public void RequestStartTreeStage(int stage)
    {
        if (TreeStageCoroutine != null)
        {
            print("TreeStage coroutine already active");
            return;
        }

        TreeStageCoroutine = StartTreeStage(stage);
    }

    private IEnumerator StartTreeStage(int stage)
    {
        print("Tree stage " + stage + " started");
        yield return new WaitForSeconds(TreeStagesDurations[stage]);
        OnTreeStageEnded(stage);
    }

    public void OnTreeStageEnded(int stage)
    {
        TreeStageCoroutine = null;
        print("Tree stage " + stage + " ended");
    }

    // Start is called before the first frame update
    void Start()
    {
        IEnumerator coroutine = StartGameAfterTime(10);
        StartCoroutine(coroutine);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
