using UnityEngine;

[System.Serializable]
public class MoveDecision
{
    public MovementState movementState;
    [Range(0f, 100f)] public float chance;
}

[CreateAssetMenu(fileName = "EnemySettings", menuName = "ScriptableObjects/EnemySettings")]
public class BotSettings : TankSettings
{
    [Header("Enemy Settings")]
    public Vector2 shootInterval = new Vector2(1f, 2f);
    public Vector2 decisionInterval = new Vector2(2f, 3f);

    public MoveDecision[] moveDecisions;

    private void OnValidate()
    {
        float totalChance = 100f;
        for (int i = 1; i < moveDecisions.Length; i++)
        {
            if (moveDecisions[i].chance > totalChance - moveDecisions[i - 1].chance)
            {
                moveDecisions[i].chance = totalChance - moveDecisions[i - 1].chance;
            }
            totalChance -= moveDecisions[i - 1].chance;
        }
    }
}