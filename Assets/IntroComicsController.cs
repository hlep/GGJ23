using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class IntroComicsController : MonoBehaviour
{
    [SerializeField] private SpriteResolver spriteResolver;
    [SerializeField] private SpriteLibrary spriteLibrary;
    [SerializeField] private float spriteDuration;
    [SerializeField] private string[] labels;
    [SerializeField] private TreeController treeController;

    private int currentStage = 0;

    private Coroutine IntroComicsCoroutine = null;

    public void RequestStartComicsStage(int stage)
    {
        if (IntroComicsCoroutine != null)
        {
            print("IntroComics coroutine already active");
            return;
        }

        IntroComicsCoroutine = StartCoroutine(StartComicsStage(stage));
    }

    private IEnumerator StartComicsStage(int stage)
    {
        print("Comics stage " + stage + " started");

        currentStage = stage;

        spriteResolver.SetCategoryAndLabel(spriteResolver.GetCategory(), labels[currentStage]);
        
        yield return new WaitForSeconds(spriteDuration);

        OnComicsStageEnded(stage);
    }

    public void OnComicsStageEnded(int stage)
    {
        StopCoroutine(IntroComicsCoroutine);
        IntroComicsCoroutine = null;
        print("Comics stage " + stage + " ended");

        if (stage + 1 < labels.Length)
        {
            RequestStartComicsStage(stage + 1);
        }
        else
        {
            EndComics();
        }
    }

    public void EndComics()
    {
        SpriteRenderer spriteRenderer = GetComponentInParent<SpriteRenderer>();
        spriteRenderer.sortingOrder = -1000;
        treeController.startTree();
    }

    // Start is called before the first frame update
    void Start()
    {
        RequestStartComicsStage(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
