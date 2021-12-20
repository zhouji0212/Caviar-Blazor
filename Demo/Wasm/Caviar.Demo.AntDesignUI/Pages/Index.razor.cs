﻿using System;
using System.Threading.Tasks;
using Caviar.AntDesignUI.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Caviar.Demo.AntDesignUI.Pages
{
    partial class Index
    {
        [Inject]
        UserConfig UserConfig { get; set; }
        public void test()
        {
            var list = UserConfig.LanguageService.GetLanguageList();
        }
    }
}