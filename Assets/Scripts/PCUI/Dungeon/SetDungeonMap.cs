using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SetDungeonMap {

    public Vector2 SetSize(List<DungeonModelNew> generatedDungeon)
    {
        float panelTop = 0;
        float panelLeft = 0;
        float panelRight = 0;
        float panelBottom = 0;

        panelTop =generatedDungeon.Max(t => t.roomPosition.y); //find Top
        panelBottom= Mathf.Abs(generatedDungeon.Min(t => t.roomPosition.y)); //find Bottom
        panelRight = generatedDungeon.Max(t => t.roomPosition.x); //find Right
        panelLeft = Mathf.Abs(generatedDungeon.Min(t => t.roomPosition.x)); //find Left

        return (new Vector2(panelRight > panelLeft ? panelRight : panelLeft, 
            panelTop > panelBottom ? panelTop : panelBottom));
    }

}
