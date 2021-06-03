using Caviar.Control;
using Caviar.Models.SystemData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Caviar.Models.SystemData;
/// <summary>
/// 生成者：admin
/// 生成时间：2021/6/3 14:38:03
/// 代码由代码生成器自动生成，更改的代码可能被进行替换
/// 可在上层目录使用partial关键字进行扩展
/// </summary>
namespace Caviar.Control.Menu
{
    [DisplayName("菜单控制器")]
    public partial class MenuController : CaviarBaseController
    {
        #region 属性注入
        MenuAction _action;
        /// <summary>
        /// 方法操作器
        /// </summary>
        MenuAction Action 
        {
            get 
            {
                if (_action == null)
                {
                    _action = CreateModel<MenuAction>();
                }
                return _action;
            }
            set
            {
                _action = value;
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="Menu">方法操作器</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add(MenuAction Menu)
        {
            try
            {
                var count = await Menu.AddEntity();
                if (count > 0)
                {
                    return ResultOK();
                }
            }
            catch(Exception e)
            {
                return ResultErrorMsg("添加失败", e.Message);
            }
            return ResultErrorMsg("添加失败");
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="Menu">方法操作器</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Update(MenuAction Menu)
        {
            try
            {
                var count = await Menu.UpdateEntity();
                if (count > 0)
                {
                    return ResultOK();
                }
            }
            catch(Exception e)
            {
                return ResultErrorMsg("修改失败", e.Message);
            }
            return ResultErrorMsg("修改失败");
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="Menu">方法操作器</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Delete(MenuAction Menu)
        {
            try
            {
                var count = await Menu.DeleteEntity();
                if (count > 0)
                {
                    return ResultOK();
                }
            }
            catch(Exception e)
            {
                return ResultErrorMsg("删除失败", e.Message);
            }
            return ResultErrorMsg("删除失败");
        }
        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="isOrder"></param>
        /// <param name="isNoTracking"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, bool isOrder = true, bool isNoTracking = true)
        {
            var pages = await Action.GetPages(u => true, pageIndex, pageSize, isOrder, isNoTracking);
            ResultMsg.Data = pages;
            return ResultOK();
        }
        #endregion

    }
}