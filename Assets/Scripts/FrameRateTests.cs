using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateTests : MonoBehaviour
{

    public int targetFrameRate = 60;

    void Start()
    {
        Application.targetFrameRate = targetFrameRate;
        QualitySettings.vSyncCount = 0;
    }
}
