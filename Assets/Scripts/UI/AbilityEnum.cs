using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//What ability is currently active.
//Tempted to try to refactor this into giving Player a current ability they can use and executing that
//However, for now I just want to get things working.
public enum AbilityEnum {
    NOT_USING_ABILITIES_ATM,
    INVALID_ACTION,
    MOVE_PLAYER,
    FIREBALL,
    ICE_WALL,
    GRASPING_ROOTS};