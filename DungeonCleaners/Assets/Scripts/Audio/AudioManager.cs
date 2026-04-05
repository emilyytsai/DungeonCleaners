using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using System.Linq;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound[] sounds;
    [SerializeField] private string[] overworldScenes;
    [SerializeField] private string[] dungeonScenes;

    public static AudioManager instance;

    private AudioSource sfxSource;

    private List<string> overworldPlaylist = new List<string>
    {
        "Overworld Song 1",
        "Overworld Song 2",
        "Overworld Song 3"
    };

    private List<string> dungeonPlaylist = new List<string>
    {
        "Dungeon Song 1",
        "Dungeon Song 2",
        "Dungeon Song 3"
    };

    private Queue<string> overworldQueue = new Queue<string>();
    private Queue<string> dungeonQueue = new Queue<string>();

    private string currentCategory = "";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        sfxSource = gameObject.AddComponent<AudioSource>();

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = false;
        }

        RefillQueue(overworldPlaylist, overworldQueue);
        RefillQueue(dungeonPlaylist, dungeonQueue);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void Update()
    {
        if (currentCategory == "Overworld" && !AnySongPlaying(overworldPlaylist))
        {
            PlayNextFromCategory(overworldPlaylist, overworldQueue, "Overworld");
        }
        else if (currentCategory == "Dungeon" && !AnySongPlaying(dungeonPlaylist))
        {
            PlayNextFromCategory(dungeonPlaylist, dungeonQueue, "Dungeon");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);

        if (overworldScenes.Contains(scene.name))
        {
            if (currentCategory != "Overworld")
            {
                StopAllMusic();
                currentCategory = "Overworld";
                PlayNextFromCategory(overworldPlaylist, overworldQueue, "Overworld");
            }
        }
        else if (dungeonScenes.Contains(scene.name))
        {
            if (currentCategory != "Dungeon")
            {
                StopAllMusic();
                currentCategory = "Dungeon";
                PlayNextFromCategory(dungeonPlaylist, dungeonQueue, "Dungeon");
            }
        }
        else
        {
            StopAllMusic();
            currentCategory = "";
        }
    }

    private void PlayNextFromCategory(List<string> playlist, Queue<string> queue, string category)
    {
        if (queue.Count == 0)
        {
            RefillQueue(playlist, queue);
        }

        string nextSong = queue.Dequeue();
        Debug.Log($"Playing {category} song: {nextSong}");
        Play(nextSong);
    }

    private void RefillQueue(List<string> playlist, Queue<string> queue)
    {
        List<string> shuffled = new List<string>(playlist);

        for (int i = 0; i < shuffled.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, shuffled.Count);
            string temp = shuffled[i];
            shuffled[i] = shuffled[randomIndex];
            shuffled[randomIndex] = temp;
        }

        queue.Clear();

        foreach (string song in shuffled)
        {
            queue.Enqueue(song);
        }
    }

    private bool AnySongPlaying(List<string> playlist)
    {
        foreach (string songName in playlist)
        {
            if (IsPlaying(songName))
            {
                return true;
            }
        }
        return false;
    }

    public void Play(string name)
    {
        Sound sound = Array.Find(sounds, s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning($"Sound: {name} not found!");
            return;
        }

        if (sound.source == null)
        {
            Debug.LogWarning($"AudioSource for sound {name} is null!");
            return;
        }

        sound.source.Play();
    }

    public void PlaySFX(string name)
    {
        Sound sound = Array.Find(sounds, s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning($"Sound: {name} not found!");
            return;
        }

        if (sound.clip == null)
        {
            Debug.LogWarning($"Clip for sound {name} is null!");
            return;
        }

        sfxSource.PlayOneShot(sound.clip, sound.volume);
    }

    public void Stop(string name)
    {
        Sound sound = Array.Find(sounds, s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning($"Sound: {name} not found!");
            return;
        }

        if (sound.source != null)
        {
            sound.source.Stop();
        }
    }

    public void StopAllMusic()
    {
        foreach (Sound sound in sounds)
        {
            if (sound.isMusic && sound.source != null && sound.source.isPlaying)
            {
                sound.source.Stop();
            }
        }
    }

    public bool IsPlaying(string name)
    {
        Sound sound = Array.Find(sounds, s => s.name == name);
        if (sound == null) return false;
        return sound.source != null && sound.source.isPlaying;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}