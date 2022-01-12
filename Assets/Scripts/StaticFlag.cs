using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace COVID_RUSH
{
    public static class StaticFlag
    {
        public static GameManager gm = null;
        public static GameStatusManager gsm = null;
        // public static AudioManager am = new AudioManager();

        static StaticFlag()
        {
            if (gm || gsm) return;
            gm = new GameManager();
            gsm = new GameStatusManager();
        }
    }
}
