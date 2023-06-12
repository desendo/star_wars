using System;
using ReactiveExtension;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class WeaponUIView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _index;
    [SerializeField] private TextMeshProUGUI _id;
    [SerializeField] private Button _setButton;
    [SerializeField] private GameObject _selected;
    [SerializeField] private Image _progress;
    private IDisposable _progressSub;
    public string Id { get; private set; }

    public void Setup(Action onClick, string id, string slot, IReadonlyReactive<float> progress)
    {
        _setButton.onClick.RemoveAllListeners();
        _setButton.onClick.AddListener(() => onClick?.Invoke());
        _id.text = id;
        Id = id;
        _index.text = slot;

        _progressSub?.Dispose();
        _progressSub = progress.Subscribe(x => _progress.fillAmount = x);
        SetSelected(false);
    }

    public void SetSelected(bool selected)
    {
        _selected.SetActive(selected);
    }
}