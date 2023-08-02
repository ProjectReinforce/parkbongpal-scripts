using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill")]
public class Skill : ScriptableObject
{
    public string skillName, description, effect;
    public Sprite Icon;
}