using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeBaseFortify : MonoBehaviour
{
    public GameObject emptyWall;

    public float duration;

    public void Reconstruct(GameManager gameManager)
    {
        for (int i = 0; i < gameManager.baseWalls.Count; i++)
        {
            Transform entity = gameManager.baseWalls[i];
            BlockManager blockManager = entity.GetChild(0).GetComponentInChildren<BlockManager>();
            blockManager.myWallType = BlockName.Fortified;
            blockManager.InitializeEntity();
            blockManager.ReInitializeGraphics();

            entity.gameObject.SetActive(true);
        }

        StartCoroutine(Delay(gameManager));
    }

    private IEnumerator Delay(GameManager gameManager)
    {
        yield return new WaitForSeconds(duration);

        Deconstruct(gameManager);
    }

    public void Deconstruct(GameManager gameManager)
    {
        for (int i = 0; i < gameManager.baseWalls.Count; i++)
        {
            BlockManager blockManager = gameManager.baseWalls[i].GetChild(0).GetComponentInChildren<BlockManager>();
            blockManager.myWallType = BlockName.Normal;
            blockManager.InitializeEntity();
            blockManager.ReInitializeGraphics();
        }
    }
}