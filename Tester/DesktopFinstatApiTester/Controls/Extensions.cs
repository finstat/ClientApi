using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DesktopFinstatApiTester.Controls
{
    public interface IValidate
    {
        bool IsValid();
    }

    public interface IAppControl : IValidate
    {
        IDictionary<string, object[]> GetErrors();
    }
}
