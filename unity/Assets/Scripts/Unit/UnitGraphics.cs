﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Teleports.Utils;

public class UnitGraphics : MonoBehaviour {
    
    GameObject targetMarker;
    GameObject raceModel;
    GameObject ragdoll;
    Unit unit;

	// Use this for initialization
	void Start () {
        if (gameObject.tag != "Player")
        {
            gameObject.AddComponent<Healthbar>();
        }
        unit = GetComponent<Unit>();
    }

    void Update()
    {
        if(unit.ActiveController is PlayerController) UpdateTarget(unit.ActiveController.Target.TargetUnit);
    }

    public void UpdateTarget(Unit target)
    {
        if(targetMarker == null)
        {
            targetMarker = Instantiate(Main.StaticData.UI.HUD.TargetMarker, gameObject.transform) as GameObject;
        }
        targetMarker.GetComponent<TargetMarker>().SetTargetUnit(target);
    }

    public void showDamage(float damage)
    {
        GameObject obj = InstantiateFloatingText();
        FloatingDamage floatingDamage = obj.GetComponent<FloatingDamage>();
        floatingDamage.setText(damage.ToString());
        floatingDamage.setColor(Color.red);
        floatingDamage.SetFontScale(DamageTextScalingFactor(damage));
        obj.transform.localPosition = new Vector3(0, gameObject.GetComponent<Unit>().UnitData.Height, 0);
    }

    public void showXp(int xp)
    {
        GameObject obj = InstantiateFloatingText();
        obj.GetComponent<FloatingDamage>().setText(xp.ToString() + " XP");
        obj.GetComponent<FloatingDamage>().setColor(Color.yellow);
        obj.GetComponent<FloatingDamage>().lifetime *= 2;
        obj.GetComponent<FloatingDamage>().gravityY /= 2;
        obj.transform.localPosition = new Vector3(0, gameObject.GetComponent<Unit>().UnitData.Height, 0);
    }

    public void showMessage(string message)
    {
        GameObject obj = InstantiateFloatingText();
        obj.GetComponent<FloatingDamage>().setText(message);
        obj.GetComponent<FloatingDamage>().setColor(Color.yellow);
        obj.transform.localPosition = new Vector3(0, gameObject.GetComponent<Unit>().UnitData.Height, 0);
    }

    private float DamageTextScalingFactor(float dmg)
    {
        return Mathf.Max(0f, Mathf.Log10(dmg + 32) - 1);
    }

    private GameObject InstantiateFloatingText()
    {
        return Instantiate(Main.StaticData.UI.HUD.FloatingText, gameObject.transform) as GameObject;
    }

    public GameObject RaceModel
    {
        get { return raceModel; }
        set { raceModel = value; }
    }
}
