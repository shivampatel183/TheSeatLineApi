using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TheSeatLineApi.Modules.MasterModule.Models.Entities
{
    [Table("CouponEvents")]
    public class CouponEvent
    {
        [Key, Column(Order = 0)]
        public Guid CouponId { get; set; }

        [Key, Column(Order = 1)]
        public Guid EventId { get; set; }

        // Navigation properties
        [ForeignKey(nameof(CouponId))]
        public virtual Coupon Coupon { get; set; } = null!;

        [ForeignKey(nameof(EventId))]
        public virtual Event Event { get; set; } = null!;
    }
}



