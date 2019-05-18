using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using WindowsPiPei;
namespace WindowsPiPei
{
    class ErpProductList
    {



        string strSql = "Data Source=211.154.139.8;Initial Catalog=AIS20160412101109;Persist Security Info=True;User ID=demo;Password=demo";
        public List<ErpProduct> getErpList()
        {
            List<ErpProduct> list = new List<ErpProduct>();
            string sql = "select FNumber,FBarcode,FFullName,FName from  t_icitem  where  FDeleted=0";

            try
            {
                SqlConnection con = new SqlConnection(strSql);

                SqlCommand com = new SqlCommand(sql, con);
                con.Open();
                SqlDataReader dr = com.ExecuteReader();

                while (dr.Read())
                {
                    ErpProduct erp = new ErpProduct()
                    {
                      
                        FNumber =dr["FNumber"].ToString(),
                        FBarcode = dr["FBarcode"].ToString(),
                        FFullName = dr["FFullName"].ToString(),
                        FName = dr["FName"].ToString()

                    };
                    list.Add(erp);

                }
                dr.Close();
                con.Close();
            }
            catch (Exception)
            {
                
                throw;
            }



            return list;
        }
       




    }
}
