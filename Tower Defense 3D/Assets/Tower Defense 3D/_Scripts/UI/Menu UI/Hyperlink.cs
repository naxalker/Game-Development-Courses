using UnityEngine;

public class Hyperlink : MonoBehaviour
{
    [SerializeField] private string _url;

    public void OpenURL() => Application.OpenURL(_url);
}
