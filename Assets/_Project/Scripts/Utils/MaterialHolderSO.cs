using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "MaterialHolder", menuName = "MaterialHolder", order = 0)]
public class MaterialHolderSO : ScriptableObject
{
    [SerializeField] private SerializedDictionary<ContentColor, Material> _materials = new ();
    
    public Material GetMaterialByColor(ContentColor color)
    {
        return _materials.GetValueOrDefault(color);
    }
}