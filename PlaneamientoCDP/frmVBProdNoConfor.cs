using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GPNETv4.Windows.Frm;
using Janus.Windows.GridEX;
using GPNETv4.Sistema.Config;
using System.Diagnostics;
using System.Data.SqlClient;
using GPNETv4.EntidadesNegocio;
using GPNETv4.Sistema.Util.GridEx;
using GPNETv4.Sistema.Util.Frm;
using GPNETv4.Sistema.Data;

namespace PlaneamientoCDP
{
    public partial class frmVBProdNoConfor : frmReporte
    {
        #region Variables
        private DataTable VL_TABLA;

        #endregion

        #region Constructor
        public frmVBProdNoConfor()
        {
            InitializeComponent();
        }

        public frmVBProdNoConfor(string[] args) : base(args)
        {
            InitializeComponent();
        }

        #endregion

        #region Eventos
        private void frmVBProdNoConfor_Load(object sender, EventArgs e)
        {
            Asignar_Titulo_Ventana("Visto Bueno de Productos No Conforme");

            Aplicar_Formato_controles();

            Crear_GridExRoot();


            On_Limpiar();
        }



        #endregion

        #region Metodos
        private async void Procesar_Busqueda()
        {
            //bool bExistenDatos = false;
            string sFiltroPedido = "";


          

            Mensaje_Proceso("Buscando Registros.....", null, true);

            StringBuilder VI_DATA = new StringBuilder();

            VI_DATA.Append(GlobalIdentity.Instance.P_Sys_Default_Cia);
            VI_DATA.Append('¦');
            VI_DATA.Append(GlobalIdentity.Instance.P_Sys_Default_Sede);
            VI_DATA.Append('¦');
            VI_DATA.Append(dtpFechaInicio.Value.ToString("yyyy-MM-dd"));
            VI_DATA.Append('¦');
            VI_DATA.Append(dtpFechaFin.Value.ToString("yyyy-MM-dd"));
            


            //Parametros
            List<SqlParameter> loSqlParameter;
            string sProcediminetoAlm;

            sProcediminetoAlm = "sp_VB_Prod_No_Confor";

            loSqlParameter = new List<SqlParameter>
            {
                new SqlParameter("@Data",SqlDbType.VarChar,8000){Value=VI_DATA.ToString()}

            };

            daDatabase odaDatabase = new daDatabase();

            //Obtener datos
            try
            {
                beDatabaseResult obeDatabaseResult = await odaDatabase.GetDataSetDataResultAsync(sProcediminetoAlm, loSqlParameter);

                if (obeDatabaseResult.Exito)
                {


                    //Limpiar tablas
                    if (VL_TABLA.Rows.Count > 0)
                    {
                        VL_TABLA.Rows.Clear();
                    }

                    
                    


                    VL_TABLA = ((DataSet)obeDatabaseResult.Data).Tables[0];

                    //Asignacion de la tabla a la grilla
                    if (VL_TABLA != null)
                    {
                        if (VL_TABLA.Rows.Count > 0)
                        {
                            dgvProNoConfor.GridEx1.DataSource = VL_TABLA;
                        }
                        else
                        {
                            Mensaje_Proceso("No existen datos", Properties.Resources.Info_24px, false);
                        }

                    }


                }
                else
                {
                    Mensaje_Proceso(obeDatabaseResult.Resultado, Properties.Resources.Error, false, true, TipoMessageBoxGPNET.Error);

                }

            }
            catch (Exception ex)
            {

            }



            Mensaje_Proceso(string.Empty, null, false);










        }

