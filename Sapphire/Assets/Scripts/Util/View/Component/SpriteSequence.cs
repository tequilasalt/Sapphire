using UnityEngine;
using UnityEngine.UI;

public class SpriteSequence:AbstractViewElement,IUpdateProxy {

    private int _current;
    private int _currentFrame;

    private int _counter;
    private int _frameStep;
    private int _startCount;
    private int _endCount;
    private int _digitLength;

    private string _prefix;
    private string _suffix;

    private Image _baseImage;

    public SpriteSequence(RectTransform parent, string prefix, string suffix, int startCount, int endCount, int digitLenght = 3, int counter = 1, int frameStep = 1) : base(parent) {

        _prefix = prefix;
        _suffix = suffix;
        _startCount = startCount;
        _endCount = endCount;
        _digitLength = digitLenght;
        _counter = counter;
        _frameStep = frameStep;

        _baseImage = RectTransform.gameObject.AddComponent<Image>();
        _baseImage.gameObject.AddComponent<FixedUpdater>().Init(this);

        _current = startCount;
        _currentFrame = 1;

        SetSprite();
    }

    public bool Pause { get; set; }

    public bool Play { get; set; }

    public bool Loop { get; set; }

    public Color Color {
        get { return _baseImage.color; }
        set { _baseImage.color = value; }
    }

    public void SetFrames(int startCount, int endCount) {
        _startCount = startCount;
        _endCount = endCount;

        _current = startCount;
        _currentFrame = 1;

        SetSprite();
    }

    public void Update() {

        if (Pause) {
            return;
        }

        if (!Play) {
            return;
        }

        if (_currentFrame == _frameStep) {

            if (_current < _endCount) {
                _current += _counter;
                SetSprite();
            }
            else {
                if (Loop) {
                    _current = _startCount;
                    SetSprite();
                }
                else {
                    Play = false;
                    Pause = false;
                }
            }

            _currentFrame = 1;
        }
        else {
            _currentFrame++;
        }

    }

    private void SetSprite() {
        _baseImage.sprite = AssetCache.GetAsset<Sprite>(_prefix + StringUtil.FormatDigit(_current, _digitLength) + _suffix);
    }
}

