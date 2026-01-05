using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    
    [SerializeField] int poolSize = 24;
    List<AudioSource> sources;
    public static SoundManager Instance;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        sources = new List<AudioSource>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject g = new GameObject("SFX_Source_" + i);
            g.transform.parent = transform;

            AudioSource src = g.AddComponent<AudioSource>();
            src.spatialBlend = 1f;
            src.playOnAwake = false;

            sources.Add(src);
        }
    }

    AudioSource GetFreeSource()
    {
        foreach (var src in sources)
            if (!src.isPlaying)
                return src;

        return sources[0]; 
    }



    public void PlayOneShot(AudioClip clip, Vector3 pos)
    {
        AudioSource src = GetFreeSource();
        src.transform.position = pos;
        src.pitch = Random.Range(0.95f, 1.05f);
        src.PlayOneShot(clip);
    }


}
