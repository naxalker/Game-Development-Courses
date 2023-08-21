using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumberController : MonoBehaviour
{
    public static DamageNumberController Instance;

    [SerializeField] DamageNumber numberToSpawn;
    [SerializeField] Transform numberCanvas;

    private List<DamageNumber> numberPool = new List<DamageNumber>();

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnDamage(float damage, Vector3 location)
    {
        int rounded = Mathf.RoundToInt(damage);

        //DamageNumber newDamage = Instantiate(numberToSpawn, location, Quaternion.identity, numberCanvas);
        DamageNumber newDamage = GetFromPool();

        newDamage.gameObject.SetActive(true);
        newDamage.Setup(rounded);
        newDamage.transform.position = location;
    }

    private DamageNumber GetFromPool()
    {
        DamageNumber numberToOutput = null;

        if (numberPool.Count == 0)
        {
            numberToOutput = Instantiate(numberToSpawn, numberCanvas);
        } else
        {
            numberToOutput = numberPool[0];
            numberPool.RemoveAt(0);
        }

        return numberToOutput;
    }

    public void PlaceInPool(DamageNumber numberToPlace)
    {
        numberToPlace.gameObject.SetActive(false);

        numberPool.Add(numberToPlace);
    }
}
