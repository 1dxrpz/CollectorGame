using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	public static float Rage = 0;
	public static float MaxRage = 200;
	public static int Debt = 0;
	public static int MaxAmmo = 2;
	public static float MaxShotDelay = 10;
	public static float ReloadTime = 2;
	public static bool HasSecondShotgun = false;
	public static float DamageMultiplier = 1;
	public static float EnemyDamageMultiplier = 1;
	public static float EnemyHealthMultiplier = 1;
	public static float RageMultiplier = 1;


	public float Health = 100.0f;
	public List<GameObject> Fingers;


	//public AudioSource SfxCasino;
	//public AudioSource SfxClick;
	//public AudioSource Bg1, Bg2, Bg3;

	public AudioSource[] audioSources;

	public GameObject GameOverUI;
	public GameObject ShopUI;

	public GameObject SecondShotgun;
	public TMP_Text DebtText;
	public GameObject RageSprite;

	public List<ShopSkill> ShopSkills = new List<ShopSkill>();
	public List<Skill> AllSkills;
	public List<Skill> PlayerSkills;

	public static Action<Skill> BuySkill;

	private void Awake()
	{

	}

	private void OnDisable()
	{
		BuySkill -= OnBuySkill;
	}

	private void OnEnable()
	{
		BuySkill += OnBuySkill;
	}

	public void Restart()
	{
		SceneManager.LoadScene(0);
		audioSources[1].Play();
		Time.timeScale = 1;
	}

	private void Start()
	{
		SetSFXVolume();
		SetMusicVolume();
		audioSources = GetComponents<AudioSource>();

		Debug.Log(audioSources.Count());
		var source = audioSources[UnityEngine.Random.Range(2, 3)];

		source.volume = .25f;
		source.Play();

		UpdateRage();
	}

	void UpdateRage()
	{
		RageSprite.GetComponent<RectTransform>().sizeDelta = new Vector2((Rage / MaxRage * Screen.width), 10);
	}

	public void AddRage(int rage)
	{
		Rage += rage * RageMultiplier;

		UpdateRage();
		if (Rage >= MaxRage)
		{
			Player.MaxRage *= 1.2f;
			Rage = 0;
			OpenShop();
		}
	}

	private static System.Random rng = new System.Random();

	public List<T> Shuffle<T>(List<T> list)
	{
		int n = list.Count;
		while (n > 1)
		{
			n--;
			int k = rng.Next(n + 1);
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
		return list;
	}

	public void OnBuySkill(Skill skill)
	{
		audioSources[1].Play();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		Time.timeScale = 1;

		if (skill.id == 0)
		{
			ShowSecondShotgun();
		}
		if (skill.id == 1)
		{
			ReloadTime = Math.Clamp(ReloadTime - .1f, 0.2f, ReloadTime);
		}
		if (skill.id == 2)
		{
			MaxAmmo++;
		}
		if (skill.id == 3)
		{
			MaxShotDelay = Math.Clamp(MaxShotDelay - .1f, 0.2f, MaxShotDelay);
		}
		if (skill.id == 4)
		{
			GetComponent<FPSMovement>().moveSpeed += .1f;
		}
		if (skill.id == 5)
		{
			RageMultiplier += .1f;
		}

		PlayerSkills.Add(skill);
		ShopUI.SetActive(false);
		UpdateRage();
	}

	private void OpenShop()
	{
		audioSources[0].Play();
		CameraShake.StopShake();

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		Time.timeScale = 0;

		ShopUI.SetActive(true);

		var available = AllSkills
			.Where(v => !(v.OnlyOne && PlayerSkills.Contains(v)))
			.ToList();

		available = Shuffle(available).Take(3).ToList();

		int index = 0;
		foreach (var item in ShopSkills)
		{
			item.UpdateSkill(available[index]);
			index++;
		}
	}

	public void AddDebt(int debt)
	{
		Debt += debt;
		DebtText.text = $"-{Debt:N0}$";
	}

	void ShowSecondShotgun()
	{
		HasSecondShotgun = true;
		SecondShotgun.SetActive(true);
	}

	public void TakeDamage(float amount)
	{
		CameraShake.Shake(.1f);
		audioSources[5].Play();

		Health -= amount;
		if (Health <= 0)
		{
			audioSources[6].Play();
			Die();
			return;
		}
		var count = Mathf.Ceil((100.0f - Health) / 10);

		foreach (var finger in Fingers.Take((int)count))
		{
			finger.SetActive(false);
		}
	}

	private void Die()
	{
		CameraShake.StopShake();
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		Time.timeScale = 0;
		GameOverUI.SetActive(true);
	}

	public Slider MusicSlider;
	public Slider SFXSlider;
	public AudioMixer AudioMixer;

	public void SetSFXVolume()
	{
		float volume = SFXSlider.value;
		AudioMixer.SetFloat("SFX", volume);
	}

	public void SetMusicVolume()
	{
		float volume = MusicSlider.value;
		AudioMixer.SetFloat("Music", volume);
	}

	public GameObject SettingsTab;

	public void HideSettings()
	{
		Time.timeScale = 1;
		SettingsTab.SetActive(false);
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	public void ShowSettings()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		Time.timeScale = 0;
		SettingsTab.SetActive(true);
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			ShowSettings();
		}
	}
}
