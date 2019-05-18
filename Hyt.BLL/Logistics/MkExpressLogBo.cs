using System.Security.Cryptography;
using Hyt.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hyt.BLL.Logistics
{
    /// <summary>
    /// 物流刷单操作
    /// </summary>
    /// <remarks>2014-01-09 沈强 创建</remarks>
    public class MkExpressLogBo : BOBase<MkExpressLogBo>
    {
        //随机数生成器
        private RNGCryptoServiceProvider rng = null;

        /// <summary>
        /// 获取物流日志
        /// </summary>
        /// <param name="pagerFilter">查询过滤对象</param>
        /// <returns></returns>
        /// <remarks>2013-12-19 沈强 创建</remarks>
        public Pager<MkExpressLog> GetLogisticsDeliveryItems(Pager<MkExpressLog> pagerFilter)
        {
            return DataAccess.Logistics.IMkExpressLogDao.Instance.GetLogisticsDeliveryItems(pagerFilter);
        }

        /// <summary>
        /// 根据物流单号，获取物流日志
        /// </summary>
        /// <param name="expressNo">物流单号</param>
        /// <returns></returns>
        /// <remarks>2013-12-19 沈强 创建</remarks>
        public IList<MkExpressLog> GetMkExpressLogList(string expressNo)
        {
            return DataAccess.Logistics.IMkExpressLogDao.Instance.GetMkExpressLogList(expressNo); 
        }

        /// <summary>
        /// 自动生成物流日志
        /// </summary>
        /// <param name="expressNo">物流号</param>
        /// <returns></returns>
        /// <remarks>2013-12-20 沈强 创建</remarks>
        public void GenerateLogisticsLog(string expressNo)
        {
            var list = this.GetRandomLogisticsLogs(expressNo);
            var now = DateTime.Now;
            //不能超过最晚时间
            var dateLatest = now.Date.AddHours(21);

            for (var i = 0; i < list.Count; i++)
            {
                var item = list[i];

                var minute = this.Next(30, 180);
                now = now.AddMinutes(minute);
                if (now > dateLatest)
                {//如果当前时间超过最晚时间，就设置为从第二天开始
                    var tmpM = this.Next(20, 90);
                    now = now.AddDays(1).Date.AddHours(9).AddMinutes(tmpM);
                    dateLatest = now.Date.AddHours(21);
                }

                if (i == 7)
                {//最后两项处理为相同时间
                    item.OperateDate = now;
                    list[i + 1].OperateDate = now;
                    break;
                }
                item.OperateDate = now;
            }
            DataAccess.Logistics.IMkExpressLogDao.Instance.Insert(list);
        }
        #region 获取随机数

        /// <summary>
        /// 获取指定范围内的随机数
        /// </summary>
        /// <param name="minValue">产生随机数的最小值（包含最小值）</param>
        /// <param name="maxValue">产生随机数的最大值（不包含最大值）</param>
        /// <returns></returns>
        private int Next(int minValue, int maxValue)
        {
            int returnValue = GetRandomValue(maxValue - 1);
            if (returnValue < minValue)
            {
                returnValue = Next(minValue, maxValue);
            }
            return returnValue;
        }

        /// <summary>
        /// 获取0 ~ maxValue的强随机数（含maxValue）
        /// </summary>
        /// <param name="maxValue">随机数产生的最大值</param>
        /// <returns>返回一个随机数</returns>
        /// <remarks>2013-12-20 沈强 创建</remarks>
        private int GetRandomValue(int maxValue)
        {
            if (rng == null)
            {
                //实例化随机数生成器
                rng = new RNGCryptoServiceProvider();
            }
            //这样产生0 ~ maxValue的强随机数（含maxValue）
            var max = maxValue;
            var rnd = int.MinValue;
            const decimal _base = (decimal)long.MaxValue;
            var rndSeries = new byte[8];
            rng.GetBytes(rndSeries);
            //不含maxValue需去掉+1 
            rnd = (int)(Math.Abs(BitConverter.ToInt64(rndSeries, 0)) / _base * (max + 1));
            return rnd;
        }

        /// <summary>
        /// 获取0 ~ maxValue的一组强随机数（含maxValue)
        /// </summary>
        /// <param name="maxValue">随机数产生的最大值</param>
        /// <param name="randomNum">想要获得的随机数个数</param>
        /// <returns>返回一个随机数数组</returns>
        /// <remarks>2013-12-20 沈强 创建</remarks>
        private int[] GetRandomValue(int maxValue, int randomNum)
        {
            if (rng == null)
            {
                //实例化随机数生成器
                rng = new RNGCryptoServiceProvider();
            }
            var returnValues = new int[randomNum];
            //这样产生0 ~ maxValue的强随机数（含maxValue）
            var max = maxValue;
            var rnd = int.MinValue;
            const decimal _base = (decimal)long.MaxValue;
            var rndSeries = new byte[8];
            for (int i = 0; i < randomNum; i++)
            {
                rng.GetBytes(rndSeries);
                //不含maxValue需去掉+1 
                rnd = (int)(Math.Abs(BitConverter.ToInt64(rndSeries, 0)) / _base * (max + 1));
                returnValues[i] = rnd;
            }
           
            return returnValues;
        }
        #endregion

        /// <summary>
        /// 随机获取操作人
        /// </summary>
        /// <returns></returns>
        /// <remarks>2013-12-19 沈强 创建</remarks>
        private string GetDeliveryUserName()
        {
            string[] users =
                {
                    "蒋军", "孙斌", "刘伟凡"
                    , "钟伦清", "吴艳波", "向用"
                    , "陈琛", "陈宇", "赵佳"
                    , "刘芳", "刘伟", "亢超"
                    , "肖雪", "任桂玉", "徐天儒"
                };
            var index = GetRandomValue(users.Length - 1);
            return users[index];
        }

        /// <summary>
        /// 获取物流日志模板
        /// </summary>
        /// <param name="expressNo">物流编号</param>
        /// <returns></returns>
        /// <remarks>2013-12-19 沈强 创建</remarks>
        private List<MkExpressLog> GetRandomLogisticsLogs(string expressNo)
        {
            return new List<MkExpressLog>()
                {
                    new MkExpressLog()
                        {
                            OperatorName = this.GetDeliveryUserName(),
                            ExpressNo = expressNo,
                            LogContent = "订单升舱成功，等待客服确认"
                        },
                        new MkExpressLog()
                        {
                            OperatorName = this.GetDeliveryUserName(),
                            ExpressNo = expressNo,
                            LogContent = "订单审核通过，等待分配出库"
                        },
                        new MkExpressLog()
                        {
                            OperatorName = this.GetDeliveryUserName(),
                            ExpressNo = expressNo,
                            LogContent = "订单分配出库成功，待选择配送方式"
                        },
                        new MkExpressLog()
                        {
                            OperatorName = this.GetDeliveryUserName(),
                            ExpressNo = expressNo,
                            LogContent = "订单生成配送方式:<span style='color:#ff0000;'>百城当日达</span>，待拣货打包"
                        },
                        new MkExpressLog()
                        {
                            OperatorName = this.GetDeliveryUserName(),
                            ExpressNo = expressNo,
                            LogContent = "订单拣货打包完毕，待分配配送"
                        },
                        new MkExpressLog()
                        {
                            OperatorName = this.GetDeliveryUserName(),
                            ExpressNo = expressNo,
                            LogContent = "订单分配配送成功"
                        },
                        new MkExpressLog()
                        {
                            OperatorName = this.GetDeliveryUserName(),
                            ExpressNo = expressNo,
                            LogContent = "订单由百城当日达配送中，联系电话<span style='color:#ff0000;'>400-088-9898</span>"
                        },
                        new MkExpressLog()
                        {
                            OperatorName = this.GetDeliveryUserName(),
                            ExpressNo = expressNo,
                            LogContent = "订单已签收，配送完成"
                        },
                        new MkExpressLog()
                        {
                            OperatorName = this.GetDeliveryUserName(),
                            ExpressNo = expressNo,
                            LogContent = "订单生成结算，本次交易完成，欢迎您再次光临！"
                        }
                };
        }
    }
}