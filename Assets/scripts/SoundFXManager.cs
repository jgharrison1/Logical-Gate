using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;

    [SerializeField] private AudioSource soundFXObject;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    public void playSoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume) {
        //spawn in the game object
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        //assign the audio clip
        audioSource.clip = audioClip;
        //assign volume level to audio clip
        audioSource.volume = volume;
        //play sound clip
        audioSource.Play();
        //get length of sound clip
        float clipLength = audioSource.clip.length;
        //when sound clip is done, destroy the sound game object
        Destroy(audioSource.gameObject, clipLength);
    }
}
