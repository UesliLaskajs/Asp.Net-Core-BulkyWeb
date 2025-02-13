﻿using Bulky.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Models.ViewModel
{
    public class OrderVM
    {
        public OrderHeader orderHeader { get; set; }

        public IEnumerable<OrderDetails> orderDetails { get; set; } 
    }
}
