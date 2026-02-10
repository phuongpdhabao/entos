using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.XtraGrid;
using Microsoft.Extensions.DependencyInjection;
using ENTOS.Module.BusinessObjects;
using ENTOS.Module.SystemObjects;
using ENTOS.Module.Services;
using ListView = DevExpress.ExpressApp.ListView;


namespace ENTOS.Module.Controllers 
{
    public partial class WorkflowViewController: BaseViewController<Module.BusinessObjects.Workflow>
    {      
        
        public WorkflowViewController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Module.BusinessObjects.Workflow);    
            //TargetViewNesting = Nesting.Nested;
        }

        #region DependencyInjection
  
  
  
  
        #endregion DependencyInjection
		
		protected override void OnActivated()
        {
            base.OnActivated();
        }
        
        private Module.Services.WorkflowService workflowService;
        protected override void OnViewControlsCreated()
        {
             base.OnViewControlsCreated();
      
             workflowService = new Module.Services.WorkflowService(this);
             
        }
        
        protected override void OnDeactivated()
        {                 
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }


        
        //Code: 3941            Oid: 9901ec65-f8fd-4ef9-9165-cf9d454b6882
		private void WorkflowMermaid_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(WorkflowMermaid), "Tạo mã lưu đồ");              
      
            #region WorkflowMermaidImportCode
            if (View.CurrentObject is not Workflow workflow)
                return;
            string desc = workflow.Description;
            string mermaidCode = workflow.MermaidCode;

            if (!string.IsNullOrWhiteSpace(mermaidCode))
            {
                var lines = mermaidCode.Split(
                    new[] { "\r\n", "\r", "\n" },
                    StringSplitOptions.None
                );

                if (lines.Length > 2 && lines[0].Trim() == "```mermaid")
                {
                    // Bỏ dòng đầu và dòng cuối
                    mermaidCode = string.Join("\n", lines.Skip(1).Take(lines.Length - 2));
                    workflow.MermaidCode = string.Join("\n", lines.Skip(1).Take(lines.Length - 2));
                }
            }

            string name = workflow.Name;
            string type = "";
            string codeID = "";

            // Đường dẫn cho input/output
            string inputPath = Path.Combine(Path.GetTempPath(), $"{name}.mmd");
            string outputFormat = "png";
            string rootPath = Module.SystemObjects.Tools.GetParameterValueOrDefault(ObjectSpace, "SolutionFolder", "").Value;
            string finalOutputPath = Path.Combine(rootPath, type, codeID, $"{name}.{outputFormat}");
            string user = Module.SystemObjects.Tools.GetParameterValueOrDefault(ObjectSpace, "FileServerUser", "null").Value;
            string pass = Module.SystemObjects.Tools.GetParameterValueOrDefault(ObjectSpace, "FileServerPassword", "null").Value;
            var credentials = new System.Net.NetworkCredential(user, pass);

            try
            {
                using (new Module.SystemObjects.NetworkConnection(rootPath, credentials))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(finalOutputPath)!);

                    // Tạo thư mục chứa input nếu chưa có
                    Directory.CreateDirectory(Path.GetDirectoryName(inputPath)!);

                    // Ghi nội dung vào file .mmd
                    File.WriteAllText(inputPath, mermaidCode);

                    var psi = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = @"V:\Company\HabaoAI\Nodejs\node.exe",
                        Arguments = $@"lib\npm\node_modules\@mermaid-js\mermaid-cli\src\cli.js -i ""{inputPath}"" -o ""{finalOutputPath}"" -t default --outputFormat {outputFormat} --scale 3",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        WorkingDirectory = @"V:\Company\HabaoAI\Nodejs"
                    };

                    using var process = new System.Diagnostics.Process { StartInfo = psi };

                    string stdOut = "";
                    string stdErr = "";

                    process.OutputDataReceived += (s, e) => { if (e.Data != null) stdOut += e.Data + Environment.NewLine; };
                    process.ErrorDataReceived += (s, e) => { if (e.Data != null) stdErr += e.Data + Environment.NewLine; };

                    process.Start();

                    // Bắt đầu đọc stream bất đồng bộ
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    process.WaitForExit();

                    if (process.ExitCode != 0)
                        throw new Exception($"Lỗi Mermaid CLI: {stdErr}");


                    // Xóa file tạm input
                    if (File.Exists(inputPath))
                        File.Delete(inputPath);

                    workflow.File = finalOutputPath;
                    Module.SystemObjects.Tools.ShowMessage(Application, "Hoàn thành", "Hoàn thành tạo sơ đồ Mermaid", InformationType.Success);
                    return;
                }
            }
            catch (Exception ex)
            {
                Module.SystemObjects.Tools.ShowMessage(Application, "Lỗi", "Lỗi khi tạo sơ đồ Mermaid", InformationType.Error);
                workflow.MermaidCode += "\r\nBáo lỗi khi chạy mermaid cli hãy sửa lại cho đúng\r\n" + ex;
                return;
            }

            #endregion WorkflowMermaidImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
        //Code: 3753            Oid: 00c84552-1465-44fc-a126-4796f1c1c291
		private void WorkflowShare_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is null)
                return;
    
            LogActionStart(nameof(WorkflowShare), "Chia sẻ lưu đồ");              
      
            #region WorkflowShareImportCode
            var selectedObjects = View.SelectedObjects;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (Workflow obj in selectedObjects)
            {
                var name = string.IsNullOrEmpty(obj.Name) ? "Không có tên" : obj.Name;
                var link = string.IsNullOrEmpty(obj.File) ? "Không có link" : obj.File;

                sb.AppendLine($"{name}\n{link}\n");
            }

            // copy vào clipboard
            if (sb.Length > 0)
            {
                Module.SystemObjects.Tools.ClipboardSetText(sb.ToString() + "\n");
            }
            #endregion WorkflowShareImportCode
            //Module.Helpers.LogHelper.Info(logMessage + " - End");
		}
     }
}