        private void Crear_GridExRoot()
        {

            VL_TABLA = new DataTable();
            //Creando columnas
            VL_TABLA.Columns.Add("SELECTOR", typeof(bool));
            VL_TABLA.Columns.Add("CIA", typeof(string));
            VL_TABLA.Columns.Add("SEDE", typeof(string));
            VL_TABLA.Columns.Add("ANIO", typeof(int));
            VL_TABLA.Columns.Add("NRO_PRODUCTO", typeof(int));
            VL_TABLA.Columns.Add("ID_AREA_DETECT", typeof(string));
            VL_TABLA.Columns.Add("AREA", typeof(string));
            VL_TABLA.Columns.Add("FECHA_REG", typeof(DateTime));
            VL_TABLA.Columns.Add("FLAG_I_RECEPCION", typeof(bool));
            VL_TABLA.Columns.Add("FLAG_I_ALMACENAMIENTO", typeof(bool));
            VL_TABLA.Columns.Add("FLAG_I_PRODUCCION", typeof(bool));
            VL_TABLA.Columns.Add("FLAG_I_DISTRIBUCION", typeof(bool));
            VL_TABLA.Columns.Add("FLAG_I_DEVOLUCIONES", typeof(bool));
            VL_TABLA.Columns.Add("FLAG_OTRO", typeof(bool));
            VL_TABLA.Columns.Add("OTRO_I", typeof(string));
            VL_TABLA.Columns.Add("ID_ARTICULO", typeof(string));
            VL_TABLA.Columns.Add("Articulo", typeof(string));
            VL_TABLA.Columns.Add("NRO_SERIE", typeof(string));
            VL_TABLA.Columns.Add("NRO_LOTE", typeof(string));
            VL_TABLA.Columns.Add("FECHA_VENCE_LOTE", typeof(string));
            VL_TABLA.Columns.Add("CANTIDAD", typeof(float));           
            VL_TABLA.Columns.Add("ID_PROVEEDOR", typeof(string));
            VL_TABLA.Columns.Add("PROVEEDOR", typeof(string));
            VL_TABLA.Columns.Add("OBS_NO_CONFORMIDAD", typeof(string));
            VL_TABLA.Columns.Add("PATH_IMG_NO_CONFOR", typeof(string));
            VL_TABLA.Columns.Add("PATH_IMG_NO_CONFOR_2", typeof(string));
            VL_TABLA.Columns.Add("FLAG_T_RECUPERACION", typeof(bool));
            VL_TABLA.Columns.Add("FLAG_T_CONCESION", typeof(bool));
            VL_TABLA.Columns.Add("FLAG_T_RESIDUO", typeof(bool));
            VL_TABLA.Columns.Add("FLAG_T_DEVOLUCION", typeof(bool));
            VL_TABLA.Columns.Add("OBS_TRATAMIENTO", typeof(string));
            VL_TABLA.Columns.Add("PATH_IMG_TRATAMIENTO", typeof(string));
            VL_TABLA.Columns.Add("ID_ARTICULO_T", typeof(string));
            VL_TABLA.Columns.Add("ARTICULO_T", typeof(string));
            VL_TABLA.Columns.Add("ID_ESTADO", typeof(string));
            VL_TABLA.Columns.Add("ESTADO", typeof(string));
            VL_TABLA.Columns.Add("UC", typeof(string));
            VL_TABLA.Columns.Add("FC", typeof(string));
            VL_TABLA.Columns.Add("UM", typeof(string));
            VL_TABLA.Columns.Add("FM", typeof(string));
            VL_TABLA.Columns.Add("CONVERT_KILO", typeof(float));
            VL_TABLA.Columns.Add("CANTIDAD_TRA", typeof(float));
            VL_TABLA.Columns.Add("CONVERT_KILO_TRA", typeof(float));
            VL_TABLA.Columns.Add("ID_ARTICULO_FINAL", typeof(string));
            VL_TABLA.Columns.Add("CANTIDAD_TRA_F", typeof(float));





            //---Creamos el GridExtable-----------------
            GridEXTable dtGridExRoot = new GridEXTable();

            //Creamos las columnas con referncia a la tabla creada
            GridEXColumn col = dtGridExRoot.Columns.Add("SELECTOR", ColumnType.CheckBox, EditType.CheckBox);
            col.ActAsSelector = true;
            col.Width = 30;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.Selectable = true;

            col = dtGridExRoot.Columns.Add("CIA", ColumnType.Text, EditType.TextBox);
            col.Caption = "Cia";
            col.Visible = false;
            col.Selectable = false;
            col.Width = 35;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;

            col = dtGridExRoot.Columns.Add("SEDE", ColumnType.Text, EditType.TextBox);
            col.Caption = "Sede";
            col.Visible = false;
            col.Selectable = false;
            col.Width = 35;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;

            col = dtGridExRoot.Columns.Add("ANIO", ColumnType.Text, EditType.TextBox);
            col.Caption = "Anio";
            col.Visible = true;
            col.Selectable = false;
            col.Width = 40;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;

            col = dtGridExRoot.Columns.Add("NRO_PRODUCTO", ColumnType.Text, EditType.TextBox);
            col.Caption = "Nro Producto";
            col.Visible = true;
            col.Selectable = false;
            col.Width = 45;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;

            col = dtGridExRoot.Columns.Add("FECHA_REG", ColumnType.Text, EditType.TextBox);
            col.Caption = "Fecha";
            col.Visible = false;
            col.Selectable = false;
            col.Width = 80;
            col.FormatString = "dd/MM/yyyy";
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;

            col = dtGridExRoot.Columns.Add("ID_AREA_DETECT", ColumnType.Text, EditType.TextBox);
            col.Caption = "ID_AREA_DETECT";
            col.Visible = false;
            col.Selectable = false;
            col.Width = 45;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;

            col = dtGridExRoot.Columns.Add("AREA", ColumnType.Text, EditType.TextBox);
            col.Caption = "AREA";
            col.Visible = true;
            col.Selectable = false;
            col.Width = 90;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;


            col = dtGridExRoot.Columns.Add("FLAG_I_RECEPCION", ColumnType.CheckBox, EditType.CheckBox);
            col.Caption = "Recep..";
            col.Visible = true;
            col.Selectable = false;
            col.Width = 45;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;

            col = dtGridExRoot.Columns.Add("FLAG_I_ALMACENAMIENTO", ColumnType.CheckBox, EditType.CheckBox);
            col.Caption = "Almace..";
            col.Visible = true;
            col.Selectable = false;
            col.Width = 45;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;

            col = dtGridExRoot.Columns.Add("FLAG_I_PRODUCCION", ColumnType.CheckBox, EditType.CheckBox);
            col.Caption = "Produc...";
            col.Visible = true;
            col.Selectable = false;
            col.Width = 45;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;

            col = dtGridExRoot.Columns.Add("FLAG_I_DISTRIBUCION", ColumnType.CheckBox, EditType.CheckBox);
            col.Caption = "Distri...";
            col.Visible = true;
            col.Selectable = false;
            col.Width = 45;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;

            col = dtGridExRoot.Columns.Add("FLAG_I_DEVOLUCIONES", ColumnType.CheckBox, EditType.CheckBox);
            col.Caption = "Devol...";
            col.Visible = true;
            col.Selectable = false;
            col.Width = 45;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;

            col = dtGridExRoot.Columns.Add("FLAG_OTRO", ColumnType.CheckBox, EditType.CheckBox);
            col.Caption = "Otro";
            col.Visible = true;
            col.Selectable = false;
            col.Width = 45;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;

            //OTRO_I
            col = dtGridExRoot.Columns.Add("OTRO_I", ColumnType.Text, EditType.TextBox);
            col.Caption = "Otro";
            col.Visible = true;
            col.Selectable = false;
            col.Width = 120;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;


            col = dtGridExRoot.Columns.Add("ID_ARTICULO", ColumnType.Text, EditType.TextBox);
            col.Caption = "ID_ARTICULO";
            col.Visible = true;
            col.Selectable = false;
            col.Width = 80;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;


            col = dtGridExRoot.Columns.Add("Articulo", ColumnType.Text, EditType.TextBox);
            col.Caption = "Articulo";
            col.Visible = true;
            col.Selectable = false;
            col.Width = 220;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;

            col = dtGridExRoot.Columns.Add("NRO_SERIE", ColumnType.Text, EditType.TextBox);
            col.Caption = "NRO_SERIE";
            col.Visible = false;
            col.Selectable = false;
            col.Width = 100;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;


            col = dtGridExRoot.Columns.Add("NRO_LOTE", ColumnType.Text, EditType.TextBox);
            col.Caption = "NRO_LOTE";
            col.Visible = false;
            col.Selectable = false;
            col.Width = 100;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;


            col = dtGridExRoot.Columns.Add("FECHA_VENCE_LOTE", ColumnType.Text, EditType.TextBox);
            col.Caption = "FCH VENCE LOTE";
            col.Visible = true;
            col.Selectable = false;
            col.Width = 80;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;

            col = dtGridExRoot.Columns.Add("CANTIDAD", ColumnType.Text, EditType.TextBox);
            col.Caption = "Cantidad ";
            col.Visible = true;
            col.Selectable = false;
            col.Width = 70;
            col.TextAlignment = TextAlignment.Far;
            col.FormatString = "######,##0.00;(-######,##0.00)";
            col.DefaultGroupFormatString = "######,##0.00;(-######,##0.00)";
            col.TotalFormatString = "######,##0.00;(-######,##0.00)";
            col.AggregateFunction = AggregateFunction.Sum;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;


            col = dtGridExRoot.Columns.Add("CONVERT_KILO", ColumnType.Text, EditType.TextBox);
            col.Caption = "Convert Kilos ";
            col.Visible = true;
            col.Selectable = false;
            col.Width = 70;
            col.TextAlignment = TextAlignment.Far;
            col.FormatString = "######,##0.00;(-######,##0.00)";
            col.DefaultGroupFormatString = "######,##0.00;(-######,##0.00)";
            col.TotalFormatString = "######,##0.00;(-######,##0.00)";
            col.AggregateFunction = AggregateFunction.Sum;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;

            col = dtGridExRoot.Columns.Add("ID_PROVEEDOR", ColumnType.Text, EditType.TextBox);
            col.Caption = "ID_PROVEEDOR";
            col.Visible = false;
            col.Selectable = false;
            col.Width = 60;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;

            col = dtGridExRoot.Columns.Add("PROVEEDOR", ColumnType.Text, EditType.TextBox);
            col.Caption = "PROVEEDOR";
            col.Visible = false;
            col.Selectable = false;
            col.Width = 60;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;

            col = dtGridExRoot.Columns.Add("OBS_NO_CONFORMIDAD", ColumnType.Text, EditType.TextBox);
            col.Caption = "OBS NO CONFORMIDAD";
            col.Visible = true;
            col.Selectable = false;
            col.Width = 220;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;


            col = dtGridExRoot.Columns.Add("btnImgArtnConf");
            col.Caption = "...";
            col.AllowSort = false;
            col.ButtonStyle = ButtonStyle.ButtonCell;
            col.EditType = EditType.NoEdit;
            col.ButtonDisplayMode = CellButtonDisplayMode.Always;
            col.ButtonImage = Properties.Resources.Camara_16px;
            col.Width = 40;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;


            col = dtGridExRoot.Columns.Add("btnImgArtnConf2");
            col.Caption = "...";
            col.AllowSort = false;
            col.ButtonStyle = ButtonStyle.ButtonCell;
            col.EditType = EditType.NoEdit;
            col.ButtonDisplayMode = CellButtonDisplayMode.Always;
            col.ButtonImage = Properties.Resources.Camara_16px;
            col.Width = 40;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;


            col = dtGridExRoot.Columns.Add("PATH_IMG_NO_CONFOR", ColumnType.Text, EditType.TextBox);
            col.Caption = "PATH_IMG_NO_CONFOR";
            col.Visible = false;
            col.Selectable = false;
            col.Width = 60;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;

            col = dtGridExRoot.Columns.Add("PATH_IMG_NO_CONFOR_2", ColumnType.Text, EditType.TextBox);
            col.Caption = "PATH_IMG_NO_CONFOR 2";
            col.Visible = false;
            col.Selectable = false;
            col.Width = 60;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;




            col = dtGridExRoot.Columns.Add("FLAG_T_RECUPERACION", ColumnType.CheckBox, EditType.CheckBox);
            col.Caption = "Recupera..";
            col.Visible = true;
            col.Selectable = false;
            col.Width = 45;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;

            col = dtGridExRoot.Columns.Add("FLAG_T_CONCESION", ColumnType.CheckBox, EditType.CheckBox);
            col.Caption = "Concección";
            col.Visible = true;
            col.Selectable = false;
            col.Width = 45;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;

            col = dtGridExRoot.Columns.Add("FLAG_T_RESIDUO", ColumnType.CheckBox, EditType.CheckBox);
            col.Caption = "Residuo";
            col.Visible = true;
            col.Selectable = false;
            col.Width = 45;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;

            col = dtGridExRoot.Columns.Add("FLAG_T_DEVOLUCION", ColumnType.CheckBox, EditType.CheckBox);
            col.Caption = "Devol...";
            col.Visible = true;
            col.Selectable = false;
            col.Width = 45;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;

            col = dtGridExRoot.Columns.Add("OBS_TRATAMIENTO", ColumnType.Text, EditType.TextBox);
            col.Caption = "OBS TRATAMIENTO";
            col.Visible = true;
            col.Selectable = false;
            col.Width = 220;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;

            col = dtGridExRoot.Columns.Add("btnImgTratado");
            col.Caption = "...";
            col.AllowSort = false;
            col.ButtonStyle = ButtonStyle.ButtonCell;
            col.EditType = EditType.NoEdit;
            col.ButtonDisplayMode = CellButtonDisplayMode.Always;
            col.ButtonImage = Properties.Resources.Camara_16px;
            col.Width = 40;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;


            col = dtGridExRoot.Columns.Add("PATH_IMG_TRATAMIENTO", ColumnType.Text, EditType.TextBox);
            col.Caption = "PATH IMG TRATAMIENTO";
            col.Visible = false;
            col.Selectable = false;
            col.Width = 60;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;

            col = dtGridExRoot.Columns.Add("PATH_IMG_TRATAMIENTO_2", ColumnType.Text, EditType.TextBox);
            col.Caption = "PATH IMG TRATAMIENTO 2";
            col.Visible = false;
            col.Selectable = false;
            col.Width = 60;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;

            col = dtGridExRoot.Columns.Add("ID_ARTICULO_T", ColumnType.Text, EditType.TextBox);
            col.Caption = "ID_ARTICULO_T";
            col.Visible = false;
            col.Selectable = false;
            col.Width = 60;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;

            col = dtGridExRoot.Columns.Add("ARTICULO_T", ColumnType.Text, EditType.TextBox);
            col.Caption = "ARTICULO_T";
            col.Visible = false;
            col.Selectable = false;
            col.Width = 60;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;

            col = dtGridExRoot.Columns.Add("ID_ESTADO", ColumnType.Text, EditType.TextBox);
            col.Caption = "ID_ESTADO";
            col.Visible = false;
            col.Selectable = false;
            col.Width = 70;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;

            col = dtGridExRoot.Columns.Add("ESTADO", ColumnType.Text, EditType.TextBox);
            col.Caption = "ESTADO";
            col.Visible = true;
            col.Selectable = false;
            col.Width = 60;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;

            col = dtGridExRoot.Columns.Add("UC", ColumnType.Text, EditType.TextBox);
            col.Caption = "UC";
            col.Visible = true;
            col.Selectable = false;
            col.Width = 60;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;


            

            col = dtGridExRoot.Columns.Add("FC", ColumnType.Text, EditType.TextBox);
            col.Caption = "FC";
            col.Visible = false;
            col.Selectable = false;
            col.Width = 60;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;

            col = dtGridExRoot.Columns.Add("UM", ColumnType.Text, EditType.TextBox);
            col.Caption = "UM";
            col.Visible = false;
            col.Selectable = false;
            col.Width = 60;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;

            col = dtGridExRoot.Columns.Add("FM", ColumnType.Text, EditType.TextBox);
            col.Caption = "FM";
            col.Visible = false;
            col.Selectable = false;
            col.Width = 60;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;
            col.TextAlignment = TextAlignment.Near;

            col = dtGridExRoot.Columns.Add("CANTIDAD_TRA", ColumnType.Text, EditType.TextBox);
            col.Caption = "Cantidad Tra..";
            col.Visible = true;
            col.Selectable = false;
            col.Width = 70;
            col.TextAlignment = TextAlignment.Far;
            col.FormatString = "######,##0.00;(-######,##0.00)";
            col.DefaultGroupFormatString = "######,##0.00;(-######,##0.00)";
            col.TotalFormatString = "######,##0.00;(-######,##0.00)";
            col.AggregateFunction = AggregateFunction.Sum;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;


            col = dtGridExRoot.Columns.Add("CONVERT_KILO_TRA", ColumnType.Text, EditType.TextBox);
            col.Caption = "Convert Kilos Tra..";
            col.Visible = true;
            col.Selectable = false;
            col.Width = 70;
            col.TextAlignment = TextAlignment.Far;
            col.FormatString = "######,##0.00;(-######,##0.00)";
            col.DefaultGroupFormatString = "######,##0.00;(-######,##0.00)";
            col.TotalFormatString = "######,##0.00;(-######,##0.00)";
            col.AggregateFunction = AggregateFunction.Sum;
            col.HeaderStyle.BackColor = Color.FromArgb(222, 234, 248);
            col.HeaderStyle.BackColorGradient = Color.FromArgb(209, 224, 239);
            col.HeaderStyle.BackgroundGradientMode = BackgroundGradientMode.Vertical;


            dgvProNoConfor.GridEx1.RootTable = dtGridExRoot;


            //Creacion de grupos
            GridEXGroup group;

            col = dgvProNoConfor.GridEx1.RootTable.Columns["FECHA_REG"];
            group = new GridEXGroup(col, Janus.Windows.GridEX.SortOrder.Descending);
            dgvProNoConfor.GridEx1.RootTable.Groups.Add(col);


            //Formato de Grilla
            //Tamaño de la cabecera
            dgvProNoConfor.GridEx1.RootTable.HeaderLines = 2;
            dgvProNoConfor.GridEx1.RootTable.HeaderFormatStyle.TextAlignment = TextAlignment.Center;



            //Agregar Cantidad de Registros por Grupo
            GridEXGroupHeaderTotal grpTotales = new GridEXGroupHeaderTotal();

            grpTotales.AggregateFunction = AggregateFunction.Count;
            grpTotales.TotalPrefix = "          Cantidad Registros (";
            grpTotales.TotalSuffix = ")";
            grpTotales.TotalFormatMode = FormatMode.UseIFormattable;
            grpTotales.TotalFormatString = "######,##0;(-######,##0)";

            dgvProNoConfor.GridEx1.RootTable.GroupHeaderTotals.Add(grpTotales);

            //Agregar sumatoria

            dgvProNoConfor.GridEx1.RootTable.GroupTotals = GroupTotals.ExpandedGroup;

            //Totales al pie
            dgvProNoConfor.GridEx1.TotalRow = InheritableBoolean.True;
            dgvProNoConfor.GridEx1.TotalRowPosition = TotalRowPosition.BottomFixed;


        }

