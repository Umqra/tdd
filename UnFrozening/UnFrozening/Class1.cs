using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace UnFrozening
{
    class Program
    {
        private class Team
        {
            public string Name;
            public int Id;
            public override string ToString()
            {
                return string.Format("{0} {1}", 1000 + Id, Name);
            }
        }

        private class Submit
        {
            public Team Team;
            public string Problem;
            public int Minute;
            public bool Success;
            public override string ToString()
            {
                string status = Success ? "0 Accepted" : "1 Wrong answer";
                return string.Format("{0}     {1}     {2}     {3}", 1000 + Team.Id, Problem, Minute, status);
            }
        }

        public static void Main()
        {
            var document = new XmlDocument();
            var teams = new List<Team>();
            var submits = new List<Submit>();
            document.Load(new StreamReader("standings.xml", Encoding.GetEncoding(1251)));
            int count = 0;
            foreach (XmlNode sessionNode in document.SelectNodes("standings/contest/session"))
            {
                var team = new Team { Name = sessionNode.Attributes["party"].Value, Id = ++count };
                teams.Add(team);
                foreach (XmlNode problemNode in sessionNode.ChildNodes)
                {
                    var problemName = problemNode.Attributes["alias"].Value;
                    foreach (XmlNode runNode in problemNode.ChildNodes)
                    {
                        bool success = runNode.Attributes["accepted"].Value == "yes";
                        int minute = int.Parse(runNode.Attributes["time"].Value) / 60000;
                        submits.Add(new Submit { Team = team, Problem = problemName, Minute = minute, Success = success });
                    }
                }
            }

            var logBuilder = new StringBuilder();
            foreach (var s in submits.OrderBy(s => s.Minute))
                logBuilder.AppendLine(s.ToString());
            File.WriteAllText("full.log", logBuilder.ToString());

            var teamsBuilder = new StringBuilder();
            foreach (var t in teams)
                teamsBuilder.AppendLine(t.ToString());
            File.WriteAllText("teams.txt", teamsBuilder.ToString());
        }
    }
}
