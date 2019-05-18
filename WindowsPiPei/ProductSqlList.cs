using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsPiPei
{
    class ProductSqlList
    {

        string strSql = "Data Source=112.74.65.202;Initial Catalog=xingying;Persist Security Info=True;User ID=demo;Password=demo";

        public List<Product> SelectPro()
        {
            List<Product> list = new List<Product>();
            //string[] filed = new string[8] { "SysNo", "BrandSysNo", "ErpCode", "EasName", "Barcode", "QRCode", "ProductType", "ProductName" };
            //StringBuilder str = new StringBuilder();
            //str.Append("select");
            //str.Append(""+filed+"");
            //str.Append("from PdProduct");
            //string sql= str.ToString();
            string sql = "select SysNo,BrandSysNo,ErpCode,EasName,Barcode,QRCode,ProductType,ProductName from PdProduct";

            try
            {
                SqlConnection con = new SqlConnection(strSql);
                SqlCommand com = new SqlCommand(sql, con);
                con.Open();
                SqlDataReader dr = com.ExecuteReader();

                while (dr.Read())
                {
                    Product pro = new Product()
                    {
                        //FNumber,FBarcode,FFullName,


                        SysNo = Convert.ToInt32(dr["SysNo"]),
                        BrandSysNo = Convert.ToInt32(dr["BrandSysNo"]),
                        ErpCode = dr["ErpCode"].ToString(),
                        EasName = dr["EasName"].ToString(),
                        Barcode = dr["Barcode"].ToString(),
                        QRCode = dr["QRCode"].ToString(),
                        ProductType = Convert.ToInt32(dr["ProductType"]),
                        ProductName = dr["ProductName"].ToString()

                    };
                    list.Add(pro);
                }
                dr.Close();
                con.Close();

            }

            catch (Exception e)
            {

                throw ;
            }


            return list;
        }

        


    }
}