        public void Aplicar_Formato_controles()
        {
            grbUIGroupBox_GPNET1.VisualStyleManager = vsmVisualStyleManager1;

            dgvProNoConfor.GridEx1.VisualStyleManager = vsmVisualStyleManager1;

            //Formato perzonalizado para dar 
            //dgvProNoConfor.GridEx1.VisualStyle = VisualStyle.Standard;
            //dgvProNoConfor.GridEx1.ThemedAreas = ThemedArea.ScrollBars;


            //Dar Formato a la Grilla
            dgvProNoConfor.GridEx1.Tipo_Operacion_GridEx = GPNETv4.Windows.Controles.Comunes.Tipo_OperacionGridEx.Reporte;
            dgvProNoConfor.GridEx1.CellSelectionMode = Janus.Windows.GridEX.CellSelectionMode.EntireRow;
            dgvProNoConfor.GridEx1.SegExportarExcel = true;
            //Tamaño de texto
            dgvProNoConfor.GridEx1.Font = new Font("Arial", 8, FontStyle.Regular);
        }

        #endregion

        #region Override

        public override bool fInicializarObjetos()
        {
            DateTime dtFch = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            dtpFechaInicio.Value = dtFch;
            dtpFechaFin.Value = DateTime.Now;

            Procesar_Busqueda();
            return base.fInicializarObjetos();
        }

