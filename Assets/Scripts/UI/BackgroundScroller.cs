using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class BackgroundScroller : MonoBehaviour
{
    #region EXTERNAL DATA
    [SerializeField] private float _xScrollSpeed;
    [SerializeField] private float _scrollTime; // Before scene transition
    #endregion

    #region INTERNAL DATA
    private RawImage _rawImg;
    #endregion

    private void Awake()
    {
        _rawImg = GetComponent<RawImage>();
    }

    private void OnEnable()
    {
        EventManager.Subscribe(EventType.MAINMENU_OUTRO_START, MainMenuOutroHandler);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(EventType.MAINMENU_OUTRO_START, MainMenuOutroHandler);
        StopAllCoroutines();
    }

    private void ScrollBackground()
    {
        _rawImg.uvRect = new Rect(_rawImg.uvRect.position + new Vector2(
            _xScrollSpeed, _rawImg.uvRect.position.y) * Time.deltaTime, _rawImg.uvRect.size);
    }

    private IEnumerator ScrollBackgroundRoutine()
    {
        float timer = 0f;

        while (timer < _scrollTime)
        {
            ScrollBackground();
            timer += Time.deltaTime;
            yield return null;
        }

        // Trigger scene change
        EventManager.Trigger(EventType.PLAY_GAME, null);

        // So scroll doesn't go forever in case of bug
        while (timer < 1000f)
        {
            ScrollBackground();
            timer += Time.deltaTime;
            yield return null;
        }
    }

    #region EVENT HANDLERS
    private void MainMenuOutroHandler(object data)
    {
        StartCoroutine(ScrollBackgroundRoutine());
    }
    #endregion
}
