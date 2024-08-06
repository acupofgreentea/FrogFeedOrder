using UnityEngine;

public class GameManager : MonoBehaviour
{
    [field: SerializeField] public GridCellPrefabHolder GridCellPrefabHolder { get; private set; }
    [field: SerializeField] public TextureHolderSO GrapeTextureHolder { get; private set; }
    [field: SerializeField] public TextureHolderSO FrogTextureHolder { get; private set; }
    [field: SerializeField] public TextureHolderSO SquareTextureHolder { get; private set; }
    
    [field: SerializeField] public AudioClip PopClip { get; private set; }
    
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}