        public override bool fProcesar()
        {

            Procesar_Busqueda();


            return base.fProcesar();
        }



        #endregion

        private void dgvProNoConfor_grdGridColumnButtonClickGP(object sender, ColumnActionEventArgs e)
        {

            //Validar que no este vacio la grilla
            if (dgvProNoConfor.GridEx1.DataSource == null)
                return;

            //Validamos si cuenta con registros
            if (dgvProNoConfor.GridEx1.RowCount == 0)
                return;


            //Obtenemos la fila seleccionada
            GridEXRow row_Current = dgvProNoConfor.GridEx1.CurrentRow;

            if (e.Column.Key.Equals("btnImgArtnConf"))
            {
                if (row_Current.Cells["PATH_IMG_NO_CONFOR"].Value.ToString().Trim().Length > 0)
                {
                    frmVisorImg frm = new frmVisorImg();
                    frm.sPathImagen = row_Current.Cells["PATH_IMG_NO_CONFOR"].Value.ToString();
                    frm.Show();
                }
                else
                {
                    Mensaje_Proceso("No existe imagen", null, false, true, TipoMessageBoxGPNET.Informacion);

                }
            }
            if (e.Column.Key.Equals("btnImgArtnConf2"))
            {
                if (row_Current.Cells["PATH_IMG_NO_CONFOR_2"].Value.ToString().Trim().Length > 0)
                {
                    frmVisorImg frm = new frmVisorImg();
                    frm.sPathImagen = row_Current.Cells["PATH_IMG_NO_CONFOR_2"].Value.ToString();
                    frm.Show();
                }
                else
                {
                    Mensaje_Proceso("No existe imagen",null,false,true,TipoMessageBoxGPNET.Informacion);

                }
            }
            if (e.Column.Key.Equals("btnImgTratado"))
            {
                if (row_Current.Cells["PATH_IMG_TRATAMIENTO"].Value.ToString().Trim().Length > 0)
                {
                    frmVisorImg frm = new frmVisorImg();
                    frm.sPathImagen = row_Current.Cells["PATH_IMG_TRATAMIENTO"].Value.ToString();
                    frm.Show();
                }
                else
                {
                    Mensaje_Proceso("No existe imagen", null, false, true, TipoMessageBoxGPNET.Informacion);

                }
            }
        }

