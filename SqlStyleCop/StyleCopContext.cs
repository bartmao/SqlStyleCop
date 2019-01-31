using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TST;
using TST.SqlStyleCop;

namespace SqlStyleCop
{
    public class LogInfo
    {
        public string PositionStr { get; set; }

        public int Level { get; set; }

        public string Content { get; set; }
    }

    public class StyleCopContext
    {
        public XmlDocument Doc { get; set; }

        public string SqlFileName { get; set; }

        public string FilePath { get; set; }

        public string SqlScript { get; set; }

        public StringBuilder Logger { get; set; }

        public List<LogInfo> LogList { get; set; }

        public List<string> Positions { get; set; }

        public Dictionary<string, object> UserDefinedDict { get; set; }

        public bool HasRun { get; set; }

        public StyleCopContext(string fpath)
        {
            UserDefinedDict = new Dictionary<string, object>();
            FilePath = fpath;
            SqlFileName = HelperMethod.GetFileNameByPath(fpath);
            if (!string.IsNullOrEmpty(FilePath))
                SqlScript = File.ReadAllText(FilePath);
            else
                SqlScript = string.Empty;
        }

        public void Init()
        {
            Positions = new List<string>();
            Logger = new StringBuilder();
            LogList = new List<LogInfo>();
            UserDefinedDict.Clear();
            Doc = ASTHelperMethod.ParseAndGetAST(SqlScript);
            HasRun = false;
        }

        public static StyleCopContext Load(string script)
        {
            return new StyleCopContext(string.Empty)
            {
                SqlScript = script
            };
        }

        public void WriteLog(string pos, string content, int level = 1)
        {
            Positions.Add(pos);

            LogList.Add(new LogInfo()
            {
                PositionStr = pos,
                Content = content,
                Level = level
            });
        }

        public static string[] LevelColor = new string[] { "<B>", "<G>", "<R>", "</B>", "</G>", "</R>" };

        public XmlNode CurBatch { get; set; }

        public Stack<string> ParameterFrame { get; set; }

        public Stack<TableDefination> TableFrame { get; set; }
    }
}
