using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Janus.Windows.GridEX;
using GPNETv4.Sistema.Util.Frm;
using GPNETv4.Windows.Frm;
using GPNETv4.Sistema.Config;
using GPNETv4.DataString;
using GPNETv4.EntidadesNegocio;
using System.Data.SqlClient;
using GPNETv4.Sistema.Data;
using System.IO;
using GPNETv4.Sistema.Util.IO;
using GPNETv4.Sistema.Drawing;

namespace PlaneamientoCDP
{
    public partial class frmManProdNoCon : frmMantenimiento
    {

        #region Parametros

        public string DatosRegistro { get; set; }

        #endregion


        #region Variables




        //Combos
        //List<beItemLista> lAddItem = new List<beItemLista>();
        DataTable VL_COMBO_AREA;
        DataTable VL_COMBO_AREA_ORIG;
        DataTable VL_Linea_Negocio;

        //Mant
        bool bNuevoRegistro = true;
        string sIdEstado="11", sMotivoAnulacion="";
        object oValorLLaveBuscado = null;
        int nValoId_Reg;
        string sValorBusqArtTraBase = string.Empty,
                sValorBusqArtTraFinal = string.Empty;
        bool bDelaVentanaBusqueda = false,            
            bDelaVentanaBusqArtTraBase = false,
            bDelaVentanaBusqArtTraFinal = false;
        

        private string strId_CodTrabajor = string.Empty;

        bool bCtrlActIdProd = false,
            bCtrlActNrSerie = false,
            bCrtActNroLote = false;


        #endregion

        #region Constructor

        public frmManProdNoCon()
        {
            InitializeComponent();
        }
        public frmManProdNoCon(string[] args):base(args)
        {
            InitializeComponent();

            tsbImprimir.Enabled = false;
            tsmImprimir.Enabled = false;
        }

        #endregion

        #region Funciones Override


        public override bool fLimpiar()
        {

            //cboAreaDefecto.SelectedIndex = 1;
            txtEstado.Text = string.Empty;
            rbRecepcion.Checked = true;
            txtOtroProceso.Text = string.Empty;
            txtIdProducto.Text = string.Empty;
            txtDescripProducto.Text = string.Empty;
            txtSerieProd.Text = string.Empty;
            txtNroLoteProd.Text = string.Empty;
            txtFchVenceLote.Text = string.Empty;
            txtUndMendida.Text = string.Empty;
            txtCantidad.Text = string.Empty;
            txtProveedor.Text = string.Empty;
            txtProveedorDescrip.Text = string.Empty;
            txtDescrpNoConformi.Text = string.Empty;
            rbRecuperación.Checked = false;
            rbConcesion.Checked = false;
            rbResiduo.Checked = false;
            rbDevolucion.Checked = false;

            txtDescripTratamiento.Text = string.Empty;
            txtIdArticuloTrata.Text = string.Empty;
            txtDescripTrata.Text = string.Empty;

            txtPathImg1.Text = string.Empty;
            txtPathImg1_2.Text = string.Empty;
            txtPathImg2.Text = string.Empty;
            btnDelet1.Visible = false;
            btnDelete2.Visible = false;
            btnDelet1_2.Visible = false;
            btnImgPrdNoConf.Image = Properties.Resources.camara_79px_vacio;
            btnImgPrdNoConf2.Image= Properties.Resources.camara_79px_vacio; 
            btnImgProdTratado.Image = Properties.Resources.camara_79px_vacio;
            sMotivoAnulacion = string.Empty;
            txtCantProdTratado.Text = string.Empty;
            txtIdArticuloTrataFinal.Text = string.Empty;
            txtDescripTrataFinal.Text = string.Empty;
            txtCantProdTratadoFinal.Text = string.Empty;
            txtUndMendida.Text = string.Empty;
            txtRefKardex.Text = string.Empty;

            cboLineaProd.SelectedIndex = 0;

            //grbTrataMiento.Enabled = false;

            Mensaje_Proceso("", null);


            return base.fLimpiar();
        }

        public override bool fInicializarObjetos()
        {
            sIdEstado = "11";

            if (cboAreaDefecto != null)
            {
                if (cboAreaDefecto.Items.Count>0)
                    cboAreaDefecto.SelectedIndex = 0;
            }

            if (cboAreaOrigen != null)
            {
                if (cboAreaOrigen.Items.Count > 0)
                    cboAreaOrigen.SelectedIndex = 0;
            }

            if (cboTipoFalla != null)
            {
                if (cboTipoFalla.Items.Count > 0)
                    cboTipoFalla.SelectedIndex = 0;
            }

            if (cboLineaProd != null)
            {
                if (cboLineaProd.Items.Count > 0)
                    cboLineaProd.SelectedIndex = 0;
            }

            txtAnio.Text = DateTime.Now.Year.ToString();
            dtpFechaReg.Value = DateTime.Now;
            


            //Obtener el Ultimo registro
            Obtener_El_Ultimo_Registro();


            //foco
            cboAreaOrigen.Focus();
            cboAreaOrigen.Select();

            Accesso_Segun_Estado(sIdEstado);


            return base.fInicializarObjetos();
        }

        public override bool fAntesDeGrabar()
        {

            
            if (bNuevoRegistro || sIdEstado.Equals("11"))
            {
                if (txtDescrpNoConformi.Text.Trim().Length > 0)
                {
                    if (txtPathImg1.Text.Trim().Length==0 && txtPathImg1_2.Text.Trim().Length == 0)
                    {
                        Mensaje_Proceso("Debe de ingresar una imagen del producto no conforme", Properties.Resources.Info_24px, null, true, TipoMessageBoxGPNET.Informacion);
                        btnImgPrdNoConf.Select();
                        return false;
                    }
                }
                    
            }



            if (sIdEstado.Equals("12"))
            {
                Mensaje_Proceso("El registro se encuentra anulado", Properties.Resources.Info_24px, null, true, TipoMessageBoxGPNET.Informacion);
                return false;
            }


            //if (sIdEstado.Equals("41"))
            //{
            //    Mensaje_Proceso("No se puede modificar un Producto no Conforme con VB",Properties.Resources.Info_24px,null,true,TipoMessageBoxGPNET.Informacion);
            //    return false;
            //}

            //Validar la Seguridad de Segun politicas de usuario
            //if (bNuevoRegistro)
            //{
            //    if (!ListCodSeguridadERP[0].Estado)
            //    {
            //        //Uti_frm.MsjError("El usuario " + GlobalIdentity.Instance.P_Sys_Default_Usuario + ", no esta autorizado en insertar un nuevo registro");
            //        Mensaje_Proceso("El usuario " + GlobalIdentity.Instance.P_Sys_Default_Usuario + ", no esta autorizado en insertar un nuevo registro", Properties.Resources.Info_24px, null, true, TipoMessageBoxGPNET.Informacion);
            //        On_Limpiar();
            //        return false;
            //    }
            //}
            //else
            //{
            //    if (!ListCodSeguridadERP[5].Estado)
            //    {
            //        //Uti_frm.MsjError("El usuario " + GlobalIdentity.Instance.P_Sys_Default_Usuario + ", no esta autorizado en modificar un registro");
            //        Mensaje_Proceso("El usuario " + GlobalIdentity.Instance.P_Sys_Default_Usuario + ", no esta autorizado en modificar un registro", Properties.Resources.Info_24px, null, true, TipoMessageBoxGPNET.Informacion);
            //        On_Limpiar();
            //        return false;
            //    }
            //}


            if (!bNuevoRegistro && (txtEstado.Text == "Anulado" || txtEstado.Text == "Rechazado"))
            {
                //Uti_frm.MsjError("No se puede actualizar un registro Rechazado");
                Mensaje_Proceso("No se puede actualizar un registro Rechazado", Properties.Resources.Info_24px, null, true, TipoMessageBoxGPNET.Informacion);
                On_Limpiar();
                return false;
            }

            if (cboAreaOrigen.SelectedValue.ToString().Equals(""))
            {
                Mensaje_Proceso("Favor de seleccionar un área", Properties.Resources.Info_24px, null, true, TipoMessageBoxGPNET.Informacion);
                cboAreaOrigen.Select();
                return false;
            }


            if (cboAreaDefecto.SelectedValue.ToString().Equals(""))
            {
                Mensaje_Proceso("Favor de seleccionar un área", Properties.Resources.Info_24px, null, true, TipoMessageBoxGPNET.Informacion);
                cboAreaDefecto.Select();
                return false;
            }

            if (cboTipoFalla.SelectedIndex==0)
            {
                Mensaje_Proceso("Favor de seleccionar un tipo de no conformidad", Properties.Resources.Info_24px, null, true, TipoMessageBoxGPNET.Informacion);
                cboTipoFalla.Select();
                return false;
            }



            if (rbOtros.Checked)
            {
                if (txtOtroProceso.Text.Trim().Length == 0)
                {
                    //Uti_frm.MsjError("Favor de especificar en que proceso reslto el origen del producto no conforme.");
                    Mensaje_Proceso("Favor de especificar en que proceso resulto el origen del producto no conforme.", Properties.Resources.Info_24px, null, true, TipoMessageBoxGPNET.Informacion);
                    txtOtroProceso.Select();
                    return false;
                }
            }


            //Validar que el campo del trabajador no este vacio
            if (txtIdProducto.Text.Trim().Length == 0 || txtDescripProducto.Text.Trim().Length == 0)
            {
                //Uti_frm.MsjError("Favor de ingresar un Producto no conforme");
                Mensaje_Proceso("Favor de ingresar un Producto no conforme", Properties.Resources.Info_24px, null, true, TipoMessageBoxGPNET.Informacion);
                txtIdProducto.Select();
                return false;
            }

            if (txtCantidad.Text.Trim().Length==0 || txtCantidad.Text.Equals("0"))
            {
                Mensaje_Proceso("Debe de Inresar la cantidad de producto no conforme", Properties.Resources.Info_24px, null, true, TipoMessageBoxGPNET.Informacion);
                txtCantidad.Select();
                return false;
            }
            

            if (txtDescrpNoConformi.Text.Trim().Length == 0)
            {
                Mensaje_Proceso("Debe de Inresar la descripción del producto no conforme", Properties.Resources.Info_24px, null, true, TipoMessageBoxGPNET.Informacion);
                txtDescrpNoConformi.Select();
                return false;
            }


            //if (txtPathImg1.Text.Trim().Length == 0)
            //{
            //    Mensaje_Proceso("Debe de ingresar la imagen del producto no conforme", Properties.Resources.Info_24px, null, true, TipoMessageBoxGPNET.Informacion);
            //    btnImgPrdNoConf.Select();
            //    return false;
            //}

            //Valida para el llenado del segundo tratramiento
            //if (!bNuevoRegistro)
            //{

              


                //if (txtDescripTratamiento.Text.Trim().Length >0 && txtPathImg2.Text.Trim().Length == 0)
                //{
                //    Mensaje_Proceso("Debe de Ingresar la imagen del producto tratado", Properties.Resources.Info_24px, null, true, TipoMessageBoxGPNET.Informacion);
                //    btnImgProdTratado.Select();
                //    return false;
                //}


            if(sIdEstado.Equals("41") || sIdEstado.Equals("42"))
            {
                if (cboLineaProd.SelectedValue.ToString().Equals(""))
                {
                    Mensaje_Proceso("Favor de seleccionar la Linea de negocio", Properties.Resources.Info_24px, null, true, TipoMessageBoxGPNET.Informacion);
                    cboLineaProd.Select();
                    return false;
                }

                //if (rbRecuperación.Checked==false &&
                //    rbConcesion.Checked==false &&
                //    rbResiduo.Checked==false &&
                //    rbDevolucion.Checked == false)
                //{
                //    Mensaje_Proceso("Debe de Ingresar que proceso de recuperación", Properties.Resources.Info_24px, null, true, TipoMessageBoxGPNET.Informacion);
                //    rbRecuperación.Select();
                //    return false;
                //}

                //if (txtDescripTratamiento.Text.Trim().Length == 0)
                //{
                //    Mensaje_Proceso("Debe de describir el proceso de recuperación del producto tratado", Properties.Resources.Info_24px, null, true, TipoMessageBoxGPNET.Informacion);
                //    txtDescripTratamiento.Select();
                //    return false;
                //}



                if (txtIdArticuloTrata.Text.Trim().Length>0)
                {
                    if (txtCantProdTratado.Text.Equals("0") || txtCantProdTratado.Text.Trim().Length == 0)
                    {
                        Mensaje_Proceso("Debe de ingresar la cantidad tratado del articulo base", Properties.Resources.Info_24px, null, true, TipoMessageBoxGPNET.Informacion);
                        txtCantProdTratado.Select();
                        return false;
                    }
                }

                if (txtIdArticuloTrataFinal.Text.Trim().Length > 0)
                {
                    if (txtCantProdTratadoFinal.Text.Equals("0") || txtCantProdTratadoFinal.Text.Trim().Length == 0)
                    {
                        Mensaje_Proceso("Debe de ingresar la cantidad tratado del articulo final", Properties.Resources.Info_24px, null, true, TipoMessageBoxGPNET.Informacion);
                        txtCantProdTratadoFinal.Select();
                        return false;
                    }

                    //if (txtUndMendida.Text.Trim().Equals("KGR") && txtIdProducto.Text.Substring(0,1).Equals("B"))
                    //{
                    //    if (txtIdArticuloTrata.Text.Trim().Length == 0)
                    //    {
                    //        Mensaje_Proceso("Debe de ingresar el articulo base",
                    //       Properties.Resources.Info_24px, false, true, TipoMessageBoxGPNET.Informacion);

                    //        txtIdArticuloTrata.Select();

                    //        return false;
                    //    }
                    //}
                }


                if (rbRecuperación.Checked)
                {

                    if (txtIdArticuloTrataFinal.Text.Trim().Length == 0)
                    {
                        

                        Mensaje_Proceso("Debe de ingresar articulo final como resultado de la recuperación",
                            Properties.Resources.Info_24px, false, true, TipoMessageBoxGPNET.Informacion);

                        txtIdArticuloTrataFinal.Select();

                        return false;
                        
                    }



                    //if (txtIdProducto.Text.Equals(txtIdArticuloTrata.Text))
                    //{
                    //    Mensaje_Proceso("El articulo base no puede ser igual al articulo encontrado como producto no conforme",
                    //        Properties.Resources.Info_24px, false, true, TipoMessageBoxGPNET.Informacion);
                    //    txtIdArticuloTrata.Select();
                    //    return false;
                    //}


                    //if (txtIdProducto.Text.Equals(txtIdArticuloTrataFinal.Text))
                    //{
                    //    Mensaje_Proceso("El articulo final no puede ser igual al articulo encontrado como producto no conforme",
                    //        Properties.Resources.Info_24px, false, true, TipoMessageBoxGPNET.Informacion);
                    //    txtIdArticuloTrataFinal.Select();
                    //    return false;
                    //}

                    //if (txtIdArticuloTrata.Text.Equals(txtIdArticuloTrataFinal.Text))
                    //{
                    //    Mensaje_Proceso("El articulo base no puede ser igual al articulo final",
                    //        Properties.Resources.Info_24px, false, true, TipoMessageBoxGPNET.Informacion);
                    //    txtIdArticuloTrataFinal.Select();
                    //    return false;
                    //}


                }


                


            }



            

                

            //}


            return true;
        }

