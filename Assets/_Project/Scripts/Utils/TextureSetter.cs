using UnityEngine;

public class TextureChanger : MonoBehaviour
{
    [SerializeField] private Texture _newTexture;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private int materialIndex = 0;
    private MaterialPropertyBlock _propertyBlock;
    private static readonly int MainTex = Shader.PropertyToID(Constants.MainTex);

    private void Awake()
    {
        _propertyBlock = new MaterialPropertyBlock();
    }

    private void Start()
    {
        ChangeTexture(_newTexture);
    }

    public void ChangeTexture(Texture texture)
    {
        if (_renderer == null || texture == null)
            return;

        _renderer.GetPropertyBlock(_propertyBlock, materialIndex);

        _propertyBlock.SetTexture(MainTex, texture);

        _renderer.SetPropertyBlock(_propertyBlock, materialIndex);
    }
}