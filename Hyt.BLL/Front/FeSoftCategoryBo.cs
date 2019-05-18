using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hyt.Model;
using Hyt.DataAccess.Front;


namespace Hyt.BLL.Front
{
    public class FeSoftCategoryBo : BOBase<FeSoftCategoryBo>
    {
        /// <summary>
        /// 保存软件分类信息，添加或修改
        /// </summary>
        /// <param name="model">软件分类实体数据</param>
        /// <returns>保存结果对象</returns>
        /// <remarks>2014-01-16 唐永勤 创建</remarks>
        public Result FeSoftCategorySave(FeSoftCategory model)
        {
            Result result = new Result();
            if (string.IsNullOrEmpty(model.Name))
            {
                result.StatusCode = -1;
            }
            else
            {
                //数据重复性检测
                bool isExists = IFeSoftCategoryDao.Instance.IsExists(model);
                if (isExists)
                {
                    result.StatusCode = -2;
                    result.Message = "软件分类名称已存在";
                    return result;
                }

                //数据操作
                if (model.SysNo > 0)
                {
                    result.Status = IFeSoftCategoryDao.Instance.Update(model);
                }
                else
                {
                    model.SysNo = IFeSoftCategoryDao.Instance.Create(model);
                    if (model.SysNo > 0)
                    {
                        result.Status = true;
                    }
                }
                if (result.Status)
                {
                    result.StatusCode = 1;
                    result.Message = "软件分类信息保存成功";
                }
            }
            return result;
        }

        /// <summary>
        /// 获取指定编号的软件分类信息
        /// </summary>
        /// <param name="sysNo">软件分类编号</param>
        /// <returns>软件分类实体信息</returns>
        /// <remarks>2014-01-16 唐永勤 创建</remarks>
        public FeSoftCategory GetEntity(int sysNo)
        {
            return IFeSoftCategoryDao.Instance.GetEntity(sysNo);
        }

        /// <summary>
        /// 更新软件分类状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="sysNo">软件分类编号</param>
        /// <returns>更新行数</returns>
        /// <remarks>2014-01-16 唐永勤 创建</remarks>
        public int UpdateStatus(Hyt.Model.WorkflowStatus.ForeStatus.软件分类状态 status, int sysNo)
        {
            return IFeSoftCategoryDao.Instance.UpdateStatus(status, sysNo);

        }

        /// <summary>
        /// 获取软件分类列表
        /// </summary>
        /// <param name="pager">软件分类查询条件</param>
        /// <returns>软件分类列表</returns>
        /// <remarks>2014-01-16 唐永勤 创建</remarks>
        public Pager<FeSoftCategory> GetFeSoftCategoryList(Pager<FeSoftCategory> pager)
        {
            return IFeSoftCategoryDao.Instance.GetFeSoftCategoryList(pager);
        }

        /// <summary>
        /// 获取软件分类列表
        /// </summary>
        /// <returns>软件分类列表</returns>
        /// <remarks>2014-03-04 苟治国 创建</remarks>
        public List<FeSoftCategory> GetFeSoftCategoryList()
        {
            return IFeSoftCategoryDao.Instance.GetFeSoftCategoryList();
        }
    }
}
