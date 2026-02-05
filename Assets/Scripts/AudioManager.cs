using FMODUnity;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private GameObject audioPlayer;
    private GameObject audioInstance;
    private StudioEventEmitter audioEmitter;
    
    void Start()
    {
    }

    public GameObject CreateAudioInstance(EventReference eventReference)
    {
        audioInstance = Instantiate(audioPlayer);
        audioEmitter = audioInstance.GetComponent<StudioEventEmitter>();
        audioEmitter.EventReference = eventReference;
        audioEmitter.Play();
        return audioInstance;
    }

    public void DestroyAudioInstance(GameObject audioInstance)
    {
        audioEmitter = audioInstance.GetComponent<StudioEventEmitter>();
        audioEmitter.Stop();
        Destroy(audioInstance);
    }
}