        public async void Grabar_Transaccion()
        {
            string sPathImagenProdNoConfor = "",
                sPathImagenProdNoConfor_2="",
                sPathImagenTratada = "";




            if (bNuevoRegistro)
            {
               sIdEstado = "11";
            }
            else
            {
                if (sIdEstado == "41")
                    sIdEstado = "42";                
            }



            //Copiado de archivos si no fuera el origen compartido

            if (txtPathImg1.Text.Trim().Length > 0)
            {
                //Imagen prod no conforme
                if (!GlobalIdentity.Instance.P_Plan_path_img_prod_no_confor.Equals(Path.GetDirectoryName(txtPathImg1.Text.Trim())))
                {


                    sPathImagenProdNoConfor = Path.Combine(GlobalIdentity.Instance.P_Plan_path_img_prod_no_confor, txtAnio.Text + "_" + txtId_Registro.Text + "_NC" +
                        Path.GetExtension(txtPathImg1.Text));

                    //Copiar el archivo con su tamaño original
                    // FileLibrary.CopyFile(txtPathImg1.Text, sPathImagenProdNoConfor,true);

                    //reduciendo su tamaño
                    Image img = Image.FromFile(txtPathImg1.Text);
                    Bitmap imgbitmap = new Bitmap(img);
                    Image resizedImage = Lib_Imagen.resizeImage_Op2(imgbitmap, 300, 300);

                    resizedImage.Save(sPathImagenProdNoConfor);



                }
                else
                    sPathImagenProdNoConfor = txtPathImg1.Text.Trim();
            }

            //Segunda imagen de producto no conforme
            if (txtPathImg1_2.Text.Trim().Length > 0)
            {
                //Imagen prod no conforme
                if (!GlobalIdentity.Instance.P_Plan_path_img_prod_no_confor.Equals(Path.GetDirectoryName(txtPathImg1_2.Text.Trim())))
                {


                    sPathImagenProdNoConfor_2 = Path.Combine(GlobalIdentity.Instance.P_Plan_path_img_prod_no_confor, txtAnio.Text + "_" + txtId_Registro.Text + "_2_NC" +
                        Path.GetExtension(txtPathImg1_2.Text));

                    //Copiar el archivo con su tamaño original
                    // FileLibrary.CopyFile(txtPathImg1.Text, sPathImagenProdNoConfor,true);

                    //reduciendo su tamaño
                    Image img = Image.FromFile(txtPathImg1_2.Text);
                    Bitmap imgbitmap = new Bitmap(img);
                    Image resizedImage = Lib_Imagen.resizeImage_Op2(imgbitmap, 300, 300);

                    resizedImage.Save(sPathImagenProdNoConfor_2);



                }
                else
                    sPathImagenProdNoConfor_2 = txtPathImg1_2.Text.Trim();
            }


            //Para la imagen Tratada
            if (txtPathImg2.Text.Trim().Length > 0)
            {
                if (!GlobalIdentity.Instance.P_Plan_path_img_prod_no_confor.Equals(Path.GetDirectoryName(txtPathImg2.Text.Trim())))
                {


                    sPathImagenTratada = Path.Combine(GlobalIdentity.Instance.P_Plan_path_img_prod_no_confor, txtAnio.Text + "_" + txtId_Registro.Text + "_T" +
                        Path.GetExtension(txtPathImg2.Text));

                    //FileLibrary.CopyFile(txtPathImg2.Text, sPathImagenTratada, true);

                    //reduciendo su tamaño
                    Image img = Image.FromFile(txtPathImg2.Text);
                    Bitmap imgbitmap = new Bitmap(img);
                    Image resizedImage = Lib_Imagen.resizeImage_Op2(imgbitmap, 300, 300);

                    resizedImage.Save(sPathImagenTratada);
                }
                else
                    sPathImagenTratada = txtPathImg2.Text.Trim();
            }


            List<SqlParameter> loSqlParameter;
            string sProcediminetoAlm;

            //Leer si la tabla compania_expand el tiempo y cuando fue la ultima recalculada de saldo
            sProcediminetoAlm = "sp_ing_upd_PRODUCTO_NO_CONFORME";


            bool bRecuperacion = false,
                bconcesion = false,
                bresiduo = false,
                bdevolucion = false,
                bOferta=false;

            if (sIdEstado.Equals("11"))
            {
                bRecuperacion = false;
                bconcesion = false;
                bresiduo = false;
                bdevolucion = false;
                bOferta = false;
            }
            else
            {
                bRecuperacion = rbRecuperación.Checked;
                bconcesion = rbConcesion.Checked;
                bresiduo = rbResiduo.Checked;
                bdevolucion = rbDevolucion.Checked;
                bOferta = rbOferta.Checked;
            }

            loSqlParameter = new List<SqlParameter>
            {
                new SqlParameter("@CIA",SqlDbType.Char,2){Value=GlobalIdentity.Instance.P_Sys_Default_Cia},
                new SqlParameter("@SEDE",SqlDbType.Char,2){Value=GlobalIdentity.Instance.P_Sys_Default_Sede},
                new SqlParameter("@ANIO",SqlDbType.Int){Value=Int32.Parse(txtAnio.Text.Trim())},
                new SqlParameter("@NRO_PRODUCTO",SqlDbType.Int){Value=Int32.Parse(txtId_Registro.Text.Trim())},
                new SqlParameter("@ID_AREA_ORIG",SqlDbType.Char,2){Value=cboAreaOrigen.SelectedValue},
                new SqlParameter("@ID_AREA_DETECT",SqlDbType.Char,2){Value=cboAreaDefecto.SelectedValue},
                new SqlParameter("@FECHA_REG",SqlDbType.DateTime){Value=dtpFechaReg.Value},
                new SqlParameter("@FLAG_I_RECEPCION",SqlDbType.Char,1){Value=rbRecepcion.Checked==true? "1":"0"},
                new SqlParameter("@FLAG_I_ALMACENAMIENTO",SqlDbType.Char,1){Value=rbAlmacenamiento.Checked==true? "1":"0"},
                new SqlParameter("@FLAG_I_PRODUCCION",SqlDbType.Char,1){Value=rbProduccion.Checked==true? "1":"0"},
                new SqlParameter("@FLAG_I_DISTRIBUCION",SqlDbType.Char,1){Value=rbDistribucion.Checked==true? "1":"0"},
                new SqlParameter("@FLAG_I_DEVOLUCIONES",SqlDbType.Char,1){Value=rbDevoluciones.Checked==true? "1":"0"},
                new SqlParameter("@FLAG_OTRO",SqlDbType.Char,1){Value=rbOtros.Checked==true? "1":"0"},
                new SqlParameter("@OTRO_I",SqlDbType.VarChar,100){Value=txtOtroProceso.Text.Trim()},
                new SqlParameter("@ID_ARTICULO",SqlDbType.VarChar,20){Value=txtIdProducto.Text.Trim()},
                new SqlParameter("@NRO_SERIE",SqlDbType.VarChar,60){Value=txtSerieProd.Text},
                new SqlParameter("@NRO_LOTE",SqlDbType.VarChar,60){Value=txtNroLoteProd.Text},
                new SqlParameter("@FECHA_VENCE_LOTE",SqlDbType.DateTime){Value=txtFchVenceLote.Text},
                new SqlParameter("@CANTIDAD",SqlDbType.Float){Value=float.Parse(txtCantidad.Text)},
                new SqlParameter("@ID_PROVEEDOR",SqlDbType.VarChar,20){Value=txtProveedor.Text},
                new SqlParameter("@OBS_NO_CONFORMIDAD",SqlDbType.VarChar,1000){Value=txtDescrpNoConformi.Text},
                new SqlParameter("@PATH_IMG_NO_CONFOR",SqlDbType.VarChar,100){Value=sPathImagenProdNoConfor },
                new SqlParameter("@PATH_IMG_NO_CONFOR_2",SqlDbType.VarChar,100){Value=sPathImagenProdNoConfor_2 },
                new SqlParameter("@FLAG_T_RECUPERACION",SqlDbType.Char,1){Value=bRecuperacion==true? "1":"0"},
                new SqlParameter("@FLAG_T_CONCESION",SqlDbType.Char,1){Value=bconcesion==true? "1":"0"},
                new SqlParameter("@FLAG_T_RESIDUO",SqlDbType.Char,1){Value=bresiduo==true? "1":"0"},
                new SqlParameter("@FLAG_T_DEVOLUCION",SqlDbType.Char,1){Value=bdevolucion==true? "1":"0" },
                new SqlParameter("@OBS_TRATAMIENTO",SqlDbType.VarChar,1000){Value=txtDescripTratamiento.Text},
                new SqlParameter("@PATH_IMG_TRATAMIENTO",SqlDbType.VarChar,100){Value=sPathImagenTratada},                
                new SqlParameter("@ID_ARTICULO_T",SqlDbType.VarChar,20){Value=txtIdArticuloTrata.Text},
                new SqlParameter("@UC",SqlDbType.VarChar,20){Value=GlobalIdentity.Instance.P_Sys_Default_Usuario},
                new SqlParameter("@ID_ESTADO",SqlDbType.VarChar,20){Value=sIdEstado},
                new SqlParameter("@Nuevo_Reg",SqlDbType.Bit){Value=bNuevoRegistro},
                new SqlParameter("@CANTIDAD_TRA",SqlDbType.Float){ Value=float.Parse(txtCantProdTratado.Text.Equals("")? "0": txtCantProdTratado.Text)},
                new SqlParameter("@ID_ARTICULO_FINAL",SqlDbType.VarChar,20){Value=txtIdArticuloTrataFinal.Text},
                new SqlParameter("@CANTIDAD_TRA_F",SqlDbType.Float){ Value=float.Parse(txtCantProdTratadoFinal.Text.Equals("")? "0": txtCantProdTratadoFinal.Text)},
                new SqlParameter("@TIPO_NOCONFORMIDAD",SqlDbType.VarChar,100){Value=cboTipoFalla.Text},
                new SqlParameter("@COD_LINEA_NEGOCIO",SqlDbType.Char,3){Value=cboLineaProd.SelectedValue},
                new SqlParameter("@FLAG_T_OFERTA",SqlDbType.Char,1){Value=bOferta==true? "1":"0"}
            };

            daDatabase odaDatabase = new daDatabase();
            beDatabaseResult obeDatabaseResult;
            //Obterner
            try
            {
                obeDatabaseResult = await odaDatabase.Execute_beDataAsync(sProcediminetoAlm, loSqlParameter);

                if (obeDatabaseResult.Exito)
                {
                    Uti_frm.MsjInformacion("Se grabó con éxito");
                    bNuevoRegistro = false;

                    
                }
                else
                    throw new Exception(obeDatabaseResult.Resultado);



                //Limpiar Registros
                On_Limpiar();


                //return obeDatabaseResult.Exito;
            }
            catch (Exception ex)
            {
                Uti_frm.MsjError(ex.Message);
                return;
            }
        }

        public  override  bool fDespuesDeGrabar()
        {

           

            Grabar_Transaccion();

            return true;
        }

