using GoStay.DataAccess.Base;
using System;
using System.Collections.Generic;

namespace GoStay.DataAccess.Entities
{
    public partial class ViewDirection : BaseEntity
    {
        public ViewDirection()
        {
            RoomViews = new HashSet<RoomView>();
        }


        public string? NameView { get; set; }


        public virtual ICollection<RoomView> RoomViews { get; set; }
    }
}
