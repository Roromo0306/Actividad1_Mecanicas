using UnityEngine;

[CreateAssetMenu(menuName = "Potions/Potion Data")]
public class PotionData : ScriptableObject
{
    public PotionResultType potionType;

    public Sprite undiscoveredIcon;
    public Sprite discoveredIcon;

    [TextArea]
    public string description;
}