        public override bool fAntesDeEliminar()
        {
            if (!bNuevoRegistro)
            {
                if (sIdEstado.Equals("12"))
                {
                    Mensaje_Proceso("El registro se encuentra anulado", Properties.Resources.Info_24px, null, true, TipoMessageBoxGPNET.Informacion);
                    return false;
                }

                //Revisar si el campo no esta vacio
                if (txtId_Registro.Text.Trim().Length == 0)
                {
                    Uti_frm.MsjError("El registro no debe de estar vacío");
                    txtId_Registro.Select();
                    return false;
                }



                if (MessageBox.Show("¿Esta seguro de anular el registro de Productos no Conforme?", "Convertidora del Pacifico", MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.No)
                {
                    return false;
                }
            }

            if (bNuevoRegistro)
                return false;




            return true;
        }

        public override bool fEliminar()
        {

            frmMotivoAnulacion frmRecha = new frmMotivoAnulacion();

            frmRecha.ShowDialog();

            if (frmRecha.EsAnulado)
            {
                sMotivoAnulacion = frmRecha.MotivoAnulacion;

                //Eliminar Registro
                Anular_Registro();
            }
            else
                return false;

            return true;
        }



        #endregion

        #region Metodos

        private void Anular_Registro()
        {

            //Parametros
            List<SqlParameter> loSqlParameter;
            string sProcediminetoAlm;


            //Leer si la tabla compania_expand el tiempo y cuando fue la ultima recalculada de saldo
            sProcediminetoAlm = "sp_ProducNoConforme_Anular";

            loSqlParameter = new List<SqlParameter>
            {
                new SqlParameter("@Cia",SqlDbType.Char,2){Value=GlobalIdentity.Instance.P_Sys_Default_Cia},
                new SqlParameter("@Sede",SqlDbType.Char,2){Value=GlobalIdentity.Instance.P_Sys_Default_Sede},
                new SqlParameter("@Anio",SqlDbType.Int){Value=Int32.Parse(txtAnio.Text)},
                new SqlParameter("@Nro_Producto",SqlDbType.Int){Value=txtId_Registro.Text},
                new SqlParameter("@UC",SqlDbType.VarChar,20){Value=GlobalIdentity.Instance.P_Sys_Default_Usuario},
                new SqlParameter("@Motivo",SqlDbType.VarChar,100){Value=sMotivoAnulacion}

            };


            daDatabase odaDatabase = new daDatabase();
            //Obterner
            try
            {
                beDatabaseResult obeDatabaseResult = odaDatabase.Execute_beData(sProcediminetoAlm, loSqlParameter);

                if (obeDatabaseResult.Exito)
                {
                    Uti_frm.MsjInformacion("Registro anulado exitosamente");
                    On_Limpiar();

                }
                else
                {
                    bNuevoRegistro = false;
                    throw new Exception(obeDatabaseResult.Resultado);
                }


            }
            catch (Exception ex)
            {
                Uti_frm.MsjError(ex.Message);
            }

        }
        private void Asignar_diseño_controles()
        {
            grbProcesos.VisualStyleManager = vsmVisualStyleManager1;
            grbIdentificacionProd.VisualStyleManager = vsmVisualStyleManager1;
            grbTrataMiento.VisualStyleManager = vsmVisualStyleManager1;
            grbUIGroupBox_GPNET4.VisualStyleManager = vsmVisualStyleManager1;

        }
        private void Obtener_El_Ultimo_Registro()
        {
            //Parametros
            List<SqlParameter> loSqlParameter;
            string sProcediminetoAlm;


            //Leer si la tabla compania_expand el tiempo y cuando fue la ultima recalculada de saldo
            sProcediminetoAlm = "sp_PRODUCTO_NO_CONFORMEUltReg";

            loSqlParameter = new List<SqlParameter>
            {
                new SqlParameter("@Cia",SqlDbType.Char,2){Value=GlobalIdentity.Instance.P_Sys_Default_Cia},
                new SqlParameter("@ANIO",SqlDbType.Int){ Value=int.Parse(txtAnio.Text)}
            };


            daDatabase odaDatabase = new daDatabase();
            //Obterner
            try
            {
                beDatabaseResult obeDatabaseResult = odaDatabase.GetUnicoValor(sProcediminetoAlm, loSqlParameter);

                if (obeDatabaseResult.Exito)
                {
                    bNuevoRegistro = true;
                    oValorLLaveBuscado = obeDatabaseResult.Data;
                    txtId_Registro.Text = oValorLLaveBuscado.ToString();

                }
                else
                {
                    bNuevoRegistro = false;
                    throw new Exception(obeDatabaseResult.Resultado);
                }


            }
            catch (Exception ex)
            {
                //Uti_frm.MsjError(ex.Message);

                Mensaje_Proceso(ex.Message,null);
            }
        }
        private void Cargar_Combo_Area()
        {
            try
            {
                string sProcedimientoAlm = "sp_Area_PersonalPNC";

                List<SqlParameter> losqlParameters;

                losqlParameters = new List<SqlParameter> { 
                    new SqlParameter("@CIA",SqlDbType.Char,2){ Value=GlobalIdentity.Instance.P_Sys_Default_Cia}
                };

                daDatabase odaDatabase = new daDatabase();

                beDatabaseResult obeDatabaseResult = odaDatabase.GetDataSet(sProcedimientoAlm, losqlParameters);

                if (obeDatabaseResult.Exito)
                {

                    VL_COMBO_AREA = ((DataSet)obeDatabaseResult.Data).Tables[0];

                    if (VL_COMBO_AREA != null)
                    {
                        if (VL_COMBO_AREA.Rows.Count > 1)
                        {
                            cboAreaDefecto.DataSource = VL_COMBO_AREA;
                            cboAreaDefecto.DisplayMember = "DESCRIPCION";
                            cboAreaDefecto.ValueMember = "ID_DPTO_CIA";
                        }
                    }


                    VL_COMBO_AREA_ORIG = VL_COMBO_AREA.Clone();


                    foreach (DataRow rw in VL_COMBO_AREA.Rows)
                    {
                        VL_COMBO_AREA_ORIG.ImportRow(rw);
                    }



                    if (VL_COMBO_AREA_ORIG != null)
                    {
                        if (VL_COMBO_AREA_ORIG.Rows.Count > 1)
                        {
                            cboAreaOrigen.DataSource = VL_COMBO_AREA_ORIG;
                            cboAreaOrigen.DisplayMember = "DESCRIPCION";
                            cboAreaOrigen.ValueMember = "ID_DPTO_CIA";
                        }
                    }

                }
                else
                {
                    if (VL_COMBO_AREA_ORIG != null)
                        VL_COMBO_AREA_ORIG.Rows.Clear();

                    if (VL_COMBO_AREA != null)
                        VL_COMBO_AREA.Rows.Clear();

                    Mensaje_Proceso(obeDatabaseResult.Resultado, Properties.Resources.Error, null, true, TipoMessageBoxGPNET.Error);
                }




            }
            catch(Exception ex)
            {
                Mensaje_Proceso(ex.Message, Properties.Resources.Error, null, true, TipoMessageBoxGPNET.Error);
            }
        }

        private void ConfigurarBusquedaIdArticulo()
        {
            //--------------Configurar el tipo de busqueda del Articulo------------------------------------

            //Llenando Campos Adicionales
            List<beCustomFile> olbeCustomFile = new List<beCustomFile>(){
                new beCustomFile(){NomCampo="ID_UNIDAD"},
                new beCustomFile(){NomCampo="FACTOR_KILO"},
                new beCustomFile(){NomCampo="CANT_ETIQUETA"},
                new beCustomFile(){NomCampo="NRO_SERIE"},
                new beCustomFile(){NomCampo="NRO_LOTE"},
                new beCustomFile(){NomCampo="fecha_vence_lote"},
                new beCustomFile(){NomCampo="Unid_Med" },
                new beCustomFile(){NomCampo="Tp_Abrev"},
                new beCustomFile(){NomCampo="ID_TIPO_CERTIFICACION"},
                new beCustomFile(){NomCampo="Largo_Art"},
                new beCustomFile(){NomCampo="NumHojas_Art"},
                new beCustomFile(){NomCampo="NumHojas_Arti"}
            };


            //Campos Adicionales
            StringBuilder sCamposAdic = new StringBuilder();
            sCamposAdic.AppendLine(", a.id_articulo + ' ' + a.descripcion as Articulo, v.NRO_SERIE,v.nro_lote, convert(varchar(30),v.fecha_vence_lote,103) fecha_vence_lote,");
            sCamposAdic.AppendLine("SUM(convert(decimal(13,2), isnull(v.cant_disponible,0))) as CANT_ETIQUETA,");
            sCamposAdic.AppendLine(" um.ABREVIATURA as Unid_Med,	isnull(v.ID_TIPO_CERTIFICACION,'') as ID_TIPO_CERTIFICACION, isnull(TCC.ABREVIATURA,'') as Tp_Abrev, ");
            sCamposAdic.AppendLine(" isnull(car4.ABREVIATURA,'') as Tipo_Art, isnull(car6.ABREVIATURA,'') as Tipo_Present, isnull(car2.ABREVIATURA,'') as Largo_Art, isnull(car8.descripcion,'') as NumHojas_Art, isnull(car8.ABREVIATURA,'') as NumHojas_Arti, a.ID_UNIDAD, ");
            sCamposAdic.AppendLine("(((convert(decimal(13,2), car8.abreviatura))*convert(decimal(13,2), car1.abreviatura)*convert(decimal(13,2), car3.abreviatura)*convert(decimal(13,2), car2.abreviatura))/10000000) as FACTOR_KILO");

            //From
            StringBuilder sFromSql = new StringBuilder();
            sFromSql.AppendLine(" from ARTICULO a with(nolock) ");

            //Inner Join
            StringBuilder sInnerJoin = new StringBuilder();            
            sInnerJoin.AppendLine("INNER JOIN (SELECT v.cia, v.sede, v.id_almacen, v.id_articulo, null as 'nro_serie',  ");
            sInnerJoin.AppendLine(" v.cant_disponible, 0 as 'nro_pieza', null as 'nro_lote', null as 'fecha_vence_lote', null as 'nro_movil',  ");
            sInnerJoin.AppendLine(" '0' as 'flag_pieza_cortada', a.car_art_06, ID_TIPO_CERTIFICACION=null ");
            sInnerJoin.AppendLine(" FROM existencia_almacen v with(nolock) ");
            sInnerJoin.AppendLine(" INNER JOIN articulo a ON a.cia=v.cia and a.id_articulo=v.id_articulo and isnull(a.flag_stock_x_serie,'0')='0' ");
            sInnerJoin.AppendLine(" INNER JOIN compania c ON c.cia=v.cia and isnull(c.flag_stock_x_lote, '0')='0' ");
            //sInnerJoin.AppendLine(" WHERE v.cia='" + GlobalIdentity.Instance.P_Sys_Default_Cia + "' and v.id_estado='01' and convert(decimal(13,2),isnull(v.cant_disponible,0))>0  and v.sede in ('" + GlobalIdentity.Instance.P_Sys_Default_Sede +"' ) and v.sede+v.id_almacen in ( '0101','0102','0108','0110','0114','0115','0117','0118' )");
            sInnerJoin.AppendLine(" WHERE v.cia='" + GlobalIdentity.Instance.P_Sys_Default_Cia + "' and v.id_estado='01' and v.sede in ('" + GlobalIdentity.Instance.P_Sys_Default_Sede + "' ) and v.sede+v.id_almacen in ( '0101','0102','0108','0110','0114','0115','0117','0118','0141' )");
            sInnerJoin.AppendLine(" UNION ALL ");
            sInnerJoin.AppendLine(" SELECT v.cia, v.sede, v.id_almacen, v.id_articulo, v.nro_serie, convert(decimal(13,2),isnull(v.cant_disponible,0)), 1 as 'nro_pieza', v.nro_lote, v.fecha_vence_lote, v.nro_movil,  ");
            sInnerJoin.AppendLine(" '0' , a.car_art_06 , v.ID_TIPO_CERTIFICACION ");
            sInnerJoin.AppendLine(" FROM existencia_serie v with(nolock) ");
            sInnerJoin.AppendLine(" INNER JOIN articulo a ON a.cia=v.cia and a.id_articulo=v.id_articulo and isnull(a.flag_stock_x_serie,'0')='1' ");
            //sInnerJoin.AppendLine(" WHERE v.cia='" + GlobalIdentity.Instance.P_Sys_Default_Cia + "' and v.id_estado='01' and convert(decimal(13,2),isnull(v.cant_disponible,0))>0 and v.sede in ('" + GlobalIdentity.Instance.P_Sys_Default_Sede + "' ) and v.sede+v.id_almacen in ( '0101','0102','0108','0110','0114','0115','0117','0118' )  ");
            sInnerJoin.AppendLine(" WHERE v.cia='" + GlobalIdentity.Instance.P_Sys_Default_Cia + "' and v.id_estado='01'  and v.sede in ('" + GlobalIdentity.Instance.P_Sys_Default_Sede + "' ) and v.sede+v.id_almacen in ( '0101','0102','0108','0110','0114','0115','0117','0118','0141' )  ");
            sInnerJoin.AppendLine(" UNION ALL  ");
            sInnerJoin.AppendLine(" SELECT v.cia, v.sede, v.id_almacen, v.id_articulo, null as 'nro_serie',  v.cant_disponible, 0 as 'nro_pieza', ");
            sInnerJoin.AppendLine(" v.nro_lote as 'nro_lote', v.fecha_vence_lote as 'fecha_vence_lote', null as 'nro_movil', ");
            sInnerJoin.AppendLine(" '0' as 'flag_pieza_cortada' , a.car_art_06 , ID_TIPO_CERTIFICACION=null ");
            sInnerJoin.AppendLine(" FROM existencia_lote v with(nolock) ");
            sInnerJoin.AppendLine(" INNER JOIN articulo a ON a.cia=v.cia and a.id_articulo=v.id_articulo and isnull(a.flag_stock_x_serie,'0')='0' ");
            sInnerJoin.AppendLine(" INNER JOIN compania c ON c.cia=v.cia and c.flag_stock_x_lote='1' ");
            //sInnerJoin.AppendLine(" WHERE v.cia='" + GlobalIdentity.Instance.P_Sys_Default_Cia + "' and v.id_estado='01' and convert(decimal(13,2),isnull(v.cant_disponible,0))>0 and v.sede in ('" + GlobalIdentity.Instance.P_Sys_Default_Sede + "') ");
            sInnerJoin.AppendLine(" WHERE v.cia='" + GlobalIdentity.Instance.P_Sys_Default_Cia + "' and v.id_estado='01' and  v.sede in ('" + GlobalIdentity.Instance.P_Sys_Default_Sede + "') ");
            sInnerJoin.AppendLine(" and v.sede+v.id_almacen in ('0101','0102','0108','0110','0114','0115','0117','0118','0141' ) ) v ON v.cia=a.cia and v.id_articulo=a.id_articulo ");
            sInnerJoin.AppendLine(" LEFT JOIN unidad_medida um ON um.cia=a.cia and um.id_unidad=a.id_unidad ");
            sInnerJoin.AppendLine(" LEFT JOIN DBO.caracteristica_articulo_codigo  car1 ON car1.cia=a.cia and car1.id_caracteristica_articulo='01' and car1.id_codigo=a.car_art_01 ");
            sInnerJoin.AppendLine(" LEFT JOIN DBO.caracteristica_articulo_codigo   car2 ON car2.cia=a.cia and car2.id_caracteristica_articulo='02' and car2.id_codigo=a.car_art_02 ");
            sInnerJoin.AppendLine(" LEFT JOIN DBO.caracteristica_articulo_codigo  car3 ON car3.cia=a.cia and car3.id_caracteristica_articulo='03' and car3.id_codigo=a.car_art_03 ");
            sInnerJoin.AppendLine(" LEFT JOIN DBO.caracteristica_articulo_codigo  car7 ON car7.cia=a.cia and car7.id_caracteristica_articulo='07' and car7.id_codigo=a.car_art_07 ");
            sInnerJoin.AppendLine(" LEFT JOIN DBO.caracteristica_articulo_codigo  car8 ON car8.cia=a.cia and car8.id_caracteristica_articulo='08' and car8.id_codigo=a.car_art_08 ");
            sInnerJoin.AppendLine(" LEFT JOIN DBO.caracteristica_articulo_codigo  car4 on car4.CIA=a.CIA and car4.ID_CARACTERISTICA_ARTICULO='04' and car4.ID_CODIGO=a.CAR_ART_04 ");
            sInnerJoin.AppendLine(" LEFT JOIN DBO.caracteristica_articulo_codigo  car6 on car6.CIA=a.CIA and car6.ID_CARACTERISTICA_ARTICULO='06' and car6.ID_CODIGO=a.CAR_ART_06 ");
            sInnerJoin.AppendLine(" LEFT JOIN TIPO_CERTIFICACION TCC ON TCC.CIA=a.CIA and TCC.ID_TIPO_CERTIFICACION=v.ID_TIPO_CERTIFICACION ");

            //Where
            StringBuilder sWhereSql = new StringBuilder();
            sWhereSql.AppendLine(" WHERE a.cia='" + GlobalIdentity.Instance.P_Sys_Default_Cia + "'   and a.id_estado='01'  ");

            //Group by 
            StringBuilder sGroupBySql = new StringBuilder();
            sGroupBySql.AppendLine(" GROUP BY v.cia, v.sede, v.id_almacen, a.ID_ARTICULO, a.id_unidad, a.id_estado, v.nro_serie, v.nro_lote, v.fecha_vence_lote, um.id_unidad, CAR2.ABREVIATURA, CAR3.ABREVIATURA, CAR1.ABREVIATURA, CAR8.ABREVIATURA, v.car_art_06, v.ID_ARTICULO, um.ABREVIATURA, v.ID_TIPO_CERTIFICACION, TCC.ABREVIATURA, a.DESCRIPCION, car4.ABREVIATURA, car6.ABREVIATURA , isnull(car8.descripcion,'')  ");

            //Having
            StringBuilder sHavingSql = new StringBuilder();
            //sHavingSql.AppendLine(" HAVING isnull(sum(convert(decimal(13,2),isnull(v.cant_disponible,0))),0)>0 ");
            sHavingSql.AppendLine(" ");
            //Order by
            StringBuilder sOrderBy = new StringBuilder();
            sOrderBy.AppendLine(" ORDER BY  v.ID_ARTICULO, v.nro_serie, v.nro_lote ");

            //Preparando las ventanas de Busqueda
            List<SqlParameter> lsqlParaArticulos;
            lsqlParaArticulos = new List<SqlParameter>()
            {
                new SqlParameter("@CampoCodigo",SqlDbType.VarChar,60){Value="ID_ARTICULO"},
                new SqlParameter("@CampoDescripcion",SqlDbType.VarChar,60){Value="DESCRIPCION"},
                new SqlParameter("@AliasTabla", SqlDbType.VarChar,8000){Value="a"},
                new SqlParameter("@BMostrarAnulados",SqlDbType.Bit){Value=1},
                new SqlParameter("@Cia",SqlDbType.Char,2){Value=GlobalIdentity.Instance.P_Sys_Default_Cia},
                new SqlParameter("@ValorBusqueda",SqlDbType.VarChar,100){Value=txtIdProducto.Text},
                new SqlParameter("@CamposAdicionales",SqlDbType.VarChar,8000){Value=sCamposAdic.ToString()},
                new SqlParameter("@From",SqlDbType.VarChar,8000){Value=sFromSql.ToString()},
                new SqlParameter("@InnerJoin",SqlDbType.VarChar,8000){Value=sInnerJoin.ToString()},
                new SqlParameter("@Where",SqlDbType.VarChar,8000){Value=sWhereSql.ToString()},
                new SqlParameter("@GroupBy",SqlDbType.VarChar,8000){Value=sGroupBySql.ToString()},
                new SqlParameter("@Having",SqlDbType.VarChar,8000){Value=sHavingSql.ToString()},
                new SqlParameter("@OrderBy",SqlDbType.VarChar,8000){Value=sOrderBy.ToString()},
                new SqlParameter("@CampoBusquedaAlterno",SqlDbType.VarChar,100){Value=""}
            };

            //string[] sCamposOcultos = { "NRO_SERIE_SC", "ID_ESTADO", "NRO_PARTE", "NRO_LOTE_SC", "ID_TIPO_CERTIFICACION", "CODIGO", "DESCRIPCION" };
            string[] sCamposOcultos = { "ID_ESTADO", "ID_TIPO_CERTIFICACION", "CODIGO", "DESCRIPCION" };
            //string[] sCamposPintado1 = { "CANT_DISPONIBLE", "CANT_ETIQUETA" };
            string[] sCamposPintado1 = { "CANT_ETIQUETA" };
            string[] sCamposAgrupar = { "Articulo" };

            txtIdProducto.ListaCamposAdicionales = olbeCustomFile;
            txtIdProducto.SeleccionMultipleBusq = false;
            txtIdProducto.Mostrar_Anulados = false;
            txtIdProducto.DialogoTipo = GPNETv4.Windows.Ctrl.Texbox.TipoDialogo.Busqueda_Datos_Tbl_Custom;
            txtIdProducto.ListaSQLParametros = lsqlParaArticulos;
            txtIdProducto.CamposOcultosdelaBusq = sCamposOcultos;
            txtIdProducto.CamposPintadoGrupo1 = sCamposPintado1;
            txtIdProducto.CamposColAgruparBq = sCamposAgrupar;
            txtIdProducto.ColorPrimerGrupo = Color.Yellow;
            txtIdProducto.MostrarSubtotalesBusquedaAvanzada = true;
            txtIdProducto.MostrarSubtotalesPiePagBA = InheritableBoolean.True;
            txtIdProducto.TituloVentanaBusq = "Busqueda de Articulo con Stock";
            txtIdProducto.SegExportarExcel = true;
            txtIdProducto.Z_Ejecutar_TipoDialogo = true;

        }

        private void ConfigurarBusquedaNroSerie()
        {
            //--------------Configurar el tipo de busqueda del Articulo------------------------------------

            //Llenando Campos Adicionales
            List<beCustomFile> olbeCustomFile = new List<beCustomFile>(){
                new beCustomFile(){NomCampo="ID_UNIDAD"},
                new beCustomFile(){NomCampo="FACTOR_KILO"},
                new beCustomFile(){NomCampo="CANT_ETIQUETA"},
                new beCustomFile(){NomCampo="NRO_SERIE"},
                new beCustomFile(){NomCampo="NRO_LOTE"},
                new beCustomFile(){NomCampo="fecha_vence_lote"},
                new beCustomFile(){NomCampo="Unid_Med" },
                new beCustomFile(){NomCampo="Tp_Abrev"},
                new beCustomFile(){NomCampo="ID_TIPO_CERTIFICACION"},
                new beCustomFile(){NomCampo="Largo_Art"},
                new beCustomFile(){NomCampo="NumHojas_Art"},
                new beCustomFile(){NomCampo="NumHojas_Arti"}
            };


            //Campos Adicionales
            StringBuilder sCamposAdic = new StringBuilder();
            sCamposAdic.AppendLine(", a.id_articulo + ' ' + a.descripcion as Articulo, v.NRO_SERIE,v.nro_lote, convert(varchar(30),v.fecha_vence_lote,103) fecha_vence_lote,	");
            //sCamposAdic.AppendLine(" a.NRO_PARTE, case when v.car_art_06='001' then SUM(convert(decimal(13,2),isnull(v.cant_disponible,0))) ");
            //sCamposAdic.AppendLine(" else  CASE WHEN um.id_unidad='04' then SUM(convert(decimal(13,2),isnull(v.cant_disponible,0)))/((convert(decimal(8,2),isnull(car2.ABREVIATURA,0))*convert(decimal(8,2),isnull(car3.ABREVIATURA,0))*convert(decimal(8,2),isnull(car1.ABREVIATURA,0))*convert(decimal(8,2),isnull(car8.ABREVIATURA,0)))/10000000) ");
            //sCamposAdic.AppendLine(" ELSE  SUM(convert(decimal(13,2),isnull(v.cant_disponible,0)))  END  ");
            sCamposAdic.AppendLine(" SUM(convert(decimal(13,2), isnull(v.cant_disponible,0))) as CANT_ETIQUETA,");
            sCamposAdic.AppendLine(" um.ABREVIATURA as Unid_Med,	isnull(v.ID_TIPO_CERTIFICACION,'') as ID_TIPO_CERTIFICACION, isnull(TCC.ABREVIATURA,'') as Tp_Abrev, ");
            sCamposAdic.AppendLine(" isnull(car4.ABREVIATURA,'') as Tipo_Art, isnull(car6.ABREVIATURA,'') as Tipo_Present, isnull(car2.ABREVIATURA,'') as Largo_Art, isnull(car8.descripcion,'') as NumHojas_Art, isnull(car8.ABREVIATURA,'') as NumHojas_Arti, a.ID_UNIDAD, ");
            sCamposAdic.AppendLine("(((convert(decimal(13,2), car8.abreviatura))*convert(decimal(13,2), car1.abreviatura)*convert(decimal(13,2), car3.abreviatura)*convert(decimal(13,2), car2.abreviatura))/10000000) as FACTOR_KILO");

            //From
            StringBuilder sFromSql = new StringBuilder();
            sFromSql.AppendLine(" from ARTICULO a with(nolock) ");

            //Inner Join
            StringBuilder sInnerJoin = new StringBuilder();
            sInnerJoin.AppendLine("INNER JOIN (SELECT v.cia, v.sede, v.id_almacen, v.id_articulo, null as 'nro_serie',  ");
            sInnerJoin.AppendLine(" v.cant_disponible, 0 as 'nro_pieza', null as 'nro_lote', null as 'fecha_vence_lote', null as 'nro_movil',  ");
            sInnerJoin.AppendLine(" '0' as 'flag_pieza_cortada', a.car_art_06, ID_TIPO_CERTIFICACION=null ");
            sInnerJoin.AppendLine(" FROM existencia_almacen v ");
            sInnerJoin.AppendLine(" INNER JOIN articulo a ON a.cia=v.cia and a.id_articulo=v.id_articulo and isnull(a.flag_stock_x_serie,'0')='0' ");
            sInnerJoin.AppendLine(" INNER JOIN compania c ON c.cia=v.cia and isnull(c.flag_stock_x_lote, '0')='0' ");
            sInnerJoin.AppendLine(" WHERE v.cia='" + GlobalIdentity.Instance.P_Sys_Default_Cia + "' and v.id_estado='01' and convert(decimal(13,2),isnull(v.cant_disponible,0))>0  and v.sede in ('" + GlobalIdentity.Instance.P_Sys_Default_Sede + "' ) and v.sede+v.id_almacen in ( '0101','0102','0108','0110','0114','0115','0117','0118','0141' )");
            sInnerJoin.AppendLine(" UNION ALL ");
            sInnerJoin.AppendLine(" SELECT v.cia, v.sede, v.id_almacen, v.id_articulo, v.nro_serie, convert(decimal(13,2),isnull(v.cant_disponible,0)), 1 as 'nro_pieza', v.nro_lote, v.fecha_vence_lote, v.nro_movil,  ");
            sInnerJoin.AppendLine(" '0' , a.car_art_06 , v.ID_TIPO_CERTIFICACION ");
            sInnerJoin.AppendLine(" FROM existencia_serie v ");
            sInnerJoin.AppendLine(" INNER JOIN articulo a ON a.cia=v.cia and a.id_articulo=v.id_articulo and isnull(a.flag_stock_x_serie,'0')='1' ");
            sInnerJoin.AppendLine(" WHERE v.cia='" + GlobalIdentity.Instance.P_Sys_Default_Cia + "' and v.id_estado='01' and convert(decimal(13,2),isnull(v.cant_disponible,0))>0 and v.sede in ('" + GlobalIdentity.Instance.P_Sys_Default_Sede + "' ) and v.sede+v.id_almacen in ( '0101','0102','0108','0110','0114','0115','0117','0118','0141' )  ");
            sInnerJoin.AppendLine(" UNION ALL  ");
            sInnerJoin.AppendLine(" SELECT v.cia, v.sede, v.id_almacen, v.id_articulo, null as 'nro_serie',  v.cant_disponible, 0 as 'nro_pieza', ");
            sInnerJoin.AppendLine(" v.nro_lote as 'nro_lote', v.fecha_vence_lote as 'fecha_vence_lote', null as 'nro_movil', ");
            sInnerJoin.AppendLine(" '0' as 'flag_pieza_cortada' , a.car_art_06 , ID_TIPO_CERTIFICACION=null ");
            sInnerJoin.AppendLine(" FROM existencia_lote v ");
            sInnerJoin.AppendLine(" INNER JOIN articulo a ON a.cia=v.cia and a.id_articulo=v.id_articulo and isnull(a.flag_stock_x_serie,'0')='0' ");
            sInnerJoin.AppendLine(" INNER JOIN compania c ON c.cia=v.cia and c.flag_stock_x_lote='1' ");
            sInnerJoin.AppendLine(" WHERE v.cia='" + GlobalIdentity.Instance.P_Sys_Default_Cia + "' and v.id_estado='01' and convert(decimal(13,2),isnull(v.cant_disponible,0))>0 and v.sede in ('" + GlobalIdentity.Instance.P_Sys_Default_Sede + "') ");
            sInnerJoin.AppendLine(" and v.sede+v.id_almacen in ('0101','0102','0108','0110','0114','0115','0117','0118','0141' ) ) v ON v.cia=a.cia and v.id_articulo=a.id_articulo ");
            sInnerJoin.AppendLine(" LEFT JOIN unidad_medida um ON um.cia=a.cia and um.id_unidad=a.id_unidad ");
            sInnerJoin.AppendLine(" LEFT JOIN DBO.caracteristica_articulo_codigo  car1 ON car1.cia=a.cia and car1.id_caracteristica_articulo='01' and car1.id_codigo=a.car_art_01 ");
            sInnerJoin.AppendLine(" LEFT JOIN DBO.caracteristica_articulo_codigo   car2 ON car2.cia=a.cia and car2.id_caracteristica_articulo='02' and car2.id_codigo=a.car_art_02 ");
            sInnerJoin.AppendLine(" LEFT JOIN DBO.caracteristica_articulo_codigo  car3 ON car3.cia=a.cia and car3.id_caracteristica_articulo='03' and car3.id_codigo=a.car_art_03 ");
            sInnerJoin.AppendLine(" LEFT JOIN DBO.caracteristica_articulo_codigo  car7 ON car7.cia=a.cia and car7.id_caracteristica_articulo='07' and car7.id_codigo=a.car_art_07 ");
            sInnerJoin.AppendLine(" LEFT JOIN DBO.caracteristica_articulo_codigo  car8 ON car8.cia=a.cia and car8.id_caracteristica_articulo='08' and car8.id_codigo=a.car_art_08 ");
            sInnerJoin.AppendLine(" LEFT JOIN DBO.caracteristica_articulo_codigo  car4 on car4.CIA=a.CIA and car4.ID_CARACTERISTICA_ARTICULO='04' and car4.ID_CODIGO=a.CAR_ART_04 ");
            sInnerJoin.AppendLine(" LEFT JOIN DBO.caracteristica_articulo_codigo  car6 on car6.CIA=a.CIA and car6.ID_CARACTERISTICA_ARTICULO='06' and car6.ID_CODIGO=a.CAR_ART_06 ");
            sInnerJoin.AppendLine(" LEFT JOIN TIPO_CERTIFICACION TCC ON TCC.CIA=a.CIA and TCC.ID_TIPO_CERTIFICACION=v.ID_TIPO_CERTIFICACION ");

            //Where
            StringBuilder sWhereSql = new StringBuilder();
            //sWhereSql.AppendLine(" WHERE a.cia='" + GlobalIdentity.Instance.P_Sys_Default_Cia + "'   and a.id_estado='01' and a.car_art_06 in ('002', '003') and um.ID_UNIDAD!='04' ");
            sWhereSql.AppendLine(" WHERE a.cia='" + GlobalIdentity.Instance.P_Sys_Default_Cia + "'   and a.id_estado='01'  ");

            //Group by 
            StringBuilder sGroupBySql = new StringBuilder();
            sGroupBySql.AppendLine(" GROUP BY  a.ID_ARTICULO, a.id_estado, v.nro_serie, v.nro_lote, v.fecha_vence_lote, a.id_unidad, um.id_unidad, CAR2.ABREVIATURA, CAR3.ABREVIATURA, CAR1.ABREVIATURA, CAR8.ABREVIATURA, v.car_art_06, v.ID_ARTICULO, um.ABREVIATURA, v.ID_TIPO_CERTIFICACION, TCC.ABREVIATURA, a.DESCRIPCION, car4.ABREVIATURA, car6.ABREVIATURA, isnull(car8.descripcion,'') ");

            //Having
            StringBuilder sHavingSql = new StringBuilder();
            //sHavingSql.AppendLine(" HAVING isnull(sum(convert(decimal(13,2),isnull(v.cant_disponible,0))),0)>0 ");
            sHavingSql.AppendLine(" ");
            //Order by
            StringBuilder sOrderBy = new StringBuilder();
            sOrderBy.AppendLine(" ORDER BY  v.ID_ARTICULO, v.nro_serie, v.nro_lote ");

            //Preparando las ventanas de Busqueda
            List<SqlParameter> lsqlParaArticulos;
            lsqlParaArticulos = new List<SqlParameter>()
            {
                new SqlParameter("@CampoCodigo",SqlDbType.VarChar,60){Value="ID_ARTICULO"},
                new SqlParameter("@CampoDescripcion",SqlDbType.VarChar,60){Value="DESCRIPCION"},
                new SqlParameter("@AliasTabla", SqlDbType.VarChar,8000){Value="a"},
                new SqlParameter("@BMostrarAnulados",SqlDbType.Bit){Value=1},
                new SqlParameter("@Cia",SqlDbType.Char,2){Value=GlobalIdentity.Instance.P_Sys_Default_Cia},
                new SqlParameter("@ValorBusqueda",SqlDbType.VarChar,100){Value=txtSerieProd.Text},
                new SqlParameter("@CamposAdicionales",SqlDbType.VarChar,8000){Value=sCamposAdic.ToString()},
                new SqlParameter("@From",SqlDbType.VarChar,8000){Value=sFromSql.ToString()},
                new SqlParameter("@InnerJoin",SqlDbType.VarChar,8000){Value=sInnerJoin.ToString()},
                new SqlParameter("@Where",SqlDbType.VarChar,8000){Value=sWhereSql.ToString()},
                new SqlParameter("@GroupBy",SqlDbType.VarChar,8000){Value=sGroupBySql.ToString()},
                new SqlParameter("@Having",SqlDbType.VarChar,8000){Value=sHavingSql.ToString()},
                new SqlParameter("@OrderBy",SqlDbType.VarChar,8000){Value=sOrderBy.ToString()},
                new SqlParameter("@CampoBusquedaAlterno",SqlDbType.VarChar,100){Value="v.nro_serie"}
            };

            string[] sCamposOcultos = { "ID_ESTADO", "ID_TIPO_CERTIFICACION", "CODIGO", "DESCRIPCION" };
            string[] sCamposPintado1 = { "CANT_ETIQUETA" };
            string[] sCamposAgrupar = { "Articulo" };

            txtSerieProd.ListaCamposAdicionales = olbeCustomFile;
            txtSerieProd.Mostrar_Anulados = false;
            txtSerieProd.DialogoTipo = GPNETv4.Windows.Ctrl.Texbox.TipoDialogo.Busqueda_Datos_Tbl_Custom;
            txtSerieProd.ListaSQLParametros = lsqlParaArticulos;
            txtSerieProd.CamposOcultosdelaBusq = sCamposOcultos;
            txtSerieProd.CamposPintadoGrupo1 = sCamposPintado1;
            txtSerieProd.CamposColAgruparBq = sCamposAgrupar;
            txtSerieProd.ColorPrimerGrupo = Color.Yellow;
            txtSerieProd.MostrarSubtotalesBusquedaAvanzada = true;
            txtSerieProd.MostrarSubtotalesPiePagBA = InheritableBoolean.True;
            txtSerieProd.TituloVentanaBusq = "Busqueda de Articulo con Stock por Serie";
            txtSerieProd.SegExportarExcel = true;
            txtSerieProd.Z_Ejecutar_TipoDialogo = true;

        }

        private void ConfigurarBusquedaNroLote()
        {
            //--------------Configurar el tipo de busqueda del Articulo------------------------------------

            //Llenando Campos Adicionales
            List<beCustomFile> olbeCustomFile = new List<beCustomFile>(){
                new beCustomFile(){NomCampo="ID_UNIDAD"},
                new beCustomFile(){NomCampo="FACTOR_KILO"},
                new beCustomFile(){NomCampo="CANT_ETIQUETA"},
                new beCustomFile(){NomCampo="NRO_SERIE"},
                new beCustomFile(){NomCampo="NRO_LOTE"},
                new beCustomFile(){NomCampo="fecha_vence_lote"},
                new beCustomFile(){NomCampo="Unid_Med" },
                new beCustomFile(){NomCampo="Tp_Abrev"},
                new beCustomFile(){NomCampo="ID_TIPO_CERTIFICACION"},
                new beCustomFile(){NomCampo="Largo_Art"},
                new beCustomFile(){NomCampo="NumHojas_Art"},
                new beCustomFile(){NomCampo="NumHojas_Arti"}
            };


            //Campos Adicionales
            StringBuilder sCamposAdic = new StringBuilder();
            //sCamposAdic.AppendLine(", a.id_articulo + ' ' + a.descripcion as Articulo, v.NRO_SERIE,v.nro_lote,	dbo._jeff_Primera_Frase(v.NRO_SERIE) as NRO_SERIE_SC, dbo._jeff_Primera_Frase(v.NRO_LOTE) as NRO_LOTE_SC,");
            sCamposAdic.AppendLine(", a.id_articulo + ' ' + a.descripcion as Articulo, v.NRO_SERIE,v.nro_lote, convert(varchar(30),v.fecha_vence_lote,103) fecha_vence_lote, ");
            //sCamposAdic.AppendLine(" a.NRO_PARTE, case when v.car_art_06='001' then SUM(convert(decimal(13,2),isnull(v.cant_disponible,0))) ");
            //sCamposAdic.AppendLine(" else  CASE WHEN um.id_unidad='04' then SUM(convert(decimal(13,2),isnull(v.cant_disponible,0)))/((convert(decimal(8,2),isnull(car2.ABREVIATURA,0))*convert(decimal(8,2),isnull(car3.ABREVIATURA,0))*convert(decimal(8,2),isnull(car1.ABREVIATURA,0))*convert(decimal(8,2),isnull(car8.ABREVIATURA,0)))/10000000) ");
            //sCamposAdic.AppendLine(" ELSE  SUM(convert(decimal(13,2),isnull(v.cant_disponible,0)))  END  ");
            //sCamposAdic.AppendLine("	end as CANT_DISPONIBLE,	SUM(convert(decimal(13,2), isnull(v.cant_disponible,0))) as CANT_ETIQUETA,");
            sCamposAdic.AppendLine(" SUM(convert(decimal(13,2), isnull(v.cant_disponible,0))) as CANT_ETIQUETA,");
            sCamposAdic.AppendLine(" um.ABREVIATURA as Unid_Med,	isnull(v.ID_TIPO_CERTIFICACION,'') as ID_TIPO_CERTIFICACION, isnull(TCC.ABREVIATURA,'') as Tp_Abrev, ");
            sCamposAdic.AppendLine(" isnull(car4.ABREVIATURA,'') as Tipo_Art, isnull(car6.ABREVIATURA,'') as Tipo_Present, isnull(car2.ABREVIATURA,'') as Largo_Art, isnull(car8.descripcion,'') as NumHojas_Art, isnull(car8.ABREVIATURA,'') as NumHojas_Arti , a.ID_UNIDAD, ");
            sCamposAdic.AppendLine("(((convert(decimal(13,2), car8.abreviatura))*convert(decimal(13,2), car1.abreviatura)*convert(decimal(13,2), car3.abreviatura)*convert(decimal(13,2), car2.abreviatura))/10000000) as FACTOR_KILO");

            //From
            StringBuilder sFromSql = new StringBuilder();
            sFromSql.AppendLine(" from ARTICULO a with(nolock) ");

            //Inner Join
            StringBuilder sInnerJoin = new StringBuilder();
            sInnerJoin.AppendLine("INNER JOIN (SELECT v.cia, v.sede, v.id_almacen, v.id_articulo, null as 'nro_serie',  ");
            sInnerJoin.AppendLine(" v.cant_disponible, 0 as 'nro_pieza', null as 'nro_lote', null as 'fecha_vence_lote', null as 'nro_movil',  ");
            sInnerJoin.AppendLine(" '0' as 'flag_pieza_cortada', a.car_art_06, ID_TIPO_CERTIFICACION=null ");
            sInnerJoin.AppendLine(" FROM existencia_almacen v ");
            sInnerJoin.AppendLine(" INNER JOIN articulo a ON a.cia=v.cia and a.id_articulo=v.id_articulo and isnull(a.flag_stock_x_serie,'0')='0' ");
            sInnerJoin.AppendLine(" INNER JOIN compania c ON c.cia=v.cia and isnull(c.flag_stock_x_lote, '0')='0' ");
            sInnerJoin.AppendLine(" WHERE v.cia='" + GlobalIdentity.Instance.P_Sys_Default_Cia + "' and v.id_estado='01' and convert(decimal(13,2),isnull(v.cant_disponible,0))>0  and v.sede in ('" + GlobalIdentity.Instance.P_Sys_Default_Sede + "' ) and v.sede+v.id_almacen in ( '0101','0102','0108','0110','0114','0115','0117','0118','0141' )");
            sInnerJoin.AppendLine(" UNION ALL ");
            sInnerJoin.AppendLine(" SELECT v.cia, v.sede, v.id_almacen, v.id_articulo, v.nro_serie, convert(decimal(13,2),isnull(v.cant_disponible,0)), 1 as 'nro_pieza', v.nro_lote, v.fecha_vence_lote, v.nro_movil,  ");
            sInnerJoin.AppendLine(" '0' , a.car_art_06 , v.ID_TIPO_CERTIFICACION ");
            sInnerJoin.AppendLine(" FROM existencia_serie v ");
            sInnerJoin.AppendLine(" INNER JOIN articulo a ON a.cia=v.cia and a.id_articulo=v.id_articulo and isnull(a.flag_stock_x_serie,'0')='1' ");
            sInnerJoin.AppendLine(" WHERE v.cia='" + GlobalIdentity.Instance.P_Sys_Default_Cia + "' and v.id_estado='01' and convert(decimal(13,2),isnull(v.cant_disponible,0))>0 and v.sede in ('" + GlobalIdentity.Instance.P_Sys_Default_Sede + "' ) and v.sede+v.id_almacen in ( '0101','0102','0108','0110','0114','0115','0117','0118','0141' )  ");
            sInnerJoin.AppendLine(" UNION ALL  ");
            sInnerJoin.AppendLine(" SELECT v.cia, v.sede, v.id_almacen, v.id_articulo, null as 'nro_serie',  v.cant_disponible, 0 as 'nro_pieza', ");
            sInnerJoin.AppendLine(" v.nro_lote as 'nro_lote', v.fecha_vence_lote as 'fecha_vence_lote', null as 'nro_movil', ");
            sInnerJoin.AppendLine(" '0' as 'flag_pieza_cortada' , a.car_art_06 , ID_TIPO_CERTIFICACION=null ");
            sInnerJoin.AppendLine(" FROM existencia_lote v ");
            sInnerJoin.AppendLine(" INNER JOIN articulo a ON a.cia=v.cia and a.id_articulo=v.id_articulo and isnull(a.flag_stock_x_serie,'0')='0' ");
            sInnerJoin.AppendLine(" INNER JOIN compania c ON c.cia=v.cia and c.flag_stock_x_lote='1' ");
            sInnerJoin.AppendLine(" WHERE v.cia='" + GlobalIdentity.Instance.P_Sys_Default_Cia + "' and v.id_estado='01' and convert(decimal(13,2),isnull(v.cant_disponible,0))>0 and v.sede in ('" + GlobalIdentity.Instance.P_Sys_Default_Sede + "') ");
            sInnerJoin.AppendLine(" and v.sede+v.id_almacen in ('0101','0102','0108','0110','0114','0115','0117','0118','0141' ) ) v ON v.cia=a.cia and v.id_articulo=a.id_articulo ");
            sInnerJoin.AppendLine(" LEFT JOIN unidad_medida um ON um.cia=a.cia and um.id_unidad=a.id_unidad ");
            sInnerJoin.AppendLine(" LEFT JOIN DBO.caracteristica_articulo_codigo  car1 ON car1.cia=a.cia and car1.id_caracteristica_articulo='01' and car1.id_codigo=a.car_art_01 ");
            sInnerJoin.AppendLine(" LEFT JOIN DBO.caracteristica_articulo_codigo   car2 ON car2.cia=a.cia and car2.id_caracteristica_articulo='02' and car2.id_codigo=a.car_art_02 ");
            sInnerJoin.AppendLine(" LEFT JOIN DBO.caracteristica_articulo_codigo  car3 ON car3.cia=a.cia and car3.id_caracteristica_articulo='03' and car3.id_codigo=a.car_art_03 ");
            sInnerJoin.AppendLine(" LEFT JOIN DBO.caracteristica_articulo_codigo  car7 ON car7.cia=a.cia and car7.id_caracteristica_articulo='07' and car7.id_codigo=a.car_art_07 ");
            sInnerJoin.AppendLine(" LEFT JOIN DBO.caracteristica_articulo_codigo  car8 ON car8.cia=a.cia and car8.id_caracteristica_articulo='08' and car8.id_codigo=a.car_art_08 ");
            sInnerJoin.AppendLine(" LEFT JOIN DBO.caracteristica_articulo_codigo  car4 on car4.CIA=a.CIA and car4.ID_CARACTERISTICA_ARTICULO='04' and car4.ID_CODIGO=a.CAR_ART_04 ");
            sInnerJoin.AppendLine(" LEFT JOIN DBO.caracteristica_articulo_codigo  car6 on car6.CIA=a.CIA and car6.ID_CARACTERISTICA_ARTICULO='06' and car6.ID_CODIGO=a.CAR_ART_06 ");
            sInnerJoin.AppendLine(" LEFT JOIN TIPO_CERTIFICACION TCC ON TCC.CIA=a.CIA and TCC.ID_TIPO_CERTIFICACION=v.ID_TIPO_CERTIFICACION ");

            //Where
            StringBuilder sWhereSql = new StringBuilder();
            //sWhereSql.AppendLine(" WHERE a.cia='" + GlobalIdentity.Instance.P_Sys_Default_Cia + "'   and a.id_estado='01' and a.car_art_06 in ('002', '003') and um.ID_UNIDAD!='04' ");
            sWhereSql.AppendLine(" WHERE a.cia='" + GlobalIdentity.Instance.P_Sys_Default_Cia + "'   and a.id_estado='01'  ");

            //Group by 
            StringBuilder sGroupBySql = new StringBuilder();
            sGroupBySql.AppendLine(" GROUP BY  a.ID_ARTICULO, a.id_estado, v.nro_serie, v.nro_lote, v.fecha_vence_lote,  a.id_unidad,um.id_unidad, CAR2.ABREVIATURA, CAR3.ABREVIATURA, CAR1.ABREVIATURA, CAR8.ABREVIATURA, v.car_art_06, v.ID_ARTICULO, um.ABREVIATURA, v.ID_TIPO_CERTIFICACION, TCC.ABREVIATURA, a.DESCRIPCION, car4.ABREVIATURA, car6.ABREVIATURA,  isnull(car8.descripcion,'') ");

            //Having
            StringBuilder sHavingSql = new StringBuilder();
            // sHavingSql.AppendLine(" HAVING isnull(sum(convert(decimal(13,2),isnull(v.cant_disponible,0))),0)>0 ");
            sHavingSql.AppendLine(" ");
            //Order by
            StringBuilder sOrderBy = new StringBuilder();
            sOrderBy.AppendLine(" ORDER BY  v.ID_ARTICULO, v.nro_serie, v.nro_lote ");

            //Preparando las ventanas de Busqueda
            List<SqlParameter> lsqlParaArticulos;
            lsqlParaArticulos = new List<SqlParameter>()
            {
                new SqlParameter("@CampoCodigo",SqlDbType.VarChar,60){Value="ID_ARTICULO"},
                new SqlParameter("@CampoDescripcion",SqlDbType.VarChar,60){Value="DESCRIPCION"},
                new SqlParameter("@AliasTabla", SqlDbType.VarChar,8000){Value="a"},
                new SqlParameter("@BMostrarAnulados",SqlDbType.Bit){Value=1},
                new SqlParameter("@Cia",SqlDbType.Char,2){Value=GlobalIdentity.Instance.P_Sys_Default_Cia},
                new SqlParameter("@ValorBusqueda",SqlDbType.VarChar,100){Value=txtNroLoteProd.Text},
                new SqlParameter("@CamposAdicionales",SqlDbType.VarChar,8000){Value=sCamposAdic.ToString()},
                new SqlParameter("@From",SqlDbType.VarChar,8000){Value=sFromSql.ToString()},
                new SqlParameter("@InnerJoin",SqlDbType.VarChar,8000){Value=sInnerJoin.ToString()},
                new SqlParameter("@Where",SqlDbType.VarChar,8000){Value=sWhereSql.ToString()},
                new SqlParameter("@GroupBy",SqlDbType.VarChar,8000){Value=sGroupBySql.ToString()},
                new SqlParameter("@Having",SqlDbType.VarChar,8000){Value=sHavingSql.ToString()},
                new SqlParameter("@OrderBy",SqlDbType.VarChar,8000){Value=sOrderBy.ToString()},
                new SqlParameter("@CampoBusquedaAlterno",SqlDbType.VarChar,100){Value="v.nro_lote"}
            };

            string[] sCamposOcultos = { "ID_ESTADO", "ID_TIPO_CERTIFICACION", "CODIGO", "DESCRIPCION" };
            string[] sCamposPintado1 = { "CANT_ETIQUETA" };
            string[] sCamposAgrupar = { "Articulo" };

            txtNroLoteProd.ListaCamposAdicionales = olbeCustomFile;
            txtNroLoteProd.Mostrar_Anulados = false;
            txtNroLoteProd.DialogoTipo = GPNETv4.Windows.Ctrl.Texbox.TipoDialogo.Busqueda_Datos_Tbl_Custom;
            txtNroLoteProd.ListaSQLParametros = lsqlParaArticulos;
            txtNroLoteProd.CamposOcultosdelaBusq = sCamposOcultos;
            txtNroLoteProd.CamposPintadoGrupo1 = sCamposPintado1;
            txtNroLoteProd.CamposColAgruparBq = sCamposAgrupar;
            txtNroLoteProd.ColorPrimerGrupo = Color.Yellow;
            txtNroLoteProd.MostrarSubtotalesBusquedaAvanzada = true;
            txtNroLoteProd.MostrarSubtotalesPiePagBA = InheritableBoolean.True;
            txtNroLoteProd.TituloVentanaBusq = "Busqueda de Articulo con Stock por Nro_lote";
            txtNroLoteProd.SegExportarExcel = true;
            txtNroLoteProd.Z_Ejecutar_TipoDialogo = true;

        }

        

        private void ConfigurarBusquedaIdArticuloTratado()
        {
            //--------------Configurar el tipo de busqueda del Articulo------------------------------------

            //Llenando Campos Adicionales
            //List<beCustomFile> olbeCustomFile = new List<beCustomFile>(){
            //    new beCustomFile(){NomCampo="ID_UNIDAD"},
            //    new beCustomFile(){NomCampo="FACTOR_KILO"},
            //    new beCustomFile(){NomCampo="CANT_ETIQUETA"},
            //    new beCustomFile(){NomCampo="NRO_SERIE"},
            //    new beCustomFile(){NomCampo="NRO_LOTE"},
            //    new beCustomFile(){NomCampo="Tp_Abrev"},
            //    new beCustomFile(){NomCampo="ID_TIPO_CERTIFICACION"},
            //    new beCustomFile(){NomCampo="Largo_Art"},
            //    new beCustomFile(){NomCampo="NumHojas_Art"},
            //    new beCustomFile(){NomCampo="NumHojas_Arti"}
            //};


            //Campos Adicionales
            StringBuilder sCamposAdic = new StringBuilder();
            sCamposAdic.AppendLine(", a.id_articulo + ' ' + a.descripcion as Articulo ");
            //From
            StringBuilder sFromSql = new StringBuilder();
            sFromSql.AppendLine(" from ARTICULO a with(nolock) ");

            //Inner Join
            StringBuilder sInnerJoin = new StringBuilder();
            sInnerJoin.Append("");


            //Where
            StringBuilder sWhereSql = new StringBuilder();
            sWhereSql.AppendLine(" WHERE a.cia='" + GlobalIdentity.Instance.P_Sys_Default_Cia + "'   and a.id_estado='01'  ");

            //Group by 
            StringBuilder sGroupBySql = new StringBuilder();
            sGroupBySql.AppendLine(" ");

            //Having
            StringBuilder sHavingSql = new StringBuilder();
            //sHavingSql.AppendLine(" HAVING isnull(sum(convert(decimal(13,2),isnull(v.cant_disponible,0))),0)>0 ");
            sHavingSql.AppendLine(" ");
            //Order by
            StringBuilder sOrderBy = new StringBuilder();
            sOrderBy.AppendLine(" ORDER BY  a.ID_ARTICULO");

            //Preparando las ventanas de Busqueda
            List<SqlParameter> lsqlParaArticulos;
            lsqlParaArticulos = new List<SqlParameter>()
            {
                new SqlParameter("@CampoCodigo",SqlDbType.VarChar,60){Value="ID_ARTICULO"},
                new SqlParameter("@CampoDescripcion",SqlDbType.VarChar,60){Value="DESCRIPCION"},
                new SqlParameter("@AliasTabla", SqlDbType.VarChar,8000){Value="a"},
                new SqlParameter("@BMostrarAnulados",SqlDbType.Bit){Value=1},
                new SqlParameter("@Cia",SqlDbType.Char,2){Value=GlobalIdentity.Instance.P_Sys_Default_Cia},
                new SqlParameter("@ValorBusqueda",SqlDbType.VarChar,100){Value=txtIdArticuloTrata.Text},
                new SqlParameter("@CamposAdicionales",SqlDbType.VarChar,8000){Value=sCamposAdic.ToString()},
                new SqlParameter("@From",SqlDbType.VarChar,8000){Value=sFromSql.ToString()},
                new SqlParameter("@InnerJoin",SqlDbType.VarChar,8000){Value=sInnerJoin.ToString()},
                new SqlParameter("@Where",SqlDbType.VarChar,8000){Value=sWhereSql.ToString()},
                new SqlParameter("@GroupBy",SqlDbType.VarChar,8000){Value=sGroupBySql.ToString()},
                new SqlParameter("@Having",SqlDbType.VarChar,8000){Value=sHavingSql.ToString()},
                new SqlParameter("@OrderBy",SqlDbType.VarChar,8000){Value=sOrderBy.ToString()},
                new SqlParameter("@CampoBusquedaAlterno",SqlDbType.VarChar,100){Value=""}
            };

            //string[] sCamposOcultos = { "NRO_SERIE_SC", "ID_ESTADO", "NRO_PARTE", "NRO_LOTE_SC", "ID_TIPO_CERTIFICACION", "CODIGO", "DESCRIPCION" };
            //string[] sCamposOcultos = { "ID_ESTADO", "ID_TIPO_CERTIFICACION", "CODIGO", "DESCRIPCION" };
            //string[] sCamposPintado1 = { "CANT_DISPONIBLE", "CANT_ETIQUETA" };
            //string[] sCamposPintado1 = { "CANT_ETIQUETA" };
            //string[] sCamposAgrupar = { "Articulo" };

            txtIdArticuloTrata.ListaCamposAdicionales = null;//olbeCustomFile;
            txtIdArticuloTrata.SeleccionMultipleBusq = false;
            txtIdArticuloTrata.Mostrar_Anulados = false;
            txtIdArticuloTrata.DialogoTipo = GPNETv4.Windows.Ctrl.Texbox.TipoDialogo.Busqueda_Datos_Tbl_Custom;
            txtIdArticuloTrata.ListaSQLParametros = lsqlParaArticulos;
            txtIdArticuloTrata.CamposOcultosdelaBusq = null;//sCamposOcultos;
            txtIdArticuloTrata.CamposPintadoGrupo1 = null;//sCamposPintado1;
            txtIdArticuloTrata.CamposColAgruparBq = null;//sCamposAgrupar;
            txtIdArticuloTrata.ColorPrimerGrupo = Color.Yellow;
            txtIdArticuloTrata.MostrarSubtotalesBusquedaAvanzada = false;
            txtIdArticuloTrata.MostrarSubtotalesPiePagBA = InheritableBoolean.False;
            txtIdArticuloTrata.TituloVentanaBusq = "Busqueda de Articulo";
            txtIdArticuloTrata.SegExportarExcel = true;
            txtIdArticuloTrata.Z_Ejecutar_TipoDialogo = true;

        }

        private void ConfigurarBusquedaIdArticuloTratadoFinal()
        {
            //--------------Configurar el tipo de busqueda del Articulo------------------------------------

            //Llenando Campos Adicionales
            //List<beCustomFile> olbeCustomFile = new List<beCustomFile>(){
            //    new beCustomFile(){NomCampo="ID_UNIDAD"},
            //    new beCustomFile(){NomCampo="FACTOR_KILO"},
            //    new beCustomFile(){NomCampo="CANT_ETIQUETA"},
            //    new beCustomFile(){NomCampo="NRO_SERIE"},
            //    new beCustomFile(){NomCampo="NRO_LOTE"},
            //    new beCustomFile(){NomCampo="Tp_Abrev"},
            //    new beCustomFile(){NomCampo="ID_TIPO_CERTIFICACION"},
            //    new beCustomFile(){NomCampo="Largo_Art"},
            //    new beCustomFile(){NomCampo="NumHojas_Art"},
            //    new beCustomFile(){NomCampo="NumHojas_Arti"}
            //};


            //Campos Adicionales
            StringBuilder sCamposAdic = new StringBuilder();
            sCamposAdic.AppendLine(", a.id_articulo + ' ' + a.descripcion as Articulo ");            
            //From
            StringBuilder sFromSql = new StringBuilder();
            sFromSql.AppendLine(" from ARTICULO a with(nolock) ");

            //Inner Join
            StringBuilder sInnerJoin = new StringBuilder();
            sInnerJoin.Append("");
            

            //Where
            StringBuilder sWhereSql = new StringBuilder();
            sWhereSql.AppendLine(" WHERE a.cia='" + GlobalIdentity.Instance.P_Sys_Default_Cia + "'   and a.id_estado='01'  ");

            //Group by 
            StringBuilder sGroupBySql = new StringBuilder();
            sGroupBySql.AppendLine(" ");

            //Having
            StringBuilder sHavingSql = new StringBuilder();
            //sHavingSql.AppendLine(" HAVING isnull(sum(convert(decimal(13,2),isnull(v.cant_disponible,0))),0)>0 ");
            sHavingSql.AppendLine(" ");
            //Order by
            StringBuilder sOrderBy = new StringBuilder();
            sOrderBy.AppendLine(" ORDER BY  a.ID_ARTICULO");

            //Preparando las ventanas de Busqueda
            List<SqlParameter> lsqlParaArticulos;
            lsqlParaArticulos = new List<SqlParameter>()
            {
                new SqlParameter("@CampoCodigo",SqlDbType.VarChar,60){Value="ID_ARTICULO"},
                new SqlParameter("@CampoDescripcion",SqlDbType.VarChar,60){Value="DESCRIPCION"},
                new SqlParameter("@AliasTabla", SqlDbType.VarChar,8000){Value="a"},
                new SqlParameter("@BMostrarAnulados",SqlDbType.Bit){Value=1},
                new SqlParameter("@Cia",SqlDbType.Char,2){Value=GlobalIdentity.Instance.P_Sys_Default_Cia},
                new SqlParameter("@ValorBusqueda",SqlDbType.VarChar,100){Value=txtIdArticuloTrata.Text},
                new SqlParameter("@CamposAdicionales",SqlDbType.VarChar,8000){Value=sCamposAdic.ToString()},
                new SqlParameter("@From",SqlDbType.VarChar,8000){Value=sFromSql.ToString()},
                new SqlParameter("@InnerJoin",SqlDbType.VarChar,8000){Value=sInnerJoin.ToString()},
                new SqlParameter("@Where",SqlDbType.VarChar,8000){Value=sWhereSql.ToString()},
                new SqlParameter("@GroupBy",SqlDbType.VarChar,8000){Value=sGroupBySql.ToString()},
                new SqlParameter("@Having",SqlDbType.VarChar,8000){Value=sHavingSql.ToString()},
                new SqlParameter("@OrderBy",SqlDbType.VarChar,8000){Value=sOrderBy.ToString()},
                new SqlParameter("@CampoBusquedaAlterno",SqlDbType.VarChar,100){Value=""}
            };

            //string[] sCamposOcultos = { "NRO_SERIE_SC", "ID_ESTADO", "NRO_PARTE", "NRO_LOTE_SC", "ID_TIPO_CERTIFICACION", "CODIGO", "DESCRIPCION" };
            //string[] sCamposOcultos = { "ID_ESTADO", "ID_TIPO_CERTIFICACION", "CODIGO", "DESCRIPCION" };
            //string[] sCamposPintado1 = { "CANT_DISPONIBLE", "CANT_ETIQUETA" };
            //string[] sCamposPintado1 = { "CANT_ETIQUETA" };
            //string[] sCamposAgrupar = { "Articulo" };

            txtIdArticuloTrataFinal.ListaCamposAdicionales = null;//olbeCustomFile;
            txtIdArticuloTrataFinal.SeleccionMultipleBusq = false;
            txtIdArticuloTrataFinal.Mostrar_Anulados = false;
            txtIdArticuloTrataFinal.DialogoTipo = GPNETv4.Windows.Ctrl.Texbox.TipoDialogo.Busqueda_Datos_Tbl_Custom;
            txtIdArticuloTrataFinal.ListaSQLParametros = lsqlParaArticulos;
            txtIdArticuloTrataFinal.CamposOcultosdelaBusq = null;//sCamposOcultos;
            txtIdArticuloTrataFinal.CamposPintadoGrupo1 = null;//sCamposPintado1;
            txtIdArticuloTrataFinal.CamposColAgruparBq = null;//sCamposAgrupar;
            txtIdArticuloTrataFinal.ColorPrimerGrupo = Color.Yellow;
            txtIdArticuloTrataFinal.MostrarSubtotalesBusquedaAvanzada = false;
            txtIdArticuloTrataFinal.MostrarSubtotalesPiePagBA = InheritableBoolean.False;
            txtIdArticuloTrataFinal.TituloVentanaBusq = "Busqueda de Articulo";
            txtIdArticuloTrataFinal.SegExportarExcel = true;
            txtIdArticuloTrataFinal.Z_Ejecutar_TipoDialogo = true;

        }

        private void ConfigurarBusquedaProveedores()
        {

            //From
            StringBuilder sFromSql = new StringBuilder();
            sFromSql.AppendLine(" from ANALITICA a with(nolock) ");

            //Inner Join
            StringBuilder sInnerJoin = new StringBuilder();
            sInnerJoin.AppendLine("inner join ANALITICA_TIPO at on at.CIA=a.CIA and at.ID_ANALITICA=a.ID_ANALITICA and at.ID_TIPO_ANALITICA='02'  ");

            //Where
            StringBuilder sWhereSql = new StringBuilder();
            sWhereSql.AppendLine(" WHERE a.cia='" + GlobalIdentity.Instance.P_Sys_Default_Cia + "'   and a.id_estado='01' ");

            //Group by 

            StringBuilder sGroupBySql = new StringBuilder();
            sGroupBySql.AppendLine(" ");


            //Having
            StringBuilder sHavingSql = new StringBuilder();
            //sHavingSql.AppendLine(" HAVING isnull(sum(convert(decimal(13,2),isnull(v.cant_disponible,0))),0)>0 ");
            sHavingSql.AppendLine(" ");
            //Order by
            StringBuilder sOrderBy = new StringBuilder();
            sOrderBy.AppendLine(" ORDER BY  a.descripcion ");

            //Preparando las ventanas de Busqueda
            List<SqlParameter> lsqlParaArticulos;
            lsqlParaArticulos = new List<SqlParameter>()
            {
                new SqlParameter("@CampoCodigo",SqlDbType.VarChar,60){Value="ID_ANALITICA"},
                new SqlParameter("@CampoDescripcion",SqlDbType.VarChar,60){Value="DESCRIPCION"},
                new SqlParameter("@AliasTabla", SqlDbType.VarChar,8000){Value="a"},
                new SqlParameter("@BMostrarAnulados",SqlDbType.Bit){Value=1},
                new SqlParameter("@Cia",SqlDbType.Char,2){Value=GlobalIdentity.Instance.P_Sys_Default_Cia},
                new SqlParameter("@ValorBusqueda",SqlDbType.VarChar,100){Value=txtProveedor.Text},
                new SqlParameter("@CamposAdicionales",SqlDbType.VarChar,8000){Value=""},
                new SqlParameter("@From",SqlDbType.VarChar,8000){Value=sFromSql.ToString()},
                new SqlParameter("@InnerJoin",SqlDbType.VarChar,8000){Value=sInnerJoin.ToString()},
                new SqlParameter("@Where",SqlDbType.VarChar,8000){Value=sWhereSql.ToString()},
                new SqlParameter("@GroupBy",SqlDbType.VarChar,8000){Value=sGroupBySql.ToString()},
                new SqlParameter("@Having",SqlDbType.VarChar,8000){Value=sHavingSql.ToString()},
                new SqlParameter("@OrderBy",SqlDbType.VarChar,8000){Value=sOrderBy.ToString()},
                new SqlParameter("@CampoBusquedaAlterno",SqlDbType.VarChar,100){Value=""}
            };

            //string[] sCamposOcultos = { "NRO_SERIE_SC", "ID_ESTADO", "NRO_PARTE", "NRO_LOTE_SC", "ID_TIPO_CERTIFICACION", "CODIGO", "DESCRIPCION" };
            //string[] sCamposOcultos = { "ID_ESTADO", "ID_TIPO_CERTIFICACION", "CODIGO", "DESCRIPCION" };
            //string[] sCamposPintado1 = { "CANT_DISPONIBLE", "CANT_ETIQUETA" };
            //string[] sCamposPintado1 = { "CANT_ETIQUETA" };
            //string[] sCamposAgrupar = { "Articulo" };

            //txtCodCliente.ListaCamposAdicionales = olbeCustomFile;
            txtProveedor.SeleccionMultipleBusq = false;
            txtProveedor.Mostrar_Anulados = false;
            txtProveedor.DialogoTipo = GPNETv4.Windows.Ctrl.Texbox.TipoDialogo.Busqueda_Datos_Tbl_Custom;
            txtProveedor.ListaSQLParametros = lsqlParaArticulos;
            //txtCodCliente.CamposOcultosdelaBusq = sCamposOcultos;
            //txtCodCliente.CamposPintadoGrupo1 = sCamposPintado1;
            //txtCodCliente.CamposColAgruparBq = sCamposAgrupar;
            //txtCodCliente.ColorPrimerGrupo = Color.Yellow;
            txtProveedor.MostrarSubtotalesBusquedaAvanzada = true;
            txtProveedor.MostrarSubtotalesPiePagBA = InheritableBoolean.False;
            txtProveedor.TituloVentanaBusq = "Busqueda de Proveedores";
            txtProveedor.SegExportarExcel = true;
            txtProveedor.Z_Ejecutar_TipoDialogo = true;



        }

        private void MostrarDatosDespuesdeLeaveID()
        {
            if (txtId_Registro.Text.Trim().Length > 0)
            {
                if (bDelaVentanaBusqueda)
                {
                    bDelaVentanaBusqueda = false;
                    if (nValoId_Reg != Int32.Parse(txtId_Registro.Text))
                        if (!Mostrar_Resultado_Busqueda())
                        {
                            //GPNET.Sistema.Util.Frm.Uti_frm.MsjAdvertencia("No existen datos");
                            Mensaje_Proceso("No existen datos",null,null,true,TipoMessageBoxGPNET.Informacion);
                            On_Limpiar();
                        }
                }
                else
                {
                    if (bNuevoRegistro== false){

                        if (!Mostrar_Resultado_Busqueda())
                        {
                            //GPNET.Sistema.Util.Frm.Uti_frm.MsjAdvertencia("No existen datos");
                            Mensaje_Proceso("No existen datos", null, null, true, TipoMessageBoxGPNET.Informacion);
                            On_Limpiar();
                        }
                    }
                }
            }
            else
            {
                //GPNET.Sistema.Util.Frm.Uti_frm.MsjAdvertencia("Debe de ingresar un código de papeleta");
                Mensaje_Proceso("Debe de ingresar un código de registro", null, null, true, TipoMessageBoxGPNET.Informacion);
                txtId_Registro.Select();
            }


            ConfigurarBusquedaRefKardex();

        }

       

        private bool Mostrar_Resultado_Busqueda()
        {

            bool bExistenDatos = false;

            //Parametros
            List<SqlParameter> loSqlParameter;
            string sProcediminetoAlm;


            //Leer si la tabla compania_expand el tiempo y cuando fue la ultima recalculada de saldo
            sProcediminetoAlm = "sp_PRODUCTO_NO_CONFORME_x_cod";

            loSqlParameter = new List<SqlParameter>
            {
                new SqlParameter("@cia",SqlDbType.Char,2){Value=GlobalIdentity.Instance.P_Sys_Default_Cia},
                new SqlParameter("@sede",SqlDbType.Char,2){Value=GlobalIdentity.Instance.P_Sys_Default_Sede},
                new SqlParameter("@anio",SqlDbType.Int){Value=Int32.Parse(txtAnio.Text)},
                new SqlParameter("@nro_producto",SqlDbType.Int){Value=Int32.Parse(txtId_Registro.Text)}

            };

            daDatabase odaDatabase = new daDatabase();
            //Obterner
            try
            {
                beDatabaseResult obeDatabaseResult = odaDatabase.GetDataSet(sProcediminetoAlm, loSqlParameter);

                if (obeDatabaseResult.Exito)
                {
                    DataTable oDataTable = ((DataSet)obeDatabaseResult.Data).Tables[0];

                    if (oDataTable != null)
                    {
                        if (oDataTable.Rows.Count > 0)
                        {
                            bExistenDatos = true;

                            cboLineaProd.SelectedValue = oDataTable.Rows[0]["COD_LINEA_NEGOCIO"].ToString();
                            cboAreaOrigen.SelectedValue = oDataTable.Rows[0]["ID_AREA_ORIGEN"].ToString();
                            cboAreaDefecto.SelectedValue= oDataTable.Rows[0]["ID_AREA_DETECT"].ToString();
                            dtpFechaReg.Value = (DateTime)oDataTable.Rows[0]["FECHA_REG"];
                            rbRecepcion.Checked= (oDataTable.Rows[0]["FLAG_I_RECEPCION"].ToString() == "1");
                            rbAlmacenamiento.Checked= (oDataTable.Rows[0]["FLAG_I_ALMACENAMIENTO"].ToString() == "1");
                            rbProduccion.Checked= (oDataTable.Rows[0]["FLAG_I_PRODUCCION"].ToString() == "1");
                            rbDistribucion.Checked= (oDataTable.Rows[0]["FLAG_I_DISTRIBUCION"].ToString() == "1");
                            rbDevoluciones.Checked = (oDataTable.Rows[0]["FLAG_I_DEVOLUCIONES"].ToString() == "1");
                            rbOtros.Checked = (oDataTable.Rows[0]["FLAG_OTRO"].ToString() == "1");
                            txtOtroProceso.Text = oDataTable.Rows[0]["OTRO_I"].ToString();
                            txtIdProducto.Text = oDataTable.Rows[0]["ID_ARTICULO"].ToString();
                            txtDescripProducto.Text = oDataTable.Rows[0]["Articulo"].ToString();
                            txtSerieProd.Text = oDataTable.Rows[0]["NRO_SERIE"].ToString();
                            txtNroLoteProd.Text = oDataTable.Rows[0]["NRO_LOTE"].ToString();
                            txtFchVenceLote.Text = oDataTable.Rows[0]["FECHA_VENCE_LOTE"].ToString();
                            txtCantidad.Text = oDataTable.Rows[0]["CANTIDAD"].ToString();

                            txtProveedor.Text = oDataTable.Rows[0]["ID_PROVEEDOR"].ToString();
                            txtProveedorDescrip.Text = oDataTable.Rows[0]["PROVEEDOR"].ToString();
                            txtDescrpNoConformi.Text = oDataTable.Rows[0]["OBS_NO_CONFORMIDAD"].ToString();
                            txtPathImg1.Text = oDataTable.Rows[0]["PATH_IMG_NO_CONFOR"].ToString();
                            txtPathImg1_2.Text=oDataTable.Rows[0]["PATH_IMG_NO_CONFOR_2"].ToString();

                            if (txtPathImg1.Text.Trim().Equals(""))
                            {
                                btnImgPrdNoConf.Image = Properties.Resources.camara_79px_vacio;
                                btnDelet1.Visible = false;
                            }
                            else
                            {
                                btnImgPrdNoConf.Image = Properties.Resources.camara_79px_ing;
                                btnDelet1.Visible = true;
                            }

                            if (txtPathImg1_2.Text.Trim().Equals(""))
                            {
                                btnImgPrdNoConf2.Image = Properties.Resources.camara_79px_vacio;
                                btnDelet1_2.Visible = false;
                            }
                            else
                            {
                                btnImgPrdNoConf2.Image = Properties.Resources.camara_79px_ing;
                                btnDelet1_2.Visible = true;
                            }

                            rbRecuperación.Checked = (oDataTable.Rows[0]["FLAG_T_RECUPERACION"].ToString() == "1");
                            rbConcesion.Checked = (oDataTable.Rows[0]["FLAG_T_CONCESION"].ToString() == "1");
                            rbResiduo.Checked = (oDataTable.Rows[0]["FLAG_T_RESIDUO"].ToString() == "1");
                            rbDevolucion.Checked = (oDataTable.Rows[0]["FLAG_T_DEVOLUCION"].ToString() == "1");
                            txtDescripTratamiento.Text = oDataTable.Rows[0]["OBS_TRATAMIENTO"].ToString();
                            txtPathImg2.Text = oDataTable.Rows[0]["PATH_IMG_TRATAMIENTO"].ToString();

                            if (txtPathImg2.Text.Trim().Equals(""))
                            {
                                btnImgProdTratado.Image = Properties.Resources.camara_79px_vacio;
                                btnDelete2.Visible = false;
                            }
                            else
                            {
                                btnImgProdTratado.Image = Properties.Resources.camara_79px_ing;
                                btnDelete2.Visible = true;
                            }

                            txtIdArticuloTrata.Text = oDataTable.Rows[0]["ID_ARTICULO_T"].ToString();
                            txtDescripTrata.Text = oDataTable.Rows[0]["ARTICULO_T"].ToString();
                            txtCantProdTratado.Text = oDataTable.Rows[0]["CANTIDAD_TRA"].ToString();
                            txtIdArticuloTrataFinal.Text= oDataTable.Rows[0]["ID_ARTICULO_FINAL"].ToString();
                            txtDescripTrataFinal.Text= oDataTable.Rows[0]["ARTICULO_T2"].ToString();
                            txtCantProdTratadoFinal.Text= oDataTable.Rows[0]["CANTIDAD_TRA_F"].ToString();


                            txtEstado.Text = oDataTable.Rows[0]["ESTADO"].ToString();
                            sIdEstado= oDataTable.Rows[0]["ID_ESTADO"].ToString();
                            txtUndMendida.Text= oDataTable.Rows[0]["UM"].ToString();
                            cboTipoFalla.Text = oDataTable.Rows[0]["TIPO_NOCONFORMIDAD"].ToString();
                            rbOferta.Checked = (oDataTable.Rows[0]["FLAG_T_OFERTA"].ToString() == "1");
                            txtRefKardex.Text = oDataTable.Rows[0]["NRO_DOC_REF"].ToString();




                            //Accesos segun estado consultado
                            Accesso_Segun_Estado(sIdEstado);


                        }
                        else
                            bExistenDatos = false;

                    }
                    else
                        bExistenDatos = false;

                }
                else
                {

                    throw new Exception(obeDatabaseResult.Resultado);

                }

                return bExistenDatos;
            }
            catch (Exception ex)
            {

                //GPNET.Sistema.Util.Frm.Uti_frm.MsjError(ex.Message);
                Mensaje_Proceso(ex.Message, Properties.Resources.Error, null, true, TipoMessageBoxGPNET.Error);
                return bExistenDatos;
            }


        }

        private void Accesso_Segun_Estado(string vEstado)
        {
            if (vEstado.Equals("11"))
            {
                grbProcesos.Enabled = true;
                cboAreaDefecto.Enabled = true;
                cboAreaOrigen.Enabled = true;
               // cboLineaProd.Enabled = true;
                cboTipoFalla.Enabled = true;
                //grbIdentificacionProd.Enabled = true;
                Controles_Identificacion_Prod(true);
                dtpFechaReg.Enabled = true;
                grbTrataMiento.Enabled = false;
                //controles_tratamiento(false);
            }
            else
            {
                if (vEstado.Equals("41"))
                {
                    cboAreaDefecto.Enabled = false;
                    cboAreaOrigen.Enabled = false;
                    //cboLineaProd.Enabled = false;
                    cboTipoFalla.Enabled = false;
                    grbProcesos.Enabled = false;
                    //grbIdentificacionProd.Enabled = false;
                    Controles_Identificacion_Prod(false);
                    dtpFechaReg.Enabled = false;
                    //grbTrataMiento.Enabled = true;
                    controles_tratamiento(true);
                }
                else
                {
                    if (vEstado.Equals("42"))
                    {
                        cboAreaDefecto.Enabled = false;
                        cboAreaOrigen.Enabled = false;
                        //cboLineaProd.Enabled = false;
                        cboTipoFalla.Enabled = false;
                        grbProcesos.Enabled = false;
                        //grbIdentificacionProd.Enabled = false;
                        Controles_Identificacion_Prod(false);
                        dtpFechaReg.Enabled = false;
                        //grbTrataMiento.Enabled = true;
                        controles_tratamiento(false);

                    }
                    else
                    {
                        cboAreaDefecto.Enabled = false;
                        cboAreaOrigen.Enabled = false;
                        //cboLineaProd.Enabled = false;
                        cboTipoFalla.Enabled = false;
                        grbProcesos.Enabled = false;
                        //grbIdentificacionProd.Enabled = false;
                        Controles_Identificacion_Prod(false);
                        dtpFechaReg.Enabled = false;
                        //grbTrataMiento.Enabled = true;
                        controles_tratamiento(false);
                    }
                }
            }
        }

        private void Controles_Identificacion_Prod(bool v)
        {
            grbIdentificacionProd.Enabled = true;
            if (!sIdEstado.Equals("12"))
            {
                cboAreaDefecto.Enabled = v;
                cboAreaOrigen.Enabled = v;
                //cboLineaProd.Enabled = v;
                cboTipoFalla.Enabled = v;
                dtpFechaReg.Enabled = v;
                grbProcesos.Enabled = v;
                txtIdProducto.Enabled = v;
                txtDescripProducto.Enabled = v;
                txtSerieProd.Enabled = v;
                txtNroLoteProd.Enabled = v;
                txtCantidad.Enabled = v;
                txtProveedor.Enabled = v;
                txtFchVenceLote.Enabled = v;

                if (!v)
                {
                    btnImgPrdNoConf.Enabled = (txtPathImg1.Text.Trim().Length > 0);
                    btnImgPrdNoConf2.Enabled = (txtPathImg1_2.Text.Trim().Length > 0);
                }
                


                txtUndMendida.Enabled = v;
                txtProveedorDescrip.Enabled = v;
                txtDescrpNoConformi.Enabled = v;
                btnDelet1.Enabled = v;
                btnDelet1_2.Enabled = v;
            }
            else
                grbIdentificacionProd.Enabled = false;
        }

        private void controles_tratamiento(bool v)
        {

            grbTrataMiento.Enabled = true;

            if (!sIdEstado.Equals("12"))
            {
                if (ListCodSeguridadERP[5].Estado)
                {
                    cboLineaProd.Enabled = true;
                    rbRecuperación.Enabled = true;
                    rbConcesion.Enabled = true;
                    rbResiduo.Enabled = true;
                    rbOferta.Enabled = true;
                    rbDevolucion.Enabled = true;
                    btnDelete2.Enabled = true;
                    btnImgProdTratado.Enabled = true;
                    txtDescripTratamiento.Enabled = true;

                    if (rbRecuperación.Checked)
                    {

                        txtIdArticuloTrata.Enabled = true;
                        txtIdArticuloTrataFinal.Enabled = true;
                        txtCantProdTratado.Enabled = true;
                        txtCantProdTratadoFinal.Enabled = true;
                    }
                    else
                    {

                        txtIdArticuloTrata.Enabled = false;
                        txtIdArticuloTrataFinal.Enabled = false;
                        txtCantProdTratado.Enabled = false;
                        txtCantProdTratadoFinal.Enabled = false;
                    }
                }
                else
                {
                    cboLineaProd.Enabled = v;
                    rbRecuperación.Enabled = v;
                    rbConcesion.Enabled = v;
                    rbResiduo.Enabled = v;
                    rbOferta.Enabled = v;
                    rbDevolucion.Enabled = v;
                    btnDelete2.Enabled = v;
                    txtDescripTratamiento.Enabled = v;

                    if (!v)
                    {
                        btnImgProdTratado.Enabled = txtPathImg2.Text.Trim().Length > 0;
                    }
                    else
                        btnImgProdTratado.Enabled = v;

                    if (rbRecuperación.Checked)
                    {
                        txtIdArticuloTrata.Enabled = v;
                        txtIdArticuloTrataFinal.Enabled = v;
                        txtCantProdTratado.Enabled = v;
                        txtCantProdTratadoFinal.Enabled = v;
                    }
                    else
                    {

                        txtIdArticuloTrata.Enabled = false;
                        txtIdArticuloTrataFinal.Enabled = false;
                        txtCantProdTratado.Enabled = false;
                        txtCantProdTratadoFinal.Enabled = false;
                    }
                }
            }
            else
                grbTrataMiento.Enabled = false;
        }

        private void BusquedaProveedor_x_NroLote(string v)
        {

            

            //Parametros
            List<SqlParameter> loSqlParameter;
            string sProcediminetoAlm;


            //Leer si la tabla compania_expand el tiempo y cuando fue la ultima recalculada de saldo
            sProcediminetoAlm = "sp_Busq_Proveedor_x_lote";

            loSqlParameter = new List<SqlParameter>
            {
                new SqlParameter("@CIA",SqlDbType.Char,2){Value=GlobalIdentity.Instance.P_Sys_Default_Cia},
                new SqlParameter("@NRO_LOTE",SqlDbType.VarChar,20){Value=v}

            };

            daDatabase odaDatabase = new daDatabase();
            //Obterner
            try
            {
                beDatabaseResult obeDatabaseResult = odaDatabase.GetDataSet(sProcediminetoAlm, loSqlParameter);

                if (obeDatabaseResult.Exito)
                {
                    DataTable oDataTable = ((DataSet)obeDatabaseResult.Data).Tables[0];

                    if (oDataTable != null)
                    {
                        if (oDataTable.Rows.Count > 0)
                        {



                            txtProveedor.Text = oDataTable.Rows[0]["ID_ANALITICA"].ToString();
                            txtProveedorDescrip.Text = oDataTable.Rows[0]["DESCRIPCION"].ToString();


                        }
                        else
                        {
                            txtProveedor.Text = string.Empty;
                            txtProveedorDescrip.Text = string.Empty;
                        }

                    }
                    else
                    {
                        txtProveedor.Text = string.Empty;
                        txtProveedorDescrip.Text = string.Empty;
                    }

                }
                else
                {

                    throw new Exception(obeDatabaseResult.Resultado);

                }

                
            }
            catch (Exception ex)
            {

                //GPNET.Sistema.Util.Frm.Uti_frm.MsjError(ex.Message);
                Mensaje_Proceso(ex.Message, Properties.Resources.Error, null, true, TipoMessageBoxGPNET.Error);
               
            }


        }



        #endregion

        #region Eventos
        private void frmManProdNoCon_Load(object sender, EventArgs e)
        {
            Asignar_Titulo_Ventana("Mantenimiento de Producto no Conforme");

            

            //Asignar diseño de controles
            Asignar_diseño_controles();


           // Habilitar_Accesos();

            //Configurar Busqueda txtId_Articulo
            ConfigurarBusquedaIdArticulo();
            //Configurar Busqueda txtId_NroSerie
            ConfigurarBusquedaNroSerie();
            //Configurar Busqueda txtNro_lote
            ConfigurarBusquedaNroLote();
            //Configurar busqueda de proveedores
            ConfigurarBusquedaProveedores();

            //Configurar ref de kardex
            ConfigurarBusquedaRefKardex();

            //Configurar Busqueda Articulo Tratado
            ConfigurarBusquedaIdArticuloTratado();

            //Configurar Busqueda Articulo Tratado
            ConfigurarBusquedaIdArticuloTratadoFinal();

            Cargar_comboTipoFalla();
            Cargar_cmbLineaNegocio();

            On_Limpiar();

            //Cargar Combo Area
            Cargar_Combo_Area();
           

            Configurar_Busqueda_Id();

            if (DatosRegistro != null)
            {
                if (!DatosRegistro.Equals(""))
                {
                    string[] sDat = DatosRegistro.Split('|');
                    txtAnio.Text = sDat[2];
                    txtId_Registro.Text = sDat[3];


                    MostrarDatosDespuesdeLeaveID();

                    
                }
            }

            ConfigurarBusquedaRefKardex();
        }

        private void ConfigurarBusquedaRefKardex()
        {

            //Llenando Campos Adicionales

            List<beCustomFile> olbeCustomFile = new List<beCustomFile>(){
                new beCustomFile(){NomCampo="Nro_Sal_Ref"},
                new beCustomFile(){NomCampo="ID_TIPO_DOC"},
                new beCustomFile(){NomCampo="SERIE_DOC"},
                new beCustomFile(){NomCampo="NRO_DOC"},
                new beCustomFile(){NomCampo="FECHA"},
                new beCustomFile(){NomCampo="ITEM"},
                new beCustomFile(){NomCampo="ID_ARTICULO"},
                new beCustomFile(){NomCampo="Articulo"},
                

            };




            List<beCamposConsulta> lobeCamposConsulta = new List<beCamposConsulta>();
            beCamposConsulta obeCamposConsulta;
            beBusquedaAvanzada obeBusquedaAvanzada = new beBusquedaAvanzada();

            //DateTime dtFechaIni = new DateTime(DateTime.Now.Year, DateTime.Now.Month-1, 1, 0, 0, 0);
            //DateTime dtFechaIni = DateTime.Now.AddDays(-3);
            DateTime dtFechaIni = DateTime.Now;

            obeCamposConsulta = new beCamposConsulta();
            obeCamposConsulta.Nombre_Campo = "FECHA";
            obeCamposConsulta.Campo_Sql = "inv.FECHA";
            obeCamposConsulta.Titulo_Campo = "Fecha";
            obeCamposConsulta.Tipo_campo = Tipo_Campo_SQL.Fecha;
            obeCamposConsulta.Foco_Control = false;
            obeCamposConsulta.Valor_x_Defect = dtFechaIni;
            obeCamposConsulta.Valor2_x_Defect = DateTime.Now;
            lobeCamposConsulta.Add(obeCamposConsulta);


            StringBuilder ostrSelect = new StringBuilder();
            StringBuilder ostrInner = new StringBuilder();
            StringBuilder ostrWhere = new StringBuilder();
            StringBuilder ostrOrderBy = new StringBuilder();

            ostrSelect.AppendLine("select td.ABREVIATURA + '-' + inv.SERIE_DOC + '-' + inv.NRO_DOC Nro_Sal_Ref,  ");
            ostrSelect.AppendLine("inv.ID_TIPO_DOC, inv.SERIE_DOC, inv.NRO_DOC, inv.FECHA, ");
            ostrSelect.AppendLine("invs.ITEM, invs.ID_ARTICULO, art.DESCRIPCION Articulo, ");
            ostrSelect.AppendLine("invs.NRO_SERIE, invs.NRO_LOTE, invs.FECHA_VENCE_LOTE, ");
            ostrSelect.AppendLine("inv.ID_TIPO_DOC_REF, inv.SERIE_DOC_REF, inv.NRO_DOC_REF, INV.ID_ESTADO ");            
            ostrSelect.AppendLine("from INVENTARIO_MOV inv ");
            ostrInner.AppendLine("inner join INVENTARIO_MOV_DET invd on invd.CIA=inv.CIA and invd.SEDE=inv.SEDE and invd.ID_TIPO_DOC=inv.ID_TIPO_DOC and invd.SERIE_DOC=inv.SERIE_DOC and invd.NRO_DOC=inv.NRO_DOC and invd.ID_ESTADO!='02' ");
            ostrInner.AppendLine("inner join INVENTARIO_MOV_SERIE invs on invs.CIA=invd.CIA and invs.SEDE=invd.SEDE and invs.ID_TIPO_DOC=invd.ID_TIPO_DOC and invs.SERIE_DOC=invd.SERIE_DOC and invs.NRO_DOC=invd.NRO_DOC and invs.ID_ESTADO!='02' ");
            ostrInner.AppendLine("inner join TIPO_DOCUMENTO td on td.CIA=inv.CIA and td.ID_TIPO_DOC=inv.ID_TIPO_DOC ");
            ostrInner.AppendLine("inner join ARTICULO art on art.CIA=invd.CIA and art.ID_ARTICULO=invd.ID_ARTICULO ");            
            ostrWhere.AppendLine("WHERE inv.CIA='" + GlobalIdentity.Instance.P_Sys_Default_Cia + "' AND inv.SEDE='" + GlobalIdentity.Instance.P_Sys_Default_Sede + "'  ");
            ostrWhere.AppendLine(" and invd.id_articulo='" + txtIdProducto.Text + "' and invs.nro_serie='" + txtSerieProd.Text + "' and inv.id_estado!='12' and inv.id_tipo_doc='34' and inv.id_motivo_almacen='Z3' ");
            //ostrWhere.AppendLine(" and isnull(inv.NRO_DOC_REF,'')!='' ");
            ostrOrderBy.AppendLine("order by invd.id_articulo");


            obeBusquedaAvanzada.ListaCamposCriterio = lobeCamposConsulta;
            obeBusquedaAvanzada.sQuerySelect = ostrSelect.ToString();
            obeBusquedaAvanzada.sQueryInto = ostrInner.ToString();
            obeBusquedaAvanzada.sQueryWhere = ostrWhere.ToString();
            obeBusquedaAvanzada.sQueryOrderBy = ostrOrderBy.ToString();

            string[] sCampoAgrupar = {"Nro_Sal_Ref" };
            string[] sCamposOcultos = { "ID_TIPO_DOC", "SERIE_DOC", "NRO_DOC" };


            //txtNotificacion.ListaCamposAdicionales = olbeCustomFile;
            txtRefKardex.Mostrar_Anulados = false;
            txtRefKardex.CamposColAgruparBq = sCampoAgrupar;
            txtRefKardex.CamposOcultosdelaBusq = sCamposOcultos;
            txtRefKardex.TituloVentanaBusq = "Busqueda de Registro del Producto No Conforme";
            txtRefKardex.Campo_Scrib_Anulado = "inv.id_estado!='12'";
            txtRefKardex.ListaCamposAdicionales = olbeCustomFile;
            txtRefKardex.DialogoTipo = GPNETv4.Windows.Ctrl.Texbox.TipoDialogo.Busqueda_Perzonalizada;
            txtRefKardex.Entidad_SelectPersonalizado = obeBusquedaAvanzada;
            txtRefKardex.Campo_del_Valor_Buscar = "NRO_DOC";
            txtRefKardex.Campo_del_Valor_Dev = "NRO_DOC";
            txtRefKardex.Campo_del_Seg_Valor_Dev = "NRO_DOC_REF";
            txtRefKardex.SegExportarExcel = true;
            txtRefKardex.Z_Ejecutar_TipoDialogo = true;





        }

        private void Cargar_comboTipoFalla()
        {
            cboTipoFalla.Items.Add("(Seleccione)");
            cboTipoFalla.Items.Add("Arrugas");
            cboTipoFalla.Items.Add("Error de Notificación");
            cboTipoFalla.Items.Add("Mal Empaquetado");
            cboTipoFalla.Items.Add("Material Faltante");
            cboTipoFalla.Items.Add("Material Maltratado");
            cboTipoFalla.Items.Add("Pegado y Húmedo");

            cboTipoFalla.Items.Add("Ausencia de impresión total");
            cboTipoFalla.Items.Add("Aleta despegada");
            cboTipoFalla.Items.Add("No hay lectura de código de barras");
            cboTipoFalla.Items.Add("Ranura corrida más a 4mm");
            cboTipoFalla.Items.Add("Aleta menor a 2.5 cm");
            cboTipoFalla.Items.Add("Liner despegado");
            cboTipoFalla.Items.Add("Medidas fuera de tolerancia");
            cboTipoFalla.Items.Add("Corrugado soplado");
            cboTipoFalla.Items.Add("Cajas maltratadas o golpeadas");
            cboTipoFalla.Items.Add("Medidas incorrectas");
            cboTipoFalla.Items.Add("Score mal marcado");
            cboTipoFalla.Items.Add("Embotamiento de tinta");
            cboTipoFalla.Items.Add("Formación de cola de pescado");
            cboTipoFalla.Items.Add("Otros");
        }

        private void Cargar_cmbLineaNegocio()
        {
            VL_Linea_Negocio = new DataTable();


            VL_Linea_Negocio.Columns.Add("CODIGO",typeof(string));
            VL_Linea_Negocio.Columns.Add("DESCRIPCION", typeof(string));

            


            DataRow rw = VL_Linea_Negocio.NewRow();

            rw["CODIGO"] = "";
            rw["DESCRIPCION"] = "(Seleccione)";

            VL_Linea_Negocio.Rows.Add(rw);

            rw = VL_Linea_Negocio.NewRow();
            rw["CODIGO"] = "001";
            rw["DESCRIPCION"] = "CDP";

            VL_Linea_Negocio.Rows.Add(rw);

            rw = VL_Linea_Negocio.NewRow();
            rw["CODIGO"] = "002";
            rw["DESCRIPCION"] = "CORRUGADO";

            VL_Linea_Negocio.Rows.Add(rw);

            cboLineaProd.DataSource = VL_Linea_Negocio;
            cboLineaProd.ValueMember = "CODIGO";
            cboLineaProd.DisplayMember = "DESCRIPCION";

            cboLineaProd.SelectedIndex = 0;

        }

        private void Configurar_Busqueda_Id()
        {
            //Llenando Campos Adicionales
            
            List<beCustomFile> olbeCustomFile = new List<beCustomFile>(){
                new beCustomFile(){NomCampo="ID_AREA_ORIGEN"},
                new beCustomFile(){NomCampo="ID_AREA_DETECT"},
                new beCustomFile(){NomCampo="AREA"},
                new beCustomFile(){NomCampo="FECHA_REG"},
                new beCustomFile(){NomCampo="FLAG_I_RECEPCION"},
                new beCustomFile(){NomCampo="FLAG_I_ALMACENAMIENTO"},
                new beCustomFile(){NomCampo="FLAG_I_PRODUCCION"},
                new beCustomFile(){NomCampo="FLAG_I_DISTRIBUCION"},
                new beCustomFile(){NomCampo="FLAG_I_DEVOLUCIONES"},
                new beCustomFile(){NomCampo="FLAG_OTRO"},
                new beCustomFile(){NomCampo="OTRO_I"},
                new beCustomFile(){NomCampo="ID_ARTICULO"},
                new beCustomFile(){NomCampo="Articulo"},
                new beCustomFile(){NomCampo="NRO_SERIE"},
                new beCustomFile(){NomCampo="NRO_LOTE"},
                new beCustomFile(){NomCampo="FECHA_VENCE_LOTE"},
                new beCustomFile(){NomCampo="CANTIDAD"},
                new beCustomFile(){NomCampo="ID_PROVEEDOR"},
                new beCustomFile(){NomCampo="PROVEEDOR"},
                new beCustomFile(){NomCampo="OBS_NO_CONFORMIDAD"},
                new beCustomFile(){NomCampo="PATH_IMG_NO_CONFOR"},
                new beCustomFile(){NomCampo="PATH_IMG_NO_CONFOR_2"},
                new beCustomFile(){NomCampo="FLAG_T_RECUPERACION"},
                new beCustomFile(){NomCampo="FLAG_T_CONCESION"},
                new beCustomFile(){NomCampo="FLAG_T_RESIDUO"},
                new beCustomFile(){NomCampo="FLAG_T_DEVOLUCION"},
                new beCustomFile(){NomCampo="OBS_TRATAMIENTO"},
                new beCustomFile(){NomCampo="PATH_IMG_TRATAMIENTO"},
                new beCustomFile(){NomCampo="ID_ARTICULO_T"},
                new beCustomFile(){NomCampo="ARTICULO_T"},
                new beCustomFile(){NomCampo="ID_ESTADO"},
                new beCustomFile(){NomCampo="ESTADO"},
                new beCustomFile(){NomCampo="CANTIDAD_TRA"},
                new beCustomFile(){NomCampo="ID_ARTICULO_FINAL"},
                 new beCustomFile(){NomCampo="ARTICULO_T2"},
                new beCustomFile(){NomCampo="CANTIDAD_TRA_F"},
                new beCustomFile(){NomCampo="UM" },
                new beCustomFile(){NomCampo="COD_LINEA_NEGOCIO" },
                new beCustomFile(){NomCampo="FLAG_T_OFERTA" },
                new beCustomFile(){ NomCampo="NRO_DOC_REF"}

            };

            


            List<beCamposConsulta> lobeCamposConsulta = new List<beCamposConsulta>();
            beCamposConsulta obeCamposConsulta;
            beBusquedaAvanzada obeBusquedaAvanzada = new beBusquedaAvanzada();

            obeCamposConsulta = new beCamposConsulta();
            obeCamposConsulta.Nombre_Campo = "ANIO";
            obeCamposConsulta.Campo_Sql = "a.ANIO";
            obeCamposConsulta.Titulo_Campo = "Año";
            obeCamposConsulta.Tipo_campo = Tipo_Campo_SQL.Texto;
            obeCamposConsulta.TamañoCampo = 40;
            obeCamposConsulta.Foco_Control = false;
            obeCamposConsulta.Valor_x_Defect = txtAnio.Text;
            //obeCamposConsulta.Valor2_x_Defect = "2";
            lobeCamposConsulta.Add(obeCamposConsulta);


            obeCamposConsulta = new beCamposConsulta();
            obeCamposConsulta.Nombre_Campo = "NRO_PRODUCTO";
            obeCamposConsulta.Campo_Sql = "a.NRO_PRODUCTO";
            obeCamposConsulta.Titulo_Campo = "Id Registro";
            obeCamposConsulta.Tipo_campo = Tipo_Campo_SQL.Texto;
            obeCamposConsulta.TamañoCampo = 60;
            obeCamposConsulta.Foco_Control = true;
            obeCamposConsulta.Valor_x_Defect = "";
            //obeCamposConsulta.Valor2_x_Defect = "2";
            lobeCamposConsulta.Add(obeCamposConsulta);



            //DateTime dtFechaIni = new DateTime(DateTime.Now.Year, DateTime.Now.Month-1, 1, 0, 0, 0);
            //DateTime dtFechaIni = DateTime.Now.AddDays(-3);
            DateTime dtFechaIni = DateTime.Now;

            obeCamposConsulta = new beCamposConsulta();
            obeCamposConsulta.Nombre_Campo = "FECHA_REG";
            obeCamposConsulta.Campo_Sql = "a.FECHA_REG";
            obeCamposConsulta.Titulo_Campo = "Fecha";
            obeCamposConsulta.Tipo_campo = Tipo_Campo_SQL.Fecha;
            obeCamposConsulta.Foco_Control = false;
            obeCamposConsulta.Valor_x_Defect = dtFechaIni;
            obeCamposConsulta.Valor2_x_Defect = DateTime.Now;
            lobeCamposConsulta.Add(obeCamposConsulta);


            StringBuilder ostrSelect = new StringBuilder();
            StringBuilder ostrInner = new StringBuilder();
            StringBuilder ostrWhere = new StringBuilder();
            StringBuilder ostrOrderBy = new StringBuilder();

            ostrSelect.AppendLine("select a.NRO_PRODUCTO, isnull(a.ID_AREA_ORIGEN,'') ID_AREA_ORIGEN, dporg.DESCRIPCION AREA_ORIGEN, a.ID_AREA_DETECT,");
            ostrSelect.AppendLine("dp.DESCRIPCION as AREA,	a.FECHA_REG, 	CONVERT(BIT,a.FLAG_I_RECEPCION) FLAG_I_RECEPCION,	CONVERT(BIT,a.FLAG_I_ALMACENAMIENTO) FLAG_I_ALMACENAMIENTO, CONVERT(BIT,a.FLAG_I_PRODUCCION) FLAG_I_PRODUCCION, ");
            ostrSelect.AppendLine("CONVERT(BIT,a.FLAG_I_DISTRIBUCION) FLAG_I_DISTRIBUCION,	CONVERT(BIT,a.FLAG_I_DEVOLUCIONES) FLAG_I_DEVOLUCIONES,	CONVERT(BIT,a.FLAG_OTRO) FLAG_OTRO, a.OTRO_I,	a.ID_ARTICULO, ");
            ostrSelect.AppendLine("art.DESCRIPCION Articulo, a.NRO_SERIE, a.NRO_LOTE,	a.FECHA_VENCE_LOTE,	a.CANTIDAD, ");
            ostrSelect.AppendLine("a.ID_PROVEEDOR,	an.DESCRIPCION PROVEEDOR, a.OBS_NO_CONFORMIDAD,	a.PATH_IMG_NO_CONFOR,	a.PATH_IMG_NO_CONFOR_2, CONVERT(BIT,a.FLAG_T_RECUPERACION) FLAG_T_RECUPERACION, ");
            ostrSelect.AppendLine("CONVERT(BIT,a.FLAG_T_CONCESION) FLAG_T_CONCESION, CONVERT(BIT,a.FLAG_T_RESIDUO) FLAG_T_RESIDUO, CONVERT(BIT,a.FLAG_T_DEVOLUCION) FLAG_T_DEVOLUCION,	a.OBS_TRATAMIENTO, a.PATH_IMG_TRATAMIENTO, ");
            ostrSelect.AppendLine("a.ID_ARTICULO_T, artt.DESCRIPCION ARTICULO_T, a.ID_ESTADO, est.DESCRIPCION ESTADO, a.ID_ESTADO, a.CANTIDAD_TRA, ");
            ostrSelect.AppendLine("a.ID_ARTICULO_FINAL, artt2.DESCRIPCION ARTICULO_T2, a.CANTIDAD_TRA_F, um.ABREVIATURA UM, a.COD_LINEA_NEGOCIO, CONVERT(BIT,a.FLAG_T_OFERTA) FLAG_T_OFERTA, a.NRO_DOC_REF ");
            ostrSelect.AppendLine("from PRODUCTO_NO_CONFORME a ");
            ostrInner.AppendLine("inner join DEPARTAMENTO_CIA dp on dp.CIA=a.CIA and dp.ID_DPTO_CIA=a.ID_AREA_DETECT ");
            ostrInner.AppendLine("inner join DEPARTAMENTO_CIA dporg on dporg.CIA=a.CIA and dporg.ID_DPTO_CIA=a.ID_AREA_DETECT ");
            ostrInner.AppendLine("INNER JOIN ARTICULO art on art.CIA=a.CIA and art.ID_ARTICULO=a.ID_ARTICULO ");
            ostrInner.AppendLine("left join ARTICULO artt on artt.CIA=a.CIA and artt.ID_ARTICULO=a.ID_ARTICULO_T ");
            ostrInner.AppendLine("left join ARTICULO artt2 on artt2.CIA=a.CIA and artt2.ID_ARTICULO=a.ID_ARTICULO_FINAL ");
            ostrInner.AppendLine("left join ANALITICA an on an.CIA=a.CIA and an.ID_ANALITICA=a.ID_PROVEEDOR ");
            ostrInner.AppendLine("LEFT JOIN ESTADO est on est.CIA=a.CIA and est.ID_ESTADO=a.ID_ESTADO ");
            ostrInner.AppendLine("left join UNIDAD_MEDIDA um on um.CIA=art.CIA and um.ID_UNIDAD=art.ID_UNIDAD ");
            ostrWhere.AppendLine("WHERE a.CIA='" + GlobalIdentity.Instance.P_Sys_Default_Cia + "' AND a.SEDE='" + GlobalIdentity.Instance.P_Sys_Default_Sede + "'  ");
            ostrOrderBy.AppendLine("order by a.NRO_PRODUCTO");


            obeBusquedaAvanzada.ListaCamposCriterio = lobeCamposConsulta;
            obeBusquedaAvanzada.sQuerySelect = ostrSelect.ToString();
            obeBusquedaAvanzada.sQueryInto = ostrInner.ToString();
            obeBusquedaAvanzada.sQueryWhere = ostrWhere.ToString();
            obeBusquedaAvanzada.sQueryOrderBy = ostrOrderBy.ToString();


            //txtNotificacion.ListaCamposAdicionales = olbeCustomFile;
            txtId_Registro.Mostrar_Anulados = true;
            txtId_Registro.TituloVentanaBusq = "Busqueda de Registro del Producto No Conforme";
            txtId_Registro.Campo_Scrib_Anulado = "a.id_estado!='12'";
            txtId_Registro.ListaCamposAdicionales = olbeCustomFile;
            txtId_Registro.DialogoTipo = GPNETv4.Windows.Ctrl.Texbox.TipoDialogo.Busqueda_Perzonalizada;
            txtId_Registro.Entidad_SelectPersonalizado = obeBusquedaAvanzada;
            txtId_Registro.Campo_del_Valor_Buscar = "NRO_PRODUCTO";
            txtId_Registro.Campo_del_Valor_Dev = "NRO_PRODUCTO";
            txtId_Registro.Campo_del_Seg_Valor_Dev = "ID_AREA_DETECT";
            txtId_Registro.SegExportarExcel = true;
            txtId_Registro.Z_Ejecutar_TipoDialogo = true;
        }

        private void txtIdProducto_txtDespuesDelaBusqueda(object sender, EventArgs e)
        {

            //sValorBusqArtNoCon = txtIdProducto.Text;
            //bDelaVentanaBusqArtNoCon = true;

            txtDescripProducto.Text = txtIdProducto.SegundoValorDevuelto;

            if (txtIdProducto.ListaCamposAdicionales != null)
            {
                foreach (beCustomFile obeCustomFile in txtIdProducto.ListaCamposAdicionales)
                {
                    switch (obeCustomFile.NomCampo)
                    {
                        case "NRO_SERIE":
                            txtSerieProd.Text = obeCustomFile.Valor.ToString();
                            break;
                        case "NRO_LOTE":
                            txtNroLoteProd.Text = obeCustomFile.Valor.ToString();
                            break;
                        case "fecha_vence_lote":
                            txtFchVenceLote.Text= obeCustomFile.Valor.ToString();
                            break;
                        case "Unid_Med":
                            txtUndMendida.Text = obeCustomFile.Valor.ToString();
                            break;

                    }
                }

            }

            string[] sDatoLote = txtNroLoteProd.Text.Split(' ');
            //Buscar el proveedor por Nro lote
            BusquedaProveedor_x_NroLote(sDatoLote[0]);

            txtCantidad.Select();

        }

        

        private void txtIdProducto_TextChanged(object sender, EventArgs e)
        {
            if (bCtrlActIdProd)
            {
                //Limpiar articulos
                txtDescripProducto.Text = string.Empty;
                txtSerieProd.Text = string.Empty;
                txtNroLoteProd.Text = string.Empty;
                txtProveedor.Text = string.Empty;
                txtProveedorDescrip.Text = string.Empty;
                txtFchVenceLote.Text = string.Empty;
                txtUndMendida.Text = string.Empty;
            }



        }

        private void txtProveedor_txtDespuesDelaBusqueda(object sender, EventArgs e)
        {
            txtProveedorDescrip.Text = txtProveedor.SegundoValorDevuelto;
        }

        private void txtProveedor_TextChanged(object sender, EventArgs e)
        {
            txtProveedorDescrip.Text = string.Empty;
        }

        private void txtSerieProd_TextChanged(object sender, EventArgs e)
        {
            if (bCtrlActNrSerie)
            {
                txtDescripProducto.Text = string.Empty;
                txtIdProducto.Text = string.Empty;
                txtNroLoteProd.Text = string.Empty;
                txtProveedor.Text = string.Empty;
                txtProveedorDescrip.Text = string.Empty;
                txtFchVenceLote.Text = string.Empty;
                txtUndMendida.Text = string.Empty;
            }
        }

        private void txtNroLoteProd_TextChanged(object sender, EventArgs e)
        {
            if (bCrtActNroLote)
            {
                txtDescripProducto.Text = string.Empty;
                txtIdProducto.Text = string.Empty;
                txtSerieProd.Text = string.Empty;
                txtProveedor.Text = string.Empty;
                txtProveedorDescrip.Text = string.Empty;
                txtFchVenceLote.Text = string.Empty;
                txtUndMendida.Text = string.Empty;
            }
        }

        private void txtIdProducto_Enter(object sender, EventArgs e)
        {
            bCtrlActIdProd = true;
        }

        private void txtIdProducto_Leave(object sender, EventArgs e)
        {
            bCtrlActIdProd = false;

           // Ejecutar_Busqx_Art_No_Conforme();

        }

        

        private void txtSerieProd_Enter(object sender, EventArgs e)
        {
            bCtrlActNrSerie = true;
        }

        private void txtSerieProd_Leave(object sender, EventArgs e)
        {
            bCtrlActNrSerie = false;
        }

        private void txtNroLoteProd_Enter(object sender, EventArgs e)
        {
            bCrtActNroLote = true;
        }

        private void txtNroLoteProd_Leave(object sender, EventArgs e)
        {
            bCrtActNroLote = false;
        }

        private void txtIdArticuloTrata_TextChanged(object sender, EventArgs e)
        {
            txtDescripTrata.Text = string.Empty;
        }

        private void txtIdArticuloTrata_txtDespuesDelaBusqueda(object sender, EventArgs e)
        {
            txtDescripTrata.Text = txtIdArticuloTrata.SegundoValorDevuelto;
            txtCantProdTratado.Select();
        }

        private void btnImgPrdNoConf_Click(object sender, EventArgs e)
        {

            if (btnDelet1.Visible)
            {


                frmVisorImg frm = new frmVisorImg();
                frm.sPathImagen = txtPathImg1.Text;
                frm.Show();


            }
            else
            {
                OpenFileDialog ofdAbrirImagen;
                ofdAbrirImagen = new OpenFileDialog();
                ofdAbrirImagen.FileName = "";
                ofdAbrirImagen.Filter = "Archivos de Imagen (*.bmp,*.jpg, *.jpeg, *.jpe, *.jfif, *.png)|*.BMP; *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                ofdAbrirImagen.Title = "Buscar Imagen";

                if (ofdAbrirImagen.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        //var filePath = ofdAbrirImagen.FileName;
                        txtPathImg1.Text = ofdAbrirImagen.FileName;
                        //pbImagenEtiqueta.Image = Image.FromFile(filePath);
                        //pbImagenEtiqueta.SizeMode = PictureBoxSizeMode.StretchImage;
                        btnImgPrdNoConf.Image = Properties.Resources.camara_79px_ing;
                        btnDelet1.Visible = true;
                    }
                    catch (IOException ex)
                    {
                        Uti_frm.MsjError(ex.Message);
                    }
                    catch (Exception ex1)
                    {
                        Uti_frm.MsjError(ex1.Message);
                    }
                }
            }


        }

