using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Skill;
using static Jobs.Job;

public class Jobs : MonoBehaviour {
    public enum Job {
        jobless,
        businessman,

        actor,
        engineer,
        osteopath,
        librarian,

        worker,
        philosopher,
        scientist,
        salesman,

        adventurer,
        blacksmith,
        firefighter,

        musician,
        linguist,
        writter,

        IT_guy,
        teacher,
        doctor,

        influencer,
        streamer,
        psychologist,

        archeologist,
        lumberjack,
        sports_coach,
        phd,
        film_writer, //todo: change
        politician,

        error,
    }

    public static Job ConvertRangeSkillStringToJob(string str) {
        return str switch {
            "lowlowlowlow" => jobless,
            "medmedmedmed" => businessman,

            "medmedmedlow" => actor,
            "medmedlowmed" => engineer,
            "medlowmedmed" => jobless,
            "lowmedmedmed" => jobless,

            "highlowlowlow" => worker,
            "lowhighlowlow" => philosopher,
            "lowlowhighlow" => osteopath,
            "lowlowlowhigh" => librarian,

            "highmedlowlow" => adventurer,
            "highlowmedlow" => blacksmith,
            "highlowlowmed" => writter,

            "medhighlowlow" => musician,
            "lowhighmedlow" => linguist,
            "lowhighlowmed" => writter,

            "medlowhighlow" => IT_guy,
            "lowmedhighlow" => teacher,
            "lowlowhighmed" => doctor,

            "medlowlowhigh" => influencer,
            "lowmedlowhigh" => streamer,
            "lowlowmedhigh" => psychologist,

            "medmedlowlow" => archeologist,
            "medlowmedlow" => lumberjack,
            "medlowlowmed" => sports_coach,

            "lowmedmedlow" => phd,
            "lowmedlowmed" => film_writer,

            "lowlowmedmed" => politician,

            _ => error,
        };
    }

    public static string JobToString(Job job) {
        return job switch {
            jobless => "Jobless",
            businessman => "Businessman",

            actor => "Actor",
            engineer => "Engineer",
            osteopath => "Osteopath",
            librarian => "Librarian",

            worker => "Worker",
            philosopher => "Philosopher",
            scientist => "Scientist",
            salesman => "Salesman",

            adventurer => "Adventurer",
            blacksmith => "Blacksmith",
            firefighter => "Firefighter",

            musician => "Musician",
            linguist => "Linguist",
            writter => "Writter",

            IT_guy => "IT Guy",
            teacher => "Teacher",
            doctor => "Doctor",

            influencer => "Influencer",
            streamer => "Streamer",
            psychologist => "Psychologist",

            archeologist => "Archeologist",
            lumberjack => "Lumberjack",
            sports_coach => "Sports Coach",
            phd => "PhD",
            film_writer => "Film Writter",
            politician => "Politician",

            _ => "Error"
        };
    }
}
