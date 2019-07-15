//DO NOT EDIT - Autogenerated file
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Teleports.Utils;

[System.Serializable]
public partial class TeleportData : IDeepCopyable {

	[SerializeField] private int tier;
	[SerializeField] private Attribute power;
	[SerializeField] private Attribute time;
	[SerializeField] private List<GemSlot> gemSlots;
	[SerializeField] private string graphicsId;


	public TeleportData(TeleportData other){
		tier = other.tier;
		power = new Attribute(other.power);
		time = new Attribute(other.time);
		gemSlots = new List<GemSlot>(other.gemSlots);
		graphicsId = other.graphicsId;
	}

	public object DeepCopy(){
		return new TeleportData(this);
	}

	public Attribute GetAttribute(AttributeType type)
	{
		switch(type)
		{
			case AttributeType.Power:
				return power;
			case AttributeType.Time:
				return time;
			default:
				return null;
		}
	}

    public void ModifyAttribute(AttributeType type, float bonus, float multiplier)
	{
		GetAttribute(type).Modify(bonus, multiplier);
	}

	public int Tier => tier;
	public float Power => power.Value;
	public float Time => time.Value;
	public List<GemSlot> GemSlots => gemSlots;
	public string GraphicsId => graphicsId;

	public enum AttributeType {
		Power,
		Time,
	}

}
	