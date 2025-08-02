using csiimnida.CSILib.SoundManager.RunTime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace moon._01.Script
{
    public class GoTitle : MonoBehaviour
    {
        [SerializeField] private string titleName;

        public void GoTitleToBtn()
        {
            SoundManager.Instance.PlaySound("ButtonSFX");
            SceneManager.LoadScene(titleName);
        }
    }
}
