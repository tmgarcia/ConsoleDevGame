using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Damageable : MonoBehaviour
{
	public float StartingIntegrity = 100.0f;
	public float CurrentIntegrity;
	private float lastIntegrityUpdate = -1.0f;
	public DamageRole damageRole = DamageRole.Villager;
	//protected

	void Start()
	{
		CurrentIntegrity = StartingIntegrity;
	}

	void Update()
	{
		DamageUpdate();
	}

	protected void DamageUpdate()
	{
		if (lastIntegrityUpdate != CurrentIntegrity && PhotonNetwork.isMasterClient)
		{
			GetComponent<PhotonView>().RPC("SetIntegrity", PhotonTargets.AllBuffered);
		}
	}

	[RPC]
	public void SetIntegrity()
	{
		if (PlayersManager.instance.GetPlayer(PlayersManager.instance.localPlayerID).GetComponent<BasePlayerScript>().playerCharacter == gameObject)
		{
			GameObject.Find("LocalPlayerHealthGraphic").GetComponent<Image>().fillAmount = CurrentIntegrity / StartingIntegrity;
			GameObject.Find("LocalPlayerHealthInfo").GetComponent<Text>().text = CurrentIntegrity + " / " + StartingIntegrity;
		}

		if (damageRole == DamageRole.Dragon && PlayersManager.instance.GetPlayerRole(PlayersManager.instance.localPlayerID) != PlayerRole.Dragon)
		{
			GameObject.Find("DragonHealthGraphic").GetComponent<Image>().fillAmount = CurrentIntegrity / StartingIntegrity;
			GameObject.Find("DragonHealthInfo").GetComponent<Text>().text = CurrentIntegrity + " / " + StartingIntegrity;
		}

		lastIntegrityUpdate = CurrentIntegrity;
	}
}
