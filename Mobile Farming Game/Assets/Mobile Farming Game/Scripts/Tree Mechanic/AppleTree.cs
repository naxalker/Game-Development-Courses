using UnityEngine;

public class AppleTree : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject _treeCam;

    public void EnableCam()
    {
        _treeCam.SetActive(true);
    }

    public void DisableCam()
    {
        _treeCam.SetActive(false);
    }
}
