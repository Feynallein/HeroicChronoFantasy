using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Jobs.Job;

public class Jobs : MonoBehaviour {
    public enum Job {
        wanderer,
        knight,

        barbarian,
        sorcerer,
        rogue,

        paladin,
        samurai,

        battlemage,
        bard,

        archer,
        assassin,

        clerk,
        monk,
        ninja,

        error,
    }

    public static Job ConvertRangeSkillStringToJob(string str) {
        return str switch {
            "lowlowlow" => wanderer,
            "medmedmed" => knight,

            "highlowlow" => barbarian,
            "lowhighlow" => sorcerer,
            "lowlowhigh" => rogue,

            "highmedlow" => paladin,
            "highlowmed" => samurai,

            "medhighlow" => battlemage,
            "lowhighmed" => bard,

            "medlowhigh" => archer,
            "lowmedhigh" => assassin,

            "medmedlow" => clerk,
            "medlowmed" => monk,
            "lowmedmed" => ninja,

            _ => error,
        };
    }

    public static string JobToString(Job job) {
        return job switch {
            wanderer => "Wanderer",
            knight => "Knight",

            barbarian => "Barbarian",
            sorcerer => "Sorcerer",
            rogue => "Rogue",

            paladin => "Paladin",
            samurai => "Samurai",

            battlemage => "Battlemage",
            bard => "Bard",

            archer => "Archer",
            assassin => "Assassin",

            clerk => "Clerk",
            monk => "Monk",
            ninja => "Ninja",

            _ => "Error"
        };
    }
}
