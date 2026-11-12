using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Sounds : MonoBehaviour
{
    public static Sounds Instance;

    public AudioClip[] click;   // botones del menu
    public AudioClip[] place;   // colocar
    public AudioClip[] select;  // seleccionar un mueble
    public AudioClip[] remove;  // borrar
    public AudioClip[] error;   // no cabe

    public Transform menu;

    private AudioSource source;

    void Awake()
    {
        Instance = this;
        source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
    }

    IEnumerator Start()
    {
        yield return null;
        Button[] buttons = menu.GetComponentsInChildren<Button>(true);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.AddListener(() => Play(click));
        }
    }

    public void Play(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return;

        AudioClip clip = clips[Random.Range(0, clips.Length)];
        source.pitch = Random.Range(0.95f, 1.05f);
        source.PlayOneShot(clip);
    }
}