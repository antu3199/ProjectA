using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Ubh homing shot.
/// </summary>
[AddComponentMenu("UniBulletHell/Shot Pattern/Homing Shot")]
public class HomingShot : BulletShooterBase
{
    [Header("===== HomingShot Settings =====")]
    // "Set a delay time between bullet and next bullet. (sec)"
    public float m_betweenDelay = 0.1f;
    // "Set a speed of homing angle."
    public float m_homingAngleSpeed = 20f;
    // "Set a target with tag name."
    public bool m_setTargetFromTag = true;
    // "Set a unique tag name of target at using SetTargetFromTag."
    public string m_targetTagName = "Player";
    // "Flag to randomly select from GameObjects of the same tag."
    public bool m_randomSelectTagTarget;
    // "Transform of lock on target."
    // "It is not necessary if you want to specify target in tag."
    public Transform m_targetTransform;

    public override void Shot()
    {
        StartCoroutine(ShotCoroutine());
    }

    private IEnumerator ShotCoroutine()
    {
        if (m_bulletNum <= 0 || m_bulletSpeed <= 0f)
        {
            Debug.LogWarning("Cannot shot because BulletNum or BulletSpeed is not set.");
            yield break;
        }
        if (m_shooting)
        {
            yield break;
        }
        m_shooting = true;

        for (int i = 0; i < m_bulletNum; i++)
        {
            if (0 < i && 0f < m_betweenDelay)
            {
                FiredShot();
                yield return new WaitForSeconds(m_betweenDelay);
            }

            var bullet = GetBullet(transform.position);
            if (bullet == null)
            {
                break;
            }

            if (m_targetTransform == null && m_setTargetFromTag)
            {
                m_targetTransform = Utils2D.GetTransformFromTagName(m_targetTagName, m_randomSelectTagTarget);
            }

            float angle = Utils2D.GetAngleFromTwoPosition(transform, m_targetTransform, Utils2D.AXIS.X_AND_Y);

            ShotBullet(bullet, m_bulletSpeed, angle, true, m_targetTransform, m_homingAngleSpeed);
        }

        FiredShot();

        FinishedShot();

        yield break;
    }
}