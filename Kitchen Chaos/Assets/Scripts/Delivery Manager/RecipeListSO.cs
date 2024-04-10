using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RecipeListSO : ScriptableObject
{
    [field: SerializeField] public List<RecipeSO> AllRecipes;
}
