using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Order;
using Hyt.Model;

namespace Hyt.DataAccess.Oracle.Order
{
    /// <summary>
    /// 订单收货地址
    /// </summary>
    /// <remarks>2013-06-13 朱成果 创建</remarks>
    public class SoReceiveAddressDaoImpl : ISoReceiveAddressDao
    {
        /// <summary>
        /// 获取订单的收货地址信息
        /// </summary>
        /// <param name="SysNo">编号</param>
        /// <returns>订单的收货地址信息</returns>
        /// <remarks>2013-06-13 朱成果 创建</remarks>
        public override SoReceiveAddress GetOrderReceiveAddress(int sysNo)
        {
            return Context.Sql("select * from SoReceiveAddress where SysNo=@0", sysNo).QuerySingle<Model.SoReceiveAddress>();
        }

        /// <summary>
        /// 保存订单收货地址
        /// </summary>
        /// <param name="entity">订单收货地址实体</param>
        /// <returns>订单收货地址实体（带编号)</returns>
        /// <remarks>2013-06-27 黄志勇 创建</remarks>
        public override SoReceiveAddress InsertEntity(SoReceiveAddress entity)
        {
            //entity.Name = entity.Name.Trim();
            //entity.ZipCode = entity.ZipCode.Trim();
            //entity.StreetAddress = entity.StreetAddress.Trim();
            //entity.PhoneNumber = entity.PhoneNumber.Trim();
            //entity.MobilePhoneNumber = entity.MobilePhoneNumber.Trim();
            //entity.EmailAddress = entity.EmailAddress.Trim();
            //entity.FaxNumber = entity.FaxNumber.Trim();            
            entity.SysNo = Context.Insert("SoReceiveAddress", entity)
                                            .AutoMap(o => o.SysNo)
                                            .ExecuteReturnLastId<int>("SysNo");
            return entity;
        }

        /// <summary>
        /// 更新收货地址信息
        /// </summary>
        /// <param name="entity">订单收货地址实体</param>
        /// <returns></returns>
        /// <remarks>2013-06-27 朱成果 创建</remarks>
        public override void UpdateEntity(SoReceiveAddress entity)
        {
            Context.Update("SoReceiveAddress", entity).AutoMap(o => o.SysNo).Where("SysNo", entity.SysNo).Execute();
        }
        /// <summary>
        /// 删除收货地址信息
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-07-13 王耀发 创建</remarks>
        public override int DeleteEntity(int sysNo)
        {
            return Context.Delete("SoReceiveAddress").Where("SysNo", sysNo).Execute();
        }
        /// <summary>
        /// 判断当前收货信息是否已经存在
        /// </summary>
        /// <param name="name">收货人</param>
        /// <param name="mobliePhoneNumber">收货人手机号</param>
        /// <param name="areaSysNo">收货区域信息</param>
        /// <param name="streetAddress">街道地址</param>
        /// <param name="sIDCardNo">身份证</param>
        /// <returns></returns>
        /// <remarks>2016-5-9 杨浩 创建</remarks>
        public override int ExistReceiveAddress(string name, string mobliePhoneNumber, string areaSysNo, string streetAddress, string sIDCardNo)
        {
            return Context.Sql(" SELECT SysNo FROM SoReceiveAddress WHERE [Name]=@0 AND MobilePhoneNumber=@1 AND AreaSysNo=@2 AND StreetAddress=@3 AND IDCardNo=@4 ", name, mobliePhoneNumber, areaSysNo, streetAddress, sIDCardNo).QuerySingle<int>();
        }

        public override List<CBSoReceiveAddress> GetOrderReceiveAddressByList(string SysNos)
        {
            string sql = @" select SoReceiveAddress.* 
                                ,a.AreaName as ProvinceName
                                ,b.AreaName as CityName 
                                ,c.AreaName as CountryName
                     from SoOrder inner join SoReceiveAddress on SoOrder.ReceiveAddressSysNo=SoReceiveAddress.SysNo ";
            sql += " inner join BsArea c on c.SysNo=SoReceiveAddress.AreaSysNo inner join BsArea b on c.ParentSysNo=b.SysNo inner join  BsArea a on b.ParentSysNo=a.SysNo";
            sql += " where  SoOrder.SysNo in (" + SysNos + ") ";
            return Context.Sql(sql).QueryMany<CBSoReceiveAddress>();
        }
    }
}
