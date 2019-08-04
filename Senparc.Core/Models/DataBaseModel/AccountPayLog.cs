﻿using System;
using System.Collections.Generic;

namespace Senparc.Core.Models
{
    [Serializable]
    public partial class AccountPayLog
    {
        public AccountPayLog()
        {
            this.PointsLogs = new HashSet<PointsLog>();
        }

        public int Id { get; set; }

        public int AccountId { get; set; }

        public string OrderNumber { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal PayMoney { get; set; }

        public decimal? UsedPoints { get; set; }

        public System.DateTime AddTime { get; set; }

        public System.DateTime CompleteTime { get; set; }

        public string AddIp { get; set; }

        public decimal GetPoints { get; set; }

        public byte Status { get; set; }

        public string Description { get; set; }

        public byte? Type { get; set; }
        public string TradeNumber { get; set; }
        public string PrepayId { get; set; }
        public int PayType { get; set; }
        public int OrderType { get; set; }
        public string PayParam { get; set; }
        public decimal Price { get; set; }
        public decimal Fee { get; set; }

        public virtual ICollection<PointsLog> PointsLogs { get; set; }
        public virtual Account Account { get; set; }
    }
}
