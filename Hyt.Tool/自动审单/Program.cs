using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace 自动审单
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Hyt.BLL.Base.DataProviderBo.Set(
            Activator.CreateInstance(Type.GetType("Hyt.DataAccess.Oracle.DataProvider,Hyt.DataAccess.Oracle")));
            Application.Run(new FormAuto());
        }
    }
}
