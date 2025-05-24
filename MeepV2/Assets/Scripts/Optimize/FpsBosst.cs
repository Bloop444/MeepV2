using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsBosst : MonoBehaviour
{
    public int boost;

    private void Update()
    {
        Application.targetFrameRate = boost;
    }
}
