using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelSpawn : MonoBehaviour
{
    public DisplayInventory displayInventory;
    public GameObject nextLevelBeam;
    private bool hasSpawned = false;

    private void Start()
    {
        if(nextLevelBeam != null)
            nextLevelBeam.SetActive(false);

        StartCoroutine(CheckGlyphCount());
    }

    private IEnumerator CheckGlyphCount()
    {
        while (!hasSpawned)
        {
            if (displayInventory.glyphCount >= 5)
            {
                nextLevelBeam.SetActive(true);
                hasSpawned = true;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void SpawnNextLevel()
    {
        if (nextLevelBeam != null)
        {
            nextLevelBeam.SetActive(true);
        }
    }
}
