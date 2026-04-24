using MbUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demos.OpenResource.Dapper
{
    public class WaveShipOrderItemRelation
    {
        public long? Id { get; set; }
        public Guid? CreatorId { get; set; }

        public string? CreatorName { get; set; }

        public Guid? LastModifierId { get; set; }

        public string? LastModifierName { get; set; }

        public long? CreationTime { get; set; }

        public long? LastModificationTime { get; set; }
        /// <summary>
        /// 关联的出库单明细表id
        /// </summary>

        public virtual long ShipOrderItemId { get; set; }
        /// <summary>
        /// 关联的出库申请单明细表id
        /// </summary>
        public virtual long ApplyShipOrderItemId { get; set; }
        /// <summary>
        /// 波次需求数量
        /// </summary
        public virtual decimal RequiredNumber { get; set; }
        /// <summary>
        /// 已拣货数量
        /// </summary>
        public virtual decimal PickedNumber { get; set; }
    }
}
