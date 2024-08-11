using UnityEngine;

public class TextureChanger : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;

    public void ChangeMaterial(Material newMat, int materialIndex = Constants.GRID_CELL_MATERIAL_INDEX)
    {
        if (materialIndex == 0)
        {
            _renderer.sharedMaterial = newMat;
            return;
        }

        var mats = _renderer.sharedMaterials;
        
        mats[materialIndex] = newMat;
        
        _renderer.sharedMaterials = mats;
    }
}