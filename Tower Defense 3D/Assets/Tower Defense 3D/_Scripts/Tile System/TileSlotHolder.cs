using UnityEngine;

public class TileSlotHolder : MonoBehaviour
{
    [field: Header("Straight Tiles")]
    [field: SerializeField] public GameObject TileRoad { get; private set; }
    [field: SerializeField] public GameObject TileField { get; private set; }
    [field: SerializeField] public GameObject TileSideway { get; private set; }

    [field: Header("Corner Tiles")]
    [field: SerializeField] public GameObject TileInnerCorner { get; private set; }
    [field: SerializeField] public GameObject TileInnerCornerSmall { get; private set; }
    [field: SerializeField] public GameObject TileOuterCorner { get; private set; }
    [field: SerializeField] public GameObject TileOuterCornerSmall { get; private set; }

    [field: Header("Hills Tiles")]
    [field: SerializeField] public GameObject TileHill1 { get; private set; }
    [field: SerializeField] public GameObject TileHill2 { get; private set; }
    [field: SerializeField] public GameObject TileHill3 { get; private set; }

    [field: Header("Bridges Tiles")]
    [field: SerializeField] public GameObject TileBridgeField { get; private set; }
    [field: SerializeField] public GameObject TileBridgeRoad { get; private set; }
    [field: SerializeField] public GameObject TileBridgeSideway { get; private set; }
}
