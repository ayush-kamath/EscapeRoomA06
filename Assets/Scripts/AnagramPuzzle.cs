using System;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class AnagramPuzzle : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject schedule;
    public GameObject password;
    public GameObject passwordDisplay;

    private XRSimpleInteractable[] tiles;
    private XRSimpleInteractable firstTile;
    private TMP_Text selectedText;
    private Color selectedTextColor;
    private bool completed;

    private void OnEnable()
    {
        tiles = GetComponentsInChildren<XRSimpleInteractable>();
        Array.Sort(tiles, (left, right) =>
            right.transform.localPosition.x.CompareTo(left.transform.localPosition.x));

        foreach (var tile in tiles)
            tile.selectEntered.AddListener(OnTileSelected);
    }

    private void OnDisable()
    {
        ClearSelection();

        if (tiles == null)
            return;

        foreach (var tile in tiles)
            tile.selectEntered.RemoveListener(OnTileSelected);
    }

    private void OnTileSelected(SelectEnterEventArgs args)
    {
        if (completed || !(args.interactableObject is XRSimpleInteractable tile))
            return;

        if (firstTile == null)
        {
            firstTile = tile;
            selectedText = tile.GetComponentInChildren<TMP_Text>();
            if (selectedText != null)
            {
                selectedTextColor = selectedText.color;
                selectedText.color = Color.yellow;
            }
            return;
        }

        if (firstTile == tile)
        {
            ClearSelection();
            return;
        }

        var firstIndex = Array.IndexOf(tiles, firstTile);
        var secondIndex = Array.IndexOf(tiles, tile);
        if (firstIndex < 0 || secondIndex < 0)
            return;

        var firstTransform = firstTile.transform;
        var secondTransform = tile.transform;
        var position = firstTransform.localPosition;
        var rotation = firstTransform.localRotation;
        firstTransform.localPosition = secondTransform.localPosition;
        firstTransform.localRotation = secondTransform.localRotation;
        secondTransform.localPosition = position;
        secondTransform.localRotation = rotation;

        tiles[firstIndex] = tile;
        tiles[secondIndex] = firstTile;
        ClearSelection();

        var word = "";
        foreach (var letterTile in tiles)
        {
            var label = letterTile.GetComponentInChildren<TMP_Text>();
            if (label == null)
                return;
            word += label.text.Trim();
        }

        if (string.Equals(word, "ENGLISH", StringComparison.OrdinalIgnoreCase))
        {
            completed = true;
            gameManager.CompletePuzzle(1);
            Debug.Log("Puzzle 2 completed!");
            if (schedule != null)
                schedule.SetActive(true);
            passwordDisplay.SetActive(true);
            password.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    private void ClearSelection()
    {
        if (selectedText != null)
            selectedText.color = selectedTextColor;

        selectedText = null;
        firstTile = null;
    }
}
