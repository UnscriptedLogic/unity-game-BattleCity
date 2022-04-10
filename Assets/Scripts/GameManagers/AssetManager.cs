using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetManager : MonoBehaviour
{
    private static AssetManager _instance;

    public static AssetManager instance
    {
        get
        {
            if (_instance == null) _instance = (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<AssetManager>();
            return _instance;
        }
    }

    public GameObject forcefield;
    public GameObject yellowforcefield;
    public GameObject piercingAura;
    public GameObject timeStopAura;
    public GameObject timeStopAuraIndividual;
}
