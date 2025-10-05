using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSkill : MonoBehaviour
{
    public int id;
    public TMP_Text Name;
    public TMP_Text Description;
    public GameObject Icon;
	public Skill SkillData;

	public void Buy()
    {
        Player.BuySkill?.Invoke(SkillData);
    }

	public void UpdateSkill(Skill skillData)
    {
        SkillData = skillData;

		id = skillData.id;
		Name.text = skillData.Name;
		Description.text = skillData.Description;
        Icon.GetComponent<RawImage>().texture = skillData.Icon;
    }
}
