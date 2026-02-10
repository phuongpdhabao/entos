namespace ENTOS.Module.Controllers
{
    partial class VoiceViewController
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
			// CalculatorSpeelingOfMinutes
            this.CalculatorSpeelingOfMinutes = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // CalculatorSpeelingOfMinutes
            // 
            this.CalculatorSpeelingOfMinutes.Caption = "Tính Âm phút";
            this.CalculatorSpeelingOfMinutes.Category = "Edit";
            this.CalculatorSpeelingOfMinutes.ConfirmationMessage = null;
            this.CalculatorSpeelingOfMinutes.Id = "CalculatorSpeelingOfMinutes";
			
			this.CalculatorSpeelingOfMinutes.ToolTip = "Tính số lượng âm trong 1 phút";  
            this.CalculatorSpeelingOfMinutes.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.CalculatorSpeelingOfMinutes.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.CalculatorSpeelingOfMinutes.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;   
            this.CalculatorSpeelingOfMinutes.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CalculatorSpeelingOfMinutes_Execute);
            // 
            // VoiceViewController
            // 
            this.Actions.Add(this.CalculatorSpeelingOfMinutes);
			// DemoVoice
            this.DemoVoice = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // DemoVoice
            // 
            this.DemoVoice.Caption = "Nghe thử";
            this.DemoVoice.Category = "Edit";
            this.DemoVoice.ConfirmationMessage = null;
            this.DemoVoice.Id = "DemoVoice";
            this.DemoVoice.TargetViewNesting = DevExpress.ExpressApp.Nesting.Any;
            this.DemoVoice.TargetViewType = DevExpress.ExpressApp.ViewType.Any;            
			this.DemoVoice.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;   
            this.DemoVoice.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DemoVoice_Execute);
            // 
            // VoiceViewController
            // 
            this.Actions.Add(this.DemoVoice);
        }

        #endregion
		private DevExpress.ExpressApp.Actions.SimpleAction CalculatorSpeelingOfMinutes;
		private DevExpress.ExpressApp.Actions.SimpleAction DemoVoice;
    }
}