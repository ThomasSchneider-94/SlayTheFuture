using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource backgroundMusic;

    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        backgroundMusic.loop = true;
        backgroundMusic.Play();
    }

    public IEnumerator PlayAudio(ElementType elementType)
    {
        switch (elementType)
        {
            case ElementType.PLANT:
                audioSource.clip = Resources.Load<AudioClip>("Audio/heal");
                Debug.Log("heal");
                break;
            case ElementType.PHYSICAL:
                audioSource.clip = Resources.Load<AudioClip>("Audio/sword");
                Debug.Log("sword");
                break;
            case ElementType.FIRE:
                audioSource.clip = Resources.Load<AudioClip>("Audio/pierce");
                Debug.Log("pierce");
                break;
            case ElementType.EARTH:
                audioSource.clip = Resources.Load<AudioClip>("Audio/shield");
                Debug.Log("shield");
                break;
            case ElementType.POISON:
                audioSource.clip = Resources.Load<AudioClip>("Audio/poison");
                Debug.Log("poison");
                break;
            default:
                audioSource.clip = null;
                break;
        }

        if (audioSource.clip != null)
        {
            audioSource.Play();
        }

        yield return new WaitForSeconds(2);
    }
}
