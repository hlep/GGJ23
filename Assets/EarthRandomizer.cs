using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

[Serializable]
public struct RowSetup
{
    public RowSetup(int LOC, int LRC, int RRC, int ROC)
    {
        leftRequiredCells= LRC;
        rightRequiredCells= RRC;
        leftOptionalCells= LOC;
        rightOptionalCells= ROC;
    }
    public int leftRequiredCells;
    public int rightRequiredCells;

    public int leftOptionalCells;
    public int rightOptionalCells;

    public int GetSize()
    {
        return leftOptionalCells + leftRequiredCells * rightRequiredCells + rightOptionalCells;
    }
}



public class EarthRandomizer : MonoBehaviour
{
    [SerializeField] private SpriteShapeController m_EarthShape;
    [SerializeField] private Vector3 startLocation;
    [SerializeField] private Vector2 cellSize;
    [SerializeField] private ShapeTangentMode shapeTangentMode = ShapeTangentMode.Continuous;

    private bool bIsIslandGenerated = false;

    private Vector3[] leftPoints;
    private Vector3[] rightPoints;

    [SerializeField] private RowSetup[] GridSetup = {
        new RowSetup(2, 3, 3, 2),
        new RowSetup(1, 6, 6, 1),
        new RowSetup(2, 7, 7, 2),
        new RowSetup(1, 8, 8, 1),
        new RowSetup(1, 8, 8, 1),
        new RowSetup(1, 8, 8, 1),
        new RowSetup(2, 5, 5, 2),
    };

    public int GetRowsCount() { return GridSetup.Length;  }
    public Vector3[] GetIslandLeftPoints() { return leftPoints; }
    public Vector3[] GetIslandRightPoints() { return rightPoints; }

    void Debug_DrawMaximizedIsland(Color color, float time, bool bWithHorizontals)
    {
        for (int i = 0; i < GridSetup.Length; i++)
        {
            int currentLeftMaxCellsInclusive = GridSetup[i].leftRequiredCells + GridSetup[i].leftOptionalCells;
            int currentRightMaxCellsInclusive = GridSetup[i].rightRequiredCells + GridSetup[i].rightOptionalCells;
            Vector3 leftPos = startLocation + new Vector3(-currentLeftMaxCellsInclusive * cellSize.x, i * cellSize.y);
            Vector3 rightPos = startLocation + new Vector3(currentRightMaxCellsInclusive * cellSize.x, i * cellSize.y);

            if (i == 0)
            {
                Debug.DrawLine(startLocation, leftPos, color, time);
                Debug.DrawLine(startLocation, rightPos, color, time);
                if (bWithHorizontals)
                    Debug.DrawLine(leftPos, rightPos, color, time);
            }
            else
            {
                int prevLeftMaxCellsExclusive = GridSetup[i - 1].leftRequiredCells + GridSetup[i - 1].leftOptionalCells;
                int prevRightMaxCellsExclusive = GridSetup[i - 1].rightRequiredCells + GridSetup[i - 1].rightOptionalCells;
                Vector3 prevLeftPos = startLocation + new Vector3(-prevLeftMaxCellsExclusive * cellSize.x, (i - 1) * cellSize.y);
                Vector3 prevRightPos = startLocation + new Vector3(prevRightMaxCellsExclusive * cellSize.x, (i - 1) * cellSize.y);

                Debug.DrawLine(prevLeftPos, leftPos, color, time);
                Debug.DrawLine(prevRightPos, rightPos, color, time);
                if (bWithHorizontals)
                    Debug.DrawLine(leftPos, rightPos, color, time);
            }

            if (i == GridSetup.Length - 1)
            {
                Debug.DrawLine(leftPos, rightPos, color, time);
            }
        }
    }

    void Debug_DrawMinimizedIsland(Color color, float time)
    {
        for (int i = 0; i < GridSetup.Length; i++)
        {
            int currentLeftMinCellsInclusive = GridSetup[i].leftRequiredCells;
            int currentRightMinCellsInclusive = GridSetup[i].rightRequiredCells;
            Vector3 leftPos = startLocation + new Vector3(-currentLeftMinCellsInclusive * cellSize.x, i * cellSize.y);
            Vector3 rightPos = startLocation + new Vector3(currentRightMinCellsInclusive * cellSize.x, i * cellSize.y);

            if (i == 0)
            {
                Debug.DrawLine(startLocation, leftPos, color, time);
                Debug.DrawLine(startLocation, rightPos, color, time);
            }
            else
            {
                int prevLeftMinCellsExclusive = GridSetup[i - 1].leftRequiredCells;
                int prevRightMinCellsExclusive = GridSetup[i - 1].rightRequiredCells;
                Vector3 prevLeftPos = startLocation + new Vector3(-prevLeftMinCellsExclusive * cellSize.x, (i - 1) * cellSize.y);
                Vector3 prevRightPos = startLocation + new Vector3(prevRightMinCellsExclusive * cellSize.x, (i - 1) * cellSize.y);

                Debug.DrawLine(prevLeftPos, leftPos, color, time);
                Debug.DrawLine(prevRightPos, rightPos, color, time);
            }

            if (i == GridSetup.Length - 1)
            {
                Debug.DrawLine(leftPos, rightPos, color, time);
            }
        }
    }

