using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "TextureHolder", menuName = "TextureHolder", order = 0)]
public class TextureHolderSO : ScriptableObject
{
    [SerializeField] private SerializedDictionary<ContentColor, Texture> _textures = new ();
    
    public Texture GetTextureByColor(ContentColor color)
    {
        return _textures.GetValueOrDefault(color);
    }
}