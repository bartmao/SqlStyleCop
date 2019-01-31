using SqlStyleCop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TST.SqlStyleCop
{
    /// <summary>
    /// Manages style cop runtime.
    /// </summary>
    public class StyleCopRunManager
    {
        static List<Type> StyleCopTypes;

        public List<string> AvailableCopNames { get; set; }

        public List<StyleCopContext> ExecutionContexts { get; set; }

        public int LogLevel { get; set; }

        public bool IsAsyncMode = true;

        public event EventHandler<StyleCopContext> ContextExecuted;

        static StyleCopRunManager()
        {
            StyleCopTypes = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t => typeof(ASTTraverseHandler).IsAssignableFrom(t) && !t.IsAbstract)
                    .ToList();
        }

        /// <summary>
        /// Run all selected style cops.
        /// </summary>
        public void Run(List<StyleCopContext> contexts, params string[] copNames)
        {
            AvailableCopNames = copNames.ToList();
            if (copNames == null || copNames.Length == 0)
                AvailableCopNames = StyleCopTypes.Select(t => t.Name).ToList();
            ExecutionContexts = contexts;

            ExecuteByThreadMode();
        }

        public void Run(string folderName, string level, string[] copNames)
        {
            if (!Directory.Exists(folderName)) return;
            if (copNames == null || copNames.Length == 0)
                AvailableCopNames = StyleCopTypes.Select(t => t.Name).ToList();

            var fileNames = Directory.GetFiles(folderName)
                .Where(f => f.EndsWith(".sql", StringComparison.InvariantCultureIgnoreCase)).ToList();
            ExecutionContexts = fileNames.Select(f => new StyleCopContext(f)).ToList();

            var logLevel = 7;
            int.TryParse(level, out logLevel);
            LogLevel = logLevel;
            IsAsyncMode = false;

            ExecuteByThreadMode();
        }

        private void ExecuteByThreadMode()
        {
            if (IsAsyncMode)
                ThreadPool.QueueUserWorkItem(o =>
                {
                    Execute();
                });
            else Execute();
        }

        private void Execute()
        {
            ExecutionContexts.ForEach(c =>
            {
                c.Init();
                var toBeExecutedCops = StyleCopTypes.Where(t => AvailableCopNames.Contains(t.Name))
                    .Select(t => (ASTTraverseHandler)Activator.CreateInstance(t))
                    .ToList();
                var handlerManager = new ASTTraverseManager(toBeExecutedCops);
                handlerManager.RunHandlers(c);
                OutputLog(c);
                c.HasRun = true;
                OnContextExecuted(c);
            });
        }

        /// <summary>
        /// Occurs when one context is executed.
        /// </summary>
        protected void OnContextExecuted(StyleCopContext context)
        {
            if (ContextExecuted != null)
            {
                ContextExecuted(this, context);
            }
        }

        private void OutputLog(StyleCopContext context)
        {
            Console.WriteLine(new string('*', 100));
            Console.WriteLine(string.Format("Check Script [{0}]" ,context.SqlFileName));
            Console.WriteLine();
            var logs = context.LogList;
            foreach (var log in logs)
            {
                if ((log.Level & LogLevel) != log.Level) continue;
                Console.WriteLine(string.Format("Position:{0} {1}", log.PositionStr, log.Content));
            }
        }

        public List<ASTTraverseHandler> GetStyleCopInformation()
        {
            return StyleCopTypes.Select(t => (ASTTraverseHandler)Activator.CreateInstance(t)).ToList();
        }
    }
}
