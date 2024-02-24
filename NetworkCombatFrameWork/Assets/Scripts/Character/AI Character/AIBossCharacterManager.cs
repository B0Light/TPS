using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class AIBossCharacterManager : AICharacterManager
{
    public int bossID = 0;
    [SerializeField] private bool hasBeenDefeated = false;
    [SerializeField] private bool hasBeenAwakened = false;
    [SerializeField] private List<FogWallInteractable> fogWalls;
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
        {
            // first time
            if (!WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
            {
                WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.Add(bossID, false);
                WorldSaveGameManager.Instance.currentCharacterData.bossesDefeated.Add(bossID, false);
            }
            // load data
            else
            {
                hasBeenDefeated = WorldSaveGameManager.Instance.currentCharacterData.bossesDefeated[bossID];
                hasBeenAwakened = WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened[bossID];
            }

            StartCoroutine(GetFogWallsFromWorldObjectManager());
            
            if (hasBeenAwakened)
            {
                for (int i = 0; i < fogWalls.Count; i++)
                {
                    fogWalls[i].isActive.Value = true;
                }
            }
            
            if (hasBeenDefeated)
            {
                for (int i = 0; i < fogWalls.Count; i++)
                {
                    fogWalls[i].isActive.Value = false;
                }
                
                aiCharacterNetworkManager.isActive.Value = false;
            }
            
                
        }
    }

    private IEnumerator GetFogWallsFromWorldObjectManager()
    {
        while (WorldObjectManager.Instance.fogWalls.Count == 0)
            yield return new WaitForEndOfFrame();
        
        fogWalls = new List<FogWallInteractable>();
        
        foreach (var fogWall in WorldObjectManager.Instance.fogWalls)
        {
            if(fogWall.fogWallID == bossID)
                fogWalls.Add(fogWall);
        }
    }
    
    public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
    {
        if (IsOwner)
        {
            characterNetworkManager.currentHealth.Value = 0;
            isDead.Value = true;
            
            if(!manuallySelectDeathAnimation)
            {
                characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);
            }
            
            hasBeenDefeated = true;
            
            //
            // first time
            if (!WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
            {
                WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                WorldSaveGameManager.Instance.currentCharacterData.bossesDefeated.Add(bossID, true);
            }
            // load data
            else
            {
                WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.Remove(bossID);
                WorldSaveGameManager.Instance.currentCharacterData.bossesDefeated.Remove(bossID);
                WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                WorldSaveGameManager.Instance.currentCharacterData.bossesDefeated.Add(bossID, true);
            }
            WorldSaveGameManager.Instance.SaveGame();
        }
        
        yield return new WaitForSeconds(5f);
    }
}
