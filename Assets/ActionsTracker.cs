using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsTracker : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    int MaxActions = 3;

    [SerializeField]
    public SpriteRenderer[] ActionIcons;

    [SerializeField]
    public SpriteRenderer Sprite1;
    [SerializeField]
    public SpriteRenderer Sprite2;
    [SerializeField]
    public SpriteRenderer Sprite3;

    [SerializeField]
    Sprite ActiveActionSprite;

    [SerializeField]
    Sprite EmptyActionSprite;

    int m_CurrentActions = 0;
    void Start()
    {
        m_CurrentActions = MaxActions;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateActions(int ActionChange)
    {
        print(m_CurrentActions);
        if (ActionChange > 0) { ActionIcons[m_CurrentActions].sprite = ActiveActionSprite; }
        else if (ActionChange < 0) { ActionIcons[m_CurrentActions - 1].sprite = EmptyActionSprite; }
/*
        switch (m_CurrentActions)
        {
            case 0:
                Sprite1.sprite = ActionChange > 0 ? ActiveActionSprite : EmptyActionSprite;
                break;
            case 1:
                Sprite2.sprite = ActionChange > 0 ? ActiveActionSprite : EmptyActionSprite;
                break;
            case 2:
                Sprite3.sprite = ActionChange > 0 ? ActiveActionSprite : EmptyActionSprite;
                break;

        }*/

        m_CurrentActions += ActionChange;
        // print(m_CurrentActions);
    }

    public bool HasFreeActions() { return m_CurrentActions > 0; }
}
