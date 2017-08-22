using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SetDungeonMap
{

    float panelTop = 0;
    float panelLeft = 0;
    float panelRight = 0;
    float panelBottom = 0;

    public Vector2 SetSize(List<DungeonModelNew> generatedDungeon)
    {
        panelTop = generatedDungeon.Max(t => t.roomPosition.y); //find Top
        panelBottom = generatedDungeon.Min(t => t.roomPosition.y); //find Bottom
        panelRight = generatedDungeon.Max(t => t.roomPosition.x); //find Right
        panelLeft = generatedDungeon.Min(t => t.roomPosition.x); //find Left

        return (new Vector2(panelTop > Mathf.Abs(panelBottom) ? panelTop : Mathf.Abs(panelBottom),
            panelRight > Mathf.Abs(panelLeft) ? panelRight : Mathf.Abs(panelLeft)));
    }

    public Vector2 CalculateMaxScrollMap()
    {
        float maxPanelTopBottom = 0;
        float maxPanelRightLeft = 0;

        if (panelTop >= Mathf.Abs(panelBottom))
            maxPanelTopBottom = panelTop;
        else
            maxPanelTopBottom = panelBottom;

        if (panelRight >= Mathf.Abs(panelLeft))
            maxPanelRightLeft = panelRight;
        else
            maxPanelRightLeft = panelLeft;

        return new Vector2(SetMaxNormalization(FindNormalizationPerRoom(maxPanelTopBottom, maxPanelRightLeft)).x
            , SetMaxNormalization(FindNormalizationPerRoom(maxPanelTopBottom, maxPanelRightLeft)).y);
    }

    public Vector2 FindNormalizationPerRoom(float maxPanelTopBottom, float maxPanelRightLeft)
    {
        float PositionPerRoomTopBottom;
        float PositionPerRoomRightLeft;

        if (maxPanelTopBottom >= 0)
        {
            PositionPerRoomTopBottom = 0.5f / ((maxPanelTopBottom) / 150f);
        }
        else
        {
            PositionPerRoomTopBottom = 0.5f / ((maxPanelTopBottom) / 150f);
        }

        if (maxPanelRightLeft >= 0)
        {
            PositionPerRoomRightLeft = 0.5f / ((maxPanelRightLeft) / 150f);
        }
        else
        {
            PositionPerRoomRightLeft = 0.5f / ((maxPanelRightLeft) / 150f);
        }

        return new Vector2(PositionPerRoomRightLeft, PositionPerRoomTopBottom);
    }

    public Vector2 SetMaxNormalization(Vector2 NormalizationPerRoom)
    {
        float verticalNormalPosition;
        float horizontalNormalPosition;

        if (NormalizationPerRoom.y >= 0)
        {
            verticalNormalPosition = 0.5f + (NormalizationPerRoom.y * (panelBottom / 150f));
        }
        else
        {
            verticalNormalPosition = 0.5f + (NormalizationPerRoom.y * (panelTop / 150f));
        }

        if (NormalizationPerRoom.x >= 0)
        {
            horizontalNormalPosition = 0.5f + (NormalizationPerRoom.x * (panelLeft / 150f));
        }
        else
        {
            horizontalNormalPosition = 0.5f + (NormalizationPerRoom.x * (panelRight / 150f));
        }

        return new Vector2(horizontalNormalPosition, verticalNormalPosition);
    }

}
