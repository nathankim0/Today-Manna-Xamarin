using System;
namespace TodaysManna.ExtensionMethods
{
    public static class MyExtensions
    {
        public static int WordCount(this String str)
        {
            return str.Split(new char[] { ' ', '.', '?' },
                             StringSplitOptions.RemoveEmptyEntries).Length;
        }

        public static string BibleBookKorToEng(this String str)
        {
            return str switch
            {
                "창" => "Gen",
                "출" => "exo",
                "레" => "Lev",
                "민" => "Num",
                "신" => "DEU",
                "수" => "JOS",
                "삿" => "jdg",
                "룻" => "RUT",
                "삼상" => "1SA",
                "삼하" => "2SA",
                "왕상" => "1KI",
                "왕하" => "2KI",
                "대상" => "1CH",
                "대하" => "2CH",
                "스" => "ezr",
                "느" => "Neh",
                "에" => "Est",
                "욥" => "Job",
                "시" => "PSA",
                "잠" => "PRO",
                "전" => "Ecc",
                "아" => "SNG",
                "사" => "Isa",
                "렘" => "Jer",
                "애" => "Lam",
                "겔" => "Ezk",
                "단" => "Dan",
                "호" => "Hos",
                "욜" => "JOL",
                "암" => "AMO",
                "옵" => "Oba",
                "욘" => "jon",
                "미" => "Mic",
                "나" => "Nam",
                "합" => "Hab",
                "습" => "Zep",
                "학" => "Hag",
                "슥" => "Zec",
                "말" => "Mal",
                "마" => "Mat",
                "막" => "MRK",
                "눅" => "LUK",
                "요" => "JHN",
                "행" => "ACT",
                "롬" => "Rom",
                "고전" => "1CO",
                "고후" => "2CO",
                "갈" => "Gal",
                "엡" => "Eph",
                "빌" => "PHP",
                "골" => "Col",
                "살전" => "1Th",
                "살후" => "2Th",
                "딤전" => "1TI",
                "딤후" => "2TI",
                "딛" => "Tit",
                "몬" => "PHM",
                "히" => "Heb",
                "약" => "Jas",
                "벧전" => "1PE",
                "벧후" => "2PE",
                "요일" => "1Jn",
                "요이" => "2Jn",
                "요삼" => "3Jn",
                "유" => "JUD",
                "계" => "Rev",
                _ => "Gen",
            };
        }
    }
}