using UnityEngine;
using TMPro;
using System.Collections;

public class LoadingDots : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI loadingText; // kéo TextMeshPro (UI) vào đây

    [Header("Settings")]
    public string baseText = "Loading";
    public int maxDots = 3;
    public float interval = 0.2f; // thời gian giữa các bước

    Coroutine loopCoroutine;

    void OnEnable()
    {
        // Nếu muốn tự động chạy khi active:
        StartLoading();
    }

    void OnDisable()
    {
        StopLoading();
    }

    public void StartLoading()
    {
        if (loadingText == null) return;
        StopLoading();
        loopCoroutine = StartCoroutine(LoadingLoop());
    }

    public void StopLoading()
    {
        if (loopCoroutine != null)
        {
            StopCoroutine(loopCoroutine);
            loopCoroutine = null;
        }
        if (loadingText != null)
            loadingText.text = baseText; // reset hoặc "" tuỳ ý
    }

    IEnumerator LoadingLoop()
    {
        int dots = 0;
        while (true)
        {
            dots = (dots + 1) % (maxDots + 1); // 0..maxDots
            loadingText.text = baseText + (dots == 0 ? "" : " " + new string('.', dots));
            yield return new WaitForSeconds(interval);
        }
    }
}
