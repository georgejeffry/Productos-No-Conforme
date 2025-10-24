
namespace PlaneamientoCDP
{
    partial class frmReporteProdNoConf
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
            Janus.Windows.GridEX.GridEXLayout GridEx1_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.grbUIGroupBox_GPNET1 = new GPNETv4.Windows.Controles.Comunes.grbUIGroupBox_GPNET();
            this.dtpFechaFin = new System.Windows.Forms.DateTimePicker();
            this.dtpFechaInicio = new System.Windows.Forms.DateTimePicker();
            this.lblLabelGP2 = new GPNETv4.Windows.Ctrl.LabelGP.lblLabelGP();
            this.lblLabelGP1 = new GPNETv4.Windows.Ctrl.LabelGP.lblLabelGP();
            this.dgvProNoConfor = new GPNETv4.Windows.Controles.Comunes.grdGridEx_GPNET();
            this.GridEx1 = new GPNETv4.Windows.Controles.Comunes.dgvGridExJanusGPNET(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.grbUIGroupBox_GPNET1)).BeginInit();
            this.grbUIGroupBox_GPNET1.SuspendLayout();
            this.dgvProNoConfor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridEx1)).BeginInit();
            this.SuspendLayout();
            // 
            // grbUIGroupBox_GPNET1
            // 
            this.grbUIGroupBox_GPNET1.Campo_Seguridad = "";
            this.grbUIGroupBox_GPNET1.Controls.Add(this.dtpFechaFin);
            this.grbUIGroupBox_GPNET1.Controls.Add(this.dtpFechaInicio);
            this.grbUIGroupBox_GPNET1.Controls.Add(this.lblLabelGP2);
            this.grbUIGroupBox_GPNET1.Controls.Add(this.lblLabelGP1);
            this.grbUIGroupBox_GPNET1.Location = new System.Drawing.Point(12, 53);
            this.grbUIGroupBox_GPNET1.Name = "grbUIGroupBox_GPNET1";
            this.grbUIGroupBox_GPNET1.Size = new System.Drawing.Size(228, 74);
            this.grbUIGroupBox_GPNET1.TabIndex = 6;
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
            // dgvProNoConfor
            // 
            this.dgvProNoConfor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvProNoConfor.CampoDescripDelete = "";
            this.dgvProNoConfor.CampoIdDelete = "";
            this.dgvProNoConfor.Controls.Add(this.GridEx1);
            this.dgvProNoConfor.Location = new System.Drawing.Point(12, 133);
            this.dgvProNoConfor.Name = "dgvProNoConfor";
            this.dgvProNoConfor.SegExportarExcel = false;
            this.dgvProNoConfor.Size = new System.Drawing.Size(1036, 532);
            this.dgvProNoConfor.TabIndex = 5;
            this.dgvProNoConfor.Tipo_Oparcion_GridEx = GPNETv4.Windows.Controles.Comunes.Tipo_OperacionGridEx.Mantenimiento;
            this.dgvProNoConfor.grdGridDobleClid += new GPNETv4.Windows.Controles.Comunes.DoubleClickEventHandlerGP(this.dgvProNoConfor_grdGridDobleClid);
            this.dgvProNoConfor.grdGridColumnButtonClickGP += new GPNETv4.Windows.Controles.Comunes.ColumnButtonClickGP(this.dgvProNoConfor_grdGridColumnButtonClickGP);
            // 
            // GridEx1
            // 
            this.GridEx1.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.True;
            this.GridEx1.AutoEdit = true;
            this.GridEx1.CampoDescripcionDelete = "";
            this.GridEx1.CampoIDDelete = "";
            GridEx1_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
            this.GridEx1.DesignTimeLayout = GridEx1_DesignTimeLayout;
            this.GridEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridEx1.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.GridEx1.FilterEditorRef = null;
            this.GridEx1.FocusNuevoRegistro = true;
            this.GridEx1.GroupByBoxVisible = false;
            this.GridEx1.Location = new System.Drawing.Point(0, 0);
            this.GridEx1.Name = "GridEx1";
            this.GridEx1.RowHeaderContent = Janus.Windows.GridEX.RowHeaderContent.RowPosition;
            this.GridEx1.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.GridEx1.SegExportarExcel = false;
            this.GridEx1.SelectionMode = Janus.Windows.GridEX.SelectionMode.MultipleSelection;
            this.GridEx1.SettingsKey = "GridEx1";
            this.GridEx1.Size = new System.Drawing.Size(1036, 532);
            this.GridEx1.TabIndex = 2;
            this.GridEx1.Tipo_Operacion_GridEx = GPNETv4.Windows.Controles.Comunes.Tipo_OperacionGridEx.Mantenimiento;
            // 
            // frmReporteProdNoConf
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1060, 692);
            this.Controls.Add(this.grbUIGroupBox_GPNET1);
            this.Controls.Add(this.dgvProNoConfor);
            this.Name = "frmReporteProdNoConf";
            this.Text = "frmReporteProdNoConf";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmReporteProdNoConf_Load);
            this.Controls.SetChildIndex(this.dgvProNoConfor, 0);
            this.Controls.SetChildIndex(this.grbUIGroupBox_GPNET1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.grbUIGroupBox_GPNET1)).EndInit();
            this.grbUIGroupBox_GPNET1.ResumeLayout(false);
            this.dgvProNoConfor.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridEx1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GPNETv4.Windows.Controles.Comunes.grbUIGroupBox_GPNET grbUIGroupBox_GPNET1;
        private System.Windows.Forms.DateTimePicker dtpFechaFin;
        private System.Windows.Forms.DateTimePicker dtpFechaInicio;
        private GPNETv4.Windows.Ctrl.LabelGP.lblLabelGP lblLabelGP2;
        private GPNETv4.Windows.Ctrl.LabelGP.lblLabelGP lblLabelGP1;
        private GPNETv4.Windows.Controles.Comunes.grdGridEx_GPNET dgvProNoConfor;
        private GPNETv4.Windows.Controles.Comunes.dgvGridExJanusGPNET GridEx1;
    }
}