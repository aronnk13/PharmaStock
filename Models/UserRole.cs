<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;

namespace PharmaStock.Models;

public partial class UserRole
{
    public int RoleId { get; set; }

    public string RoleType { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
=======
using System;
using System.Collections.Generic;
 
namespace PharmaStock.Models;
 
public partial class UserRole
{
    public int RoleId { get; set; }
 
    public string RoleType { get; set; } = null!;
 
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
 
>>>>>>> 2b7da832cc7536e37c1cc672957d091dcf9108bc
