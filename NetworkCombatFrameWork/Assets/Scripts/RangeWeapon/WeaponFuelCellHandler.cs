
using UnityEngine;

[RequireComponent(typeof(WeaponController))]
public class WeaponFuelCellHandler : MonoBehaviour
{
    public bool SimultaneousFuelCellsUsage = false;
    public GameObject[] FuelCells;
    public Vector3 FuelCellUsedPosition;
    public Vector3 FuelCellUnusedPosition = new Vector3(0f, -0.1f, 0f);
    WeaponController m_Weapon;
    bool[] m_FuelCellsCooled;

    void Start()
    {
        m_Weapon = GetComponent<WeaponController>();
        m_FuelCellsCooled = new bool[FuelCells.Length];
        for (int i = 0; i < m_FuelCellsCooled.Length; i++)
        {
            m_FuelCellsCooled[i] = true;
        }
    }

    void Update()
    {
        if (SimultaneousFuelCellsUsage)
        {
            for (int i = 0; i < FuelCells.Length; i++)
            {
                FuelCells[i].transform.localPosition = Vector3.Lerp(FuelCellUsedPosition, FuelCellUnusedPosition,
                    m_Weapon.CurrentAmmoRatio);
            }
        }
        else
        {
            for (int i = 0; i < FuelCells.Length; i++)
            {
                float length = FuelCells.Length;
                float lim1 = i / length;
                float lim2 = (i + 1) / length;

                float value = Mathf.InverseLerp(lim1, lim2, m_Weapon.CurrentAmmoRatio);
                value = Mathf.Clamp01(value);

                FuelCells[i].transform.localPosition =
                    Vector3.Lerp(FuelCellUsedPosition, FuelCellUnusedPosition, value);
            }
        }
    }
}
