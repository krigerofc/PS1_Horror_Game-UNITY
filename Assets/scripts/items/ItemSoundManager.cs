using UnityEngine;
using System.Collections;

public class ItemSoundManager : MonoBehaviour
{
    // Permite que outros scripts chamem o som via ItemSoundManager.instance
    private static ItemSoundManager _instance;
    public static ItemSoundManager instance {
        get {
            if (_instance == null) _instance = FindFirstObjectByType<ItemSoundManager>();
            return _instance;
        }
    }

    private AudioSource audioPlayer;

    private void Awake() {
        SetupSingleton();
        SetupAudioSource();
    }

    // Garante que o som não pare ao mudar de cena e que só exista um Gerente
    private void SetupSingleton() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Configura o som para ser 2D (toca "dentro" do player)
    private void SetupAudioSource() {
        audioPlayer = GetComponent<AudioSource>();
        if (audioPlayer == null) audioPlayer = gameObject.AddComponent<AudioSource>();
        audioPlayer.spatialBlend = 0; 
    }

    // Toca um único som "dentro" do player (2D)
    public void Play(AudioClip clip) {
        if (clip != null) audioPlayer.PlayOneShot(clip);
    }

    // Toca um som em um ponto específico do mapa (3D - como o item batendo no chão)
    public void PlayAtPosition(AudioClip clip, Vector3 position, float volume = 1.0f) {
        if (clip != null) {
            // AudioSource.PlayClipAtPoint não permite volume direto, então usamos uma alternativa
            GameObject tempGO = new GameObject("TempAudio");
            tempGO.transform.position = position;
            AudioSource aSource = tempGO.AddComponent<AudioSource>();
            aSource.clip = clip;
            aSource.volume = volume;
            aSource.spatialBlend = 1.0f; // Som 3D
            aSource.Play();
            Destroy(tempGO, clip.length);
        }
    }

    // Toca uma lista de sons com intervalo (útil para comer/beber em etapas)
    public void PlaySequence(AudioClip[] clips, float indexGap = 1.0f) {
        if (clips == null || clips.Length == 0) return;
        StartCoroutine(ExecuteSequence(clips, indexGap));
    }

    private IEnumerator ExecuteSequence(AudioClip[] clips, float gap) {
        foreach (AudioClip clip in clips) {
            if (clip != null) audioPlayer.PlayOneShot(clip);
            yield return new WaitForSeconds(gap);
        }
    }
}