using System.Collections.Generic;

public interface IOnAttackEffect
{
    void OnAttack(SkillContext context, Monster target, AttackResult result);
}

public class PlayerSkills
{
    private Dictionary<PlayerSkill, int> _currentSkills;

    public PlayerSkills()
    {
        _currentSkills = new();
    }

    public void AddPlayerSkill(PlayerSkill skill)
    {
        if (!_currentSkills.ContainsKey(skill))
            _currentSkills.Add(skill, 1);
        else
            _currentSkills[skill] += 1;
    }

    public Dictionary<PlayerSkill, int> ReadPlayerSkill()
    {
        return _currentSkills;
    }

    public void SubPlayerSkill(PlayerSkill skill, int amount)
    {
        if (!_currentSkills.ContainsKey(skill)) return;

        _currentSkills[skill] -= amount;

        if (_currentSkills[skill] <= 0)
            _currentSkills.Remove(skill);
    }

    public void RemovePlayerSkill(PlayerSkill skill)
    {
        if (!_currentSkills.ContainsKey(skill)) return;

        _currentSkills.Remove(skill);
    }

    public void RemoveAllPlayerSkills()
    {
        _currentSkills.Clear();
    }

    public void InvokeOnAttack(SkillContext context, Monster target, AttackResult result)
    {
        foreach (var pair in _currentSkills)
        {
            PlayerSkill skill = pair.Key;
            int stack = pair.Value;

            foreach (var effect in skill.Effects)
            {
                if (effect is IOnAttackEffect onAttack)
                {
                    onAttack.OnAttack(context, target, result);
                }
            }
        }
    }

    public bool HasSkill(string skillId)
    {
        foreach (var pair in _currentSkills)
        {
            if (pair.Key.SkillID == skillId)
                return true;
        }
        return false;
    }

    public bool HasAnySkill(string[] skillIds)
    {
        foreach (var pair in _currentSkills)
        {
            foreach (var id in skillIds)
            {
                if (pair.Key.SkillID == id)
                    return true;
            }
        }
        return false;
    }

    public bool HasAllSkills(string[] skillIds)
    {
        foreach (var id in skillIds)
        {
            if (!HasSkill(id))
                return false;
        }
        return true;
    }
}