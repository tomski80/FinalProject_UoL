using UnityEngine;

[CreateAssetMenu(fileName = "CraftPart", menuName = "ScriptableObjects/CraftPart", order = 1)]
public class CraftPartInfo : ScriptableObject
{
    public string prefabName;
    public GameObject prefab;
    public Sprite Image;
    public string tag;
    public int id;
}
