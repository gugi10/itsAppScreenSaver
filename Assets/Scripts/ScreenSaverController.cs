using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.IO;
[RequireComponent(typeof(VideoPlayer), typeof(AudioSource))]
public class ScreenSaverController : MonoBehaviour {

    public float timeout = 1f;
    public RawImage rawImage;

    [Header("Configurations")]
    [SerializeField] bool screenSleep = false;
    [SerializeField] bool playVideoWithSound = true;

    AudioSource audioSource;
    VideoPlayer videoPlayer;
    float timeSinceLastAction;
    List<string> videoUrls = new List<string>();
    int indexOfClipPlayed;

    public void ShowImage() {
        rawImage.color = new Color(255, 255, 255, 255);
        rawImage.gameObject.SetActive(true);
    }

    public void HideImage() {
        rawImage.color = new Color(255, 255, 255, 0);
        rawImage.gameObject.SetActive(false);
    }

    void Awake() {
        Init();
    }

    void OnEnable() {
        videoPlayer.loopPointReached += PrepareNextVideoFromUrls;
    }

    void OnDisable() {
        videoPlayer.loopPointReached -= PrepareNextVideoFromUrls;
    }

    void Update() {
        if (Input.anyKey || Input.touchCount != 0) {
            if (videoPlayer.isPlaying) {
                StopVideo();
            }
            ResetTimer();
        }

        if (timeSinceLastAction > timeout && !videoPlayer.isPlaying) {
            StartCoroutine(PlayVideoCoroutine());
        }
        if (!videoPlayer.isPlaying) {
            timeSinceLastAction += Time.deltaTime;
        }

    }

    void Reset() {
        ScreenSaverInitializer initalizer = gameObject.AddComponent<ScreenSaverInitializer>();
        initalizer.SetUp();
        DestroyImmediate(initalizer);
    }

    void Init() {
        DontDestroyOnLoad(this);
        videoPlayer = GetComponent<VideoPlayer>();
        audioSource = GetComponent<AudioSource>();

        LoadVideosFromPath(ScreenSaverConfigurations.videosPath);
        PrepareNextVideoFromUrls(videoPlayer);
        HideImage();
        timeout = InputFile.Get<float>("timeout");

        if (!ScreenSaverConfigurations.screenSleep) {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
        if (ScreenSaverConfigurations.playWithSound) {
            PrepareAudio();
        }
        videoPlayer.Prepare();
    }

    IEnumerator PlayVideoCoroutine() {
        videoPlayer.Prepare();
        while (!videoPlayer.isPrepared) {
            yield return new WaitForEndOfFrame();
        }
        rawImage.texture = videoPlayer.texture;
        ShowImage();
        if (playVideoWithSound) {
            PrepareAudio();
        }
        videoPlayer.Play();
    }

    void StopVideo() {
        ResetTimer();
        HideImage();
        videoPlayer.Stop();
        StopCoroutine(PlayVideoCoroutine());
    }

    void PrepareNextVideoFromUrls(VideoPlayer vp) {
        if(indexOfClipPlayed < videoUrls.Count) {
            vp.url = videoUrls[indexOfClipPlayed];
        }
        else {
            ResetClipList();
            vp.url = videoUrls[indexOfClipPlayed];
        }
        indexOfClipPlayed++;
    }

    void ResetTimer() {
        timeSinceLastAction = 0;
        PrepareNextVideoFromUrls(videoPlayer);
    }
    
    void ResetClipList() {
        indexOfClipPlayed = 0;
    }

    void PrepareAudio() {
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.SetTargetAudioSource(0, audioSource);
    }

    void LoadVideosFromPath(string path) {
        DirectoryInfo directory = new DirectoryInfo(path);
        FileInfo[] fileInfo = directory.GetFiles("*.mp4");
        foreach (FileInfo file in fileInfo) {
            videoUrls.Add(path + file.Name);
        }
    }



}
