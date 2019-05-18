using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
namespace WindowsPiPei
{
   public  class OutToExcel
    {

        public OutToExcel() { 
        
        
        }

        public void exportToExcel(string fileName,DataGridView myDgv) {
            string saveFileName = "";//保存路径
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.DefaultExt = "xlsx";//保存格式为xls
            saveDialog.Filter = "Excel文件|*.xlsx";
            saveDialog.FileName = fileName;
            saveDialog.ShowDialog();
            saveFileName = saveDialog.FileName;
            if (saveFileName.IndexOf(":") < 0) return;//取消了
            Microsoft.Office.Interop.Excel._Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            
            if(xlApp==null){
                MessageBox.Show("无法创建Excel对象，是否有安装excel");
                return;
            }
            Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];
            //得到sheet1

            for (int i = 0; i < myDgv.ColumnCount; i++)
            {
                worksheet.Cells[1, i + 1] = myDgv.Columns[i].HeaderText;
            }

            //写入数值
            for (int r = 0; r < myDgv.Rows.Count; r++)
            {
                for (int i = 0; i < myDgv.ColumnCount; i++)
                {
                    worksheet.Cells[r + 2, i + 1] = myDgv.Rows[r].Cells[i].Value;
                }
                System.Windows.Forms.Application.DoEvents();
            }
            worksheet.Columns.EntireColumn.AutoFit();//自适应列宽
            if(saveFileName!=""){

                try
                {
                    workbook.Saved = true;
                    workbook.SaveCopyAs(saveFileName);
                }
                catch (Exception e)
                {

                    MessageBox.Show("导出文件出错，文件可能正在打开使用中"+e.Message);
                }
          
            }
            xlApp.Quit();
            GC.Collect();//销毁
            MessageBox.Show("文件："+fileName+".xlsx 保存成功","信息提示",MessageBoxButtons.OK,MessageBoxIcon.Information);




        }


    }
}