        private void btnDelet1_Click(object sender, EventArgs e)
        {
            txtPathImg1.Text = string.Empty;
            btnImgPrdNoConf.Image = Properties.Resources.camara_79px_vacio;
            btnDelet1.Visible = false;
        }

        private void btnImgProdTratado_Click(object sender, EventArgs e)
        {

            if (btnDelete2.Visible)
            {

                frmVisorImg frm = new frmVisorImg();
                frm.sPathImagen = txtPathImg2.Text;
                frm.Show();
            }
            else
            {
                //op
                OpenFileDialog ofdAbrirImagen;

                ofdAbrirImagen = new OpenFileDialog();
                ofdAbrirImagen.FileName = "";
                ofdAbrirImagen.Filter = "Archivos de Imagen (*.bmp,*.jpg, *.jpeg, *.jpe, *.jfif, *.png)|*.BMP; *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                ofdAbrirImagen.Title = "Buscar Imagen";

                if (ofdAbrirImagen.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        //var filePath = ofdAbrirImagen.FileName;
                        txtPathImg2.Text = ofdAbrirImagen.FileName;
                        //pbImagenEtiqueta.Image = Image.FromFile(filePath);
                        //pbImagenEtiqueta.SizeMode = PictureBoxSizeMode.StretchImage;
                        btnImgProdTratado.Image = Properties.Resources.camara_79px_ing;
                        btnDelete2.Visible = true;
                    }
                    catch (IOException ex)
                    {
                        Uti_frm.MsjError(ex.Message);
                    }
                    catch (Exception ex1)
                    {
                        Uti_frm.MsjError(ex1.Message);
                    }
                }
            }
        }

