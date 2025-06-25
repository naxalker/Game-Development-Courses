using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(InventoryDisplay))]
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    private Inventory inventory;
    private InventoryDisplay inventoryDisplay;
    private string dataPath;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        dataPath = Application.dataPath + "/inventoryData.txt";

        LoadInventory();
        ConfigureInventoryDisplay();

        CropTile.onCropHarvested += CropHarvestedCallback;
        AppleTree.OnAppleHarvested += CropHarvestedCallback;
    }

    private void OnDestroy()
    {
        CropTile.onCropHarvested -= CropHarvestedCallback;
        AppleTree.OnAppleHarvested -= CropHarvestedCallback;
    }

    public Inventory GetInventory()
    {
        return inventory;
    }

    private void CropHarvestedCallback(CropType cropType)
    {
        inventory.CropHarvestedCallback(cropType);
        inventoryDisplay.UpdateDisplay(inventory);
        SaveInventory();
    }

    [NaughtyAttributes.Button]
    public void ClearInventory()
    {
        inventory.Clear();
        inventoryDisplay.UpdateDisplay(inventory);

        SaveInventory();
    }

    private void LoadInventory()
    {
        string data;

        if (File.Exists(dataPath))
        {
            data = File.ReadAllText(dataPath);
            inventory = JsonUtility.FromJson<Inventory>(data);

            if (inventory == null)
                inventory = new Inventory();
        }
        else
        {
            File.Create(dataPath);
            inventory = new Inventory();
        }
    }

    private void SaveInventory()
    {
        string data = JsonUtility.ToJson(inventory, true);
        File.WriteAllText(dataPath, data);
    }

    private void ConfigureInventoryDisplay()
    {
        inventoryDisplay = GetComponent<InventoryDisplay>();
        inventoryDisplay.Configure(inventory);
    }
}
