﻿using GameServer.EntitySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMORPG.Common.Proto.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection.Metadata;

namespace GameServer.BuffSystem
{
    public class StateBuff : Buff
    {
        private JArray _stateModifier;
        public StateBuff(int buffId, BuffManager buffManager, Actor? caster, float duration, string StateModifier)
            : base(buffId, buffManager, caster, duration)
        {
            _stateModifier = JArray.Parse(StateModifier);
        }

        public override void Start()
        {
            base.Start();
            Modify("Start");
        }

        public override void Exit()
        {
            base.Exit();
            Modify("Exit");
        }

        private void Modify(string stage)
        {
            foreach (JObject obj in _stateModifier)
            {
                if (obj[stage] == null) return;
                JObject stageValue = obj[stage] as JObject;
                if (stageValue == null) return;

                foreach (var property in stageValue.Properties())
                {
                    string name = property.Name;
                    bool enable = property.Value.Value<bool>();
                    FlagStates flag = FlagStates.Zero;
                    switch (name)
                    {
                        case "Stun":
                            flag = FlagStates.Stun;
                            break;
                        case "Root":
                            flag = FlagStates.Root;
                            break;
                        case "Silence":
                            flag = FlagStates.Silence;
                            break;
                        case "Invincible":
                            flag = FlagStates.Invincible;
                            break;
                        case "Invisible":
                            flag = FlagStates.Invisible;
                            break;
                    }
                    if (enable)
                    {
                        BuffManager.OwnerActor.AddFlagState(flag);
                    }
                    else
                    {
                        BuffManager.OwnerActor.RemoveFlagState(flag);
                    }
                }
            }
        }
    }
}