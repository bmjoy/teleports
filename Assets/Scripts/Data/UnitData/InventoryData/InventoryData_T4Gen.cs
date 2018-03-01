//DO NOT EDIT - Autogenerated file
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Teleports.Utils;

[System.Serializable]
public partial class InventoryData {

	[SerializeField] private int maxSlots;
	[SerializeField] private EquipmentData equipmentData;
	[SerializeField] private List<InventorySlotData> invSlots;

	public InventoryData(InventoryData other){
		maxSlots = other.maxSlots;
		equipmentData = new EquipmentData(other.equipmentData);
		invSlots = other.invSlots.DeepCopy();
	}

	public object DeepCopy(){
		return new InventoryData(this);
	}



	public int MaxSlots => maxSlots;
	public EquipmentData EquipmentData => equipmentData;
	public List<InventorySlotData> InvSlots => invSlots;


}
	