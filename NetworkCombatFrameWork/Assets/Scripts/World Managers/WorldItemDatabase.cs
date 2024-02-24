using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WorldItemDatabase : Singleton<WorldItemDatabase>
{
    public WeaponItem unarmedWeapon;

    [Header("Weapons")]
    [SerializeField] List<WeaponItem> weapons = new List<WeaponItem>();

    [Header("Items")]
    private  List<Item> items = new List<Item>();

    protected override void Awake()
    {
        base.Awake();

        foreach (var weapon in weapons)
        {
            items.Add(weapon);
        }

        for(int i = 0; i < items.Count; i++)
        {
            items[i].itemID = i;
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    public WeaponItem GetWeaponByID(int ID)
    {
        return weapons.FirstOrDefault(weapon => weapon.itemID == ID);
    }
}
