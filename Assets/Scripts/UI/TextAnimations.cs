using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Parsed;
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
    private List<(Label, string)> _currentTexts;
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

    public void PlayTextAnim(List<(Label, string)> texts)
    {
        _currentTexts = texts;

        if (_isAnimPlaying)
        {
            CancelTextAnim();
        }

        if (_anim == TextAnimation.TYPEWRITER)
        {
            StartCoroutine(TypewriterSequence(texts));
        }
    }

    public void CancelTextAnim()
    {
        StopAllCoroutines();
        AssignCancelledTextAnim();
        _isAnimPlaying = false;
        OnAnimComplete?.Invoke();
    }

    private void AssignCancelledTextAnim()
    {
        foreach ((Label, string) text in _currentTexts)
        {
            text.Item1.text = text.Item2;
        }
    }

    #region TYPEWRITER ANIMATION
    private IEnumerator TypewriterSequence(List<(Label, string)> texts)
    {
        foreach ((Label, string) text in texts)
        {
            yield return StartCoroutine(Typewriter(text.Item1, text.Item2));
        }
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