using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SoundtrackManager;

public class SoundtrackManager : MonoBehaviour
{
    public enum PlayState
    {
        Mute,
        Playing,
        Pending,
        PendingMute
    }

    [Serializable]
    public class Track
    {
        public PlayState state = PlayState.Mute;
        public AudioClip clip;
        public AudioSource source;
        public AudioSource sourceLoop;
        public float timeOffset = 0;
    }

    public static SoundtrackManager s;

    public float tempoSync;
    public float sectionWidth;
    public int fullTrackWidth;
    public AudioClip baseTrack;

    public List<Track> tracks;

    public float prevFrameMod;
    public float prevFrameFullMod;
    private AudioSource baseSource;
    private AudioSource baseSourceLoop;
    private bool flipFlop = false;

    private double startTime;

    const float DELAY = 2f;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        s = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        tempoSync = 0;
        prevFrameMod = 0;
        prevFrameFullMod = 0;

        startTime = AudioSettings.dspTime + DELAY;

        baseSource = gameObject.AddComponent<AudioSource>();
        baseSourceLoop = gameObject.AddComponent<AudioSource>();
        foreach (Track track in tracks)
        {
            track.source = gameObject.AddComponent<AudioSource>();
            track.source.clip = track.clip;
            track.source.volume = 0;
            track.sourceLoop = gameObject.AddComponent<AudioSource>();
            track.sourceLoop.clip = track.clip;
            track.sourceLoop.volume = 0;
            track.source.PlayScheduled(startTime + track.timeOffset);
            //track.sourceLoop.PlayScheduled(startTime + track.timeOffset + sectionWidth * fullTrackWidth);

        }


        baseSource.clip = baseTrack;
        baseSourceLoop.clip = baseTrack;
        baseSource.PlayScheduled(startTime);
        //baseSourceLoop.PlayScheduled(startTime + sectionWidth * fullTrackWidth);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //tempoSync += Time.deltaTime;
        if (Mathf.Repeat((float)AudioSettings.dspTime - (float)startTime, sectionWidth) < prevFrameMod)
        {
            Debug.Log("SPLIT");
            foreach (Track track in tracks)
            {
                if (track.state == PlayState.Pending)
                {
                    Debug.Log("Playin");
                    //track.source.time = tempoSync % (sectionWidth * fullTrackWidth) - 0.4f;
                    //track.source.Play();
                    track.source.volume = 1f;
                    track.sourceLoop.volume = 1f;
                    track.state = PlayState.Playing;
                }
                if (track.state == PlayState.PendingMute)
                {
                    Debug.Log("Mutin");
                    track.source.volume = 0f;
                    track.sourceLoop.volume = 0f;
                    track.state = PlayState.Mute;
                }
            }

            if (Mathf.Repeat((float)AudioSettings.dspTime - (float)startTime, sectionWidth *  fullTrackWidth) < prevFrameFullMod) {
                Debug.Log("Full loop");
                if (flipFlop)
                {
                    Invoke("ScheduleOriginal", 3f);
                } else
                {
                    Invoke("ScheduleLoop", 3f);
                }
                flipFlop = !flipFlop;
            }
        }

        //prevFrameMod = Mathf.Repeat(Time.time, sectionWidth);
        prevFrameMod = Mathf.Repeat((float)AudioSettings.dspTime - (float)startTime, sectionWidth);
        prevFrameFullMod = Mathf.Repeat((float)AudioSettings.dspTime - (float)startTime, sectionWidth * fullTrackWidth);
        //Debug.Log(prevFrameMod);
    }

    void ScheduleOriginal()
    {
        Debug.Log("Scheduling Original");
        baseSource.PlayScheduled(startTime + (sectionWidth * fullTrackWidth) * Mathf.Ceil(((float)AudioSettings.dspTime - (float)startTime) / (sectionWidth * fullTrackWidth)));
        foreach (var track in tracks)
        {
            track.source.PlayScheduled(startTime + (sectionWidth * fullTrackWidth) * Mathf.Ceil(((float)AudioSettings.dspTime - (float)startTime) / (sectionWidth * fullTrackWidth)));
        }
    }

    void ScheduleLoop()
    {
        Debug.Log("Scheduling Loop");
        baseSourceLoop.PlayScheduled(startTime + (sectionWidth * fullTrackWidth) * Mathf.Ceil(((float)AudioSettings.dspTime - (float)startTime) / (sectionWidth * fullTrackWidth)));
        foreach (var track in tracks)
        {
            track.sourceLoop.PlayScheduled(startTime + (sectionWidth * fullTrackWidth) * Mathf.Ceil(((float)AudioSettings.dspTime - (float)startTime) / (sectionWidth * fullTrackWidth)));
        }
    }
}
