using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartUIManager : MonoBehaviour
{
    public TextMeshProUGUI versionNumber;

    private void Start()
    {
        versionNumber.text = "Version: " + Application.version;

    }
}
