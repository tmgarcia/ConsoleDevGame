using UnityEngine;
using System.Collections.Generic;

public class DamageableHelper : MonoBehaviour
{
	public GameObject damageableObject;

	public void ApplyDamage(int damageAmount)
	{
		if (damageableObject != null)
		{
			damageableObject.GetComponent<Damageable>().CurrentLocalIntegrity -= damageAmount;
		}
	}
}
