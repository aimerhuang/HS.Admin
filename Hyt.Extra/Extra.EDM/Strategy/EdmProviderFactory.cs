using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Configuration;

namespace Extra.EDM
{
    /// <summary>
    /// EDM提供
    /// </summary>
    /// <remarks>
    /// 2014-01-13 何方 创建
    /// </remarks>
    public class EdmProviderFactory
    {
        private static IEdmProvider provider = null;

        /// <summary>
        /// Initializes the <see cref="EdmProviderFactory"/> class.
        /// </summary>
        static EdmProviderFactory()
        {           
            provider = new EDM.FocusSend.EdmProvider();
        }

        /// <summary>
        /// 创建Provider
        /// </summary>
        /// <returns>EdmProvider</returns>
        /// <remarks>
        /// 2014-01-13 何方 创建
        /// </remarks>
        public static IEdmProvider CreateProvider()
        {
            return provider;
        }
    }
}
