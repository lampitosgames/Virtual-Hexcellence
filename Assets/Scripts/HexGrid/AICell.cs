using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICell {
    public int q, r, h;
    public AICell parent = null;
    public int g = int.MaxValue;

    public AICell(int q, int r, int h) {
        this.q = q;
        this.r = r;
        this.h = h;
    }

    public bool Equals(AICell o) {
        if (q == o.q && r == o.r && h == o.h) {
            return true;
        } else {
            return false;
        }
    }
}
