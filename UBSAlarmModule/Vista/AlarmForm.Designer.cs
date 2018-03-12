namespace UBSAlarmModule
{
    partial class AlarmForm
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.splSeparador = new System.Windows.Forms.SplitContainer();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.lstAlarms = new System.Windows.Forms.ListBox();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splSeparador)).BeginInit();
            this.splSeparador.Panel1.SuspendLayout();
            this.splSeparador.Panel2.SuspendLayout();
            this.splSeparador.SuspendLayout();
            this.SuspendLayout();
            // 
            // splSeparador
            // 
            this.splSeparador.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splSeparador.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splSeparador.Location = new System.Drawing.Point(0, 0);
            this.splSeparador.Name = "splSeparador";
            this.splSeparador.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splSeparador.Panel1
            // 
            this.splSeparador.Panel1.Controls.Add(this.btnDown);
            this.splSeparador.Panel1.Controls.Add(this.btnUp);
            this.splSeparador.Panel1.Controls.Add(this.lstAlarms);
            // 
            // splSeparador.Panel2
            // 
            this.splSeparador.Panel2.Controls.Add(this.txtDescripcion);
            this.splSeparador.Size = new System.Drawing.Size(800, 450);
            this.splSeparador.SplitterDistance = 294;
            this.splSeparador.TabIndex = 1;
            // 
            // btnDown
            // 
            this.btnDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDown.Location = new System.Drawing.Point(737, 225);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(50, 50);
            this.btnDown.TabIndex = 2;
            this.btnDown.Text = "\\/";
            this.btnDown.UseVisualStyleBackColor = true;
            // 
            // btnUp
            // 
            this.btnUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUp.Location = new System.Drawing.Point(737, 11);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(50, 50);
            this.btnUp.TabIndex = 1;
            this.btnUp.Text = "/\\";
            this.btnUp.UseVisualStyleBackColor = true;
            // 
            // lstAlarms
            // 
            this.lstAlarms.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstAlarms.FormattingEnabled = true;
            this.lstAlarms.Location = new System.Drawing.Point(11, 11);
            this.lstAlarms.Name = "lstAlarms";
            this.lstAlarms.Size = new System.Drawing.Size(693, 264);
            this.lstAlarms.TabIndex = 0;
            // 
            // txtDescripcion
            // 
            this.txtDescripcion.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescripcion.Location = new System.Drawing.Point(11, 16);
            this.txtDescripcion.Multiline = true;
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(776, 123);
            this.txtDescripcion.TabIndex = 0;
            // 
            // AlarmForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splSeparador);
            this.Name = "AlarmForm";
            this.Text = "Form1";
            this.splSeparador.Panel1.ResumeLayout(false);
            this.splSeparador.Panel2.ResumeLayout(false);
            this.splSeparador.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splSeparador)).EndInit();
            this.splSeparador.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splSeparador;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.ListBox lstAlarms;
        private System.Windows.Forms.TextBox txtDescripcion;
    }
}

