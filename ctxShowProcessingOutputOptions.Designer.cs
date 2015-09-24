namespace MLocati.MediaData
{
    partial class ctxShowProcessingOutputOptions
    {
        /// <summary> 
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione componenti

        /// <summary> 
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare 
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ctxShowProcessingOutputOptions));
            this.chkAutoclose = new System.Windows.Forms.CheckBox();
            this.cbxBigger = new System.Windows.Forms.ComboBox();
            this.nudBigger = new System.Windows.Forms.NumericUpDown();
            this.rbAlways = new System.Windows.Forms.RadioButton();
            this.rbBigger = new System.Windows.Forms.RadioButton();
            this.rbNever = new System.Windows.Forms.RadioButton();
            this.gbxContainer = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudBigger)).BeginInit();
            this.gbxContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkAutoclose
            // 
            resources.ApplyResources(this.chkAutoclose, "chkAutoclose");
            this.chkAutoclose.Name = "chkAutoclose";
            this.chkAutoclose.UseVisualStyleBackColor = true;
            // 
            // cbxBigger
            // 
            resources.ApplyResources(this.cbxBigger, "cbxBigger");
            this.cbxBigger.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxBigger.FormattingEnabled = true;
            this.cbxBigger.Items.AddRange(new object[] {
            resources.GetString("cbxBigger.Items"),
            resources.GetString("cbxBigger.Items1"),
            resources.GetString("cbxBigger.Items2"),
            resources.GetString("cbxBigger.Items3")});
            this.cbxBigger.Name = "cbxBigger";
            this.cbxBigger.SelectedIndexChanged += new System.EventHandler(this.cbxBigger_SelectedIndexChanged);
            // 
            // nudBigger
            // 
            resources.ApplyResources(this.nudBigger, "nudBigger");
            this.nudBigger.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudBigger.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudBigger.Name = "nudBigger";
            this.nudBigger.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudBigger.ValueChanged += new System.EventHandler(this.nudBigger_ValueChanged);
            // 
            // rbAlways
            // 
            resources.ApplyResources(this.rbAlways, "rbAlways");
            this.rbAlways.Name = "rbAlways";
            this.rbAlways.TabStop = true;
            this.rbAlways.UseVisualStyleBackColor = true;
            // 
            // rbBigger
            // 
            resources.ApplyResources(this.rbBigger, "rbBigger");
            this.rbBigger.Name = "rbBigger";
            this.rbBigger.TabStop = true;
            this.rbBigger.UseVisualStyleBackColor = true;
            // 
            // rbNever
            // 
            resources.ApplyResources(this.rbNever, "rbNever");
            this.rbNever.Name = "rbNever";
            this.rbNever.TabStop = true;
            this.rbNever.UseVisualStyleBackColor = true;
            // 
            // gbxContainer
            // 
            resources.ApplyResources(this.gbxContainer, "gbxContainer");
            this.gbxContainer.Controls.Add(this.rbNever);
            this.gbxContainer.Controls.Add(this.chkAutoclose);
            this.gbxContainer.Controls.Add(this.rbBigger);
            this.gbxContainer.Controls.Add(this.cbxBigger);
            this.gbxContainer.Controls.Add(this.rbAlways);
            this.gbxContainer.Controls.Add(this.nudBigger);
            this.gbxContainer.Name = "gbxContainer";
            this.gbxContainer.TabStop = false;
            // 
            // ctxShowProcessingOutputOptions
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbxContainer);
            this.MaximumSize = new System.Drawing.Size(223, 88);
            this.MinimumSize = new System.Drawing.Size(223, 88);
            this.Name = "ctxShowProcessingOutputOptions";
            ((System.ComponentModel.ISupportInitialize)(this.nudBigger)).EndInit();
            this.gbxContainer.ResumeLayout(false);
            this.gbxContainer.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkAutoclose;
        private System.Windows.Forms.ComboBox cbxBigger;
        private System.Windows.Forms.NumericUpDown nudBigger;
        private System.Windows.Forms.RadioButton rbAlways;
        private System.Windows.Forms.RadioButton rbBigger;
        private System.Windows.Forms.RadioButton rbNever;
        private System.Windows.Forms.GroupBox gbxContainer;
    }
}
