using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject ene_mask;
    public GameObject ene_needle;
    private int xPos;
    private int zPos;
    private int objectToGenerate;
    private int objectQuantity=0;

    [SerializeField]
    public int total;
    public int xrange1;
    public int xrange2;
    public int zrange1;
    public int zrange2;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GenerateObjects());
    }

    IEnumerator GenerateObjects()
    {
        while (objectQuantity < total)
        {
            objectToGenerate = Random.Range(1,2);
            // xPos = Random.Range(-106, -5);
            // zPos = Random.Range(-44,56);
            xPos = Random.Range(xrange1, xrange2);
            zPos = Random.Range(zrange1, zrange2);

            // Instantiate(ene, new Vector3(xPos, 0, zPos), Quaternion.identity);

            
            if (objectToGenerate == 1)
            {
                Instantiate(ene_mask, new Vector3(xPos, 0, zPos), Quaternion.identity);
            }
            if (objectToGenerate == 2)
            {
                Instantiate(ene_needle, new Vector3(xPos, 0, zPos), Quaternion.identity);
            }

            yield return new WaitForSeconds(0.1f);
            objectQuantity++;
        }
    }
}
