using System;
using UnityEngine;
using UnityEngine.AI;

public class Collector : MonoBehaviour
{
	public float Health = 1;
	NavMeshAgent agent;
	Player player;


	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		Health *= Player.EnemyHealthMultiplier;
	}

	public float damageDelay = 10;

	private void Update()
	{
		damageDelay -= Time.deltaTime * 20;

		if (damageDelay > 0)
			return;

		damageDelay = 10;


		player = GameObject.FindWithTag("Player").GetComponent<Player>();
		agent.destination = player.transform.position;
		var dist = Vector3.Distance(agent.transform.position, player.transform.position);

		if (dist < 2.5f)
		{
			player.TakeDamage(2f * Player.EnemyDamageMultiplier);
		}
	}

	public void Damage(float damage)
	{
		Health -= damage;
		if (Health <= 0)
		{
			Die();
		}
	}

	private void Die()
	{
		player.AddRage(10 * UnityEngine.Random.Range(1, 5));
		player.AddDebt(UnityEngine.Random.Range(10, 15) * 100);
		Destroy(this.gameObject);
	}
}
