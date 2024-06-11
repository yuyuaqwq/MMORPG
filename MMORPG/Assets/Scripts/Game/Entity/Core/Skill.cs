using System;
using Serilog;
using UnityEngine;

namespace MMORPG.Game
{
    public enum SkillTargetTypes
    {
        Unit,
        Position,
        None
    }

    public enum SkillModes
    {
        Combo,
        Skill
    }

    public class Skill
    {
        public CharacterSkillManager SkillManager { get; }
        public SkillDefine Define { get; }

        public SkillTargetTypes TargetType { get; }
        public SkillModes Mode { get; }

        private PlayerHandleWeapon _handleWeapon;

        public Skill(CharacterSkillManager skillManager, SkillDefine define)
        {
            SkillManager = skillManager;
            Define = define;

            _handleWeapon = skillManager.Entity.GetComponentInChildren<PlayerHandleWeapon>();

            TargetType = define.TargetType switch
            {
                "Unit" => SkillTargetTypes.Unit,
                "Position" => SkillTargetTypes.Position,
                _ => SkillTargetTypes.None
            };

            Mode = define.Mode switch
            {
                "Combo" => SkillModes.Combo,
                "Skill" => SkillModes.Skill,
                _ => throw new Exception("未知技能模式")
            };
        }

        public void Update()
        {

        }

        public void Use(CastTarget target)
        {
            Log.Debug($"{SkillManager.Entity.EntityId}使用技能{Define.ID}");
            switch (Mode)
            {
                case SkillModes.Combo:
                    if (_handleWeapon == null)
                        throw new Exception($"Combo模式的技能({Define.ID})必须由持有PlayerHandleWeapon的对象释放!");
                    _handleWeapon.CurrentComboWeapon.ChangeCombo(Define.ID);
                    Debug.Assert(_handleWeapon.CurrentWeapon.WeaponId == Define.ID);
                    _handleWeapon.CurrentWeapon.TurnWeaponOn();
                    break;
                case SkillModes.Skill:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