    void Debug_DrawVerticalLines(Color color, float time)
    {
        int maxCells = 0;
        for (int i = 0; i < GridSetup.Length; i++)
        {
            maxCells = Mathf.Max(maxCells, GridSetup[i].GetSize());
        }

        for (int i = 0; i < maxCells; i++)
        {
            Vector3 leftColumnStartLocation = startLocation + Vector3.left * i * cellSize.x;
            Vector3 rightColumnStartLocation = startLocation + Vector3.right * i * cellSize.x;

            Debug.DrawLine(leftColumnStartLocation, leftColumnStartLocation + Vector3.up * cellSize.y * GridSetup.Length, color, time);
            Debug.DrawLine(rightColumnStartLocation, rightColumnStartLocation + Vector3.up * cellSize.y * GridSetup.Length, color, time);
        }
    }


    int Generator_GetMinCellsInclusive(int rowIndex, int requiredCells, int[] cellsCountByRow)
    {
        int halfRowIndex = Mathf.CeilToInt(GridSetup.Length / 2);
        if (rowIndex == 0 || rowIndex > halfRowIndex)
        {
            return requiredCells;
        }

        return Mathf.Max(cellsCountByRow[rowIndex - 1], requiredCells);
    }

    int Generator_GetMaxCellsExclusive(int rowIndex, int requiredCells, int optionalCells, int[] cellsCountByRow)
    {
        int halfRowIndex = Mathf.CeilToInt(GridSetup.Length / 2);
        if (rowIndex <= halfRowIndex)
        {
            return requiredCells + optionalCells + 1;
        }

        return Mathf.Min(cellsCountByRow[rowIndex - 1] + 1, requiredCells + optionalCells + 1);
    }

    void Generator_GenerateIsland()
    {
        leftPoints = new Vector3[GridSetup.Length];
        rightPoints = new Vector3[GridSetup.Length];
        int[] leftCellsCountByRow = new int[GridSetup.Length];
        int[] rightCellsCountByRow = new int[GridSetup.Length];

        for (int i = 0; i < GridSetup.Length; i++)
        {
            RowSetup Row = GridSetup[i];
            int leftMinCellsInclusive = Generator_GetMinCellsInclusive(i, Row.leftRequiredCells, leftCellsCountByRow);
            int rightMinCellsInclusive = Generator_GetMinCellsInclusive(i, Row.rightRequiredCells, rightCellsCountByRow);

            int leftMaxCellsExclusive = Generator_GetMaxCellsExclusive(i, Row.leftRequiredCells, Row.leftOptionalCells, leftCellsCountByRow);
            int rightMaxCellsExclusive = Generator_GetMaxCellsExclusive(i, Row.rightRequiredCells, Row.rightOptionalCells, rightCellsCountByRow);

            leftCellsCountByRow[i] = UnityEngine.Random.Range(leftMinCellsInclusive, leftMaxCellsExclusive);
            rightCellsCountByRow[i] = UnityEngine.Random.Range(rightMinCellsInclusive, rightMaxCellsExclusive);

            leftPoints[i] = startLocation + new Vector3(-leftCellsCountByRow[i] * cellSize.x, i * cellSize.y);
            rightPoints[i] = startLocation + new Vector3(rightCellsCountByRow[i] * cellSize.x, i * cellSize.y);
        }

        var result = leftPoints.Prepend(startLocation).Concat(rightPoints.Reverse()).ToArray();

        try
        {
            for (int i = 0; i < result.Length; i++)
            {
                // first three points are predefined
                if (i < 3)
                {
                    m_EarthShape.spline.SetPosition(i, result[i]);
                }
                else
                {
                    m_EarthShape.spline.InsertPointAt(i, result[i]);
                }
                
                m_EarthShape.spline.SetTangentMode(i, shapeTangentMode);
            }
        }
        catch
        {
            Debug.Log("Wow");
        }

        m_EarthShape.RefreshSpriteShape();
        bIsIslandGenerated = true;
    }


    void Awake()
    {
        Generator_GenerateIsland();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
