using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour {

    private GameObject player;
    private CanvasGroup canGroup;
    public float fadeDist = 0.4f;
    public Sprite icon;
    
    void Start()
    {
        player = GameObject.Find("Camera (eye)");
        canGroup = this.GetComponent<CanvasGroup>();
    }

    //Fade out UI when player is too far from an item, fade in as player gets closer so they know item is collectible
    void Update()
    {
        float dist = Vector3.Distance(player.transform.position, this.transform.position);
        if (dist <= fadeDist)
        {
            canGroup.alpha = (fadeDist / dist) * 0.03f;
        }
    }
    public Sprite ReturnSpriteImage()
    {
        return icon;
    }
}
