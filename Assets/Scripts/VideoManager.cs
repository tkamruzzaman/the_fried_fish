using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public static VideoManager Instance { get; private set; }

    [SerializeField] private VideoPlayer videoPlayer;

    [Header("Video Clips")]
    [SerializeField] private VideoClip introVideoClip;
    [SerializeField] private VideoClip betweenLevel1_2VideoClip;
    [SerializeField] private VideoClip betweenLevel2_3VideoClip;
    [SerializeField] private VideoClip betweenLevel3_4VideoClip;
    [SerializeField] private VideoClip endingVideoClip;

    [Header("Fading")]
    [SerializeField] private Image fadingPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        videoPlayer.playOnAwake = false;
        videoPlayer.isLooping = false;

        videoPlayer.loopPointReached += VideoEnded;

        fadingPanel.DOFade(0, 0.2f).OnComplete(() => { PlayIntroVideo(); });
    }

    private void VideoEnded(VideoPlayer source)
    {
        fadingPanel.DOFade(1, 0.5f).OnComplete(() =>
        {
            videoPlayer.Stop();
            videoPlayer.clip = null;
            print(source.clip.name + " ended....");
        });
    }

    public void PlayIntroVideo() => PlayVideo(introVideoClip);
    public void PlayBetweenLevel1_2Video() => PlayVideo(betweenLevel1_2VideoClip);
    public void PlayBetweenLevel2_3Video() => PlayVideo(betweenLevel2_3VideoClip);
    public void PlayBetweenLevel3_4Video() => PlayVideo(betweenLevel3_4VideoClip);
    public void PlayEndingVideo() => PlayVideo(endingVideoClip);

    private void PlayVideo(VideoClip videoClip)
    {
        if (videoPlayer.isPlaying) { return; }

        videoPlayer.clip = videoClip;
        fadingPanel.DOFade(1, 0.5f).OnComplete(() =>
        {
            videoPlayer.Play();
            fadingPanel.DOFade(0, 0.5f).OnComplete(() =>
            {
            });
        });
    }

}
