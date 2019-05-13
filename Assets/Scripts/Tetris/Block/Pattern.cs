using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern {

    int[] affectedLines;
    int score;
    int rewardedLines;

    public Pattern(int[] affectedLines)
    {
        this.affectedLines = affectedLines;
        switch (affectedLines.Length)
        {
            case 0:
                score = 0;
                rewardedLines = 0;
                break;
            case 1:
                score = 100;
                rewardedLines = 1;
                break;
            case 2:
                score = 300;
                rewardedLines = 3;
                break;
            case 3:
                score = 500;
                rewardedLines = 5;
                break;
            case 4:
                score = 800;
                rewardedLines = 8;
                break;
            default:
                throw new System.Exception("Too many lines erased at once.");
        }
    }

    public int getRewardedLines()
    {
        return rewardedLines;
    }

    public int getScore()
    {
        return score;
    }

    public int[] getAffectedLines()
    {
        return affectedLines;
    }
}