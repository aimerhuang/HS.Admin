using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.DataAccess.Product;
using Hyt.Model;

namespace Hyt.BLL.Product
{
    /// <summary>
    /// 商品品牌维护
    /// </summary>
    /// <remarks>2013-06-24 唐永勤 创建</remarks>  
    public class PdBrandBo : BOBase<PdBrandBo>
    {
        /// <summary>
        /// 保存品牌信息，添加或修改
        /// </summary>
        /// <param name="model">品牌实体数据</param>
        /// <returns>保存结果对象</returns>
        /// <remarks>2013-07-03 唐永勤 创建</remarks>
        public Result BrandSave(PdBrand model)
        {
            Result result = new Result();
            if (string.IsNullOrEmpty(model.Name))
            {
                result.StatusCode = -1;
            }
            else
            {
                //数据重复性检测
                bool isExists = IPdBrandDao.Instance.IsExists(model.Name, model.SysNo);
                if (isExists)
                {
                    result.StatusCode = -2;
                    result.Message = "品牌名称已存在";
                    return result;
                }

                //数据操作
                if (model.SysNo > 0)
                {
                    result.Status = IPdBrandDao.Instance.Update(model);
                }
                else
                {
                    model.SysNo = IPdBrandDao.Instance.Create(model);
                    if (model.SysNo > 0)
                    {
                        result.Status = true;
                    }
                }
                if (result.Status)
                {
                    result.StatusCode = 1;
                    result.Message = "品牌信息保存成功";
                }
            }
            return result;
        }

        /// <summary>
        /// 判断重复数据--品牌
        /// </summary>
        /// <param name="name">品牌名称</param>
        /// <param name="brandSysNo">品牌编号</param>
        /// <returns>存在返回true，不存在返回flase</returns>
        /// <remarks>2013-07-03 唐永勤 创建</remarks>
        public bool IsExists(string name, int brandSysNo)
        {
            return IPdBrandDao.Instance.IsExists(name, brandSysNo);
        }

        /// <summary>
        /// 获取指定编号的品牌信息
        /// </summary>
        /// <param name="brandSysNo">品牌编号</param>
        /// <returns>品牌实体信息</returns>
        /// <remarks>2013-06-24 唐永勤 创建</remarks>
        public PdBrand GetEntity(int brandSysNo)
        {
            return IPdBrandDao.Instance.GetEntity(brandSysNo);
        }

        /// <summary>
        /// 获取指定名称的品牌信息
        /// </summary>
        /// <param name="name">品牌名称</param>
        /// <returns>品牌实体信息</returns>
        /// <remarks>2015-09-10 王耀发 创建</remarks>
        public PdBrand GetEntityByName(string Name)
        {
            return IPdBrandDao.Instance.GetEntityByName(Name);
        }

        /// <summary>
        /// 获取b2b指定名称的品牌信息
        /// </summary>
        /// <param name="name">品牌名称</param>
        /// <returns>品牌实体信息</returns>
        /// <remarks>2017-10-10 罗勤瑶 创建</remarks>
        public PdBrand GetB2BEntityByName(string Name)
        {
            return IPdBrandDao.Instance.GetB2BEntityByName(Name);
        }
        /// <summary>
        /// 更新品牌状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="listSysNo">品牌编号集</param>
        /// <returns>更新行数</returns>
        /// <remarks>2013-06-25 唐永勤 创建</remarks>
        public int UpdateStatus(Hyt.Model.WorkflowStatus.ProductStatus.品牌状态 status, List<int> listSysNo)
        {
            return IPdBrandDao.Instance.UpdateStatus(status, listSysNo);

        }

        /// <summary>
        /// 获取品牌列表
        /// </summary>
        /// <param name="pager">品牌查询条件</param>
        /// <returns>品牌列表</returns>
        /// <remarks>2013-06-24 唐永勤 创建</remarks>
        public Pager<PdBrand> GetPdBrandList(Pager<PdBrand> pager)
        {
            return IPdBrandDao.Instance.GetPdBrandList(pager);
        }

        /// <summary>
        /// 品牌是否正在被使用
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-06-28 陈海裕 创建</remarks>
        public bool BrandIsBeingUsed(int sysNo)
        {
            return IPdBrandDao.Instance.BrandIsBeingUsed(sysNo);
        }

        /// <summary>
        /// 删除没被使用的品牌
        /// </summary>
        /// <param name="sysNo"></param>
        /// <returns></returns>
        /// <remarks>2016-06-28 陈海裕 创建</remarks>
        public int DeleteBrandNotBeingUsed(int sysNo)
        {
            return IPdBrandDao.Instance.DeleteBrandNotBeingUsed(sysNo);
        }
    }
}
