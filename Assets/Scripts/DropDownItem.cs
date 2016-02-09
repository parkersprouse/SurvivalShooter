using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class DropDownItem {

    [SerializeField]
    private string _caption;
    public string Caption {
        get {
            return _caption;
        }
        set {
            _caption = value;
            if (OnUpdate != null)
                OnUpdate();
        }
    }

    [SerializeField]
    private Sprite _image;
    public Sprite Image {
        get {
            return _image;
        }
        set {
            _image = value;
            if (OnUpdate != null)
                OnUpdate();
        }
    }

    [SerializeField]
    private bool _isDisabled;
    public bool IsDisabled {
        get {
            return _isDisabled;
        }
        set {
            _isDisabled = value;
            if (OnUpdate != null)
                OnUpdate();
        }
    }

    [SerializeField]
    private Resolution _res;
    public Resolution Res {
        get {
            return _res;
        }
        set {
            _res = value;
            if (OnUpdate != null)
                OnUpdate();
        }
    }

    public Action OnSelect;

    internal Action OnUpdate;

    public DropDownItem(string caption) {
        _caption = caption;
    }

    public DropDownItem(Sprite image) {
        _image = image;
    }

    public DropDownItem(string caption, bool disabled) {
        _caption = caption;
        _isDisabled = disabled;
    }

    public DropDownItem(Sprite image, bool disabled) {
        _image = image;
        _isDisabled = disabled;
    }

    public DropDownItem(string caption, Sprite image, bool disabled) {
        _caption = caption;
        _image = image;
        _isDisabled = disabled;
    }

    public DropDownItem(string caption, Sprite image, bool disabled, Action onSelect) {
        _caption = caption;
        _image = image;
        _isDisabled = disabled;
        OnSelect = onSelect;
    }

    public DropDownItem(string caption, Sprite image, Action onSelect) {
        _caption = caption;
        _image = image;
        OnSelect = onSelect;
    }

    public DropDownItem(string caption, Action onSelect) {
        _caption = caption;
        OnSelect = onSelect;
    }

    public DropDownItem(Sprite image, Action onSelect) {
        _image = image;
        OnSelect = onSelect;
    }

    public DropDownItem(Resolution r) {
        _res = r;
        _caption = r.width + " x " + r.height;
    }

}