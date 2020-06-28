using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : UnitySingleton<AudioManager>
{
    //背景音乐
    private AudioSource musicSource;
    private AudioClip musicClip;

    //音效
    private AudioSource effectSource;
    private AudioClip[] effectClips;
    //音频
    private Dictionary<string, AudioClip> dicEffectClips = new Dictionary<string, AudioClip>();
    //默认音量
    public float audioValue = 0.5f;
    //是否静音
    public bool isOpenAudio = true;

	void Start ()
    {
        InitAudioManager();
    }

    private void InitAudioManager()
    {
        musicSource = GameTool.GetTheChildComponent<AudioSource>(gameObject, "AudioSource_music");
        musicClip = Resources.Load<AudioClip>("Audio/MusicAudio/GameMusic");
        musicSource.clip = musicClip;

        effectSource = GameTool.GetTheChildComponent<AudioSource>(gameObject, "AudioSource_effest");
        effectClips = Resources.LoadAll<AudioClip>("Audio/EffectAudio");
        for (int i = 0; i < effectClips.Length; i++)
        {
            dicEffectClips.Add(effectClips[i].name, effectClips[i]);
        }

        if (GameTool.HasKey("AudioValue"))
        {
            float value = GameTool.GetFloat("AudioValue");
            musicSource.volume = value;
            effectSource.volume = value;
            audioValue = value;
        }
        else
        {
            GameTool.SetFloat("AudioValue", audioValue);
            musicSource.volume = audioValue;
            effectSource.volume = audioValue;
        }
        PlayMusic();
    }
    //播放背景音乐
    private void PlayMusic()
    {
        if (GameTool.HasKey("isOpenAudio"))
        {
            isOpenAudio = bool.Parse(GameTool.GetString("isOpenAudio"));
        }
        else
        {
            GameTool.SetString("isOpenAudio","true");
            isOpenAudio = true;
        }
        if (isOpenAudio)
        {
            musicSource.Play();
        }
    }
    //播放音效的方法
    public void PlayEffectMusic(string effectMusicName)
    {
        if (!isOpenAudio)
        {
            return;
        }

        if (dicEffectClips.ContainsKey(effectMusicName))
        {
            effectSource.clip = dicEffectClips[effectMusicName];
            effectSource.Play();
        }
        else
        {
            Debug.LogWarning("找不到该文件："+effectMusicName);
        }
    }

    //声音开关
    public void OpenOrCloseAudio(bool isOn)
    {
        isOpenAudio = isOn;
        if (isOpenAudio)
        {
            musicSource.Play();
        }
        else
        {
            musicSource.Stop();
        }
        GameTool.SetString("isOpenAudio",isOpenAudio.ToString());
    }

    //设置声音大小
    public void SetVoliceValue(float value)
    {
        musicSource.volume = value;
        effectSource.volume = value;
        GameTool.SetFloat("AudioValue",value);
    }
    void Update () {
		
	}
}
