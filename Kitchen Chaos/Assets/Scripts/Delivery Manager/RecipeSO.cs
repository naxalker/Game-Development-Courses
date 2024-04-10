using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RecipeSO : ScriptableObject
{
    [field: SerializeField] public List<KitchenObjectSO> KitchenObjetSOList { get; private set; }
    [field: SerializeField] public string RecipeName;
}
