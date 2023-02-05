using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct MineralLayerSetup
{
    [SerializeField] public int count;
}

[Serializable]
public struct MineralLayerState
{
    private GameObject[] Minerals;
}

// TODO: add minerals creation and saving

public class MineralManager : MonoBehaviour
{
    [SerializeField] private GameObject MineralPrefab;
    [SerializeField] private EarthRandomizer earthRandomizer;
    [SerializeField] private Vector3 StartPoint;
    [SerializeField] private float RowDistance;
    [SerializeField] MineralLayerSetup[] MineralLayersSetup;

    private MineralLayerState[] mineralLayerStates;


    void Generator_GenerateMinerals()
    {
        Vector3[] leftPoints = earthRandomizer.GetIslandLeftPoints();
        Vector3[] rightPoints = earthRandomizer.GetIslandRightPoints();

        mineralLayerStates = new MineralLayerState[earthRandomizer.GetRowsCount() - 1];
        for (int i = 0; i < earthRandomizer.GetRowsCount() - 1; i++)
        {
            if (i >= MineralLayersSetup.Length)
            {
                Debug.LogWarning("No mineral setup for layer " + i);
                break;
            }

            for (int j = 0; j < MineralLayersSetup[i].count; j++)
            {
                float mineralX = UnityEngine.Random.Range((leftPoints[i].x + leftPoints[i + 1].x) / 2, (rightPoints[i].x + rightPoints[i + 1].x) / 2);
                float mineralY = StartPoint.y + i * RowDistance;
                Vector3 mineralPoint = new Vector3(mineralX, mineralY);
                Instantiate(MineralPrefab, mineralPoint, Quaternion.identity);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Generator_GenerateMinerals();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
