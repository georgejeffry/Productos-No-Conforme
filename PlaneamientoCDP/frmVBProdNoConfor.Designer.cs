
namespace PlaneamientoCDP
{
    partial class frmVBProdNoConfor
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dgvProNoConfor = new GPNETv4.Windows.Controles.Comunes.grdGridEx_GPNET();
            this.grbUIGroupBox_GPNET1 = new GPNETv4.Windows.Controles.Comunes.grbUIGroupBox_GPNET();
            this.dtpFechaFin = new System.Windows.Forms.DateTimePicker();
            this.dtpFechaInicio = new System.Windows.Forms.DateTimePicker();
            this.lblLabelGP2 = new GPNETv4.Windows.Ctrl.LabelGP.lblLabelGP();
            this.lblLabelGP1 = new GPNETv4.Windows.Ctrl.LabelGP.lblLabelGP();
            this.btnVBProdConfor = new GPNETv4.Windows.Ctrl.ButtonGP.btnButtonGP();
            ((System.ComponentModel.ISupportInitialize)(this.grbUIGroupBox_GPNET1)).BeginInit();
            this.grbUIGroupBox_GPNET1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvProNoConfor
            // 
            this.dgvProNoConfor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvProNoConfor.CampoDescripDelete = "";
            this.dgvProNoConfor.CampoIdDelete = "";
            this.dgvProNoConfor.Location = new System.Drawing.Point(12, 132);
            this.dgvProNoConfor.Name = "dgvProNoConfor";
            this.dgvProNoConfor.SegExportarExcel = false;
            this.dgvProNoConfor.Size = new System.Drawing.Size(979, 547);
            this.dgvProNoConfor.TabIndex = 3;
            this.dgvProNoConfor.Tipo_Oparcion_GridEx = GPNETv4.Windows.Controles.Comunes.Tipo_OperacionGridEx.Mantenimiento;
            this.dgvProNoConfor.grdGridDobleClid += new GPNETv4.Windows.Controles.Comunes.DoubleClickEventHandlerGP(this.dgvProNoConfor_grdGridDobleClid);
            this.dgvProNoConfor.grdGridColumnButtonClickGP += new GPNETv4.Windows.Controles.Comunes.ColumnButtonClickGP(this.dgvProNoConfor_grdGridColumnButtonClickGP);
            // 
            // grbUIGroupBox_GPNET1
            // 
            this.grbUIGroupBox_GPNET1.Campo_Seguridad = "";
            this.grbUIGroupBox_GPNET1.Controls.Add(this.dtpFechaFin);
            this.grbUIGroupBox_GPNET1.Controls.Add(this.dtpFechaInicio);
            this.grbUIGroupBox_GPNET1.Controls.Add(this.lblLabelGP2);
            this.grbUIGroupBox_GPNET1.Controls.Add(this.lblLabelGP1);
            this.grbUIGroupBox_GPNET1.Location = new System.Drawing.Point(12, 52);
            this.grbUIGroupBox_GPNET1.Name = "grbUIGroupBox_GPNET1";
            this.grbUIGroupBox_GPNET1.Size = new System.Drawing.Size(228, 74);
            this.grbUIGroupBox_GPNET1.TabIndex = 4;
            this.grbUIGroupBox_GPNET1.Text = "Fecha";
            // 
            // dtpFechaFin
            // 
            this.dtpFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaFin.Location = new System.Drawing.Point(122, 42);
            this.dtpFechaFin.Name = "dtpFechaFin";
            this.dtpFechaFin.Size = new System.Drawing.Size(98, 20);
            this.dtpFechaFin.TabIndex = 2;
            // 
            // dtpFechaInicio
            // 
            this.dtpFechaInicio.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaInicio.Location = new System.Drawing.Point(122, 16);
            this.dtpFechaInicio.Name = "dtpFechaInicio";
            this.dtpFechaInicio.Size = new System.Drawing.Size(98, 20);
            this.dtpFechaInicio.TabIndex = 1;
            // 
            // lblLabelGP2
            // 
            this.lblLabelGP2.ColorLinea = System.Drawing.SystemColors.ControlDark;
            this.lblLabelGP2.ConLinea = true;
            this.lblLabelGP2.Location = new System.Drawing.Point(21, 46);
            this.lblLabelGP2.Name = "lblLabelGP2";
            this.lblLabelGP2.Size = new System.Drawing.Size(100, 16);
            this.lblLabelGP2.TabIndex = 0;
            this.lblLabelGP2.Text = "Fecha Fin";
            // 
            // lblLabelGP1
            // 
            this.lblLabelGP1.ColorLinea = System.Drawing.SystemColors.ControlDark;
            this.lblLabelGP1.ConLinea = true;
            this.lblLabelGP1.Location = new System.Drawing.Point(21, 20);
            this.lblLabelGP1.Name = "lblLabelGP1";
            this.lblLabelGP1.Size = new System.Drawing.Size(100, 16);
            this.lblLabelGP1.TabIndex = 0;
            this.lblLabelGP1.Text = "Fecha Inicio";
            // 
            // btnVBProdConfor
            // 
            this.btnVBProdConfor.Campo_Seguridad = "";
            this.btnVBProdConfor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnVBProdConfor.Image = global::PlaneamientoCDP.Properties.Resources.Aceptar;
            this.btnVBProdConfor.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnVBProdConfor.Location = new System.Drawing.Point(266, 103);
            this.btnVBProdConfor.Name = "btnVBProdConfor";
            this.btnVBProdConfor.Size = new System.Drawing.Size(182, 23);
            this.btnVBProdConfor.TabIndex = 5;
            this.btnVBProdConfor.Text = "VB. Producto no Conforme";
            this.btnVBProdConfor.UseVisualStyleBackColor = false;
            this.btnVBProdConfor.Click += new System.EventHandler(this.btnVBProdConfor_Click);
            // 
            // frmVBProdNoConfor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1003, 706);
            this.Controls.Add(this.btnVBProdConfor);
            this.Controls.Add(this.grbUIGroupBox_GPNET1);
            this.Controls.Add(this.dgvProNoConfor);
            this.Name = "frmVBProdNoConfor";
            this.Text = "frmVBProdNoConfor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmVBProdNoConfor_Load);
            this.Controls.SetChildIndex(this.dgvProNoConfor, 0);
            this.Controls.SetChildIndex(this.grbUIGroupBox_GPNET1, 0);
            this.Controls.SetChildIndex(this.btnVBProdConfor, 0);
            ((System.ComponentModel.ISupportInitialize)(this.grbUIGroupBox_GPNET1)).EndInit();
            this.grbUIGroupBox_GPNET1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GPNETv4.Windows.Controles.Comunes.grdGridEx_GPNET dgvProNoConfor;
        private GPNETv4.Windows.Controles.Comunes.grbUIGroupBox_GPNET grbUIGroupBox_GPNET1;
        private System.Windows.Forms.DateTimePicker dtpFechaFin;
        private System.Windows.Forms.DateTimePicker dtpFechaInicio;
        private GPNETv4.Windows.Ctrl.LabelGP.lblLabelGP lblLabelGP2;
        private GPNETv4.Windows.Ctrl.LabelGP.lblLabelGP lblLabelGP1;
        private GPNETv4.Windows.Ctrl.ButtonGP.btnButtonGP btnVBProdConfor;
    }
}