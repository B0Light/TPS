using System.Collections.Generic;
using UnityEngine;

public class DamageArea : DamageCollider
{
    public float AreaOfEffectDistance = 5f;

    public AnimationCurve DamageRatioOverDistance;

    [Header("Debug")]
    public Color AreaOfEffectColor = Color.red * 0.5f;

    public void InflictDamageInArea(float damage, Vector3 center, LayerMask layers,
        QueryTriggerInteraction interaction, GameObject owner)
    {
        List<CharacterManager> uniqueDamagedHealths = new List<CharacterManager>();
        Collider[] affectedColliders = Physics.OverlapSphere(center, AreaOfEffectDistance, layers, interaction);
        foreach (var coll in affectedColliders)
        {
            CharacterManager damageTarget = coll.GetComponentInParent<CharacterManager>();
            if (damageTarget)
            {
                if (!damageTarget.isDead.Value && !uniqueDamagedHealths.Contains(damageTarget))
                {
                    uniqueDamagedHealths.Add(damageTarget);
                }
            }
        }

        // Apply damages with distance falloff
        foreach (CharacterManager damageTarget in uniqueDamagedHealths)
        {
            DamageTarget(damageTarget);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = AreaOfEffectColor;
        Gizmos.DrawSphere(transform.position, AreaOfEffectDistance);
    }
}