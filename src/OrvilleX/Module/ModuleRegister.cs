using Castle.Windsor;
using System;
using System.Collections.Generic;

namespace OrvilleX.Module
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
