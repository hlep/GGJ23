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
}

public class TreeController : MonoBehaviour
{
    [SerializeField] private SpriteResolver spriteResolver;
    [SerializeField] private SpriteLibrary spriteLibrary;
    [SerializeField] private TreeStageSetup[] stagesSetup;

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
        spriteResolver.SetCategoryAndLabel(spriteResolver.GetCategory(), stagesSetup[stage].spriteLabel);
        print("Tree stage " + stage + " started");

        yield return new WaitForSeconds(stagesSetup[stage].duration);

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
        startTree();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
