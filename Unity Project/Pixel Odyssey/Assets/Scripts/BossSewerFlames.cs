using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSewerFlames : MonoBehaviour
{
    [SerializeField] public GameObject[] gameObjects;
    public float sewerFlameTime;
    bool objectActive = false;
    float sewerFlameDelay = 15;
    int j;

    void Start()
    {
        Randomize();
    }
    //for each object in gameObjects spawn one
    IEnumerator sewerFlame(GameObject gameObject)
    {
        objectActive = true;
        gameObject.SetActive(true);
        yield return new WaitForSeconds(sewerFlameTime);
        gameObject.SetActive(false);
        yield return new WaitForSeconds(sewerFlameDelay);
        objectActive = false;
        Randomize();
    }
    public void Randomize()
    {
        if (!objectActive)
        {

            int h = Random.Range(0, gameObjects.Length);
            if (j != h)
            {
                j = h;
                StartCoroutine(sewerFlame(gameObjects[h]));
            }
            else
            {
                Randomize();
            }
        }
    }
}
