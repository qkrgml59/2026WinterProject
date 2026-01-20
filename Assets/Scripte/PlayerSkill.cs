using UnityEngine;


public class PlayerSkill : MonoBehaviour
{
    public enum SKILLNAME : int
    {
        WATER,
        FIRE,
        HEALL

    }

    SkillBase skill;
    public SKILLNAME skillname = SKILLNAME.WATER;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        skill = new FireballSkill();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            skill.Use();
        }

        if(Input.GetKeyDown(KeyCode.Tab))
        {
            skillname += 1;
            if ((int)skillname == 3)
            {
                skillname = 0;
            }

            if(skillname == SKILLNAME.HEALL)
            {
                skill = new HealSkill();
            }
            else if (skillname == SKILLNAME.FIRE)
            {
                skill = new FireballSkill();
            }
            else if (skillname == SKILLNAME.WATER)
            {
                skill = new WaterSkill();
            }
        }
    }
}
