using UnityEngine;

public class PlayerAnimatorTrigger : MonoBehaviour
{
    public void EndDieAnimation()
    {
        Padeinout._instance.PadeIn("Fail");
    }
}
