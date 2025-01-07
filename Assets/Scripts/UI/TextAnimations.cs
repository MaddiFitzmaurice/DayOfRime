using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public enum TextAnimation {
    TYPEWRITER,
}

public class TextAnimations : MonoBehaviour
{
    #region EXTERNAL DATA
    [Header("Animation")]
    [SerializeField] TextAnimation _anim = TextAnimation.TYPEWRITER;

    [Header("Timings")]
    [SerializeField] private float _textAnimationSpeed = 60f;
    [SerializeField] private float _punctuationDelayTime = 0.4f;
    [SerializeField] private float _ellipsisDelayTime = 0.5f;
    public Action OnAnimComplete;
    #endregion

    #region INTERNAL DATA
    private bool _isAnimPlaying = false;
    private Label _currentTextBox;
    private string _currentSubline;
    #endregion

    private void Start()
    {
        if (_textAnimationSpeed < 0 || _punctuationDelayTime < 0)
        {
            Debug.LogError("Time fields cannot be negatives");
        }
    }

    public bool IsAnimPlaying()
    {
        return _isAnimPlaying;
    }

    public void PlayTextAnim(Label textBox, string subLine)
    {
        if (_anim == TextAnimation.TYPEWRITER)
        {
            PlayTypewriterAnim(textBox, subLine);
        }
    }

    public void StopTextAnim()
    {
        StopAllCoroutines();
        _currentTextBox.text = _currentSubline;
        _isAnimPlaying = false;
        OnAnimComplete?.Invoke();
    }

    #region TYPEWRITER ANIMATION
    private void PlayTypewriterAnim(Label textBox, string subLine)
    {
        if (_isAnimPlaying)
        {
            StopTextAnim();
        }
        
        _currentTextBox = textBox;
        _currentSubline = subLine;
        StartCoroutine(Typewriter(textBox, subLine));
    }

    private IEnumerator Typewriter(Label textBox, string subLine)
    {
        _isAnimPlaying = true;

        yield return new WaitForSeconds(0.1f);

        bool punctuationDelay = false;
        bool potentialEllipsis = false;

        foreach(char character in subLine)
        {
            if (punctuationDelay)
            {
                if (character == ' ')
                {
                    yield return new WaitForSeconds(_punctuationDelayTime);
                    punctuationDelay = false;
                }
                else if (potentialEllipsis && character == '.')
                {
                    yield return new WaitForSeconds(_ellipsisDelayTime);
                }
            }
            else 
            {
                if (character == '?' || character == '.' || character == ',' || character == ':' ||
                     character == ';' || character == '!' || character == '-') 
                {
                    punctuationDelay = true;

                    if (character == '.')
                    {
                        potentialEllipsis = true;
                    }
                    else
                    {
                        potentialEllipsis = false;
                    }
                }
                    
                yield return new WaitForSeconds(1f / _textAnimationSpeed);
            }
            
            textBox.text += character;
        }

        _isAnimPlaying = false;
        OnAnimComplete?.Invoke();
        yield return null;
    }
    #endregion
}