        private void btnDelete2_Click(object sender, EventArgs e)
        {
            txtPathImg2.Text = string.Empty;
            btnImgProdTratado.Image = Properties.Resources.camara_79px_vacio;
            btnDelete2.Visible = false;
        }

        private void txtId_Registro_TextChanged(object sender, EventArgs e)
        {
            if (bNuevoRegistro)
            {
                if (oValorLLaveBuscado.ToString() != txtId_Registro.Text)
                    bNuevoRegistro = false;
            }
        }

        private void txtId_Registro_txtDespuesDelaBusqueda(object sender, EventArgs e)
        {
            nValoId_Reg = Int32.Parse(txtId_Registro.Text);
            bDelaVentanaBusqueda = true;
            //Mostrar Resultados segun Dato Buscado


            

            if (txtId_Registro.ListaCamposAdicionales != null)
            {
                foreach (beCustomFile obeCustomFile in txtId_Registro.ListaCamposAdicionales)
                {
                    switch (obeCustomFile.NomCampo)
                    {
                        //        new beCustomFile() { NomCampo = "COD_LINEA_NEGOCIO" },
                        //new beCustomFile() { NomCampo = "FLAG_T_OFERTA" }
                        case "COD_LINEA_NEGOCIO":
                            cboLineaProd.SelectedValue = obeCustomFile.Valor.ToString();
                            break;

                        case "ID_AREA_ORIGEN":
                            cboAreaOrigen.SelectedValue = obeCustomFile.Valor.ToString();
                            break;

                        case "ID_AREA_DETECT":
                            cboAreaDefecto.SelectedValue = obeCustomFile.Valor.ToString();
                            break;
                        case "FECHA_REG":
                            dtpFechaReg.Value = DateTime.Parse(obeCustomFile.Valor.ToString());
                            break;
                        case "FLAG_I_RECEPCION":
                            rbRecepcion.Checked = obeCustomFile.Valor.ToString().Equals("True");
                            break;
                        case "FLAG_I_ALMACENAMIENTO":
                            rbAlmacenamiento.Checked = obeCustomFile.Valor.ToString().Equals("True");
                            break;
                        case "FLAG_I_PRODUCCION":
                            rbProduccion.Checked = obeCustomFile.Valor.ToString().Equals("True");
                            break;
                        case "FLAG_I_DISTRIBUCION":
                            rbDistribucion.Checked = obeCustomFile.Valor.ToString().Equals("True");
                            break;
                        case "FLAG_I_DEVOLUCIONES":
                            rbDevoluciones.Checked = obeCustomFile.Valor.ToString().Equals("True");
                            break;
                        case "FLAG_OTRO":
                            rbOtros.Checked = obeCustomFile.Valor.ToString().Equals("True");
                            break;
                        case "OTRO_I":
                            txtOtroProceso.Text = obeCustomFile.Valor.ToString();
                            break;
                        case "ID_ARTICULO":
                            txtIdProducto.Text = obeCustomFile.Valor.ToString();
                            break;
                        case "Articulo":
                            txtDescripProducto.Text = obeCustomFile.Valor.ToString();
                            break;
                        case "NRO_SERIE":
                            txtSerieProd.Text = obeCustomFile.Valor.ToString();
                            break;


                        case "NRO_LOTE":
                            txtNroLoteProd.Text = obeCustomFile.Valor.ToString();
                            break;
                        case "FECHA_VENCE_LOTE":
                            txtFchVenceLote.Text = obeCustomFile.Valor.ToString();
                            break;
                        case "CANTIDAD":
                            txtCantidad.Text = obeCustomFile.Valor.ToString();
                            break;
                        case "ID_PROVEEDOR":
                            txtProveedor.Text = obeCustomFile.Valor.ToString();
                            break;

                        case "PROVEEDOR":
                            txtProveedorDescrip.Text = obeCustomFile.Valor.ToString();
                            break;
                        case "OBS_NO_CONFORMIDAD":
                            txtDescrpNoConformi.Text = obeCustomFile.Valor.ToString();
                            break;
                        case "PATH_IMG_NO_CONFOR":
                            txtPathImg1.Text = obeCustomFile.Valor.ToString();
                            if (txtPathImg1.Text.Trim().Equals(""))
                            {
                                btnImgPrdNoConf.Image = Properties.Resources.camara_79px_vacio;
                                btnDelet1.Visible = false;
                            }else
                            {
                                btnImgPrdNoConf.Image = Properties.Resources.camara_79px_ing;
                                btnDelet1.Visible = true;
                            }
                            break;

                        case "PATH_IMG_NO_CONFOR_2":
                            txtPathImg1_2.Text = obeCustomFile.Valor.ToString();
                            if (txtPathImg1_2.Text.Trim().Equals(""))
                            {
                                btnImgPrdNoConf2.Image = Properties.Resources.camara_79px_vacio;
                                btnDelet1_2.Visible = false;
                            }
                            else
                            {
                                btnImgPrdNoConf2.Image = Properties.Resources.camara_79px_ing;
                                btnDelet1_2.Visible = true;
                            }
                            break;

                        case "FLAG_T_RECUPERACION":
                            rbRecuperación.Checked = obeCustomFile.Valor.ToString().Equals("True");
                            break;


                        case "FLAG_T_CONCESION":
                            rbConcesion.Checked = obeCustomFile.Valor.ToString().Equals("True");
                            break;
                        case "FLAG_T_RESIDUO":
                            rbResiduo.Checked = obeCustomFile.Valor.ToString().Equals("True");
                            break;
                        case "FLAG_T_DEVOLUCION":
                            rbDevolucion.Checked = obeCustomFile.Valor.ToString().Equals("True");
                            break;

                        case "FLAG_T_OFERTA":
                            rbOferta.Checked = obeCustomFile.Valor.ToString().Equals("True");
                            break;

                        case "OBS_TRATAMIENTO":
                            txtDescripTratamiento.Text = obeCustomFile.Valor.ToString();
                            break;


                        case "PATH_IMG_TRATAMIENTO":
                            txtPathImg2.Text = obeCustomFile.Valor.ToString();

                            if (txtPathImg2.Text.Trim().Equals(""))
                            {
                                btnImgProdTratado.Image = Properties.Resources.camara_79px_vacio;
                                btnDelete2.Visible = false;
                            }
                            else
                            {
                                btnImgProdTratado.Image = Properties.Resources.camara_79px_ing;
                                btnDelete2.Visible = true;
                            }

                            break;
                        case "ID_ARTICULO_T":
                            txtIdArticuloTrata.Text = obeCustomFile.Valor.ToString();
                            break;
                        case "ARTICULO_T":
                            txtDescripTrata.Text = obeCustomFile.Valor.ToString();
                            break;
                        case "ESTADO":
                            txtEstado.Text = obeCustomFile.Valor.ToString();
                            break;
                        case "ID_ESTADO":
                            sIdEstado = obeCustomFile.Valor.ToString();
                            break;

                        case "CANTIDAD_TRA":
                            txtCantProdTratado.Text= obeCustomFile.Valor.ToString();
                            break;

                        case "ID_ARTICULO_FINAL":
                            txtIdArticuloTrataFinal.Text = obeCustomFile.Valor.ToString();
                            break;
                        case "ARTICULO_T2":
                            txtDescripTrataFinal.Text = obeCustomFile.Valor.ToString();
                            break;

                        case "CANTIDAD_TRA_F":
                            txtCantProdTratadoFinal.Text = obeCustomFile.Valor.ToString();
                            break;

                        case "UM":
                            txtUndMendida.Text= obeCustomFile.Valor.ToString();
                            break;

                        case "NRO_DOC_REF":
                            txtRefKardex.Text= obeCustomFile.Valor.ToString();
                            break;

                    }
                }

            }

            //grbTrataMiento
            // grbTrataMiento.Enabled = sIdEstado.Equals("41");
            Accesso_Segun_Estado(sIdEstado);


            txtCantidad.Select();
        }

