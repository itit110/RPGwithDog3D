using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSettings))]
public class PlayerAudioSouce : MonoBehaviour
{
    public AudioClip[] clips;
    public float pitchRange = 0.1f;

    protected AudioSource source;

    private void Awake()
    {
        source = GetComponents<AudioSource>()[0];
    }

    public void PlayerAtackSE()
    {
        if (source == null || clips == null || clips.Length == 0)
        {
            Debug.Log("Noオーディオ");
                return;
        }
        source.pitch = 1.0f + Random.Range(-pitchRange, pitchRange);

        int index = Random.Range(0, clips.Length);
        source.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }
}
