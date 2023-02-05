using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsTracker : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    int MaxActions = 2; //0 is also an action

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

        m_CurrentActions += ActionChange;
    }

    public bool HasFreeActions() { return m_CurrentActions > -1; }
}
