using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource soundFXAudioSource;
    public AudioClip coreTakesDamage;
    public AudioClip playerTakesDamage;
    public AudioClip enemyTakesDamage;
    public AudioClip enemyDisarmed;
    public AudioClip powerUpPickedUp;

    private void Start()
    {
        instance = this;
    }

    public static void CoreTakesDamage() => instance.soundFXAudioSource.PlayOneShot(instance.coreTakesDamage);
    public static void PlayerTakesDamage() => instance.soundFXAudioSource.PlayOneShot(instance.playerTakesDamage);
    public static void EnemyTakesDamage() => instance.soundFXAudioSource.PlayOneShot(instance.enemyTakesDamage);
    public static void EnemyDisarmed() => instance.soundFXAudioSource.PlayOneShot(instance.enemyDisarmed);
    public static void PowerUpPickedUp() => instance.soundFXAudioSource.PlayOneShot(instance.powerUpPickedUp);
}
