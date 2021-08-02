﻿using Caviar.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caviar.Core
{
    public class AppInteractor : Interactor
    {
        public override bool IsAdmin => GetAdmin();

        private bool GetAdmin()
        {
            if (UserData.Roles == null || UserData.Roles.Count == 0) return false;
            return UserData.Roles.FirstOrDefault(u => u.Uid == CaviarConfig.SysAdminRoleGuid) == null ? false : true;
        }
    }
}
