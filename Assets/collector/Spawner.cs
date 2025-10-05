using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject Enemy;

	public float Delay = 200;
	public float enemyDelay;

	void Start()
    {
		enemyDelay = Delay;
	}

    // Update is called once per frame
    void Update()
    {
		var multiplier = 1f;

		if (Player.HasSecondShotgun)
		{
			multiplier += 1;
		}
		multiplier += Player.MaxAmmo / 20f;
		multiplier += Player.MaxShotDelay / 80f;
		multiplier += Player.ReloadTime / 20f;
		multiplier += Player.DamageMultiplier / 5f;
		multiplier += Player.Debt == 0 ? 0 : 1 - (5 / Player.Debt);

		enemyDelay -= Time.deltaTime * 5 * Mathf.Clamp(multiplier, 1, 10) / 5;

		if (enemyDelay > 0)
			return;

		enemyDelay = Delay;


		var temp = Instantiate(Enemy);
		temp.transform.position = transform.position;

		Player.DamageMultiplier = multiplier / 2f;
		Player.EnemyHealthMultiplier = multiplier / 2f;
	}
}
