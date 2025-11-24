using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.UI;

namespace jw.easycc.kr
{
    public class BundleConfig
    {
        // 묶음에 대한 자세한 내용은 https://go.microsoft.com/fwlink/?LinkID=303951을 참조하세요.
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/LibJS").Include(
                                                                    "~/js/lib/jquery/jquery.js",
                                                                    //"~/js/lib/jquery-ui/jquery-ui.js",
                                                                    "~/js/lib/datatables/jquery.dataTables.js",
                                                                    "~/js/lib/bootstrap/bootstrap.js",
                                                                    "~/js/lib/jquery.blockUI.js",
                                                                    "~/js/lib/jquery.datetimepicker.full.js",
                                                                    "~/js/lib/jquery.fileDownload.js",
                                                                    "~/js/lib/jquery.cookie.js"));
        }
    }
}