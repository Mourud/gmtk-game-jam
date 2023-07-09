using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerPreviewAnimation
{
    public string name = "";
    public string animationName = "";
}
public class AnimationTest : MonoBehaviour
{
    public List<PlayerPreviewAnimation> animations = new List<PlayerPreviewAnimation>();
    public List<int> animationTimes = new List<int>();
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        PlayerPreviewAnimation currentAnimation = animations[1];
        animator.Play(currentAnimation.animationName);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
