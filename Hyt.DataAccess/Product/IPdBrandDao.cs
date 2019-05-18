using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Base;
using Hyt.Model;

namespace Hyt.DataAccess.Product
{
    /// <summary>
    /// 商品品牌维护
    /// </summary>
    /// <remarks>2013-06-25 唐永勤 创建</remarks>    
    public abstract class IPdBrandDao : DaoBase<IPdBrandDao>
    {
        /// <summary>
        /// 添加品牌
        /// </summary>
        /// <param name="model">品牌实体信息</param>
        /// <returns>返回新建记录的sysno</returns>       
        /// <remarks>2013-06-25 唐永勤 创建</remarks>
        public abstract int Create(PdBrand model);

        /// <summary>
        /// 添加b2b品牌
        /// </summary>
        /// <param name="model">品牌实体信息</param>
        /// <returns>返回新建记录的sysno</returns>       
        /// <remarks>2017-10-10 罗勤瑶 创建</remarks>
        public abstract int CreateToB2B(PdBrand model);

        /// <summary>
        /// 判断重复数据--品牌
        /// </summary>
        /// <param name="name">品牌名称</param>
        /// <param name="brandSysNo">品牌编号</param>
        /// <returns>存在返回true，不存在返回flase</returns>
        /// <remarks>2013-07-03 唐永勤 创建</remarks>
        public abstract bool IsExists(string name, int brandSysNo);

        /// <summary>
        /// 获取指定ID的品牌信息
        /// </summary>
        /// <param name="brandSysNo">品牌编号</param>
        /// <returns>品牌实体信息</returns>
        /// <remarks>2013-06-25 唐永勤 创建</remarks>
        public abstract PdBrand GetEntity(int brandSysNo);

        /// <summary>
        /// 获取指定名称的品牌信息
        /// </summary>
        /// <param name="name">品牌名称</param>
        /// <returns>品牌实体信息</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public abstract PdBrand GetEntityByName(string Name);
        /// <summary>
        /// 获取b2b指定名称的品牌信息
        /// </summary>
        /// <param name="name">品牌名称</param>
        /// <returns>品牌实体信息</returns>
        /// <remarks>2017-10-10 罗勤瑶 创建</remarks>
        public abstract PdBrand GetB2BEntityByName(string Name);

        /// <summary>
        /// 根据品牌编号更新品牌信息
        /// </summary>
        /// <param name="model">品牌实体信息</param>
        /// <returns>成功返回true，失败返回false</returns>
        /// <remarks>2013-06-25 唐永勤 创建</remarks>
        public abstract bool Update(PdBrand model);

        /// <summary>
        /// 更新品牌状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="listSysNo">品牌编号集</param>
        /// <returns>更新行数</returns>
        /// <remarks>2013-06-25 唐永勤 创建</remarks>
        public abstract int UpdateStatus(Hyt.Model.WorkflowStatus.ProductStatus.品牌状态 status, List<int> listSysNo);

        /// <summary>
        /// 获取品牌列表
        /// </summary>
        /// <param name="pager">品牌查询条件</param>
        /// <returns>品牌列表</returns>
        /// <remarks>2013-06-25 唐永勤 创建</remarks>
        public abstract Pager<PdBrand> GetPdBrandList(Pager<PdBrand> pager);

        /// <summary>
        /// 品牌是否正在被使用
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-06-28 陈海裕 创建</remarks>
        public abstract bool BrandIsBeingUsed(int sysNo);

        /// <summary>
        /// 删除没被使用的品牌
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-06-28 陈海裕 创建</remarks>
        public abstract int DeleteBrandNotBeingUsed(int sysNo);
    }
}