        private void rbOtros_CheckedChanged(object sender, EventArgs e)
        {
            if (rbOtros.Checked)
            {
                txtOtroProceso.Enabled = true;
                txtOtroProceso.Select();
            }
        }

        private void rbRecepcion_CheckedChanged(object sender, EventArgs e)
        {
            txtOtroProceso.Enabled = !rbRecepcion.Checked;
            txtOtroProceso.Text = string.Empty;
        }

        private void rbAlmacenamiento_CheckedChanged(object sender, EventArgs e)
        {
            txtOtroProceso.Enabled = !rbAlmacenamiento.Checked;
            txtOtroProceso.Text = string.Empty;
        }

        private void btnImgPrdNoConf2_Click(object sender, EventArgs e)
        {
            if (btnDelet1_2.Visible)
            {


                frmVisorImg frm = new frmVisorImg();
                frm.sPathImagen = txtPathImg1_2.Text;
                frm.Show();


            }
            else
            {
                OpenFileDialog ofdAbrirImagen;
                ofdAbrirImagen = new OpenFileDialog();
                ofdAbrirImagen.FileName = "";
                ofdAbrirImagen.Filter = "Archivos de Imagen (*.bmp,*.jpg, *.jpeg, *.jpe, *.jfif, *.png)|*.BMP; *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                ofdAbrirImagen.Title = "Buscar Imagen";

                if (ofdAbrirImagen.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        //var filePath = ofdAbrirImagen.FileName;
                        txtPathImg1_2.Text = ofdAbrirImagen.FileName;
                        //pbImagenEtiqueta.Image = Image.FromFile(filePath);
                        //pbImagenEtiqueta.SizeMode = PictureBoxSizeMode.StretchImage;
                        btnImgPrdNoConf2.Image = Properties.Resources.camara_79px_ing;
                        btnDelet1_2.Visible = true;
                    }
                    catch (IOException ex)
                    {
                        Uti_frm.MsjError(ex.Message);
                    }
                    catch (Exception ex1)
                    {
                        Uti_frm.MsjError(ex1.Message);
                    }
                }
            }
        }

        private void btnDelet1_2_Click(object sender, EventArgs e)
        {
            txtPathImg1_2.Text = string.Empty;
            btnImgPrdNoConf2.Image = Properties.Resources.camara_79px_vacio;
            btnDelet1_2.Visible = false;
        }

        private void txtIdArticuloTrataFinal_TextChanged(object sender, EventArgs e)
        {
            txtDescripTrataFinal.Text = string.Empty;
        }

        private void txtIdArticuloTrataFinal_txtDespuesDelaBusqueda(object sender, EventArgs e)
        {
            txtDescripTrataFinal.Text = txtIdArticuloTrataFinal.SegundoValorDevuelto;
            txtCantProdTratadoFinal.Select();
        }

        private void rbRecuperación_CheckedChanged(object sender, EventArgs e)
        {
            //Habilitar campos de articulo
            Habilitar_Solo_ArticulosTratados(true);
        }

        private void Habilitar_Solo_ArticulosTratados(bool v)
        {
            bool esLineaCDP = cboLineaProd.SelectedValue.ToString().Equals("001");


            if (esLineaCDP)
            {

                txtIdArticuloTrata.Enabled = v;
                txtDescripTrata.Enabled = v;
                txtCantProdTratado.Enabled = v;

                
            }
            else
            {
                txtIdArticuloTrata.Enabled = false;
                txtDescripTrata.Enabled = false;
                txtCantProdTratado.Enabled = false;
            }


            if (rbResiduo.Checked || 
                rbOferta.Checked)
            {
                txtIdArticuloTrata.Enabled = false;
                txtDescripTrata.Enabled = false;
                txtCantProdTratado.Enabled = false;
                txtIdArticuloTrataFinal.Enabled = false;
                txtDescripTrataFinal.Enabled = false;
                txtCantProdTratadoFinal.Enabled = false;

                txtCantProdTratadoFinal.Enabled = true;
            }
            else
            {

                txtIdArticuloTrataFinal.Enabled = v;
                txtDescripTrataFinal.Enabled = v;
                txtCantProdTratadoFinal.Enabled = v;
            }



            //si es falso el articulo base se limpia y se iguala la informacion del articulo final
            if (!v)
            {

                
                txtIdArticuloTrata.Text = string.Empty;
                txtDescripTrata.Text = string.Empty;
                txtIdArticuloTrataFinal.Text = txtIdProducto.Text;
                txtDescripTrataFinal.Text = txtDescripProducto.Text;
                txtCantProdTratado.Text = string.Empty;
                txtCantProdTratadoFinal.Text = txtCantidad.Text;
                
            }
            else
            {
                txtIdArticuloTrata.Text = string.Empty;
                txtDescripTrata.Text = string.Empty;
                txtIdArticuloTrataFinal.Text = string.Empty;
                txtDescripTrataFinal.Text = string.Empty;
                txtCantProdTratado.Text = string.Empty;
                txtCantProdTratadoFinal.Text = string.Empty;

            }

            if (rbOferta.Checked)
            {
                txtIdArticuloTrata.Text = string.Empty;
                txtDescripTrata.Text = string.Empty;
                txtIdArticuloTrataFinal.Text = txtIdProducto.Text;
                txtDescripTrataFinal.Text = txtDescripProducto.Text;
                txtCantProdTratado.Text = string.Empty;
                txtCantProdTratadoFinal.Text = txtCantidad.Text;
            }

        }

        private void rbConcesion_CheckedChanged(object sender, EventArgs e)
        {
            Habilitar_Solo_ArticulosTratados(false);
        }

        private void rbResiduo_CheckedChanged(object sender, EventArgs e)
        {
            Habilitar_Solo_ArticulosTratados(false);
        }

        private void rbDevolucion_CheckedChanged(object sender, EventArgs e)
        {
            Habilitar_Solo_ArticulosTratados(false);
        }

        private void cboLineaProd_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //Habilitar campos de articulo
            Habilitar_Solo_ArticulosTratados(rbRecuperación.Checked);
        }

        private void btnActualizarRef_Click(object sender, EventArgs e)
        {
            //Validar si la referencia no esta vacia
            if (txtRefKardex.Text.Length == 0)
            {
                Mensaje_Proceso("No ha ingresado la referencia del kardex", null);
                return;
            }

            Actualizar_Referencia();



        }

        private async void Actualizar_Referencia()
        {

            List<SqlParameter> loSqlParameter;
            string sProcediminetoAlm;

            //Leer si la tabla compania_expand el tiempo y cuando fue la ultima recalculada de saldo
            sProcediminetoAlm = "sp_upd_PRODUCTO_NO_CONFORME_REF";

            loSqlParameter = new List<SqlParameter>
            {
                new SqlParameter("@CIA",SqlDbType.Char,2){Value=GlobalIdentity.Instance.P_Sys_Default_Cia},
                new SqlParameter("@SEDE",SqlDbType.Char,2){Value=GlobalIdentity.Instance.P_Sys_Default_Sede},
                new SqlParameter("@ANIO",SqlDbType.Int){Value=Int32.Parse(txtAnio.Text.Trim())},
                new SqlParameter("@NRO_PRODUCTO",SqlDbType.Int){Value=Int32.Parse(txtId_Registro.Text.Trim())},                
                new SqlParameter("@UC",SqlDbType.VarChar,20){Value=GlobalIdentity.Instance.P_Sys_Default_Usuario},
                new SqlParameter("@NRO_DOC_REF",SqlDbType.VarChar,20){Value=txtRefKardex.Text}
            };

            daDatabase odaDatabase = new daDatabase();
            beDatabaseResult obeDatabaseResult;
            //Obterner
            try
            {
                obeDatabaseResult = await odaDatabase.Execute_beDataAsync(sProcediminetoAlm, loSqlParameter);

                if (obeDatabaseResult.Exito)
                {
                    Uti_frm.MsjInformacion("Se actualizó con éxito");
                    bNuevoRegistro = false;


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

        private void txtRefKardex_Click(object sender, EventArgs e)
        {
            ConfigurarBusquedaRefKardex();
        }

        private void rbProduccion_CheckedChanged(object sender, EventArgs e)
        {
            txtOtroProceso.Enabled = !rbProduccion.Checked;
            txtOtroProceso.Text = string.Empty;
        }

        private void rbDistribucion_CheckedChanged(object sender, EventArgs e)
        {
            txtOtroProceso.Enabled = !rbDistribucion.Checked;
            txtOtroProceso.Text = string.Empty;
        }

        private void rbDevoluciones_CheckedChanged(object sender, EventArgs e)
        {
            txtOtroProceso.Enabled = !rbDevoluciones.Checked;
            txtOtroProceso.Text = string.Empty;
        }

        private void txtId_Registro_Leave(object sender, EventArgs e)
        {
            MostrarDatosDespuesdeLeaveID();
        }

        private void txtSerieProd_txtDespuesDelaBusqueda(object sender, EventArgs e)
        {

            bCtrlActNrSerie = false;
            txtIdProducto.Text = txtSerieProd.Text;
            txtDescripProducto.Text = txtSerieProd.SegundoValorDevuelto;

            if (txtSerieProd.ListaCamposAdicionales != null)
            {
                foreach (beCustomFile obeCustomFile in txtSerieProd.ListaCamposAdicionales)
                {
                    switch (obeCustomFile.NomCampo)
                    {


                        case "NRO_SERIE":
                            txtSerieProd.Text = obeCustomFile.Valor.ToString();
                            break;
                        case "NRO_LOTE":
                            txtNroLoteProd.Text = obeCustomFile.Valor.ToString();
                            break;
                        case "fecha_vence_lote":
                            txtFchVenceLote.Text = obeCustomFile.Valor.ToString();
                            break;
                        case "Unid_Med":
                            txtUndMendida.Text = obeCustomFile.Valor.ToString();
                            break;

                    }
                }

            }

            string[] sDatoLote = txtNroLoteProd.Text.Split(' ');

            //Buscar el proveedor por Nro lote
            BusquedaProveedor_x_NroLote(sDatoLote[0]);

            txtCantidad.Select();
        }

        private void txtNroLoteProd_txtDespuesDelaBusqueda(object sender, EventArgs e)
        {
            bCrtActNroLote = false;
            txtIdProducto.Text = txtNroLoteProd.Text;
            txtDescripProducto.Text = txtNroLoteProd.SegundoValorDevuelto;

            if (txtNroLoteProd.ListaCamposAdicionales != null)
            {
                foreach (beCustomFile obeCustomFile in txtNroLoteProd.ListaCamposAdicionales)
                {
                    switch (obeCustomFile.NomCampo)
                    {
                        case "NRO_SERIE":
                            txtSerieProd.Text = obeCustomFile.Valor.ToString();
                            break;
                        case "NRO_LOTE":
                            txtNroLoteProd.Text = obeCustomFile.Valor.ToString();
                            break;
                        case "fecha_vence_lote":
                            txtFchVenceLote.Text = obeCustomFile.Valor.ToString();
                            break;
                        case "Unid_Med":
                            txtUndMendida.Text = obeCustomFile.Valor.ToString();
                            break;

                    }
                }

            }

            string[] sDatoLote = txtNroLoteProd.Text.Split(' ');

            //Buscar el proveedor por Nro lote
            BusquedaProveedor_x_NroLote(sDatoLote[0]);

            txtCantidad.Select();
        }


        #endregion


    }
}
