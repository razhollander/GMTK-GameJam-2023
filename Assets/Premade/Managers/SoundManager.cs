using UnityEngine;

public class SoundManager : GaneEventListener
{
    public AudioSource audio1;
    public AudioSource alarm_sound;
    public AudioSource elec_sound;
    public AudioSource flood_sound;

    public override void OnGameEvent(GameEvent gameEvent)
    {

        if (gameEvent == GameEvent.Alarm)
        {
            alarm_sound.Play();
            elec_sound.Stop();
            flood_sound.Stop();
        }

        else if (gameEvent == GameEvent.Flood)
        {
            alarm_sound.Stop();
            elec_sound.Stop();
            flood_sound.Play();
        }

        else if (gameEvent == GameEvent.Electricity)
        {
            alarm_sound.Stop();
            elec_sound.Play();
            flood_sound.Stop();
        }
    }


    bool _flag = true;
    
    public void PlayAudio1()
    {
        audio1.Play();
    }
}
