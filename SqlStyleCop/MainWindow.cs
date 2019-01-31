using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TST;
using TST.SqlStyleCop;

namespace SqlStyleCop
{
    public partial class MainWindow : Form
    {
        private List<StyleCopContext> Contexts { get; set; }

        private List<Type> HandlerTypes { get; set; }

        private int CurContextIndex { get; set; }

        private StyleCopRunManager StyleCopRunManager { get; set; }

        private List<string> FileNames { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Contexts = new List<StyleCopContext>();
            LogLevel = 7;
            StyleCopRunManager = new StyleCopRunManager();
            FileNames = new List<string>();
        }

        #region Event Handlers
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadAllStyleCops();
            DisplayDescription();
        }

        /// <summary>
        /// Runs all selected style cops.
        /// </summary>
        private void btnRun_Click(object sender, EventArgs e)
        {
            InitRunContext();

            var selectedHandlerNames = dgvStyleCops.Rows
                        .Cast<DataGridViewRow>()
                        .Where(r => (bool)r.Cells[1].Value == true)
                        .Select(r1 => r1.Cells[0].Value.ToString())
                        .ToArray();

            StyleCopRunManager.ContextExecuted += (man, args) =>
            {
                this.Invoke((MethodInvoker)(() =>
                {
                    // Report Progress
                    pgbHandling.Value = (int)((float)(Contexts.Where(c => c.HasRun).Count()) / Contexts.Count * 100);
                    // Display the first log
                    if (Contexts.IndexOf(args) == 0)
                        DisplayRunLog();
                }));
            };
            StyleCopRunManager.Run(Contexts, selectedHandlerNames);
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            var dig = new OpenFileDialog();
            dig.Multiselect = true;
            dig.Filter = "sql files (*.sql)|*.sql";
            dig.RestoreDirectory = false;
            var rst = dig.ShowDialog();
            if (rst != System.Windows.Forms.DialogResult.OK) return;

            // Init Cop Context
            tbSql.TabPages.Clear();
            Contexts.Clear();
            FileNames = dig.FileNames.ToList();
            foreach (var fpath in FileNames)
            {
                var fileName = HelperMethod.GetFileNameByPath(fpath);
                tbSql.TabPages.Add(fileName);
                Contexts.Add(new StyleCopContext(fpath));
            }

            InitRunContext();
            tbSql_SelectedIndexChanged(sender, EventArgs.Empty);
        }

        private void txtLogger_Click(object sender, EventArgs e)
        {
            if (txtLogger.TextLength == 0) return;

            // Find selected log
            var curContext = Contexts[CurContextIndex];
            var start = txtLogger.SelectionStart;
            if (start == 0) start = 1;
            var line = txtLogger.Text.Substring(0, start).Count(c => c == '\n');
            var logs = curContext.LogList.Where(l => (l.Level & LogLevel) == l.Level).ToList();
            if (line >= logs.Count) return;

            // Get location of log and highlight it.
            var pos = logs[line].PositionStr;
            var arr = pos.Split(',').Select(s => s.Trim()).ToList();
            var pos1 = new Point(int.Parse(arr[0].Substring(arr[0].LastIndexOf('(') + 1))
                , int.Parse(arr[1].Substring(0, arr[1].Length - 1)));
            var pos2 = new Point(int.Parse(arr[2].Substring(1, arr[2].Length - 1))
                , int.Parse(arr[3].Substring(0, arr[3].Length - 2)));

            HighlightPositions(txtSql, pos1, pos2);
        }

        private void txtSql_TextChanged(object sender, EventArgs e)
        {
            if (Contexts.Count == 0) return;
            var context = Contexts[CurContextIndex];
            context.SqlScript = txtSql.Text;
        }

