using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct MineralLayerSetup
{
    [SerializeField] public int count;
    [SerializeField] public float MaxDownOffset;
    [SerializeField] public float MaxUpOffset;
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
    [SerializeField] private float SideOffsetFromEarthBorder = 0;
    [SerializeField] MineralLayerSetup[] MineralLayersSetup;

    [SerializeField] EnergyManager m_EnergyManager;
    [SerializeField] ActionsTracker m_ActionsTracker;

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

            float minMineralX = (leftPoints[i].x + leftPoints[i + 1].x) / 2 + SideOffsetFromEarthBorder;
            float maxMineralX = (rightPoints[i].x + rightPoints[i + 1].x) / 2 - SideOffsetFromEarthBorder;
            float baseMineralY = StartPoint.y + i * RowDistance;
            float minMineralY = baseMineralY - MineralLayersSetup[i].MaxDownOffset;
            float maxMineralY = baseMineralY + MineralLayersSetup[i].MaxUpOffset;

            /*Debug.DrawLine(new Vector3(minMineralX, baseMineralY), new Vector3(maxMineralX, baseMineralY), Color.red, 20);
            Debug.DrawLine(new Vector3(minMineralX, minMineralY), new Vector3(maxMineralX, minMineralY), Color.yellow, 20);
            Debug.DrawLine(new Vector3(minMineralX, maxMineralY), new Vector3(maxMineralX, maxMineralY), Color.yellow, 20);*/

            for (int j = 0; j < MineralLayersSetup[i].count; j++)
            {
                float mineralX = UnityEngine.Random.Range(minMineralX, maxMineralX);
                float mineralY = UnityEngine.Random.Range(minMineralY, maxMineralY);
                Vector3 mineralPoint = new Vector3(mineralX, mineralY);
                GameObject spawned = Instantiate(MineralPrefab, mineralPoint, Quaternion.identity);
                CrystalLogic logic = spawned.GetComponent<CrystalLogic>();
                logic.m_ActionsTracker = m_ActionsTracker;
                logic.m_EnergyManager = m_EnergyManager;
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
