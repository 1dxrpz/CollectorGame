using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
	Animator animator;
	public ParticleSystem particles;
	public Transform worldObject;
	public Transform FlashPos;
	public GameObject Flash;
	public GameObject FlashLight;
	public float flashTime = 0.1f;

	AudioSource[] SfxShotgun;

	void Start()
	{
		SfxShotgun = GetComponents<AudioSource>();
		animator = GetComponent<Animator>();
	}

	public float shotDelay = 10;

	int ammo = 2;
	void Update()
	{
		shotDelay -= Time.deltaTime * 20;

		if (shotDelay > 0)
			return;

		if (Input.GetKey(KeyCode.Mouse1))
		{
			if (ammo == 0)
			{
				SfxShotgun[2].Play();

				animator.Play("shotgun reload");
				ammo = Player.MaxAmmo;
				shotDelay = Player.MaxShotDelay * Player.ReloadTime;
			}
			else
			{
				SfxShotgun[Random.Range(0, 2)].Play();

				ammo--;
				shotDelay = Player.MaxShotDelay;
				animator.Play("shotgun shot");

				var flash = Instantiate(Flash, FlashPos);
				Destroy(flash, flashTime);

				var flashLight = Instantiate(FlashLight, FlashPos);
				Destroy(flashLight, flashTime);

				CameraShake.Shake(0.2f);

				Shoot();
			}
		}
	}

	public LayerMask navMeshAgentLayer;

	private void Shoot()
	{
		RaycastHit hit;
		Ray[] rays = new Ray[]
		{
			Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)),
			Camera.main.ViewportPointToRay(new Vector3(0.35f, 0.5f, 0)),
			Camera.main.ViewportPointToRay(new Vector3(0.25f, 0.5f, 0)),
			Camera.main.ViewportPointToRay(new Vector3(0.65f, 0.5f, 0)),
			Camera.main.ViewportPointToRay(new Vector3(0.75f, 0.5f, 0)),
		};

		foreach (var ray in rays)
		{
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, navMeshAgentLayer))
			{
				// Check if the hit object has a NavMeshAgent component.  A safety check!
				Collector agent = hit.collider.GetComponent<Collector>();
				if (agent != null)
				{
					agent.Damage(1 * Player.DamageMultiplier * (Player.HasSecondShotgun ? 2 : 1));
				}
			}
		}
	}
}
