using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Damageable : MonoBehaviour
{
	public float StartingIntegrity = 100.0f;
	public float CurrentIntegrity { get; set; }
	private float lastIntegrityUpdate = -1.0f;
	public DamageRole damageRole = DamageRole.Villager;
	//protected

	void Start()
	{
		CurrentIntegrity = StartingIntegrity;
		lastIntegrityUpdate = CurrentIntegrity;
	}

	protected void Update()
	{
		if (lastIntegrityUpdate != CurrentIntegrity)
		{
			if (PlayersManager.instance.GetPlayer(PlayersManager.instance.localPlayerID).GetComponent<BasePlayerScript>().playerCharacter == gameObject)
			{
				GameObject.Find("LocalPlayerHealthGraphic").GetComponent<Image>().fillAmount = CurrentIntegrity / StartingIntegrity;
				GameObject.Find("LocalPlayerHealthInfo").GetComponent<Text>().text = CurrentIntegrity + " / " + StartingIntegrity;
			}

			if (PlayersManager.instance.dragonPlayer == gameObject && PlayersManager.instance.GetPlayerRole(PlayersManager.instance.localPlayerID) != PlayerRole.Dragon)
			{
				GameObject.Find("DragonHealthGraphic").GetComponent<Image>().fillAmount = CurrentIntegrity / StartingIntegrity;
				GameObject.Find("DragonHealthInfo").GetComponent<Text>().text = CurrentIntegrity + " / " + StartingIntegrity;
			}

			lastIntegrityUpdate = CurrentIntegrity;
		}
	}
}
