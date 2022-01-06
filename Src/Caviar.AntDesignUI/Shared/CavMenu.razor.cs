﻿using AntDesign;
using Caviar.SharedKernel.View;
using Caviar.AntDesignUI.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Caviar.SharedKernel.Entities.View;
using Microsoft.JSInterop;

namespace Caviar.AntDesignUI.Shared
{
    partial class CavMenu
    {
        bool _inlineCollapsed;
        [Parameter]
        public bool InlineCollapsed
        {
            get { return _inlineCollapsed; }
            set
            {
                OnCollapsed(value);
                _inlineCollapsed = value;
            }
        }

        [Parameter]
        public MenuItem BreadcrumbItemCav { get; set; }
        [Parameter]
        public MenuTheme Theme { get; set; } = MenuTheme.Dark;

        public Menu AntDesignMenu { get; set; }
        [Parameter]
        public EventCallback<MenuItem> BreadcrumbItemCavChanged { get; set; }

        public string[] SelectedKeys { get; set; }

        public string[] OpenKeysNav { get; set; } = Array.Empty<string>();

        string[] _openKeysNae;

        [Inject]
        HttpHelper Http { get; set; }
        [Inject]
        UserConfig UserConfig { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }

        [Inject]
        IJSRuntime jSRuntime { get; set; }


        public async void OnMenuItemClickedNav(MenuItem menuItem)
        {
            //在server模式下且需要自动切换
            if (Config.IsServer && UserConfig.IsAutomaticSwitchWasm)
            {
                var iframeMessage = new IframeMessage();
                iframeMessage.Pattern = Pattern.Wasm;
                iframeMessage.Url = menuItem.RouterLink;
                _ = jSRuntime.InvokeVoidAsync("iframeMessage", iframeMessage);
            }
            BreadcrumbItemCav = menuItem;
            if (BreadcrumbItemCavChanged.HasDelegate)
            {
                await BreadcrumbItemCavChanged.InvokeAsync(BreadcrumbItemCav);
                
            }
        }

        /// <summary>
        /// 当收缩时候将打开的菜单关闭，防止出现第二菜单。
        /// </summary>
        /// <param name="collapsed"></param>
        public void OnCollapsed(bool collapsed)
        {
            if (collapsed == _inlineCollapsed) return;
            if (collapsed)
            {
                _openKeysNae = OpenKeysNav;
                OpenKeysNav = Array.Empty<string>();
            }
            else
            {
                OpenKeysNav = _openKeysNae;
            }
        }
        protected override void OnParametersSet()
        {
            UserConfig.RefreshMenuAction = Refresh;
            base.OnParametersSet();
        }

        protected override async Task OnInitializedAsync()
        {
            SysMenus = await GetMenus();
        }

        protected override void OnAfterRender(bool firstRender)
        {            
            base.OnAfterRender(firstRender);
        }

        private List<SysMenuView> SysMenus;

        public async void Refresh()
        {
            await OnInitializedAsync();
            StateHasChanged();
            foreach (var item in AntDesignMenu.MenuItems)
            {
                if(item.Key == AntDesignMenu.SelectedKeys.FirstOrDefault())
                {
                    OnMenuItemClickedNav(item);
                }
            }
            
        }

        async Task<List<SysMenuView>> GetMenus()
        {
            var result = await Http.GetJson<List<SysMenuView>> ("SysMenu/GetMenuBar");
            if (result.Status != StatusCodes.Status200OK) return new List<SysMenuView>();
            return result.Data;
        }


    }


}
