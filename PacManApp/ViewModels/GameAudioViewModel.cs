using Plugin.Maui.Audio;

namespace PacManApp.ViewModels;

public class GameAudioViewModel
{
    readonly IAudioManager audioManager;

    public GameAudioViewModel(IAudioManager audioManager)
    {
        this.audioManager = audioManager;
    }

    public async Task PlayAudio(string audio_state) // should be enum
    {

        try
        {
            var audioPlayer = audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync($"{audio_state}.wav"));
            audioPlayer.Play();

        }
        catch (Exception e)
        {
            Console.WriteLine($"Audio ERR => {e.Message}");

        }



    }


}