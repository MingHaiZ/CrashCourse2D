using UnityEngine;

public class PlayerAnimeEvents : MonoBehaviour
{
    private Script script;

    // Start is called before the first frame update
    void Start()
    {
        script = GetComponentInParent<Script>();
    }


    private void AnimationTrigger()
    {
        script.AttackOver();
    }
    
}