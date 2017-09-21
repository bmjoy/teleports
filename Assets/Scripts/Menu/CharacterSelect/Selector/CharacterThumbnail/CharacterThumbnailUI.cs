﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterThumbnailUI : SelectorButtonUI {

    public Image playerIcon, teleportIcon;
    public Text characterName, characterLvl;

    private int characterSlotID = 0;
    private IPlayerData playerData = null;

    public override void LoadDataInternal()
    {
        base.LoadDataInternal();

        if (playerData != null)
        {
            playerIcon.sprite = PlayerGraphics.GetPlayerIcon(playerData);
            teleportIcon.sprite = PlayerGraphics.GetTeleportIcon(playerData);

            characterName.text = playerData.CharacterName;
            characterLvl.text = "Lvl " + playerData.Level.ToString();
        }
    }

    protected override bool IsActive()
    {
        return characterSlotID == MainData.CurrentSaveData.CurrentSlotID;
    }

    protected override void OnActivate()
    {
        MainData.CurrentSaveData.CurrentSlotID = characterSlotID;
    }

    protected override void OnDeactivate()
    {
        
    }

    public void SetCharacterSlotID(int id)
    {
        characterSlotID = id;
        playerData = MainData.CurrentSaveData.GetPlayerData(characterSlotID);
        LoadDataInternal();
    }

    public int CharacterSlotID
    {
        get { return characterSlotID; }
    }
}