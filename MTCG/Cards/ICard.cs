﻿using System;
using System.Collections.Generic;
using System.Text;
using MTCG.Enums;

namespace MTCG.Cards {
    public interface ICard {
	    public string Name { get; set; }
	    public ElementType ElementType { get; set; }
    }
}
