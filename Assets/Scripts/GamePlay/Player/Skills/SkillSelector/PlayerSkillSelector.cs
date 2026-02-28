using System;
using System.Collections.Generic;

public class PlayerSkillSelector
{
    private readonly List<PlayerSkill> _availableSkills;
    private readonly Random _random = new();

    public PlayerSkillSelector(PlayerSkill[] skills)
    {
        _availableSkills = new List<PlayerSkill>(skills);
    }

    public void InitSkills(PlayerSkill[] skills)
    {
        _availableSkills.Clear();
        _availableSkills.AddRange(skills);
    }

    public PlayerSkill[] Draw(int count)
    {
        List<PlayerSkill> pool = new(_availableSkills);
        List<PlayerSkill> result = new();

        for (int i = 0; i < count && pool.Count > 0; i++)
        {
            int index = _random.Next(pool.Count);
            result.Add(pool[index]);
            pool.RemoveAt(index);
        }

        return result.ToArray();
    }

    public void RemoveSkill(PlayerSkill skill)
    {
        _availableSkills.Remove(skill);
    }
}