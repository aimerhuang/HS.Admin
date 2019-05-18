using Hyt.Util.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSocket4Net;

namespace Extra.PushService
{
    /// <summary>
    /// 代理站的websocket 服务单例模式实体
    /// </summary>
    /// <remarks>2016-8-2 杨云奕 创建</remarks>
    public class AgentPushServiceSingleton
    {
        /// <summary>
        /// 收到信息的委托事件
        /// </summary>
        /// <param name="mod"></param>
        public delegate void GetMessageHandler(MessageMod mod);
        public event GetMessageHandler GetMessageEvent;

        private static volatile AgentPushServiceSingleton instance = null;

        private static object lockHelper = new object();

        string _listenIp = "";
        int _port = 0;

        WebSocket websocket;
        List<string> MessageList = new List<string>();
        /// <summary>
        /// 历史记录
        /// </summary>
        List<string> HistoryList = new List<string>();

        /// <summary>
        /// 身份凭证
        /// </summary>
        MessageMod CertificateMod = null;

        /// <summary>
        /// 离线订单数据集合
        /// </summary>
        List<MessageMod> DisOrderList = new List<MessageMod>();

        private AgentPushServiceSingleton() { }
        /// <summary>
        /// 单件模式对象
        /// </summary>
        public static AgentPushServiceSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockHelper)
                    {
                        if (instance == null)
                        {
                            instance = new AgentPushServiceSingleton();
                        }
                    }
                }
                return instance;
            }
        } 
        /// <summary>
        /// 初始化socket
        /// </summary>
        /// <param name="listenIp">推送服务ip</param>
        /// <param name="port">推送服务端口</param>
        /// <param name="_Certificate">身份凭证的信息</param>
        /// <remarks>2016-8-2 杨云奕 创建</remarks>
        public void InitSuperWebSocket(string listenIp, int port, MessageMod _Certificate)
        {
            if (websocket == null || websocket.State== WebSocketState.Closed)
            {
                CertificateMod = _Certificate;

                _listenIp = listenIp;
                _port = port;

                HistoryList.Add("正在连接服务器...");
                websocket = new WebSocket("ws://" + listenIp + ":" + port+"");
                websocket.Opened += websocket_Opened;
                websocket.Closed += websocket_Closed;
                websocket.MessageReceived += websocket_MessageReceived;


                InitDisconnectionOrder();
                websocket.Open();

            }
        }

        /// <summary>
        /// 推送服务重新连接上后的数据推送
        /// </summary>
        /// <param name="_DisOrderList"></param>
        public void InitDisconnectionOrder()
        {
            List<MessageMod> _DisOrderList = GetDisCountOrderList();
            foreach (var mod in _DisOrderList)
            {
                var tempMod = DisOrderList.Find(p => p.OrderSysNo == mod.OrderSysNo);
                if(tempMod==null)
                {
                    DisOrderList.Add(mod);
                }
            }
        }

        /// <summary>
        /// 获取两天内离线的订单的数据集合
        /// </summary>
        /// <returns></returns>
        List<MessageMod> GetDisCountOrderList()
        {
            List<MessageMod> DiscountList = new List<MessageMod>();

            List<Hyt.Model.SoOrder> orderList = Hyt.BLL.Pos.SyWSOrderToClientJobBo.Instance.GetNotPosthOrderDataToClientList(1);
            foreach (Hyt.Model.SoOrder order in orderList)
            {
                List<int> posManageSysNo = new List<int>();
                //SoOrder order = Hyt.BLL.Order.SoOrderBo.Instance.GetEntity(OrderSysNo);
                Hyt.Model.SoReceiveAddress address = Hyt.BLL.Order.SoOrderBo.Instance.GetOrderReceiveAddress(order.ReceiveAddressSysNo);
                IList<Hyt.Model.SoOrderItem> orderItems = Hyt.BLL.Order.SoOrderBo.Instance.GetOrderItemsByOrderId(order.SysNo);
                //CBDsDealer dealerMod = DsDealerBo.Instance.GetDsDealer(order.DealerSysNo);
                List<Hyt.Model.Pos.CBDsPosManage> posManageList = Hyt.BLL.Pos.DsPosManageBo.Instance.GetEntityListByDsSysNo(order.DealerSysNo);
                List<Hyt.Model.BsPaymentType> payTypeList = Hyt.BLL.Order.SoOrderBo.Instance.LoadAllPayType().Where(b => b.Status == 1).ToList();
                Hyt.Model.BsPaymentType pTypeMod = payTypeList.Find(p => p.SysNo == order.PayTypeSysNo);
                if (pTypeMod == null)
                {
                    pTypeMod = new Hyt.Model.BsPaymentType()
                    {
                        PaymentName = "未知类型"
                    };
                }
                foreach (Hyt.Model.Pos.CBDsPosManage mod in posManageList)
                {
                    posManageSysNo.Add(mod.SysNo);
                }
                List<Hyt.Model.Pos.DsPosWebSocketManage> manageList = Hyt.BLL.Pos.DsPosWebSocketManageBo.Instance.GetSocketManage(posManageSysNo.ToArray());
                string Message = "";
                foreach (Hyt.Model.Pos.DsPosWebSocketManage mod in manageList)
                {
                    Message += "订单编号：" + order.SysNo + " 接收机器：" + mod.WS_PosName;
                    if (mod.WS_Status == 1 && !string.IsNullOrEmpty(mod.WS_PosNumber))
                    {
                        MessageMod msgMod = new MessageMod();
                        msgMod.ComeType = "订单";
                        msgMod.Function = "SendUser";
                        msgMod.ReceiveId = mod.WS_PosNumber;
                        msgMod.Message = "";
                        msgMod.OrderSysNo = order.SysNo;
                        msgMod.Receiver = address.Name;
                        msgMod.ReceiverAddress = address.StreetAddress;
                        msgMod.ReceiverTele = address.PhoneNumber + "、" + address.MobilePhoneNumber;
                        msgMod.CreateTime = order.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
                        msgMod.PayType = pTypeMod.PaymentName;
                        int totalNumber = 0;
                        foreach (Hyt.Model.SoOrderItem item in orderItems)
                        {
                            totalNumber += item.Quantity;
                        }
                        msgMod.TotalNumber = totalNumber;
                        msgMod.TotalValue = order.OrderAmount;

                        //msgMod.OrderItems = Hyt.Util.Serialization.JsonUtil.ToJson2(orderItems);
                        msgMod.OrderItems = "[]";
                        if (!string.IsNullOrEmpty(Message))
                        {
                            Message += " , ";
                        }
                        DiscountList.Add(msgMod);
                        //try
                        //{
                        //    AgentPushServiceSingleton.Instance.SendMessageToService(msgMod);
                        //    Message += "  发送成功！";
                        //}
                        //catch (Exception e)
                        //{
                        //    Message += "  发送失败！-" + e.Message;
                        //}
                    }
                }
            }

            return DiscountList;
        }

        /// <summary>
        /// 服务器客户端接受信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>2016-8-2 杨云奕 创建</remarks>
        private void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            if (GetMessageEvent!=null)
            {
                GetMessageEvent(JsonUtil.ToObject<MessageMod>(sender.ToString()));
            }
            MessageList.Add(sender.ToString());
        }
        void websocket_Opened(object sender, EventArgs e)
        {
            HistoryList.Add("连接服务器成功，发送验证信息...");
            //MessageMod mod = new MessageMod();
            //mod.Type = "Agenter";
            //mod.ShopAppId = "XiaoQuShangChao";
            //mod.Function = "OPENCheck";
            //mod.Message = "";
            string message = JsonUtil.ToJson2(CertificateMod);
            websocket.Send(message);

            foreach(var mod in DisOrderList)
            {
                if( websocket!=null)
                {
                    message = JsonUtil.ToJson2(mod);
                    websocket.Send(message);
                    Hyt.Model.Pos.SyWSOrderToClientJob jobMod = new Hyt.Model.Pos.SyWSOrderToClientJob()
                    {
                        DsSysNo = 0,
                        OrderSysNo = mod.OrderSysNo,
                        PushData = message,
                        Message = "",
                        PushTime = DateTime.Now,
                        Status = 1
                    };
                    Hyt.BLL.Pos.SyWSOrderToClientJobBo.Instance.InnerMod(jobMod);
                }
            }
        }

        /// <summary>
        /// 异常情况重新连接
        /// </summary>
        /// <remarks>2016-8-2 杨云奕 创建</remarks>
        public void CheckSuperWebSocketStart()
        {
            if (websocket != null && websocket.State == WebSocketState.Closed)
            {
                HistoryList.Add("服务器连接异常，重新连接");
                InitDisconnectionOrder();
                websocket.Open();
            }
           
        }
        /// <summary>
        /// 服务端关闭，连接断开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>2016-8-2 杨云奕 创建</remarks>
        void websocket_Closed(object sender, EventArgs e)
        {
            HistoryList.Add("服务器连接异常或者服务器关闭服务，无法进行监听。");
            //websocket.Send("一个客户端 下线");       
        }
        /// <summary>
        /// 将信息发给客户端
        /// </summary>
        /// <param name="shopAppId">门店AppId</param>
        /// <param name="message"></param>
        public void SendMessageToService(MessageMod mod)
        {
            mod.ShopAppId = CertificateMod.ShopAppId;
            mod.Type = CertificateMod.Type;
            ///给服务器发送信息指令，通知服务器进行操作。
            string message =JsonUtil.ToJson2(mod);
            websocket.Send(message);       
        }

        /// <summary>
        /// 获取和服务器进行连接的状态
        /// </summary>
        /// <returns></returns>
        /// <remarks>2016-8-2 杨云奕 创建</remarks>
        public string GetSocketStatus()
        {
            return websocket.State.ToString();
        }
        /// <summary>
        /// 地址
        /// </summary>
        /// <returns></returns>
        public string GetWSPathData()
        {
            return _listenIp + ":" + _port;
        }

    }
}
