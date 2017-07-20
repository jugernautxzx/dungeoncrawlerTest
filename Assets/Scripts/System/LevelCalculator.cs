using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelCalculator{

    public static bool CalculateLevelUp(int exp, int level)
    {
        return CalculateGainedLevel(exp, level) > 0;
    }

    public static int CalculateGainedLevel(int exp, int level)
    {
        int gained = Mathf.FloorToInt(exp / 50);
        return gained;
    }
}
