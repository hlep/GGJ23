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
        m_CurrentActions += ActionChange;
        print(m_CurrentActions);
        if (ActionChange > 0) { ActionIcons[m_CurrentActions].sprite = ActiveActionSprite; }
        else if(ActionChange < 0) { ActionIcons[m_CurrentActions].sprite = EmptyActionSprite; }
    }

    public bool HasFreeActions() { return m_CurrentActions > 0;}
}
