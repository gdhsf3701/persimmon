using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace moon._01.Script.Manager
{
    [DefaultExecutionOrder(-1)]
    public class SettingManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer myMixer;
        [SerializeField] private Slider masterSlider;
        [SerializeField] private Slider bgmSlider;
        [SerializeField] private Slider sfxSlider;

        [SerializeField] private GameObject settingPanel;
        [SerializeField] private GameObject mainPanel;


        private void Start()
        {
            LoadVolume();
            ChangeOpened(false);
        }

        public void StartGame()
        {
            const string gameSceneName = "Stage1";
            SceneManager.LoadScene(gameSceneName);
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void ChangeOpened(bool active)
        {
            if (settingPanel != null)
                settingPanel.SetActive(active);

            if (mainPanel != null)
                mainPanel.SetActive(!active);
        }
        
        public void SetMasterVolume()
        {
            float volume = masterSlider.value;
            myMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("Master", volume);
        }
        public void SetBgmVolume()
        {
            float volume = bgmSlider.value;
            myMixer.SetFloat("BGM", Mathf.Log10(volume)*20);
            PlayerPrefs.SetFloat("BGM",volume);
        }
        public void SetSfxVolume()
        {
            float volume = sfxSlider.value;
            myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
            PlayerPrefs.SetFloat("SFX", volume);
        }
        
        private void LoadVolume()
        {
            bgmSlider.value = PlayerPrefs.GetFloat("BGM" , 1f);
            sfxSlider.value = PlayerPrefs.GetFloat("SFX" , 1f);
            masterSlider.value = PlayerPrefs.GetFloat("Master" , 1f);

            SetBgmVolume();
            SetSfxVolume();
            SetMasterVolume();
        }
    }
}
