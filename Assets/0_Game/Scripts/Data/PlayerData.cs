using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static void SetMaxScore(int maxScore)
    {
        PlayerPrefs.SetInt("maxScore", maxScore);
    }

    public static int GetMaxScore()
    {
        return PlayerPrefs.GetInt("maxScore", 0);
    }
}
