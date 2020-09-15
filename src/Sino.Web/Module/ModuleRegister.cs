using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sino.Web.Module
{
    /// <summary>
    /// 模块注册基类
    /// </summary>
    public abstract class ModuleRegister : IModuleRegister
    {
        public virtual void ConfigureServices(IWindsorContainer container, List<Type> types)
        {

        }
    }
}
