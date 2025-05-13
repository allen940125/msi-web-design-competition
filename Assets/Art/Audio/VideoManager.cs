using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoManager : Singleton<VideoManager>
{
    [System.Serializable]
    public class VideoEntry
    {
        public string videoName;
        public VideoClip clip;
    }

    public VideoPlayer videoPlayer;
    public List<VideoEntry> videos;

    private Dictionary<string, VideoClip> videoDict;
    private Coroutine currentCoroutine;
    private bool isPlaying = false;

    protected override void Awake()
    {
        base.Awake();
        videoDict = new Dictionary<string, VideoClip>();
        foreach (var entry in videos)
        {
            videoDict[entry.videoName] = entry.clip;
        }
    }

    private void Start()
    {
        videoPlayer.targetCameraAlpha = 0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
            PlayVideo("NotNewComputer", false);
        else if (Input.GetKeyDown(KeyCode.P))
            PlayVideo("IsNewComputer", false);
        else if (Input.GetKeyDown(KeyCode.Escape) && isPlaying)
            StopVideoEarly();
        
        if (isPlaying)
        {
            GameManager.Instance.Player.GetComponent<PlayerController>().canMove = false;
        }
        else
        {
            GameManager.Instance.Player.GetComponent<PlayerController>().canMove = true;
        }
    }

    public void PlayVideo(string videoName, bool withFlick)
    {
        if (videoDict.TryGetValue(videoName, out VideoClip clip))
        {
            if (currentCoroutine != null) StopCoroutine(currentCoroutine);
            isPlaying = true;

            if (withFlick)
                currentCoroutine = StartCoroutine(PlayWithFlicker(clip));
            else
                currentCoroutine = StartCoroutine(PlayNormalVideo(clip));
        }
        else
        {
            Debug.LogWarning($"找不到影片：{videoName}");
        }
    }

    private IEnumerator PlayNormalVideo(VideoClip clip)
    {
        videoPlayer.clip = clip;
        videoPlayer.targetCameraAlpha = 1f;
        videoPlayer.Play();

        yield return new WaitForSeconds((float)clip.length);

        EndPlayback();
        SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator PlayWithFlicker(VideoClip clip)
    {
        videoPlayer.clip = clip;
        videoPlayer.Play();

        float flickerTime = 2f;
        float elapsed = 0f;
        float flickerInterval = 0.1f;

        while (elapsed < flickerTime)
        {
            videoPlayer.targetCameraAlpha = Random.value > 0.5f ? 1f : 0f;
            elapsed += flickerInterval;
            yield return new WaitForSeconds(flickerInterval);
        }

        videoPlayer.targetCameraAlpha = 1f;

        yield return new WaitForSeconds((float)clip.length - flickerTime);

        EndPlayback();
        PlayVideo("POPOPO", false);
    }

    private void StopVideoEarly()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        videoPlayer.Stop();
        EndPlayback();

        // 選擇回主選單或其他處理方式
        SceneManager.LoadScene("MainMenu");
    }

    private void EndPlayback()
    {
        videoPlayer.targetCameraAlpha = 0f;
        isPlaying = false;
    }
}
