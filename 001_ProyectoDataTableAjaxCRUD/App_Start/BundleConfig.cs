using System.Web;
using System.Web.Optimization;

namespace _001_ProyectoDataTableAjaxCRUD
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información sobre los formularios.  De esta manera estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            //Plugins DataTable
            bundles.Add(new StyleBundle("~/Content/Plugins/css").Include(
                      "~/Content/DataTable/dataTables.min.css"));

            bundles.Add(new ScriptBundle("~/Content/Plugins/js").Include(
                      "~/Content/DataTable/dataTables.min.js"));

            //Plugins BootsTrap
            bundles.Add(new StyleBundle("~/Content/BootsTrap/css").Include(
                      "~/Content/Bootstrap/css/bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/Content/BootsTrap/js").Include(
                      "~/Content/Bootstrap/js/bootstrap.min.js"));
        }
    }
}
