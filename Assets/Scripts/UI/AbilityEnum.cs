using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//What ability is currently active.
//Tempted to try to refactor this into giving Player a current ability they can use and executing that
//However, for now I just want to get things working.
public enum AbilityEnum {
    NOT_USING_ABILITIES = 0,
    MOVE_PLAYER = 1,
    FIREBALL = 2,
    ICE_WALL = 3,
    GRASPING_ROOTS = 4};