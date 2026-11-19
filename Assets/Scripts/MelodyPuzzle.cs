using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class MelodyPuzzle : MonoBehaviour
{
    public GameManager gameManager;
    public AudioSource noteAudio;
    public TMP_Text statusText;

    // End music starts only after the melody puzzle is solved.
    public AudioSource backgroundMusic;

    // note tiles from low to high
    public XRSimpleInteractable[] noteTiles;
    public XRSimpleInteractable replayTile;
    public Color[] noteColors =
    {
        new Color(0.85f, 0.25f, 0.25f),
        new Color(0.9f, 0.75f, 0.2f),
        new Color(0.25f, 0.7f, 0.35f),
        new Color(0.25f, 0.5f, 0.9f)
    };

    // tile indexes in the order the player has to repeat
    public int[] melody = { 0, 2, 1, 3, 2 };
    public float[] noteFrequencies = { 261.63f, 329.63f, 392f, 493.88f };
    public float noteLength = 0.5f;
    public float notePause = 0.15f;

    // turn off to make the player go by ear only
    public bool showNotesWhilePlaying = true;

    private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

    private AudioClip[] noteClips;
    private AudioClip wrongClip;
    private Renderer[] tileRenderers;
    private MaterialPropertyBlock tileProperties;
    private int progress;
    private bool playingMelody;
    private bool completed;

    private void Awake()
    {
        noteClips = new AudioClip[noteFrequencies.Length];
        for (var i = 0; i < noteClips.Length; i++)
            noteClips[i] = MakeTone(noteFrequencies[i], noteLength);
        wrongClip = MakeTone(110f, 0.35f);

        tileProperties = new MaterialPropertyBlock();
        tileRenderers = new Renderer[noteTiles.Length];
        for (var i = 0; i < noteTiles.Length; i++)
        {
            tileRenderers[i] = noteTiles[i].GetComponent<Renderer>();
            SetTileLit(i, false);
        }
    }

    private void OnEnable()
    {
        progress = 0;

        foreach (var tile in noteTiles)
            tile.selectEntered.AddListener(OnNoteSelected);

        if (replayTile != null)
            replayTile.selectEntered.AddListener(OnReplaySelected);

        if (completed)
            return;

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

        if (gameManager != null && gameManager.TimeUp)
            return;

        var index = System.Array.IndexOf(noteTiles, args.interactableObject as XRSimpleInteractable);
        if (index < 0)
            return;

        StartCoroutine(FlashTile(index, 0.25f));

        if (progress >= melody.Length || index != melody[progress])
        {
            // wrong note, start the melody over
            progress = 0;
            if (noteAudio != null)
                noteAudio.PlayOneShot(wrongClip);
            Buzz(args, 0.7f, 0.25f);
            SetStatus("Not quite, try again");
            return;
        }

        PlayNote(index);
        Buzz(args, 0.3f, 0.08f);
        progress++;
        SetStatus(progress + " / " + melody.Length);

        if (progress < melody.Length)
            return;

        completed = true;
        SetStatus("The door is open!");
        if (backgroundMusic != null)
            backgroundMusic.Play();
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
            if (showNotesWhilePlaying)
                SetTileLit(note, true);
            yield return new WaitForSeconds(noteLength);
            SetTileLit(note, false);
            yield return new WaitForSeconds(notePause);
        }

        SetStatus("Your turn");
        playingMelody = false;
    }

    private IEnumerator FlashTile(int index, float seconds)
    {
        SetTileLit(index, true);
        yield return new WaitForSeconds(seconds);
        SetTileLit(index, false);
    }

    private void PlayNote(int index)
    {
        if (noteAudio == null || index < 0 || index >= noteClips.Length)
            return;

        noteAudio.PlayOneShot(noteClips[index]);
    }

    // each tile keeps a dim version of its colour and brightens when it sounds
    private void SetTileLit(int index, bool lit)
    {
        if (index < 0 || index >= tileRenderers.Length || tileRenderers[index] == null)
            return;

        var color = index < noteColors.Length ? noteColors[index] : Color.white;
        tileRenderers[index].GetPropertyBlock(tileProperties);
        tileProperties.SetColor(BaseColorId, lit ? color : color * 0.35f);
        tileRenderers[index].SetPropertyBlock(tileProperties);
    }

    private static void Buzz(SelectEnterEventArgs args, float strength, float seconds)
    {
        if (args.interactorObject is XRBaseInputInteractor controller)
            controller.SendHapticImpulse(strength, seconds);
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
