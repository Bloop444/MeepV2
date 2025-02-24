using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class CombatWArriorSword : MonoBehaviour
{
    [Header("Basic Settings")]
    [Range(0, 1000)]
    public float Damage;
    public List<string> PlayerTags = new List<string>();
    [Space]
    [Space]
    [Header("Cooldown Settings")]
    [Range(0, 10)]
    public float AttackCooldown = 1f;
    private float OyVey = -Mathf.Infinity;
    [Space]
    [Space]
    [Header("Critical Hit Settings (in percent %)")]
    [Range(0, 100)]
    public float CriticalHitChance = 20f;
    public float CriticalHitMultiplier = 2f;
    [Space]
    [Space]
    [Header("Vibration Settings")]
    [Range(0, 1)]
    public float VibrationDuration = 1f;
    [Range(0, 10)]
    public float VibrationAmount = 0.5f;
    [Space]
    [Space]
    [Header("Managers And Stuff")]
    public HealthByyamam HealthManager;
    public Collider SwordCollider;
    [Space]
    [Space]
    [Header("Effects")]
    public ParticleSystem ParticleSystem;
    public Transform ParticleSpawnPoint;
    public AudioSource AudioSource;
    [Space]
    [Space]
    [Header("Audio Clips")]
    public AudioClip[] PlayerHitSounds;
    public AudioClip DeathSound;
    [Space]
    [Space]
    [Header("Rotation Settings")]
    public float ParticleRotationX = 0f;
    public float ParticleRotationY = 0f;
    public float ParticleRotationZ = 0f;
    [Space]
    [Space]
    [Header("Nodes, Dont Touch")]
    public XRNode LeftControllerNode = XRNode.LeftHand;
    public XRNode RightControllerNode = XRNode.RightHand;

    void OnTriggerEnter(Collider other)
    {
        if (HealthManager == null || SwordCollider == null)
            return;

        if (Time.time < OyVey + AttackCooldown)
            return;

        if (PlayerTags.Contains(other.tag))
        {
            OyVey = Time.time;
            float FinalDamage = Damage;

            if (Random.Range(0f, 100f) < CriticalHitChance)
            {
                FinalDamage *= CriticalHitMultiplier;
            }

            HealthManager.DAMAGE(FinalDamage);

            if (ParticleSystem != null)
            {
                Vector3 SpawnPos = ParticleSpawnPoint != null ? ParticleSpawnPoint.position : other.transform.position;
                Quaternion BlloodRot = Quaternion.Euler(ParticleRotationX, ParticleRotationY, ParticleRotationZ);

                ParticleSystem ParticleInst = Instantiate(ParticleSystem, SpawnPos, BlloodRot);
                ParticleInst.Play();
                Destroy(ParticleInst.gameObject, 2f);
            }

            if (AudioSource != null && PlayerHitSounds.Length > 0)
            {
                int RandomInd = Random.Range(0, PlayerHitSounds.Length);
                AudioSource.PlayOneShot(PlayerHitSounds[RandomInd]);
            }

            if (HealthManager.CurrentHealth <= 0)
            {
                if (AudioSource != null && DeathSound != null)
                {
                    AudioSource.PlayOneShot(DeathSound);
                }
            }

            Vibrations();
        }
    }
    void Vibrations()
    {
        if (VibrationAmount > 0f && VibrationDuration > 0f)
        {
            VibrateController(LeftControllerNode, VibrationAmount, VibrationDuration);
            VibrateController(RightControllerNode, VibrationAmount, VibrationDuration);
        }
    }
    void VibrateController(XRNode controllerNode, float intensity, float duration)
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(controllerNode);
        if (device.isValid)
        {
            device.SendHapticImpulse(0, intensity, duration);
        }
    }
}