        private void tbSql_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tbSql.SelectedIndex == -1) return;

            CurContextIndex = tbSql.SelectedIndex;
            var curContext = Contexts[CurContextIndex];
            txtSql.Text = curContext.SqlScript;
            txtSql.ScrollToCaret();
            DisplayRunLog();
        }

        private void dgvStyleCops_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            DisplayDescription();
        }

        private void chkAll_Click(object sender, EventArgs e)
        {
            var chk = sender as CheckBox;
            var flag = int.Parse(chk.Tag.ToString());
            if (chk.Checked)
                LogLevel |= flag;
            else
                LogLevel ^= flag;

            DisplayRunLog();
        }


        private void btnExportLog_Click(object sender, EventArgs e)
        {
            var sdig = new SaveFileDialog();
            sdig.Filter = "Text Files | *.txt";
            sdig.DefaultExt = "txt";
            sdig.FileName = "SqlStyleCopLog.txt";
            sdig.RestoreDirectory = false;
            var rst = sdig.ShowDialog();

            if (rst != System.Windows.Forms.DialogResult.OK) return;
            
            var fpath = sdig.FileName;
            File.WriteAllText(fpath, GetLogContent());
        }

        #endregion

        #region Methods

        #endregion
        private void LoadAllStyleCops()
        {
            var cops = StyleCopRunManager.GetStyleCopInformation();
            cops.ForEach(h =>
            {
                dgvStyleCops.Rows.Add(new object[] { h.TypeName, true, h.Description });
            });
            dgvStyleCops.RowHeadersVisible = false;
        }

        /// <summary>
        /// Displays cop description
        /// </summary>
        private void DisplayDescription()
        {
            txtDescription.Text = dgvStyleCops.SelectedRows[0].Cells[2].Value.ToString();
        }

        private void InitRunContext()
        {
            txtLogger.Clear();
            pgbHandling.Value = 0;

            if (FileNames.Count == 0)
            {
                Contexts.Clear();
                Contexts.Add(StyleCopContext.Load(txtSql.Text));
            }
        }

        private void HighlightPositions(RichTextBox target, Point pos1, Point pos2, Color color = default(Color))
        {
            target.SelectAll();
            target.SelectionBackColor = Color.Transparent;

            var s = GetTextboxIndexByPoint(target, pos1);
            var e = GetTextboxIndexByPoint(target, pos2);
            HighlightPositions(target, s, e, color);
        }

        private void HighlightPositions(RichTextBox target, int s, int e, Color color = default(Color))
        {
            if (color == default(Color)) color = Color.Yellow;
            target.Select(s, e - s);
            target.SelectionBackColor = color;
            target.ScrollToCaret();
        }

        private int GetTextboxIndexByPoint(TextBoxBase txtBox, Point pos)
        {
            var index = 0;
            var x = pos.X;
            while (x-- > 1)
            {
                index = txtBox.Text.IndexOf("\n", index) + 1;
            }

            return index + pos.Y - 1;
        }

        private void DisplayRunLog()
        {
            txtLogger.Clear();

            if (Contexts.Count == 0) return;
            var curContext = Contexts[CurContextIndex];
            if (curContext.Logger != null)
            {
                foreach (var logInfo in curContext.LogList.Where(l => (l.Level & LogLevel) == l.Level))
                {
                    txtLogger.AppendText(string.Format("Position:{0} ", logInfo.PositionStr));
                    var s = txtLogger.TextLength;
                    txtLogger.AppendText(logInfo.Content);
                    txtLogger.AppendText("\r\n");
                    txtLogger.Select(s, logInfo.Content.Length);
                    var level = logInfo.Level;
                    txtLogger.SelectionColor = level == 1 ? Color.Black : (level == 2 ? Color.Green : Color.Red);
                }
            }
        }

        private int logLevel;
        public int LogLevel
        {
            get { return logLevel; }
            set
            {
                logLevel = value;
                logPanel.Controls.Cast<Control>()
                    .Where(c => c is CheckBox)
                    .ToList()
                    .ForEach(c =>
                    {
                        var chk = c as CheckBox;
                        var flag = int.Parse(chk.Tag.ToString());
                        if ((logLevel & flag) == flag) chk.Checked = true;
                        else chk.Checked = false;
                    });
            }
        }

        private string GetLogContent()
        {
            var sb = new StringBuilder();
            Contexts.ForEach(c =>
            {
                sb.AppendLine(new string('*', 100));
                sb.AppendLine(string.Format("Check Script [{0}]", c.SqlFileName ?? "Default Window"));
                sb.AppendLine();
                var logs = c.LogList;
                foreach (var log in logs)
                {
                    if ((log.Level & LogLevel) != log.Level) continue;
                    sb.AppendLine(string.Format("Position:{0} {1}", log.PositionStr, log.Content));
                }
            });

            return sb.ToString();
        }
    }

}
