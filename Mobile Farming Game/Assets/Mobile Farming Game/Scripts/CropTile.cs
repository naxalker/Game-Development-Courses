using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropTile : MonoBehaviour
{
    private TileFieldState state;

    [Header("Elements")]
    [SerializeField] private Transform cropParent;
    [SerializeField] private MeshRenderer tileRenderer;
    private Crop crop;
    private CropData cropData;

    [Header("Events")]
    public static Action<CropType> onCropHarvested;

    private void Start()
    {
        state = TileFieldState.Empty;
    }

    public bool IsEmpty()
    {
        return state == TileFieldState.Empty;
    }

    public bool IsSown()
    {
        return state == TileFieldState.Sown;
    }

    public void Sow(CropData cropData)
    {
        this.cropData = cropData;

        state = TileFieldState.Sown;

        crop = Instantiate(cropData.cropPrefab, transform.position, Quaternion.identity, cropParent);
    }

    public void Water()
    {
        state = TileFieldState.Watered;

        tileRenderer.gameObject.LeanColor(Color.white * .3f, 1f);

        crop.ScaleUp();
    }

    public void Harvest()
    {
        state = TileFieldState.Empty;

        crop.ScaleDown();

        tileRenderer.gameObject.LeanColor(Color.white, 1f);

        onCropHarvested?.Invoke(cropData.cropType);
    }
}