        private void btnVBProdConfor_Click(object sender, EventArgs e)
        {
            //Revisar si existe alguno marcado
            GridEXRow[] checkedRows;
            StringBuilder sData;

            //Obtenemos un array de solo Marcados
            checkedRows = dgvProNoConfor.GridEx1.GetCheckedRows();

            if (checkedRows.Length == 0)
            {
                //Uti_frm.MsjAdvertencia("No existe ningun registro marcado, favor de marcar para porder realizar la actualización");
                Mensaje_Proceso("No existe ningun registro marcado, favor de marcar para porder realizar la actualización",
                    Properties.Resources.Info_24px,false,true,TipoMessageBoxGPNET.Informacion);
                dgvProNoConfor.GridEx1.Select();
                return;
            }
            else
            {

                //Validar si puede autorizar

                frmCodigoActivacion ofrmCodigoAct = new frmCodigoActivacion();
                ofrmCodigoAct.ShowDialog();

                if (ofrmCodigoAct.ResultadoValidacion == 1 )
                {

                    sData = new StringBuilder();

                    sData.Append(GlobalIdentity.Instance.P_Sys_Default_Cia);
                    sData.Append("¦");
                    sData.Append(GlobalIdentity.Instance.P_Sys_Default_Sede);
                    sData.Append("¦");
                    sData.Append(ofrmCodigoAct.Id_Usuario_Aprueba);
                    sData.Append("¯");

                    Int32 nCount = 0;

                    foreach (GridEXRow row in checkedRows)
                    {

                        if (nCount == 0)
                        {
                            sData.Append(row.Cells["ANIO"].Value.ToString());
                            sData.Append("¦");
                            sData.Append(row.Cells["NRO_PRODUCTO"].Value.ToString());
                        }
                        else
                        {
                            sData.Append("¬");
                            sData.Append(row.Cells["ANIO"].Value.ToString());
                            sData.Append("¦");
                            sData.Append(row.Cells["NRO_PRODUCTO"].Value.ToString());
                        }

                        nCount++;

                        //if (ofrmCodigoAct.ResultadoValidacion == 1)
                        //{
                        //    Actualizar(row, ofrmCodigoAct.Id_Usuario_Aprueba);
                        //}
                        //else
                        //{
                        //    if (row.Cells["Tipo_Calculo_HE"].Value.ToString().Equals(row.Cells["TC_HE"].Value.ToString()))
                        //        Actualizar(row, ofrmCodigoAct.Id_Usuario_Aprueba);

                        //}

                    }

                    //Actualizar registros
                    Actualizar_registro(sData.ToString());



                    //GPNET.Sistema.Util.Frm.Uti_frm.MsjInformacion("Se actualizó con éxito");
                    Mensaje_Proceso("Se autorizó con éxito",null,null,true,TipoMessageBoxGPNET.Informacion);
                    //Limpiar Registros
                    On_Limpiar();
                }
            }
        }

