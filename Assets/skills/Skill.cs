using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Objects/Skill")]
public class Skill : ScriptableObject
{
    public int id;
    public Texture2D Icon;
    public string Name;
    public string Description;
    public bool OnlyOne;
}
