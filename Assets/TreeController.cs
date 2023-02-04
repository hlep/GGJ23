using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class TreeController : MonoBehaviour
{
    [SerializeField] public float[] TreeStagesDurations;
    [SerializeField] private SpriteResolver spriteResolver;
    [SerializeField] private SpriteLibrary spriteLibrary;
    [SerializeField] private string spriteLibraryCategoryName;

    private IEnumerator TreeStageCoroutine = null;

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

    void startGame()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