        private async void Actualizar_registro(string sData)
        {
            List<SqlParameter> loSqlParameter;
            string sProcediminetoAlm;

            //Leer si la tabla compania_expand el tiempo y cuando fue la ultima recalculada de saldo
            sProcediminetoAlm = "sp_ActEstado_PRODUCTO_NO_CONFORME";

            loSqlParameter = new List<SqlParameter>
            {
                new SqlParameter("@Data",SqlDbType.VarChar,8000){Value=sData}
            };


            daDatabase odaDatabase = new daDatabase();
            beDatabaseResult obeDatabaseResult;
            //Obterner
            try
            {
                obeDatabaseResult = await odaDatabase.Execute_beDataAsync(sProcediminetoAlm, loSqlParameter);

                if (obeDatabaseResult.Exito)
                {
                   // Uti_frm.MsjInformacion("Se grabó con éxito 1");
                    //bNuevoRegistro = false;


                }
                else
                    throw new Exception(obeDatabaseResult.Resultado);



                //Limpiar Registros
               // On_Limpiar();


                //return obeDatabaseResult.Exito;
            }
            catch (Exception ex)
            {
                Uti_frm.MsjError(ex.Message);
                return;
            }
        }

        private void dgvProNoConfor_grdGridDobleClid(object Sender, EventArgs e)
        {
            if (dgvProNoConfor.GridEx1.DataSource == null)
                return;

            //obtenemos la fila seleccionada
            GridEXRow rw_Current = dgvProNoConfor.GridEx1.CurrentRow;

            if (rw_Current.RowType == RowType.Record)
            {
                //string sNomAplicacion;
                //string sParametros;//941050

                //sNomAplicacion = "mantPapeletasCDP.exe";
                //sParametros = args_Sys[0] + " " + args_Sys[1] + " " + args_Sys[2] +
                //    " " + rw_Current.Cells["CIA"].Text + " " + args_Sys[4] + " " + args_Sys[5] +
                //    " " + "941050" + " " + "1 1 1" + " " + rw_Current.Cells["ID_PAPELETA"].Text.Replace(",", "");

                //try
                //{
                //    //MessageBox.Show(sParametros);
                //    Process.Start(sNomAplicacion, sParametros);

                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message);
                //}

                //VL_TABLA.Columns.Add("ANIO", typeof(int));
                //VL_TABLA.Columns.Add("NRO_PRODUCTO", typeof(int));


                frmManProdNoCon frm = new frmManProdNoCon(args_Sys);
                frm.DatosRegistro = GlobalIdentity.Instance.P_Sys_Default_Cia +  "|"  +
                    GlobalIdentity.Instance.P_Sys_Default_Sede + "|" +
                    rw_Current.Cells["ANIO"].Value.ToString() + "|" +
                    rw_Current.Cells["NRO_PRODUCTO"].Value.ToString();


                frm.Show();



            }
        }
    }
}
