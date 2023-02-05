using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public float[] weightDifferenceForBreakStage;
    public float weightDifferenceForLose;

    private Vector3 StartTreePosition;
    [SerializeField] private Vector3 LoseTreePosition;
    [SerializeField] private float LoseTime;

    private bool bTreeWorking = false;
    private bool bIsLosingInProgress = false;
    private bool bLost = false;

    private int currentTreeStage = -1;
    private int currentBreakStage = -1;
    private float currentWeight = 0;
    private float compensatedWeight = 0;

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
        // Debug.Log("Tree not started!!!");
        StartTreePosition = transform.position;
        bTreeWorking = true;
        startTree();
    }

    void LoseGame()
    {
        StopCoroutine(TreeStageCoroutine);
        loseStartTime = Time.time;
        bIsLosingInProgress = true;
        bTreeWorking = false;
    }

    private float loseStartTime;
    void UpdateLosePosition()
    {
        float timeSinceStart = Time.time - loseStartTime;
        float finishPercent = timeSinceStart / LoseTime;
        if (finishPercent > 1)
        {
            transform.position = LoseTreePosition;
            bIsLosingInProgress = false;
            bLost = true;
        }
        else
        {
            transform.position = Vector3.Lerp(StartTreePosition, LoseTreePosition, finishPercent);
        }
    }

    public void UpdateBreakStage()
    {
        spriteResolver.SetCategoryAndLabel(spriteResolver.GetCategory(), stagesSetup[currentTreeStage].brokenSpriteLabels[currentBreakStage]);
    }

    // Update is called once per frame
    void Update()
    {
        if (bIsLosingInProgress)
        {
            UpdateLosePosition();
        }

        if (!bTreeWorking)
        {
            return;
        }

        if (currentTreeStage < 0 || currentTreeStage >= stagesSetup.Length)
        {
            return;
        }

        TreeStageSetup currentStageSetup = stagesSetup[currentTreeStage];
        if (currentStageSetup.brokenSpriteLabels.Length > 0)
        {
            if (currentWeight - compensatedWeight > weightDifferenceForLose)
            {
                LoseGame();
                return;
            }

            int nextBreakStage = currentBreakStage + 1;
            if (nextBreakStage < currentStageSetup.brokenSpriteLabels.Length)
            {
                if (currentWeight - compensatedWeight > weightDifferenceForBreakStage[nextBreakStage])
                {
                    currentBreakStage++;
                }
            }

            UpdateBreakStage();
        }
        else if (currentBreakStage != -1)
        {
            currentBreakStage = -1;
        }
    }
}
