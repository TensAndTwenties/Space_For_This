using UnityEngine;
using System.Collections;


public class Shield
{
	public float maxShield { get; set; }
	public float rechargeRate { get; set; }
	public float currentShield { get; set; }
	public float maxRechargeDelay { get; set; }
	public float currentRechargeDelay { get; set; }

	public Shield (float _maxShield, float _rechargeRate, float _maxRechargeDelay)
	{
		maxShield = _maxShield;
		currentShield = _maxShield;
		rechargeRate = _rechargeRate;
		maxRechargeDelay = _maxRechargeDelay;
	}

	public void resetRechargeDelay ()
	{
		currentRechargeDelay = maxRechargeDelay;
	}
}

