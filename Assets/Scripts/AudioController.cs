
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private static AudioController _instance;
    public static AudioController Instance {get {return _instance;}}
    
    // Initialize audio source and audio clips
    public AudioSource chicken, backgroundMusicSource, obstacleSounds, platformSounds, miscSounds, machineSounds, windSounds;
    public AudioClip chickenJump, chickenGrassLand, chickenCloudLand, chickenStarsLand, obstacleFall, pickEgg, rottenEgg, electrocution, spike, machine, siren, exclamation, wind;
    public float jumpVolume, obstacleFallVolume, barkVolume, backgroundMusicVolume, goldenEggVolume, rottenEggVolume, electrocutionVolume, spikeVolume, grassLandVolume, cloudLandVolume, starsLandVolume, machineVolume, sirenVolume, exclamationVolume, windVolume;
    public AudioClip[] chickenHits, ambientSounds, backgroundMusicTracks, gameOverSounds;
    public float[] gameOverVolumes, chickenHitVolumes;

    void Awake(){
        _instance = this;
        playRandomSound();
        playBackgroundMusic();
    }

    // Use the following functions in other scripts to call
    // desired sound effect
    public void PlayChickenHit(){
        int randomIndex = Random.Range(0, chickenHits.Length); 
        obstacleSounds.volume = chickenHitVolumes[randomIndex];
        obstacleSounds.PlayOneShot(chickenHits[randomIndex]);
    }

    public void PlayChickenJump(){
        chicken.volume = jumpVolume;
        chicken.PlayOneShot(chickenJump);
    }

    public void PlayChickenGrassLand(){
        platformSounds.volume = grassLandVolume;
        platformSounds.PlayOneShot(chickenGrassLand);
    }

    public void PlayChickenCloudLand(){
        platformSounds.volume = cloudLandVolume;
        platformSounds.PlayOneShot(chickenCloudLand);
    }

    public void PlayChickenStarsLand(){
        platformSounds.volume = starsLandVolume;
        platformSounds.PlayOneShot(chickenStarsLand);
    }

    public void PlayGoldenEggPick(){
        chicken.volume = goldenEggVolume;
        chicken.PlayOneShot(pickEgg);
    }

    public void PlayRottenEgg(){
        obstacleSounds.volume = rottenEggVolume;
        obstacleSounds.PlayOneShot(rottenEgg);
    }

    public void PlayElectrocution(){
        obstacleSounds.volume = electrocutionVolume;
        obstacleSounds.PlayOneShot(electrocution);
    }

    public void PlaySpike(){
        obstacleSounds.volume = spikeVolume;
        obstacleSounds.PlayOneShot(spike);
    }

    public void PlayExclamation(){
        miscSounds.volume = exclamationVolume;
        miscSounds.PlayOneShot(exclamation);
    }

    // Plays an ambient sound every 10 seconds 
    public void playRandomSound(){
        Invoke ("PlayAmbientSounds", 10);
    }

    public void PlayAmbientSounds(){
        miscSounds.volume = barkVolume;
        miscSounds.PlayOneShot(ambientSounds[Random.Range(0, ambientSounds.Length)]);
        playRandomSound();
    }

    public void PlayObstacleFall(AudioSource obstacleAudioSource){
        obstacleAudioSource.volume = obstacleFallVolume;
        obstacleAudioSource.clip = obstacleFall;
        obstacleAudioSource.Play();
    }

    public void playBackgroundMusic(){
        if (PlayerPrefs.GetInt("BGMute")==1){
            backgroundMusicSource.clip = backgroundMusicTracks[0]; // Assign the first background music track
            backgroundMusicSource.loop = false;
            backgroundMusicSource.volume = backgroundMusicVolume; 
            backgroundMusicSource.Play();
        }

    // Start looping another track after the first one finishes
    StartCoroutine(LoopBackgroundMusic());
    }

    IEnumerator LoopBackgroundMusic()
    {
        if (PlayerPrefs.GetInt("BGMute")==1){
            // Wait until the first track finishes playing
            yield return new WaitForSeconds(backgroundMusicTracks[0].length);
            backgroundMusicSource.clip = backgroundMusicTracks[1]; 
            backgroundMusicSource.loop = true;
            backgroundMusicSource.volume = backgroundMusicVolume;
            backgroundMusicSource.Play();
        }
    }

    public void stopBackgroundMusic(){
        backgroundMusicSource.Stop();
    }

    // Randomly plays a sound from array of game over sounds
    public void PlayGameOverSounds(){

        if (gameOverSounds.Length != gameOverVolumes.Length)
        {
            Debug.LogError("Array lengths are not equal. Ensure Game Over Sounds and Game Over Volumes are equal in size");
            return;
        }

        //int index = Random.Range(0, gameOverSounds.Length);
        int index = Random.Range(0, 1);
        backgroundMusicSource.volume = gameOverVolumes[index];
        backgroundMusicSource.PlayOneShot(gameOverSounds[index]);
    }

    public void PlaySiren(){
        machineSounds.volume = sirenVolume;
        machineSounds.PlayOneShot(siren);
    }

    public void PlayMachineRumble(){
        machineSounds.volume = machineVolume;
        machineSounds.PlayOneShot(machine);
    }

    public void StartWindSound(){
        windSounds.volume = windVolume;
        windSounds.clip = wind;
        windSounds.Play();
    }

    public void StopWindSound(){
        if (windSounds.isPlaying){
            windSounds.Stop();
        }
    }
}
