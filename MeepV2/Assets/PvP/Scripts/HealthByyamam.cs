using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HealthByyamam : MonoBehaviour
{
    [Header("Player Settings")]
    public Transform Player;
    public Transform RespawnPoint;
    [Space]
    [Space]
    [Header("Health Settings")]
    [Range(1, 200)]
    public int MaxHealth = 100;
    [Range(0, 200)]
    public int CurrentHealth = 100;
    [Space]
    [Space]
    [Header("Death and Ragdoll Settings")]
    public GameObject RagdollDeathPrefab;
    public Transform RagdollSpawnPoint;
    [Range(0, 10)]
    public float RagdollDestroySeconds = 5f;
    [Range(0, 10)]
    public float RagdollSpawnCooldown = 5f;
    [Space]
    [Space]
    [Header("Healing Settings")]
    [Range(0, 200)]
    public float EverySecondHeal = 3f;
    [Range(1, 200)]
    public int HealingAmount = 1;
    [Space]
    [Space]
    [Header("Volumes Settings")]
    public Volume Volume;
    private Vignette Vignette;
    private float Timer = 0f;
    private float DAD = -Mathf.Infinity;

    void Start()
    {
        if (Volume != null)
        {
            Volume.profile.TryGet(out Vignette);
        }
    }
    void Update()
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);

        if (Vignette != null)
        {
            if (CurrentHealth < MaxHealth * 0.85f)
            {
                Vignette.intensity.value = Mathf.Lerp(0, 1, 1 - (CurrentHealth / (MaxHealth * 0.85f)));
            }
            else
            {
                Vignette.intensity.value = 0;
            }
        }

        if (CurrentHealth == 0 && Time.time >= DAD + RagdollSpawnCooldown)
        {
            if (RagdollDeathPrefab != null && RagdollSpawnPoint != null)
            {
                GameObject spawnedRagdoll = Instantiate(RagdollDeathPrefab, RagdollSpawnPoint.position, RagdollSpawnPoint.rotation);
                Destroy(spawnedRagdoll, RagdollDestroySeconds);
                DAD = Time.time;
            }

            if (Vignette != null)
            {
                Vignette.intensity.value = 0;
            }

            Player.position = RespawnPoint.position;
        }
        else
        {
            Timer += Time.deltaTime;
            if (Timer >= EverySecondHeal && CurrentHealth < MaxHealth)
            {
                Timer = 0f;
                CurrentHealth = Mathf.Min(CurrentHealth + HealingAmount, MaxHealth);
            }
        }
    }
    public void DAMAGE(float Damage)
    {
        CurrentHealth -= (int)Damage;
    }
}