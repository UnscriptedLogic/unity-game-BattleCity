using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportHyperLinks : MonoBehaviour
{
    public void OpenBMAC()
    {
        Application.OpenURL("https://www.buymeacoffee.com/nat.alacarte");
    }

    public void OpenItch()
    {
        Application.OpenURL("https://unscriptedlogic.itch.io/");
    }
}
