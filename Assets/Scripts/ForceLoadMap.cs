using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ForceLoadMap : MonoBehaviour
{
    public int mapIndex;

    private void Start()
    {
        SceneManager.LoadSceneAsync(mapIndex, LoadSceneMode.Additive);
    }
}
