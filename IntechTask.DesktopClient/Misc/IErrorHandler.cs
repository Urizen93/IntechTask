using System;
using System.Diagnostics.CodeAnalysis;

namespace IntechTask.DesktopClient.Misc
{
    public interface IErrorHandler
    {
        void HandleError([NotNull] Exception ex);
    }
}