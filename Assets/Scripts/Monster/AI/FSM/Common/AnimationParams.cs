using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI.FSM
{
    /// <summary>
    /// 动画参数类
    /// </summary>
    [Serializable]
    public  class AnimationParams
    {
        public string Idle = "idle";
        public string Dead = "dead";
        public string Run = "run";
        public string Walk = "walk";
        public string Attack = "attack";
    }
}
