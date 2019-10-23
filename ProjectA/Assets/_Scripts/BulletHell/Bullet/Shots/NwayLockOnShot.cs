using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Ubh nway lock on shot.
/// </summary>
[AddComponentMenu("UniBulletHell/Shot Pattern/nWay Shot (Lock On)")]
public class NwayLockOnShot : NwayShot
{
    [Header("===== NwayLockOnShot Settings =====")]
    // "Set a target with tag name."
    public bool m_setTargetFromTag = true;
    // "Set a unique tag name of target at using SetTargetFromTag."
    public string m_targetTagName = "Player";
    // "Flag to randomly select from GameObjects of the same tag."
    public bool m_randomSelectTagTarget;
    // "Transform of lock on target."
    // "It is not necessary if you want to specify target in tag."
    // "Overwrite CenterAngle in direction of target to Transform.position."
    public Transform m_targetTransform;
    // "Always aim to target."
    public bool m_aiming;

    /// <summary>
    /// is lock on shot flag.
    /// </summary>
    public override bool lockOnShot { get { return true; } }

    public override void Shot()
    {
        if (m_shooting)
        {
            return;
        }

        AimTarget();

        if (m_targetTransform == null)
        {
            Debug.LogWarning("Cannot shot because TargetTransform is not set.");
            return;
        }

        base.Shot();

        if (m_aiming)
        {
            StartCoroutine(AimingCoroutine());
        }
    }

    private void AimTarget()
    {
        if (m_targetTransform == null && m_setTargetFromTag)
        {
            m_targetTransform = Utils2D.GetTransformFromTagName(m_targetTagName, m_randomSelectTagTarget);
        }
        if (m_targetTransform != null)
        {
            m_centerAngle = Utils2D.GetAngleFromTwoPosition(transform, m_targetTransform, Utils2D.AXIS.X_AND_Y);
        }
    }

    private IEnumerator AimingCoroutine()
    {
        while (m_aiming)
        {
            if (m_shooting == false)
            {
                yield break;
            }

            AimTarget();

            yield return null;
        }

        yield break;
    }
}