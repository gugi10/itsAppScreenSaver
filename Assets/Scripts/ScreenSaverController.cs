using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
[RequireComponent(typeof(VideoPlayer), typeof(AudioSource), typeof(ScreenSaveEditor))]
public class ScreenSaverController : MonoBehaviour {

    public float timeout = 1f;
    public RawImage rawImage;

    [Header("Configurations")]
    [SerializeField] bool screenSleep = false;
    [SerializeField] bool playVideoWithSound = false;

    AudioSource audioSource;
    VideoPlayer videoPlayer;
    float timeSinceLastAction;

    void Awake() {
        Init();
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

    void Init() {
        DontDestroyOnLoad(this);
        videoPlayer = GetComponent<VideoPlayer>();
        audioSource = GetComponent<AudioSource>();
        HideImage();
        timeout = InputFile.Get<float>("timeout");
        if (screenSleep) {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
        if (playVideoWithSound) {
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

    void ResetTimer() {
        timeSinceLastAction = 0;
    }

    void PrepareAudio() {
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.SetTargetAudioSource(0, audioSource);
    }

    public void ShowImage() {
        rawImage.color = new Color(255, 255, 255, 255);
        rawImage.gameObject.SetActive(true);
    }

    public void HideImage() {
        rawImage.color = new Color(255, 255, 255, 0);
        rawImage.gameObject.SetActive(false);
    }
}
