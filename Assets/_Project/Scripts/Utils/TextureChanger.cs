using UnityEngine;

public class TextureChanger : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    private MaterialPropertyBlock _propertyBlock;
    private static readonly int MainTex = Shader.PropertyToID(Constants.MAIN_TEX);

    private void Awake()
    {
        _propertyBlock = new MaterialPropertyBlock();
    }

    public void ChangeTexture(Texture texture, int materialIndex = Constants.GRID_CELL_MATERIAL_INDEX)
    {
        if (_renderer == null || texture == null)
            return;

        _renderer.GetPropertyBlock(_propertyBlock, materialIndex);

        _propertyBlock.SetTexture(MainTex, texture);

        _renderer.SetPropertyBlock(_propertyBlock, materialIndex);
    }
}