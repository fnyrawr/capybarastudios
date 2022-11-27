using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class WeaponAnimationController : MonoBehaviour
{
    public Animator animator;

    public RigBuilder rigBuilder;

    public TwoBoneIKConstraint leftHandIK;
    public Transform GunSlot;

    // Start is called before the first frame update
    void Start()
    {
        refresh();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void refresh()
    {
        var Weapon = getActiveWeapon();
        if (Weapon)
        {
            animator.SetInteger("weaponType", Weapon.GetComponent<Weapon>().weaponType);
            var temp = Weapon.transform.Find("ref_left_hand_target");
            if (temp)
            {
                leftHandIK.data.target = temp;
                rigBuilder.layers[0].active = true;
                rigBuilder.layers[1].active = false;
            }
            else
            {
                rigBuilder.layers[0].active = false;
                rigBuilder.layers[1].active = true;
            }

            rigBuilder.Build();
        }
        else
        {
            animator.SetInteger("weaponType", 0);
        }
    }

    GameObject getActiveWeapon()
    {
        GameObject firstActiveGameObject = null;
        for (int i = 0; i < GunSlot.childCount; i++)
        {
            if (GunSlot.GetChild(i).gameObject.activeSelf == true)
            {
                firstActiveGameObject = GunSlot.GetChild(i).gameObject;
            }
        }

        return firstActiveGameObject;
    }
}