﻿using Bulky.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IOrderDetails:IRepository<OrderDetails>
    {
        public void Update(OrderDetails orderDetails);
      
    }
}
