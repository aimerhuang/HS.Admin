using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsPiPei
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private static List<ErpProduct> comPareList = new List<ErpProduct>();
        private static List<ErpProduct> comPareResult = new List<ErpProduct>();
        private void btnCompare_Click(object sender, EventArgs e)
        {
            ErpProductList erp= new ErpProductList();
            ProductSqlList pro = new ProductSqlList();
            //开始比对

           // var vs = ErpProductList.erpList.Except(ProductSqlList.Prolist).Union(ProductSqlList.Prolist.Except(ErpProductList.erpList));
                var erpList=erp.getErpList();
                var proList = pro.SelectPro();
                foreach(var item in erpList)

                 if (!proList.Any(x => x.ErpCode == item.FNumber))
                  {
                    comPareList.Add(item);
                }
                this.labCount.Text = comPareList.Count.ToString();
                this.dgvCompareData.DataSource = new List<ErpProduct>();
                this.dgvCompareData.DataSource = comPareList;
            //this.dgvCompareData.DataSource = ProductSqlList.Prolist;

        }

       

        private void dgvCompareData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void labCount_Click(object sender, EventArgs e)
        {

        }

        private void btnOutToExcel_Click(object sender, EventArgs e)
        {
          OutToExcel to=new OutToExcel();
          to.exportToExcel("F:\\",this.dgvCompareData);
        }
        //查询本地数据库的商品


       // private static List<Dictionary<string,string>> list();



    }
}
