using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutCanvasController : MonoBehaviour {

    [SerializeField] Image item;
    [SerializeField] Image skill;

    PlayerController player;

    Color col = new Color();

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        col = Color.white;
        col.a = 255;
    }

    private void Start()
    {
        player.OnSkillChanged += ChangeSkill;
        player.OnItemChanged += ChangeItem;
    }

    void ChangeItem(Sprite itemSpr)
    {
        item.sprite = itemSpr;
        item.color = col;
    }

    void ChangeSkill(Sprite skillSpr)
    {
        skill.sprite = skillSpr;
        skill.color = col;
    }
}
