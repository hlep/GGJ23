using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

[Serializable]
struct TreeStageSetup
{
    public string spriteLabel;
    public string[] brokenSpriteLabels;
    public float duration;
    public float treeWeight;
}

public class TreeController : MonoBehaviour
{
    [SerializeField] private SpriteResolver spriteResolver;
    [SerializeField] private SpriteLibrary spriteLibrary;
    [SerializeField] private TreeStageSetup[] stagesSetup;

    private bool bTreeStarted = false;
    private int currentTreeStage;
    private float currentWeight;

    private Coroutine TreeStageCoroutine = null;

    public void RequestStartTreeStage(int stage)
    {
        if (TreeStageCoroutine != null)
        {
            print("TreeStage coroutine already active");
            return;
        }

        TreeStageCoroutine = StartCoroutine(StartTreeStage(stage));
    }

    private IEnumerator StartTreeStage(int stage)
    {
        print("Tree stage " + stage + " started");

        currentTreeStage = stage;
        spriteResolver.SetCategoryAndLabel(spriteResolver.GetCategory(), stagesSetup[currentTreeStage].spriteLabel);
        currentWeight = stagesSetup[currentTreeStage].treeWeight;

        yield return new WaitForSeconds(stagesSetup[currentTreeStage].duration);

        OnTreeStageEnded(stage);
    }

    public void OnTreeStageEnded(int stage)
    {
        StopCoroutine(TreeStageCoroutine);
        TreeStageCoroutine = null;
        print("Tree stage " + stage + " ended");

        if (stage + 1 < stagesSetup.Length)
        {
            RequestStartTreeStage(stage+1);
        }
    }

    void startTree()
    {
        RequestStartTreeStage(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Tree not started!!!");
        // startTree();
    }

    private float lastWeightUpdateTime;

    // Update is called once per frame
    void Update()
    {
        
    }
}
