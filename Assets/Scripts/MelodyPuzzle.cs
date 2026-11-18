using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class MelodyPuzzle : MonoBehaviour
{
    public GameManager gameManager;
    public AudioSource noteAudio;
    public TMP_Text statusText;

    // note tiles from low to high
    public XRSimpleInteractable[] noteTiles;
    public XRSimpleInteractable replayTile;

    // tile indexes in the order the player has to repeat
    public int[] melody = { 0, 2, 1, 3, 2 };
    public float[] noteFrequencies = { 261.63f, 329.63f, 392f, 523.25f };
    public float noteLength = 0.5f;
    public float notePause = 0.15f;

    private AudioClip[] noteClips;
    private int progress;
    private bool playingMelody;
    private bool completed;

    private void Awake()
    {
        noteClips = new AudioClip[noteFrequencies.Length];
        for (var i = 0; i < noteClips.Length; i++)
            noteClips[i] = MakeTone(noteFrequencies[i], noteLength);
    }

    private void OnEnable()
    {
        progress = 0;

        foreach (var tile in noteTiles)
            tile.selectEntered.AddListener(OnNoteSelected);

        if (replayTile != null)
            replayTile.selectEntered.AddListener(OnReplaySelected);

        if (!completed)
            StartCoroutine(PlayMelody(1.5f));
    }

    private void OnDisable()
    {
        playingMelody = false;

        foreach (var tile in noteTiles)
            tile.selectEntered.RemoveListener(OnNoteSelected);

        if (replayTile != null)
            replayTile.selectEntered.RemoveListener(OnReplaySelected);
    }

    private void OnReplaySelected(SelectEnterEventArgs args)
    {
        if (completed || playingMelody)
            return;

        progress = 0;
        StartCoroutine(PlayMelody(0.2f));
    }

    private void OnNoteSelected(SelectEnterEventArgs args)
    {
        if (completed || playingMelody)
            return;

        var index = System.Array.IndexOf(noteTiles, args.interactableObject as XRSimpleInteractable);
        if (index < 0)
            return;

        PlayNote(index);

        if (index != melody[progress])
        {
            // wrong note, start the melody over
            progress = 0;
            SetStatus("Not quite. Listen again.");
            return;
        }

        progress++;
        SetStatus(progress + " / " + melody.Length);

        if (progress < melody.Length)
            return;

        completed = true;
        SetStatus("That's the tune!");
        if (gameManager != null)
            gameManager.CompletePuzzle(3);
        Debug.Log("Puzzle 4 completed!");
    }

    private IEnumerator PlayMelody(float delay)
    {
        playingMelody = true;
        SetStatus("Listen...");
        yield return new WaitForSeconds(delay);

        foreach (var note in melody)
        {
            PlayNote(note);
            yield return new WaitForSeconds(noteLength + notePause);
        }

        SetStatus("Your turn");
        playingMelody = false;
    }

    private void PlayNote(int index)
    {
        if (noteAudio == null || index < 0 || index >= noteClips.Length)
            return;

        noteAudio.PlayOneShot(noteClips[index]);
    }

    private void SetStatus(string message)
    {
        if (statusText != null)
            statusText.text = message;
    }

    // sine tone with a short fade so it does not click
    private static AudioClip MakeTone(float frequency, float seconds)
    {
        const int sampleRate = 44100;
        var count = Mathf.CeilToInt(sampleRate * seconds);
        var samples = new float[count];
        var fade = sampleRate * 0.02f;

        for (var i = 0; i < count; i++)
        {
            var envelope = Mathf.Min(1f, i / fade) * Mathf.Min(1f, (count - 1 - i) / fade);
            var time = i / (float)sampleRate;
            var wave = Mathf.Sin(2f * Mathf.PI * frequency * time)
                + 0.3f * Mathf.Sin(4f * Mathf.PI * frequency * time);
            samples[i] = wave * envelope * 0.45f;
        }

        var clip = AudioClip.Create("note_" + Mathf.RoundToInt(frequency), count, 1, sampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }
}
