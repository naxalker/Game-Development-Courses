using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileSlot)), CanEditMultipleObjects]
public class TileSlotEditor : Editor
{
    private GUIStyle _centeredStyle;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        base.OnInspectorGUI();

        _centeredStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            fontSize = 14
        };

        float oneButtonWidth = EditorGUIUtility.currentViewWidth - 25;
        float twoButtonWidth = (EditorGUIUtility.currentViewWidth - 25) / 2f;
        float threeButtonWidth = (EditorGUIUtility.currentViewWidth - 25) / 3f;

        GUILayout.Label("Position and Rotation", _centeredStyle);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Rotate Left", GUILayout.Width(twoButtonWidth)))
        {
            foreach (TileSlot tileSlot in targets)
            {
                tileSlot.RotateTile(-1);
            }
        }

        if (GUILayout.Button("Rotate right", GUILayout.Width(twoButtonWidth)))
        {
            foreach (TileSlot tileSlot in targets)
            {
                tileSlot.RotateTile(1);
            }
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("- .1f on the Y", GUILayout.Width(twoButtonWidth)))
        {
            foreach (TileSlot tileSlot in targets)
            {
                tileSlot.AdjustY(-1);
            }
        }

        if (GUILayout.Button("+ .1f on the Y", GUILayout.Width(twoButtonWidth)))
        {
            foreach (TileSlot tileSlot in targets)
            {
                tileSlot.AdjustY(1);
            }
        }

        GUILayout.EndHorizontal();


        GUILayout.Label("Tile Options", _centeredStyle);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Field", GUILayout.Width(twoButtonWidth)))
        {
            GameObject newTile = FindFirstObjectByType<TileSetHolder>().TileField;

            foreach (TileSlot tileSlot in targets)
            {
                tileSlot.SwitchTile(newTile);
            }
        }

        if (GUILayout.Button("Road", GUILayout.Width(twoButtonWidth)))
        {
            GameObject newTile = FindFirstObjectByType<TileSetHolder>().TileRoad;

            foreach (TileSlot tileSlot in targets)
            {
                tileSlot.SwitchTile(newTile);
            }
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Sideway", GUILayout.Width(oneButtonWidth)))
        {
            GameObject newTile = FindFirstObjectByType<TileSetHolder>().TileSideway;

            foreach (TileSlot tileSlot in targets)
            {
                tileSlot.SwitchTile(newTile);
            }
        }

        GUILayout.EndHorizontal();


        GUILayout.Label("Corner Options", _centeredStyle);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Inner Corner", GUILayout.Width(twoButtonWidth)))
        {
            GameObject newTile = FindFirstObjectByType<TileSetHolder>().TileInnerCorner;

            foreach (TileSlot tileSlot in targets)
            {
                tileSlot.SwitchTile(newTile);
            }
        }

        if (GUILayout.Button("Outer Corner", GUILayout.Width(twoButtonWidth)))
        {
            GameObject newTile = FindFirstObjectByType<TileSetHolder>().TileOuterCorner;

            foreach (TileSlot tileSlot in targets)
            {
                tileSlot.SwitchTile(newTile);
            }
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Inner Corner Small", GUILayout.Width(twoButtonWidth)))
        {
            GameObject newTile = FindFirstObjectByType<TileSetHolder>().TileInnerCornerSmall;

            foreach (TileSlot tileSlot in targets)
            {
                tileSlot.SwitchTile(newTile);
            }
        }

        if (GUILayout.Button("Outer Corner Small", GUILayout.Width(twoButtonWidth)))
        {
            GameObject newTile = FindFirstObjectByType<TileSetHolder>().TileOuterCornerSmall;

            foreach (TileSlot tileSlot in targets)
            {
                tileSlot.SwitchTile(newTile);
            }
        }

        GUILayout.EndHorizontal();


        GUILayout.Label("Bridges and Hills", _centeredStyle);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Hill 1", GUILayout.Width(threeButtonWidth)))
        {
            GameObject newTile = FindFirstObjectByType<TileSetHolder>().TileHill1;

            foreach (TileSlot tileSlot in targets)
            {
                tileSlot.SwitchTile(newTile);
            }
        }

        if (GUILayout.Button("Hill 2", GUILayout.Width(threeButtonWidth)))
        {
            GameObject newTile = FindFirstObjectByType<TileSetHolder>().TileHill2;

            foreach (TileSlot tileSlot in targets)
            {
                tileSlot.SwitchTile(newTile);
            }
        }

        if (GUILayout.Button("Hill 3", GUILayout.Width(threeButtonWidth)))
        {
            GameObject newTile = FindFirstObjectByType<TileSetHolder>().TileHill3;

            foreach (TileSlot tileSlot in targets)
            {
                tileSlot.SwitchTile(newTile);
            }
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Bridge with field", GUILayout.Width(threeButtonWidth)))
        {
            GameObject newTile = FindFirstObjectByType<TileSetHolder>().TileBridgeField;

            foreach (TileSlot tileSlot in targets)
            {
                tileSlot.SwitchTile(newTile);
            }
        }

        if (GUILayout.Button("Bridge with road", GUILayout.Width(threeButtonWidth)))
        {
            GameObject newTile = FindFirstObjectByType<TileSetHolder>().TileBridgeRoad;

            foreach (TileSlot tileSlot in targets)
            {
                tileSlot.SwitchTile(newTile);
            }
        }

        if (GUILayout.Button("Bridge with sideway", GUILayout.Width(threeButtonWidth)))
        {
            GameObject newTile = FindFirstObjectByType<TileSetHolder>().TileBridgeSideway;

            foreach (TileSlot tileSlot in targets)
            {
                tileSlot.SwitchTile(newTile);
            }
        }

        GUILayout.EndHorizontal();
    }
}
