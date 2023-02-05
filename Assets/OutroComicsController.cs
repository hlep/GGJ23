using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class OutroComicsController : MonoBehaviour
{
    [SerializeField] private SpriteResolver spriteResolver;
    [SerializeField] private SpriteLibrary spriteLibrary;
    [SerializeField] private float spriteDuration;
    [SerializeField] private string[] labels;

    private int currentStage = 0;

    private Coroutine OutroComicsCoroutine = null;

    public void RequestStartComicsStage(int stage)
    {
        if (OutroComicsCoroutine != null)
        {
            print("OutroComics coroutine already active");
            return;
        }

        OutroComicsCoroutine = StartCoroutine(StartComicsStage(stage));
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
        StopCoroutine(OutroComicsCoroutine);
        OutroComicsCoroutine = null;
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
        print("end");
    }

    public void StartComics()
    {
        SpriteRenderer spriteRenderer = GetComponentInParent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 1000;
        RequestStartComicsStage(0);
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
