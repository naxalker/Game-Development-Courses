using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class SoundManager : IInitializable, IDisposable
{
    private AudioClipRefsSO _audioClipRefs;
    private readonly DeliveryManager _deliveryManager;
    private readonly Player _player;

    private float _volume = 1f;

    public SoundManager(AudioClipRefsSO audioClipRefs, DeliveryManager deliveryManager, Player player)
    {
        _deliveryManager = deliveryManager;
        _audioClipRefs = audioClipRefs;
        _player = player;
    }

    public float Volume => _volume;

    public void Initialize()
    {
        _deliveryManager.RecipeSuccessed += RecipeSuccessedHandler;
        _deliveryManager.RecipeFailed += RecipeFailedHandler;

        CuttingCounter.AnyCut += AnyCutHandler;

        _player.PickedSomething += PickedSomethingHandler;

        BaseCounter.AnyObjectPlaced += AnyObjectPlacedHandler;

        TrashCounter.AnyObjectTrashed += AnyObjectTrashedHandler;
    }

    public void Dispose()
    {
        _deliveryManager.RecipeSuccessed -= RecipeSuccessedHandler;
        _deliveryManager.RecipeFailed -= RecipeFailedHandler;

        CuttingCounter.AnyCut -= AnyCutHandler;

        _player.PickedSomething -= PickedSomethingHandler;

        BaseCounter.AnyObjectPlaced -= AnyObjectPlacedHandler;

        TrashCounter.AnyObjectTrashed -= AnyObjectTrashedHandler;
    }

    public void ChangeVolume()
    {
        _volume += .1f;

        if (_volume > 1f)
        {
            _volume = 0f;
        }
    }

    public void PlayFootstepSound(Vector3 position)
    {
        PlaySound(_audioClipRefs.Footstep, position);
    }

    private void AnyObjectTrashedHandler(TrashCounter counter)
    {
        PlaySound(_audioClipRefs.Trash, counter.transform.position);
    }

    private void AnyObjectPlacedHandler(BaseCounter counter)
    {
        PlaySound(_audioClipRefs.Chop, counter.transform.position);
    }

    private void PickedSomethingHandler()
    {
        PlaySound(_audioClipRefs.ObjectPickup, _player.transform.position);
    }

    private void AnyCutHandler(CuttingCounter counter)
    {
        PlaySound(_audioClipRefs.Chop, counter.transform.position);
    }

    private void RecipeSuccessedHandler()
    {
        PlaySound(_audioClipRefs.DeliverySuccess, Camera.main.transform.position, .15f);
    }

    private void RecipeFailedHandler()
    {
        PlaySound(_audioClipRefs.DeliveryFail, Camera.main.transform.position, .15f);
    }

    private void PlaySound(AudioClip[] audioClips, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClips[Random.Range(0, audioClips.Length)], position, volume);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * _volume);
    }
}
