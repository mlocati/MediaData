namespace MLocati.MediaData
{
    partial class ctxDateTime
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ctxDateTime));
            this.lblTime = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.dtpTime = new System.Windows.Forms.DateTimePicker();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // lblTime
            // 
            resources.ApplyResources(this.lblTime, "lblTime");
            this.lblTime.Name = "lblTime";
            // 
            // lblDate
            // 
            resources.ApplyResources(this.lblDate, "lblDate");
            this.lblDate.Name = "lblDate";
            // 
            // dtpTime
            // 
            this.dtpTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            resources.ApplyResources(this.dtpTime, "dtpTime");
            this.dtpTime.Name = "dtpTime";
            this.dtpTime.ShowUpDown = true;
            // 
            // dtpDate
            // 
            resources.ApplyResources(this.dtpDate, "dtpDate");
            this.dtpDate.Name = "dtpDate";
            // 
            // ctxDateTime
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.dtpTime);
            this.Controls.Add(this.dtpDate);
            this.MaximumSize = new System.Drawing.Size(231, 49);
            this.MinimumSize = new System.Drawing.Size(231, 49);
            this.Name = "ctxDateTime";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.DateTimePicker dtpTime;
        private System.Windows.Forms.DateTimePicker dtpDate;
    }
}
