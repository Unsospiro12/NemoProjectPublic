using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource[] bgmPlayer;
    [SerializeField] private AudioSource[] sfxUPlayer;
    [SerializeField] private Queue<AudioSource> sfxUPlayerQueue = new Queue<AudioSource>();
    [SerializeField] private AudioSource[] sfxEPlayer;
    [SerializeField] private Queue<AudioSource> sfxEPlayerQueue = new Queue<AudioSource>();

    // 오디오 믹서와 믹서 그룹
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioMixerGroup bgmMix;
    [SerializeField] private AudioMixerGroup sfxMix;
    [SerializeField] private AudioMixerGroup eSfxMix;

    public Slider MasterVolumeSlider;
    public Slider BgmVolumeSlider;
    public Slider SfxVolumeSlider;
    //public Slider ESfxVolumeSlider;

    public AudioMixer AudioMixer;

    public float MasterVolumeValue = 0.5f;
    private float bgmVolumeValue = 0.5f;
    private float sfxVolumeValue = 0.5f;
    private float eSfxVolumeValue = 0.5f;

    [SerializeField] private float muteThreshold = 0f;
    [SerializeField] private float muteVolume = -80f;
    [SerializeField] private float volumeAdjust = 20f;

    [SerializeField][Range(0f, 1f)] private float soundEffectPitchVariance;

    [SerializeField][Range(0f, 3f)] private float bgmCrossDuration = 2;

    // BGMChange 코루틴 관리용
    private Coroutine bgmChangeCoroutine;

    protected override void Awake()
    {
        base.Awake();

        // Queue 초기화
        for (int i = 0; i < sfxUPlayer.Length; i++)
        {
           sfxUPlayerQueue.Enqueue(sfxUPlayer[i]);
        }

        for (int i = 0; i < sfxEPlayer.Length; i++)
        {
            sfxEPlayerQueue.Enqueue(sfxEPlayer[i]);
        }
    }

    public void PlayBGM(AudioClip clip)
    {
        // 현재 재생 중인 오디오 소스를 추적
        AudioSource activePlayer = null;
        AudioSource inactivePlayer = null;

        if (bgmPlayer[0].isPlaying && bgmPlayer[1].isPlaying)
        {
            // 두 플레이어가 모두 재생 중인 경우
            if (bgmPlayer[0].volume > bgmPlayer[1].volume)
            {
                activePlayer = bgmPlayer[0];
                inactivePlayer = bgmPlayer[1];
            }
            else
            {
                activePlayer = bgmPlayer[1];
                inactivePlayer = bgmPlayer[0];
            }
        }
        else if (bgmPlayer[0].isPlaying)
        {
            activePlayer = bgmPlayer[0];
            inactivePlayer = bgmPlayer[1];
        }
        else
        {
            activePlayer = bgmPlayer[1];
            inactivePlayer = bgmPlayer[0];
        }

        // 기존 코루틴 중지
        if (bgmChangeCoroutine != null)
        {
            StopCoroutine(bgmChangeCoroutine);
        }

        // 새로운 클립을 비활성 플레이어에 설정하고 전환 시작
        inactivePlayer.clip = clip;
        bgmChangeCoroutine = StartCoroutine(BGMChanger(inactivePlayer, activePlayer));
    }

    IEnumerator BGMChanger(AudioSource target, AudioSource turnOff)
    {
        float lerpTime = bgmCrossDuration; // BGM 전환에 걸리는 시간
        float curTime = 0f;
        target.Play();
        while (curTime < lerpTime)
        {
            curTime += Time.deltaTime;
            float targetValue = Mathf.Lerp(0, 1, curTime / lerpTime);
            float offValue = Mathf.Lerp(1, 0, curTime / lerpTime);

            target.volume = targetValue;
            turnOff.volume = offValue;
            yield return null;
        }
        target.volume = 1;
        turnOff.Stop();
        turnOff.clip = null;
    }

    // SFX 플레이 메서드
    public void PlayUnitSFX(AudioClip clip, float volume = 1)
    {
        var source = GetUSFXSource();
        source.volume = volume;
        source.pitch = 1f + Random.Range(-soundEffectPitchVariance, soundEffectPitchVariance);
        source.PlayOneShot(clip);
    }

    // 사운드 클립 배열 중 무작위 플레이 메서드
    public void PlayUnitSFX(AudioClip[] clips, float volume = 1)
    {
        var source = GetUSFXSource();
        source.volume = volume;
        int i = Random.Range(0, clips.Length);
        source.PlayOneShot(clips[i]);
    }

    public void PlayEnemySFX(AudioClip clip, float volume = 1)
    {
        var source = GetESFXSource();
        source.volume = volume;
        source.pitch = 1f + Random.Range(-soundEffectPitchVariance, soundEffectPitchVariance);
        source.PlayOneShot(clip);
    }

    public void PlayEnemySFX(AudioClip[] clips, float volume = 1)
    {
        var source = GetESFXSource();
        source.volume = volume;
        int i = Random.Range(0, clips.Length);
        source.PlayOneShot(clips[i]);
    }

    private AudioSource GetUSFXSource()
    {
        for (int i = 0; i < sfxUPlayer.Length; i++)
        {
            // sfx 플레이어 갯수만큼 큐를 돌아서 재생 중이지 않은 오디오 소스를 찾아서 반환
            AudioSource AS = sfxUPlayerQueue.Dequeue();
            sfxUPlayerQueue.Enqueue(AS);

            if (AS.isPlaying)
            {
                continue;
            }
            else
            {
                return AS;
            }
        }

        // 모든 플레이어가 재생중이었다면 가장 오래된 플레이어를 반환
        AudioSource audioSource = sfxUPlayerQueue.Dequeue();
        sfxUPlayerQueue.Enqueue(audioSource);

        return audioSource;
    }
    
    private AudioSource GetESFXSource()
    {
        for (int i = 0; i < sfxEPlayer.Length; i++)
        {
            // sfx 플레이어 갯수만큼 큐를 돌아서 재생 중이지 않은 오디오 소스를 찾아서 반환
            AudioSource AS = sfxEPlayerQueue.Dequeue();
            sfxEPlayerQueue.Enqueue(AS);

            if (AS.isPlaying)
            {
                continue;
            }
            else
            {
                return AS;
            }
        }

        // 모든 플레이어가 재생중이었다면 가장 오래된 플레이어를 반환
        AudioSource audioSource = sfxEPlayerQueue.Dequeue();
        sfxEPlayerQueue.Enqueue(audioSource);

        return audioSource;
    }



    public void PlaySwordSound()
    {
        //GameObject obj = UserData.Instance.SoundObjectPool.SpawnFromPool(SoundType.SFX);
        //int i = Random.Range(0, swordSfxList.Count);
        //obj.GetComponent<SoundSource>().Play(swordSfxList[i], sfxVolumeValue);
    }

    public void SetMasterVolume(float volume)
    {
        // 슬라이더 값을 -20에서 10 사이로 변환
        float adjustedVolume = Mathf.Lerp(-20f, 10f, volume);

        if (volume > muteThreshold)
        {
            AudioMixer.SetFloat("MasterVolume", adjustedVolume);
        }
        else
        {
            AudioMixer.SetFloat("MasterVolume", muteVolume);
        }

        MasterVolumeValue = volume;
    }


    public void SetBGMVolume(float volume)
    {
        // 슬라이더 값을 -20에서 10 사이로 변환
        float adjustedVolume = Mathf.Lerp(-20f, 10f, volume);

        if (volume > muteThreshold)
        {
            AudioMixer.SetFloat("BGMVolume", adjustedVolume);
        }
        else
        {
            AudioMixer.SetFloat("BGMVolume", muteVolume);
        }
        bgmVolumeValue = volume;
    }

    public void SetSFXVolume(float volume)
    {
        // 슬라이더 값을 -20에서 10 사이로 변환
        float adjustedVolume = Mathf.Lerp(-20f, 10f, volume);

        if (volume > muteThreshold)
        {
            AudioMixer.SetFloat("SFXVolume", adjustedVolume);
        }
        else
        {
            AudioMixer.SetFloat("SFXVolume", muteVolume);
        }
        sfxVolumeValue = volume;
    }

    //public void SetESFXVolume(float volume)
    //{
    //    // 슬라이더 값을 -20에서 10 사이로 변환
    //    float adjustedVolume = Mathf.Lerp(-20f, 10f, volume);

    //    if (volume > muteThreshold)
    //    {
    //        AudioMixer.SetFloat("ESFXVolume", adjustedVolume);
    //    }
    //    else
    //    {
    //        AudioMixer.SetFloat("ESFXVolume", muteVolume);
    //    }
    //    eSfxVolumeValue = volume;
    //}

    public void SetSlider()
    {
        MasterVolumeSlider.value = MasterVolumeValue;
        BgmVolumeSlider.value = bgmVolumeValue;
        SfxVolumeSlider.value = sfxVolumeValue;
        //ESfxVolumeSlider.value = eSfxVolumeValue;

        MasterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        BgmVolumeSlider.onValueChanged.AddListener(SetBGMVolume);
        SfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        //ESfxVolumeSlider.onValueChanged.AddListener(SetESFXVolume);
    }
}
