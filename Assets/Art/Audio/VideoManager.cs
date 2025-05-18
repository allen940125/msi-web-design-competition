using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.IO;
using Game.Audio;

public class VideoManager : Singleton<VideoManager>
{
    [System.Serializable]
    public class VideoEntry
    {
        public string videoName;
        public string fileName; // 檔名，例如 "intro.ogv"
    }

    public VideoPlayer videoPlayer;
    public List<VideoEntry> videos;

    private Dictionary<string, string> videoDict;
    private Coroutine currentCoroutine;
    [SerializeField] private bool isPlaying = false;
    
    [SerializeField] private string currentVideoName;


    [SerializeField] GameObject videoMenu;

    [SerializeField] private float stepMusicVolume = 0.5f;
    
    protected override void Awake()
    {
        base.Awake();
        videoDict = new Dictionary<string, string>();
        foreach (var entry in videos)
        {
            videoDict[entry.videoName] = entry.fileName;
        }
    }

    private void Start()
    {
        AudioManager.Instance.MasterVolume = 0.5f;
        AudioManager.Instance.MusicVolume = 0.5f;
        AudioManager.Instance.UIVolume = 0.5f;
        AudioManager.Instance.SFXVolume = 0.5f;
        AudioManager.Instance.AmbientVolume = 0.5f;
        
        videoPlayer.targetCameraAlpha = 0f;
        PlayVideo("Opening", false);
    }

    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.O))
        //     PlayVideo("NotNewComputer", false);
        // else if (Input.GetKeyDown(KeyCode.P))
        //     PlayVideo("IsNewComputer", false);
        if (Input.GetKeyDown(KeyCode.Escape) && isPlaying)
        {
            // 如果影片不是 Opening 且不是 IsNewComputer 才能提前跳過
            if (currentVideoName != "Opening" && currentVideoName != "IsNewComputer")
            {
                Debug.Log(currentVideoName);
                StopVideoEarly();
            }
        }

        
        if (isPlaying)
        {
            GameManager.Instance.Player.GetComponent<PlayerController>().canMove = false;
        }
        else
        {
            GameManager.Instance.Player.GetComponent<PlayerController>().canMove = true;
        }
    }

    private void SetVideoUIActive(bool active)
    {
        if (videoMenu != null)
            videoMenu.SetActive(active);
    }

    public void PlayVideo(string videoName, bool withFlick)
    {
        stepMusicVolume = AudioManager.Instance.MusicVolume;
        AudioManager.Instance.MusicVolume = 0f;
        
        currentVideoName = videoName;
        
        if (videoDict.TryGetValue(videoName, out string fileName))
        {
            string path = Path.Combine(Application.streamingAssetsPath, fileName);

#if UNITY_WEBGL
            path = path.Replace("file://", ""); // WebGL 中不要加 file://
#else
        if (!path.StartsWith("file://"))
            path = "file://" + path;
#endif

            if (currentCoroutine != null) StopCoroutine(currentCoroutine);
            isPlaying = true;
            SetVideoUIActive(true); // 👈 新增這行

            if (withFlick)
                currentCoroutine = StartCoroutine(PlayWithFlicker(path));
            else
                currentCoroutine = StartCoroutine(PlayNormalVideo(path));
        }
        else
        {
            Debug.LogWarning($"找不到影片：{videoName}");
        }
    }

    private IEnumerator PlayNormalVideo(string url)
    {
        videoPlayer.url = url;
        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        videoPlayer.targetCameraAlpha = 1f;
        videoPlayer.Play();

        yield return new WaitForSeconds((float)videoPlayer.length);

        EndPlayback();
    }

    private IEnumerator PlayWithFlicker(string url)
    {
        videoPlayer.url = url;
        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

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

        yield return new WaitForSeconds((float)videoPlayer.length - flickerTime);

        EndPlayback();
        PlayVideo("POPOPO", false);
    }

    private void StopVideoEarly()
    {
        AudioManager.Instance.MusicVolume = stepMusicVolume;
        
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        videoPlayer.Stop();
        EndPlayback();

        //SceneManager.LoadScene("MainMenu");
    }

    private void EndPlayback()
    {
        AudioManager.Instance.MusicVolume = stepMusicVolume;
        
        videoPlayer.targetCameraAlpha = 0f;
        isPlaying = false;
        SetVideoUIActive(false); // 👈 新增這行
        
        if (currentVideoName == "IsNewComputer")
        {
            Debug.Log(currentVideoName + "結束了");
           // PlayVideo("End", false);
        }
    }
}
