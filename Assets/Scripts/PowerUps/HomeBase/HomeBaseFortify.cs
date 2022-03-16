using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeBaseFortify : MonoBehaviour
{
    public GameObject emptyWall;
    public float duration;
    private BaseBlockManager baseBlockManager;

    public void Reconstruct(BaseBlockManager blockManager)
    {
        baseBlockManager = blockManager;

        for (int i = 0; i < blockManager.baseWallManagers.Count; i++)
        {
            baseBlockManager.baseWallManagers[i].myWallType = BlockName.Fortified;
            baseBlockManager.baseWallManagers[i].InitializeEntity();
            baseBlockManager.baseWallManagers[i].ReInitializeGraphics();

            baseBlockManager.baseWallManagers[i].GetComponent<BlockHealth>().root.SetActive(true);
        }

        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(duration);

        Deconstruct();
    }

    public void Deconstruct()
    {
        for (int i = 0; i < baseBlockManager.baseWallManagers.Count; i++)
        {
            BlockManager blockManager = baseBlockManager.baseWallManagers[i];
            blockManager.myWallType = BlockName.Normal;
            blockManager.InitializeEntity();
            blockManager.ReInitializeGraphics();
        }
    }
}