using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlaneamientoCDP
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new frmRutaDespacho());

#if (DEBUG)
            /////Prueba con parametros
            string[] args2 = { "PACIFIDAT", "xis", "xis", "01", "01", "ccanchari", "912010", "1", "1", "1", "PN" };
            // string[] args2 = { "PACIFIDAT", "xis", "xis", "01", "01", "jmiranda", "913010", "1", "1", "1", "PN" };
            //string[] args2 = { "PACIFIDAT", "xis", "xis", "01", "01", "ccanchari", "912010", "1", "1", "1" , "PN" };            
           
             args = args2;

#endif

            ///////------------args[10]--> Parametro adicional desde el ERP
            if (args != null)
            {
                if (args.Length > 0)
                {

                    if (args.Length >= 4)
                    {
                        // Pruebas obtener parametro ERP
                        /*  StringBuilder smostrar = new StringBuilder();
                          int n = 0;

                          foreach (string str in args)
                          {

                              smostrar.AppendLine(n.ToString() + " - " + str);
                              n++;
                          }

                          MessageBox.Show(smostrar.ToString());
                        */



                        //Application.Run(new FrmMapPuntos());


                        Application.EnableVisualStyles();
                        //Application.SetCompatibleTextRenderingDefault(false);

                        switch (args[10])
                        {
                            case "PN":
                                frmManProdNoCon frmObject;
                                frmObject = new frmManProdNoCon(args);
                                Application.Run(frmObject);
                                break;
                            case "VB":
                                frmVBProdNoConfor frmPv;
                                frmPv = new frmVBProdNoConfor(args);
                                Application.Run(frmPv);
                                break;

                            case "RP":
                                frmReporteProdNoConf frmPRGv;
                                frmPRGv = new frmReporteProdNoConf(args);
                                Application.Run(frmPRGv);
                                break;
/*

                            case "PA":
                                frmPuntoAlertaN frmPA;
                                frmPA = new frmPuntoAlertaN(args);
                                Application.Run(frmPA);
                                break;


                            case "MP":
                                FrmMapPuntos frmMP;
                                frmMP = new FrmMapPuntos(args);
                                Application.Run(frmMP);
                                break;
                            //MPW7
                            case "MPW7":
                                frmMapPuntosW7 frmMPW7;
                                frmMPW7 = new frmMapPuntosW7(args);
                                Application.Run(frmMPW7);
                                break;

                            case "RV":
                                frmRutaDeschVigilancia frmRV;
                                frmRV = new frmRutaDeschVigilancia(args);
                                Application.Run(frmRV);
                                break;
                            case "MA":
                                frmMantAyudante frmAy;
                                frmAy = new frmMantAyudante(args);
                                Application.Run(frmAy);
                                break;
                            case "RRD":
                                frmReportRutaDespacho frmRRD;
                                frmRRD = new frmReportRutaDespacho(args);
                                Application.Run(frmRRD);
                                break;
                            */
                        }







                        //frmPuntoVenta frmObject = new frmPuntoVenta(args);


                    }
                }
                else
                {


                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new frmManProdNoCon());
                }
            }
        }
    }
}
