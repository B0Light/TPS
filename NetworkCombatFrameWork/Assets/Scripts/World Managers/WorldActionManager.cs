using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldActionManager : Singleton<WorldActionManager>
{
    [Header("Weapon Item Action")]
    public WeaponItemAction[] weaponItemActions;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < weaponItemActions.Length; i++)
        {
            weaponItemActions[i].actionID = i;
        }
    }

    public WeaponItemAction GetWeaponItemActionByID(int ID)
    {
        return weaponItemActions.FirstOrDefault(action => action.actionID == ID);
    }
}
